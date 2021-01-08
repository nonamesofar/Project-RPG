using Cinemachine;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RPG.Controller
{
    public class PlayerController : MonoBehaviour
    {
        public GameObject sceneCamera = null;
        public Transform lockOnPosition = null;
        
        #region Input Manager Variables
        PlayerControls keys;

        //Triggers & bumpers
        bool Rb, Rt, Lb, Lt, isAttacking, b_Input, y_Input, x_Input, inventoryInput,
        leftArrow, rightArrow, upArrow, downArrow, lockInput;

        ButtonStatus rb, rt, lb, lt;

        float vertical;
        float horizontal;
        float moveAmount;
        float mouseX;
        float mouseY;
        bool rollFlag;
        float rollTimer;

        Vector2 moveDirection;
        Vector2 cameraDirection;

        public float Vertical { get => vertical; set => vertical = value; }
        public float Horizontal { get => horizontal; set => horizontal = value; }
        public Vector2 MoveDirection { get => moveDirection; set => moveDirection = value; }
        public Vector2 CameraDirection { get => cameraDirection; set => cameraDirection = value; }
        #endregion

        private Transform camera;       
        private PlayerMovement controller;
        private Fighter fighter;
        private AnimationHandler animationHandler;

        private ILockable currentLockable;
        private CinemachineFreeLook cmFreeLook = null;
        private Transform lockTarget = null;

        private void Start()
        {
            //new input manager
            keys = new PlayerControls();
            keys.Player.Movement.performed += i => MoveDirection = i.ReadValue<Vector2>();
            keys.Player.Camera.performed += i => CameraDirection = i.ReadValue<Vector2>();
            keys.Player.Lock.started += i => lockInput = true;
            keys.Enable();

            controller = GetComponent<PlayerMovement>();
            controller.Init();
            camera = Camera.main.transform;
            fighter = GetComponent<Fighter>();
            cmFreeLook = sceneCamera.GetComponentInChildren<CinemachineFreeLook>();
            animationHandler = GetComponent<AnimationHandler>();
        }

        private void OnDisable()
        {
            keys.Disable();
        }

        private void Update()
        {
            if (controller == null)
                return;

            float delta = Time.deltaTime;

            HandleInput();

            if (b_Input)
            {
                rollFlag = true;
                rollTimer += delta;
            }
        }

        private void FixedUpdate()
        {
            if (controller == null)
                return;

            float delta = Time.fixedDeltaTime;

            HandleMovement(delta);

            if (lockTarget != null)
            {
                CameraFollowTarget(Time.fixedDeltaTime);
            }
        }

        void HandleInput()
        {
            bool retVal = false;
            isAttacking = false;

            vertical = moveDirection.y;
            horizontal = moveDirection.x;
            mouseX = cameraDirection.x;
            mouseY = cameraDirection.y;

            rb = GetButtonStatus(keys.Player.RB);
            rt = GetButtonStatus(keys.Player.RT);
            lb = GetButtonStatus(keys.Player.LB);
            lt = GetButtonStatus(keys.Player.LT);
            b_Input = GetButtonStatus(keys.Player.Roll.phase);
            x_Input = GetButtonStatus(keys.Player.Consume.phase);

            moveAmount = Mathf.Clamp01(Mathf.Abs(vertical) + Mathf.Abs(horizontal));

            if (!controller.isInteracting)
            {
                if (retVal == false)
                    retVal = HandleRolls();
            }

            if (retVal == false)
            {
                retVal = fighter.AttackBehaviour(rb, lb, rt, lt);
                controller.isInteracting = true;
            }

            if (lockInput)
            {
                lockInput = false;

                if (controller.lockOn)
                {
                    DisableLockOn();
                }
                else
                {
                    currentLockable = fighter.FindLocakbleTarget();
                    
                    if (currentLockable != null)
                    {
                        print("lock");
                        lockTarget = currentLockable.GetLockOnTarget(controller.mTransform);
                    }
                    
                    if (lockTarget != null)
                    {
                        animationHandler.SetBool("OnLock", true);
                        SetLockOnCamera(true);
                        controller.lockOn = true;
                        controller.currentLockTarget = lockTarget;
                    }
                    else
                    {
                        SetLockOnCamera(false);
                        controller.lockOn = false;
                    }
                }
            }

            if (controller.lockOn)
            {
                if (!currentLockable.IsAlive())
                {
                    DisableLockOn();
                }
            }

        }

        #region Lock On Target
        void DisableLockOn()
        {
            SetLockOnCamera(false);
            controller.lockOn = false;
            controller.currentLockTarget = null;
            currentLockable = null;
            animationHandler.SetBool("OnLock", false);
        }

        private void SetLockOnCamera(bool isLockOn)
        {
            cmFreeLook.enabled = !isLockOn;
            if (lockTarget == null)
            {

            }
            else
            {
                Vector3 dir = lockTarget.position - transform.position;
                dir.Normalize();
                dir.y = 0;

                Quaternion targetRotation = Quaternion.LookRotation(dir);
                transform.rotation = targetRotation;

                dir = lockTarget.position - camera.position;
                dir.Normalize();

                targetRotation = Quaternion.LookRotation(dir);
                Vector3 e = targetRotation.eulerAngles;
                e.y = 0;
                camera.localEulerAngles = e;

                //pivotAngle = pivot.localEulerAngles.x;
                //lookAngle = mTransform.eulerAngles.y;
            }
        }
        #endregion

        bool HandleRolls()
        {
            controller.isSprinting = false;

            if (b_Input == false && rollFlag)
            {
                rollFlag = false;

                if (rollTimer < .5f)
                {
                    if (moveAmount > 0)//rollTimer > 0.5f ||
                    {
                        //Vector3 movementDirection = camera.right * horizontal;
                        //movementDirection += camera.forward * vertical;
                        //movementDirection.Normalize();
                        //movementDirection.y = 0;

                        //Quaternion dir = Quaternion.LookRotation(movementDirection);
                        //controller.transform.rotation = dir;
                        controller.isRolling = true;
                        controller.isInteracting = true;
                        controller.HandleRolls(horizontal, vertical);
                        return true;

                    }
                    else
                    {
                        controller.PlayTargetAnimation("Step Back", true, false);
                    }
                }
            }
            //else if (rollFlag)
            //{
            //    if (moveAmount > 0.5f)
            //    {
            //        controller.isSprinting = true;
            //        //you can start to run if you are locked
            //        if (controller.lockOn)
            //        {
            //            DisableLockOn();
            //        }
            //    }
            //}

            if (b_Input == false)
            {
                rollTimer = 0;
            }

            return false;
        }

        void HandleMovement(float delta)
        {
            Vector3 movementDirection = camera.right * horizontal;
            movementDirection += camera.forward * vertical;
            movementDirection.Normalize();

            controller.MoveCharacter(vertical, horizontal, movementDirection, delta);

        }

        bool GetButtonStatus(InputActionPhase phase)
        {
            return phase == InputActionPhase.Started;
        }

        ButtonStatus GetButtonStatus(InputAction button)
        {
            if (InputActionButtonExtensions.GetButtonDown(button))
                return ButtonStatus.ButtonDown;
            if (InputActionButtonExtensions.GetButtonUp(button))
                return ButtonStatus.ButtonUp;
            if (InputActionButtonExtensions.GetButton(button))
                return ButtonStatus.ButtonHold;
            return ButtonStatus.NONE;
        }

        public float followSpeed = 0.1f;
        public void CameraFollowTarget(float delta)
        {
            Vector3 targetPosition = Vector3.Lerp(camera.position, lockOnPosition.position, delta / followSpeed);
            
            camera.position = targetPosition;
            camera.LookAt(lockTarget);
            //HandleCollisions(delta);
        }
    }
}