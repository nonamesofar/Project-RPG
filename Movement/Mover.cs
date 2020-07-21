using System;
using RPG.Core;
using UnityEngine;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] float walkSpeed = 2f;
        [SerializeField] float sprintSpeed = 6f;
        [SerializeField] float turnSmoothTime = 0.1f;
        [SerializeField] float animationDampTime = 0.2f;

        [Header("Falling Settings")]
        [SerializeField] float mv = 0;
        [SerializeField] float G = 50f;
        [SerializeField] float minDistanceToGround = 0.02f;
        [SerializeField] float maxDistanceToGround = 0.2f;
        [SerializeField] float feetAdjustmentSpeed = 1.5f;
        [SerializeField] float mediumFallTime = 1f;


        //Should be changed to RigidBody?
        private CharacterController controller;
        private AnimationHandler animationHandler;
        private Transform cameraMain;
        private Transform target;
        
        private bool isInAir = false;
        private float timeInAir = 0;
        private Vector3 moveDirection;

        private float turnSmoothVelocity;

        private RaycastHit hitInfo;

        private void Start()
        {
            animationHandler = GetComponent<AnimationHandler>();
            controller = GetComponent<CharacterController>();
            

        }

        public void SetCamera(Transform camera)
        {
            cameraMain = camera;
        }

        public void SetTarget(Transform target)
        {
            this.target = target;
        }


        //controls the movement of the player (i.e. moves the Controller/Rigid Body, no navmesh)
        public float MoveController(float horizontal, float vertical)
        {
            float moveAmount = 0;
            
            HandleAirMovement();

            if (target)
            {
                Vector3 dir = target.position - transform.position;
                dir.Normalize();
                dir.y = 0;
                transform.rotation = Quaternion.LookRotation(dir);
            }

            if (PlayerCanMove())
            {
                moveAmount = PlayerMovement(horizontal, vertical);

                PlayMoveAnimation(moveAmount);
            }

            return moveAmount;
        }

        private void HandleAirMovement()
        {
            //Vector3 moveAhead = (transform.position - moveDirection) * 0.3f;
            //Vector3 forward = transform.TransformDirection(Vector3.down) * 1;
            //Debug.DrawRay(moveAhead, forward, Color.green, 0.1f, false);

            //ignore layer 8 and 11?
            int ignoreForGroundCheck = ~(1 << 8 | 1 << 11);
            //RaycastHit hitInfo;
            
            bool nowInAir = true; ;
            if (Physics.Raycast(transform.position, Vector3.down, out hitInfo, 1f, ignoreForGroundCheck))
            {
                //close to the ground, just adjust the player Y postion
                //we are raycasting from somewhere inside the player so this adjustments might just be a bit weird...
                if (hitInfo.distance < maxDistanceToGround && hitInfo.distance > minDistanceToGround)
                {
                    nowInAir = false;
                }
                if(hitInfo.distance < minDistanceToGround)
                {
                    nowInAir = false;
                }
            }

            if (isInAir == false)
            {
                //character just took a leap, set states
                if (nowInAir)
                {
                    animationHandler.PlayAnimation("Fall");
                    animationHandler.SetBool("Falling", true);
                    timeInAir = 0;
                    isInAir = true;
                    animationHandler.SetBool(Constants.ANIMATION_INACTION, true);
                }
            }
            else
            {
                //boy is not flying anymore, pannick
                if (nowInAir == false)
                {
                    animationHandler.SetBool("Falling", false);
                    if (timeInAir > mediumFallTime)
                    {
                        animationHandler.PlayAnimation("Land");
                    }
                    mv = 0;
                    
                    isInAir = false;
                }

                timeInAir += Time.deltaTime;

                //handle gravity - should also add a forward vector to push you over the ledge!!!!
                mv -= -G * Time.deltaTime;
                moveDirection.y = -mv;
                controller.Move(moveDirection * Time.deltaTime*walkSpeed);
            }
        }

        private float PlayerMovement(float horizontal, float vertical)
        {
            float moveAmount;
            Vector3 direction = new Vector3(horizontal, transform.position.y, vertical).normalized;

            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));

            if (moveAmount >= 0.1f)
            {
                moveDirection = GetDirection(direction);

                if (hitInfo.distance > minDistanceToGround)
                {
                    moveDirection += (Vector3.down * hitInfo.distance);
                }

                moveAmount = ClampMoveAmount(moveAmount);
                controller.Move(moveDirection.normalized * moveAmount * walkSpeed * Time.deltaTime);

            }
            else if(hitInfo.distance > minDistanceToGround)
            {
                moveDirection = (Vector3.down * hitInfo.distance);
                controller.Move(moveDirection.normalized * feetAdjustmentSpeed * Time.deltaTime);
            }
            else
            {
                moveAmount = 0;
            }

            return moveAmount;
        }

        private Vector3 GetDirection(Vector3 direction)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraMain.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            transform.rotation = Quaternion.Euler(0, angle, 0);

            moveDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
            return moveDirection;
        }

        private float ClampMoveAmount(float moveAmount)
        {
            //clamp the move amount to remove silly behaviour
            //TODO: values are hardcoded to match animation
            if (moveAmount < 0.6f && moveAmount > 0.3f)
                moveAmount = 0.2f;
            return moveAmount;
        }

        private void PlayMoveAnimation(float moveAmount)
        {
            animationHandler.SetFloat(Constants.ANIMATION_WALKSPEED, moveAmount, animationDampTime, Time.deltaTime);
        }

        //Do not let the player move if he is in combat or in other actions
        private bool PlayerCanMove()
        {
            return !animationHandler.IsInAction();
        }

        //keep this maybe we need in the future
        private int CreateLayerMask(bool aExclude, params int[] aLayers)
        {
            //layerMask = CreateLayerMask(true, 0, 1, 6, 8); // cast against all layers except layers 0, 1, 6, 8
            int v = 0;
            foreach (var L in aLayers)
                v |= 1 << L;
            if (aExclude)
                v = ~v;
            return v;
        }

        #region AnimationEvents
        void FootL() { }
        void FootR() { }
        void Land() { }
        #endregion
    }
}
