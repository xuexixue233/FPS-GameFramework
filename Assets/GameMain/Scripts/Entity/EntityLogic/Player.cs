using System.Collections;
using FPSFramework;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityGameFramework.Runtime;

namespace FPS
{
    public class Player : Soldier
    {
        /// <summary>
        /// 配置数据
        /// </summary>
        [SerializeField] 
        private PlayerData m_PlayerData = null;

        /// <summary>
        /// 外部数据
        /// </summary>
        [SerializeField]
        public PlayerExData m_PlayerExData;

        private CharacterController m_characterController;
        private DefaultInput _defaultInput;
        public Vector2 input_Movement;
        public Vector2 input_View;
        private Vector3 newCameraRotation;
        
        public float viewClampYMin = -70;
        public float viewClampYMax = 80;
        
        public float playerGravity=(float)-9.81;
        private Vector3 movementSpeed;
        private Vector3 newMovementSpeed;
        private Vector3 newMovementSpeedVelocity;
        private Vector3 jumpingForce;
        private Vector3 jumpingForceVelocity;
        public bool isSprinting;

        private float stanceCheckErrorMargin = 0.05f;
        private float cameraHeight;
        private float cameraHeightVelocity;
        private float stanceHeightVelocity;
        private Vector3 stanceCenterVelocity;

        [HideInInspector]
        public float weaponAnimationSpeed;

        [HideInInspector]
        public bool isGrounded;
        [HideInInspector]
        public bool isFalling;
        [HideInInspector]
        public bool isAimingIn;

        public float currentLean;
        public float targetLean;
        public float leanVelocity;
        private bool isLeaningLeft;
        private bool isLeaningRight;

        private bool isFire;


        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            m_PlayerExData = GetComponent<PlayerExData>();
            _defaultInput = new DefaultInput();
            _defaultInput.Character.Movement.performed += e => input_Movement = e.ReadValue<Vector2>();
            _defaultInput.Character.View.performed += e => input_View = e.ReadValue<Vector2>();
            _defaultInput.Character.Jump.performed += e => Jump();

            _defaultInput.Character.Crouch.performed += e => Crouch();
            _defaultInput.Character.Prone.performed += e => Prone();
            _defaultInput.Character.Sprinting.performed += e => ToggleSprint();
            _defaultInput.Character.SprintingReleased.performed += e => StopSprint();
            _defaultInput.Weapon.FireAim.performed += e => AimingIn();

            _defaultInput.Character.LeanLeftPressed.performed += e => { isLeaningLeft = true;};
            _defaultInput.Character.LeanLeftReleased.performed += e => { isLeaningLeft = false;};
            _defaultInput.Character.LeanRightPressed.performed += e => { isLeaningRight = true;};
            _defaultInput.Character.LeanRightReleased.performed += e => { isLeaningRight = false;};

            _defaultInput.Weapon.FirePressed.performed += e => FireStart();
            _defaultInput.Weapon.FireReleased.performed += e=> FireStop();
            
            _defaultInput.Enable();
            
            newCameraRotation = m_PlayerExData.cameraHolder.localRotation.eulerAngles;
            newSoldierRotation = m_PlayerExData.soldierTransform.localRotation.eulerAngles;
            m_characterController = GetComponentInChildren<CharacterController>();

            cameraHeight = m_PlayerExData.cameraHolder.localPosition.y;
            gameObject.SetLayerRecursively(Constant.Layer.PlayerLayerId);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            m_PlayerData = userData as PlayerData;
            if (m_PlayerData == null)
            {
                Log.Error("m_PlayerData is invalid.");
                return;
            }

            m_PlayerData.HP = 100;
            
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            SetIsGrounded();
            SetIsFalling();
            
            CalculateView();
            CalculateMovement();
            CalculateJump();
            CalculateStance();
            CalculateLeaning();
        }

        protected override void OnAttached(EntityLogic childEntity, Transform parentTransform, object userData)
        {
            base.OnAttached(childEntity, parentTransform, userData);
            if (childEntity is Weapon entity)
            {
                entity.Initialise(this);
            }
        }

        private void AimingIn()
        {
            isAimingIn = !isAimingIn;
            if (showedWeapon==null)
            {
                return;
            }

            showedWeapon.isAimingIn = isAimingIn;
        }

        private void FireStart()
        {
            isFire = true;
            if (showedWeapon==null)
            {
                return;
            }
            showedWeapon.isFire = isFire;
        }
        
