using UnityEngine;

namespace Invector.vCharacterController
{
    public class vThirdPersonInput : MonoBehaviour
    {
        #region Variables       

        [Header("Controller Input")]
        public string horizontalInput = "Horizontal";
        public string verticallInput = "Vertical";
        public KeyCode jumpInput = KeyCode.Space;
        //public KeyCode strafeInput = KeyCode.Tab;
        public KeyCode strafeInput = KeyCode.Tab;
        public KeyCode attackInput = KeyCode.Mouse0;
        public KeyCode sprintInput = KeyCode.LeftShift;
        public KeyCode dashInput = KeyCode.Mouse1;
        [Header("Camera Input")]
        public string rotateCameraXInput = "Mouse X";
        public string rotateCameraYInput = "Mouse Y";

        [HideInInspector] public vThirdPersonController cc;
        [HideInInspector] public vThirdPersonCamera tpCamera;
        [HideInInspector] public Camera cameraMain;

        #endregion

        protected virtual void Start()
        {
            Cursor.lockState= CursorLockMode.Locked;
            InitilizeController();
            InitializeTpCamera();
        }

        protected virtual void FixedUpdate()
        {
            cc.UpdateMotor();               // updates the ThirdPersonMotor methods
            cc.ControlLocomotionType();     // handle the controller locomotion type and movespeed
            cc.ControlRotationType();       // handle the controller rotation type
        }

        protected virtual void Update()
        {
            InputHandle();                  // update the input methods
            cc.UpdateAnimator();            // updates the Animator Parameters
        }

        public virtual void OnAnimatorMove()
        {
            cc.ControlAnimatorRootMotion(); // handle root motion animations 
        }

        #region Basic Locomotion Inputs

        protected virtual void InitilizeController()
        {
            cc = GetComponent<vThirdPersonController>();

            if (cc != null)
                cc.Init();
        }

        protected virtual void InitializeTpCamera()
        {
            if (tpCamera == null)
            {
                tpCamera = FindObjectOfType<vThirdPersonCamera>();
                if (tpCamera == null)
                    return;
                if (tpCamera)
                {
                    tpCamera.SetMainTarget(this.transform);
                    tpCamera.Init();
                }
            }
        }

        protected virtual void InputHandle()
        {
            MoveInput();
            CameraInput();
            SprintInput();
            StrafeInput();
            JumpInput();
            AttackInput();
            DashInput();
            //ZoomInput();
        }

        public virtual void MoveInput()
        {
            cc.input.x = Input.GetAxis(horizontalInput);
            cc.input.z = Input.GetAxis(verticallInput);
        }

        protected virtual void CameraInput()
        {
            if (!cameraMain)
            {
                if (!Camera.main) Debug.Log("Missing a Camera with the tag MainCamera, please add one.");
                else
                {
                    cameraMain = Camera.main;
                    cc.rotateTarget = cameraMain.transform;
                }
            }

            if (cameraMain)
            {
                cc.UpdateMoveDirection(cameraMain.transform);
            }

            if (tpCamera == null)
                return;

            var Y = Input.GetAxis(rotateCameraYInput);
            var X = Input.GetAxis(rotateCameraXInput);

            tpCamera.RotateCamera(X, Y);
        }

        protected virtual void StrafeInput()
        {
            if (Input.GetKeyDown(strafeInput))
            {
                cc.Strafe();
            }
            else if (Input.GetKeyUp(strafeInput))
            {
                cc.Strafe();
            }
        }
        // protected virtual void ZoomInput()
        // {
        //     if (Input.GetMouseButton(1))
        //     {
        //         //cc.vcam.m_Lens.FieldOfView = Mathf.Lerp(cc.vcam.m_Lens.FieldOfView, cc.zoom, cc.zoom * Time.deltaTime);
        //         Cursor.visible = true;
        //         Cursor.SetCursor(UIManager.instance.aimTexture, Vector2.zero, CursorMode.Auto);
        //     }
        //     else
        //     {
        //         //cc.vcam.m_Lens.FieldOfView = Mathf.Lerp(cc.vcam.m_Lens.FieldOfView, 60f, cc.zoom * Time.deltaTime);
        //         Cursor.visible = false;
        //     }
        // }
        protected virtual void SprintInput()
        {
            if (Input.GetKeyDown(sprintInput))
                cc.Sprint(true);
            else if (Input.GetKeyUp(sprintInput))
                cc.Sprint(false);
        }

        /// <summary>
        /// Conditions to trigger the Jump animation & behavior
        /// </summary>
        /// <returns></returns>
        protected virtual bool JumpConditions()
        {
            return cc.isGrounded && cc.GroundAngle() < cc.slopeLimit && !cc.isJumping && !cc.stopMove;
        }

        /// <summary>
        /// Input to trigger the Jump 
        /// </summary>
        protected virtual void JumpInput()
        {
            if (Input.GetKeyDown(jumpInput) && JumpConditions())
                cc.Jump();
        }

        #endregion       

        protected virtual bool AttackCondition()
        {
            return cc.isGrounded && !cc.isJumping && !cc.stopMove;
        }
        protected virtual void AttackInput()
        {
            cc.attackTimer += Time.deltaTime;
            
            if (Input.GetKey(attackInput) && cc.attackTimer >= cc.attackCoolDown)
            {
                cc.attackTimer = 0;
                StartCoroutine(cc.Attack());
            }
        }

        protected virtual void DashInput()
        {
            cc.dashTimer += Time.deltaTime;

            if (Input.GetKeyDown(dashInput) && cc.dashTimer >= cc.dashCoolDown)
            {
                cc.dashTimer = 0;
                StartCoroutine(cc.Dash());
            }
        }
    }
}