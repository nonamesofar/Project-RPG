using UnityEngine;
using RPG.Core;
using System;

namespace RPG.Movement
{

    public class PlayerMovement : MonoBehaviour
    {
        public bool lockOn;
        public bool isOnAir;
        public bool isGrounded;
        public bool isRolling;
        public bool isInteracting;
        public bool isSprinting;
        public AnimationCurve rollCurve;
        public AnimationClip rollClip;

        [Header("Controller")]
        public float movementSpeed = 6;
        public float sprintSpeed = 3;
        public float rollSpeed = 1;
        public float adaptSpeed = 1;
        public float rotationSpeed = 10;
        public float attackRotationSpeed = 3;
        public float groundDownDistanceOnAir = .4f;
        public float groundedSpeed = 0.1f;
        public float groundedDistanceRay = .5f;
        float velocityMultiplier = 1;

        new Rigidbody rigidbody;

        [HideInInspector]
        public Transform currentLockTarget;
        [HideInInspector]
        public Transform mTransform;

        LayerMask ignoreForGroundCheck;

        [HideInInspector]
        public AnimationHandler animationHandler;
        Vector3 currentNormal;       

        Vector3 currentPosition;

        private void Update()
        {
            isInteracting = animationHandler.IsInAction();
            isRolling = isInteracting;
        }

        public void Init()
        {
            mTransform = this.transform;
            rigidbody = GetComponentInChildren<Rigidbody>();
            animationHandler = GetComponent<AnimationHandler>();
            //anim = GetComponentInChildren<Animator>();
            
            currentPosition = mTransform.position;
            ignoreForGroundCheck = ~(1 << 9 | 1 << 10 | 1 << 11);
        }

        public void MoveCharacter(float vertical, float horizontal, Vector3 moveDirection, float delta)
        {
            CheckGround();
            float moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
            //HandleDamageCollider();
            //HANDLE ROTATION
            if (!isInteracting || animationHandler.CanRotate())
            {
                Vector3 rotationDir = moveDirection;

                if (lockOn && !isSprinting)
                {
                    rotationDir = currentLockTarget.position - mTransform.position;
                }

                HandleRotation(moveAmount, rotationDir, delta);
            }

            Vector3 targetVelocity = Vector3.zero;

            moveAmount = ClampMoveAmount(moveAmount);

            float speed = movementSpeed;

            if (isSprinting)
            {
                speed = sprintSpeed;
            }

            if (isRolling)
            {
                speed = rollSpeed;
            }

            //if (lockOn && !isSprinting)
            //{
            //    float f = ClampNegativeMoveAmount(vertical);
            //    float s = ClampNegativeMoveAmount(horizontal);

            //    targetVelocity = mTransform.forward * f * speed;
            //    targetVelocity += mTransform.right * s * speed;
            //}
            //else
            {
                targetVelocity = moveDirection * speed * moveAmount;
            }

            //HANDLE MOVEMENT
            if (isGrounded && !isInteracting)
            {
                targetVelocity = Vector3.ProjectOnPlane(targetVelocity, currentNormal);
                rigidbody.velocity = targetVelocity;

                Vector3 groundedPosition = mTransform.position;
                groundedPosition.y = currentPosition.y;
                mTransform.position = Vector3.Lerp(mTransform.position, groundedPosition, delta / groundedSpeed);
            }


            HandleAnimations(vertical, horizontal, moveAmount);
        }

        void CheckGround()
        {
            RaycastHit hit;
            Vector3 origin = mTransform.position;
            origin.y += .5f;

            float dis = groundedDistanceRay;
            if (isOnAir)
            {
                dis = groundDownDistanceOnAir;
            }

            Debug.DrawRay(origin, Vector3.down * dis, Color.red);
            if (Physics.SphereCast(origin, .2f, Vector3.down, out hit, dis, ignoreForGroundCheck))
            {
                isGrounded = true;
                currentPosition = hit.point;
                currentNormal = hit.normal;

                float angle = Vector3.Angle(Vector3.up, currentNormal);
                if (angle > 45)
                {
                    isGrounded = false;
                }

                if (isOnAir)
                {
                    isOnAir = false;
                    animationHandler.PlayAnimation("Empty");
                    animationHandler.SetBool("Falling", false);
                }
            }
            else
            {
                if (isGrounded)
                {
                    isGrounded = false;
                }

                if (isOnAir == false)
                {
                    isOnAir = true;
                    animationHandler.PlayAnimation("Fall");
                    animationHandler.SetBool("Falling", true);
                }

            }
        }

        void HandleRotation(float moveAmount, Vector3 targetDir, float delta)
        {
            float moveOverride = moveAmount;
            if (lockOn)
            {
                moveOverride = 1;
            }

            targetDir.Normalize();
            targetDir.y = 0;
            if (targetDir == Vector3.zero)
                targetDir = mTransform.forward;

            float actualRotationSpeed = rotationSpeed;
            if (isInteracting)
                actualRotationSpeed = attackRotationSpeed;

            Quaternion tr = Quaternion.LookRotation(targetDir);
            Quaternion targetRotation = Quaternion.Slerp(
                mTransform.rotation, tr,
                delta * moveOverride * actualRotationSpeed);

            mTransform.rotation = targetRotation;
        }

        public void HandleAnimations(float vertical, float horizontal, float moveAmount)
        {
            if (isGrounded)
            {
                animationHandler.SetBool("IsSprinting", isSprinting);

                if (lockOn && !isSprinting)
                {
                    float f = ClampNegativeMoveAmount(vertical);

                    animationHandler.SetFloat("Forward", f, 0, Time.deltaTime);

                    float s = ClampNegativeMoveAmount(horizontal);

                    animationHandler.SetFloat("Sideways", s, 0, Time.deltaTime);
                }
                else
                {
                    animationHandler.SetFloat("Forward", moveAmount, 0, Time.deltaTime);
                }
            }
            else
            {
            }
        }

        public void PlayTargetAnimation(string targetAnim, bool isInteracting, bool isMirror = false, float velocityMultiplier = 1)
        {
            animationHandler.SetBool("InAction", isInteracting);
            animationHandler.PlayAnimation(targetAnim);
            this.isInteracting = isInteracting;
            this.velocityMultiplier = velocityMultiplier;
        }

        internal void HandleRolls(float horizontal, float vertical)
        {
            animationHandler.SetBool("InAction", isInteracting);
            isInteracting = true;
            //animationHandler.SetBool("Roll", true);
            if (lockOn)
            {
                animationHandler.SetFloat("Forward", vertical);
                animationHandler.SetFloat("Sideways", horizontal);
            }
            animationHandler.PlayAnimation("Roll");
        }

        private static float ClampNegativeMoveAmount(float vertical)
        {
            float v = Mathf.Abs(vertical);
            if (v < 0.2f)
                return 0;
            float f = 0;
            if (v > 0 && v <= .5f)
                f = 1f;
            else if (v > 0.5f)
                f = 1;

            if (vertical < 0)
                f = -f;
            return f;
        }

        private float ClampMoveAmount(float moveAmount)
        {
            //clamp the move amount to remove silly behaviour
            //TODO: values are hardcoded to match animation
            if (moveAmount < 0.05f)
                return 0;
            if(moveAmount < 0.2f)
                moveAmount = 0.2f;
            else if (moveAmount < 0.5f)
                moveAmount = 0.5f;
            else
            {
                moveAmount = 1f;
            }
            return moveAmount;
        }

        #region AnimationEvents
        void FootL() { }
        void FootR() { }
        void Land() { }
        #endregion
    }
}