        private void FireStop()
        {
            isFire = false;
            if (showedWeapon==null)
            {
                return;
            }
            showedWeapon.isFire = isFire;
        }

        private void CalculateView()
        {
            newSoldierRotation.y += m_PlayerExData.playerSetting.ViewXSensitivity * (isAimingIn ? m_PlayerExData.playerSetting.AimingSpeedEffector : 1)*
                                    (m_PlayerExData.playerSetting.ViewXInverted ? -input_View.x:input_View.x) * Time.deltaTime;
            m_PlayerExData.soldierTransform.rotation = Quaternion.Euler(newSoldierRotation);
            
            newCameraRotation.x += m_PlayerExData.playerSetting.ViewYSensitivity * (isAimingIn ? m_PlayerExData.playerSetting.AimingSpeedEffector : 1)*
                                   (m_PlayerExData.playerSetting.ViewYInverted ? input_View.y:-input_View.y) * Time.deltaTime;
            newCameraRotation.x = Mathf.Clamp(newCameraRotation.x, viewClampYMin, viewClampYMax);

            m_PlayerExData.cameraHolder.localRotation = Quaternion.Euler(newCameraRotation);
        }

        private void CalculateMovement()
        {
            
            if (input_Movement.y<=0.2f)
            {
                isSprinting = false;
            }
            
            var verticalSpeed = m_PlayerData.WalkingForwardSpeed;
            var horizontalSpeed = m_PlayerData.WalkingStrafeSpeed;

            if (isSprinting)
            {
                verticalSpeed = m_PlayerData.RunningForwardSpeed;
                horizontalSpeed = m_PlayerData.RunningStrafeSpeed;
            }

            if (!isGrounded)
            {
                m_PlayerExData.playerSetting.SpeedEffector = m_PlayerExData.playerSetting.FallingSpeedEffector;
            }
            else if (m_PlayerExData.playerStance==Scr_Models.PlayerStance.Crouch)
            {
                m_PlayerExData.playerSetting.SpeedEffector = m_PlayerExData.playerSetting.CrouchSpeedEffector;
            }
            else if(m_PlayerExData.playerStance==Scr_Models.PlayerStance.Prone)
            {
                m_PlayerExData.playerSetting.SpeedEffector = m_PlayerExData.playerSetting.ProneSpeedEffector;
            }
            else if (isAimingIn)
            {
                m_PlayerExData.playerSetting.SpeedEffector = m_PlayerExData.playerSetting.AimingSpeedEffector;
            }
            else
            {
                m_PlayerExData.playerSetting.SpeedEffector = 1;
            }

            weaponAnimationSpeed = m_characterController.velocity.magnitude / (m_PlayerData.WalkingForwardSpeed*m_PlayerExData.playerSetting.SpeedEffector);
            
            if (weaponAnimationSpeed > 1)
            {
                weaponAnimationSpeed = 1;
            }
            
            verticalSpeed *= m_PlayerExData.playerSetting.SpeedEffector;
            horizontalSpeed *= m_PlayerExData.playerSetting.SpeedEffector;

            newMovementSpeed = Vector3.SmoothDamp(newMovementSpeed,
                new Vector3(horizontalSpeed * input_Movement.x, movementSpeed.y, verticalSpeed * input_Movement.y),
                ref newMovementSpeedVelocity, isGrounded? m_PlayerExData.playerSetting.MovementSmoothing : m_PlayerExData.playerSetting.FallingSmoothing);

            movementSpeed = m_characterController.transform.TransformDirection(newMovementSpeed);
            

            if (isGrounded && movementSpeed.y < 0)
            {
                movementSpeed.y = 0f;
            }
            
            movementSpeed.y += playerGravity * Time.deltaTime;
            
            movementSpeed += jumpingForce;

            m_characterController.Move(movementSpeed*Time.deltaTime);
        }

        private void CalculateLeaning()
        {
            if (isLeaningLeft)
            {
                targetLean = m_PlayerExData.leanAngle;
            }
            else if (isLeaningRight)
            {
                targetLean = -m_PlayerExData.leanAngle;
            }
            else
            {
                targetLean = 0;
            }
            currentLean = Mathf.SmoothDamp(currentLean, targetLean, ref leanVelocity, m_PlayerExData.leanSmoothing);
            
            m_PlayerExData.leanPivot.localRotation=Quaternion.Euler(new Vector3(0,0,currentLean));
        }

        private void CalculateJump()
        {
            jumpingForce = Vector3.SmoothDamp(jumpingForce, Vector3.zero, ref jumpingForceVelocity,
                m_PlayerExData.playerSetting.JumpingFalloff);
        }

        private void CalculateStance()
        {
            var stance = m_PlayerExData.standStance;
            if (m_PlayerExData.playerStance==Scr_Models.PlayerStance.Crouch)
            {
                stance = m_PlayerExData.crouchStance;
            }
            else if (m_PlayerExData.playerStance==Scr_Models.PlayerStance.Prone)
            {
                stance = m_PlayerExData.proneStance;
            }


            var localPosition = m_PlayerExData.cameraHolder.localPosition;
            cameraHeight = Mathf.SmoothDamp(localPosition.y, stance.CameraHeight,
                ref cameraHeightVelocity, m_PlayerExData.playerStanceSmoothing);
            localPosition = new Vector3(localPosition.x, cameraHeight, localPosition.z);
            m_PlayerExData.cameraHolder.localPosition = localPosition;

            m_characterController.height = Mathf.SmoothDamp(m_characterController.height, stance.CharacterHeight,
                ref stanceHeightVelocity, m_PlayerExData.playerStanceSmoothing);
            m_characterController.center = Vector3.SmoothDamp(m_characterController.center, stance.CharacterCenter,
                ref stanceCenterVelocity, m_PlayerExData.playerStanceSmoothing);
        }

        private void Jump()
        {
            if (!isGrounded)
            {
                return;
            }

            if (m_PlayerExData.playerStance is Scr_Models.PlayerStance.Crouch or Scr_Models.PlayerStance.Prone)
            {
                if (StanceCheck(m_PlayerExData.standStance.CharacterHeight))
                {
                    return;
                }
                m_PlayerExData.playerStance = Scr_Models.PlayerStance.Stand;
                return;
            }
            

            jumpingForce = Vector3.up*m_PlayerExData.playerSetting.JumpingHeight;
            showedWeapon.TriggerJump();
        }

        private void SetIsGrounded()
        {
            isGrounded = Physics.CheckSphere(m_PlayerExData.feetTransform.position,
                m_PlayerExData.playerSetting.isGroundedRadius, m_PlayerExData.groundMask);
        }

        private void SetIsFalling()
        {
            isFalling = !isGrounded &&
                        m_characterController.velocity.magnitude >= m_PlayerExData.playerSetting.isFallingSpeed;
        }

        private void Crouch()
        {
            if (m_PlayerExData.playerStance==Scr_Models.PlayerStance.Crouch)
            {
                if (StanceCheck(m_PlayerExData.standStance.CharacterHeight))
                {
                    return;
                }
                m_PlayerExData.playerStance = Scr_Models.PlayerStance.Stand;
                return;
            }

            if (StanceCheck(m_PlayerExData.crouchStance.CharacterHeight))
            {
                return;
            }
            
            m_PlayerExData.playerStance = Scr_Models.PlayerStance.Crouch;
            
        }

        private void Prone()
        {
            if (m_PlayerExData.playerStance==Scr_Models.PlayerStance.Prone)
            {
                if (StanceCheck(m_PlayerExData.standStance.CharacterHeight))
                {
                    return;
                }
                m_PlayerExData.playerStance = Scr_Models.PlayerStance.Stand;
                return;
            }
            m_PlayerExData.playerStance = Scr_Models.PlayerStance.Prone;
        }

        private bool StanceCheck(float stanceCheckHeight)
        {
            var feetPosition = m_PlayerExData.feetTransform.position;
            var radius = m_characterController.radius;
            var start = new Vector3(feetPosition.x,feetPosition.y+radius+stanceCheckErrorMargin,feetPosition.z);
            var end = new Vector3(feetPosition.x,feetPosition.y-radius-stanceCheckErrorMargin+stanceCheckHeight,feetPosition.z);
            
            return Physics.CheckCapsule(start,end,radius,m_PlayerExData.playerMask);
        }

        private void ToggleSprint()
        {
            if (input_Movement.y<=0.2f)
            {
                isSprinting = false;
                return;
            }
            isSprinting = !isSprinting;
        }
        private void StopSprint()
        {
            if (m_PlayerExData.playerSetting.SprintingHold)
            {
                isSprinting = false;
            }
        }
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(m_PlayerExData.feetTransform.position,m_PlayerExData.playerSetting.isGroundedRadius);
        }
    }
}