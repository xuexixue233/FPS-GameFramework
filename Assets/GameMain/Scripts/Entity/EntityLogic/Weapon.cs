using System.Collections.Generic;
using FPSFramework;
using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace FPS
{
    public class Weapon : Entity
    {
        private const string AttachPoint = "Weapon Point";

        [SerializeField]
        public WeaponData m_WeaponData = null;
        
        public WeaponExData m_WeaponExData;
        public WeaponAttribute weaponAttribute;
        
        public Soldier soldier;
        public Player player;
        private bool isPlayerWeapon;

        public Animator weaponAnimator;
        public int bulletNum;
        public int leftBulletNum;

        private Vector3 newWeaponRotation;
        private Vector3 newWeaponRotationVelocity;

        private Vector3 targetWeaponRotation;
        private Vector3 targetWeaponRotationVelocity;

        private Vector3 newWeaponMovementRotation;
        private Vector3 newWeaponMovementRotationVelocity;
        
        private Vector3 targetWeaponMovementRotation;
        private Vector3 targetWeaponMovementRotationVelocity;
        private static readonly int IsSprinting = Animator.StringToHash("IsSprinting");

        private bool isGroundTrigger;
        private bool isFallingTrigger;

        private float fallingDelay;
        private static readonly int Jump = Animator.StringToHash("Jump");
        private static readonly int Land = Animator.StringToHash("Land");
        private static readonly int Falling = Animator.StringToHash("Falling");
        private static readonly int WeaponAnimationSpeed = Animator.StringToHash("WeaponAnimationSpeed");
        private static readonly int IsWalking = Animator.StringToHash("IsWalking");

        private float swayTime;
        public Vector3 swayPosition;
        
        private Vector3 weaponSwayPosition;
        private Vector3 weaponSwayPositionVelocity;
        [HideInInspector] 
        public bool isAimingIn;
        [HideInInspector] 
        public bool isFire;

        private Vector3 currentRotation;
        private Vector3 targetRortation;
        private Vector3 targetPosition;
        private Vector3 currentPosition;
        private Vector3 initialGunPosition;

        [SerializeField]
        public Dictionary<Mod, WeaponMod> weaponMods = new Dictionary<Mod, WeaponMod>();

        public int currentBullets=30;
        public int currentMaxBullets=30;
        private float fireTimer;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            m_WeaponExData = GetComponent<WeaponExData>();
            weaponAnimator = GetComponentInChildren<Animator>();

            newWeaponRotation = transform.localRotation.eulerAngles;
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            m_WeaponData = userData as WeaponData;
            if (m_WeaponData == null)
            {
                Log.Error("Weapon data is invalid.");
                return;
            }

            weaponAttribute = new WeaponAttribute();
            weaponAttribute.Init(m_WeaponData);
            soldier = GameEntry.Entity.GetGameEntity(m_WeaponData.OwnerId) as Soldier;

            if (soldier == null)
            {
                return;
            }

            GameEntry.Entity.AttachEntity(Entity, m_WeaponData.OwnerId, soldier.soldierExData.WeaponTransform);
            soldier.showedWeapon = this;
            if (m_WeaponData.weaponModId!=null)
            {
                foreach (var modId in m_WeaponData.weaponModId)
                {
                    GameEntry.Entity.ShowWeaponMod(new WeaponModData(GameEntry.Entity.GenerateSerialId(),modId.Value,m_WeaponData.Id,CampType.Player));
                }
            }
            
            transform.transform.localPosition = m_WeaponExData.CameraTransform.localPosition * -1;
            transform.localScale = Vector3.one;
            initialGunPosition = m_WeaponExData.recoilTransform.localPosition;
            
        }
        
        

        public void Initialise(Player m_player)
        {
            player = m_player;
            isPlayerWeapon = true;
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            if (!isPlayerWeapon)
            {
                return;
            }

            CalculateWeaponRotation();
            SetWeaponAnimations();
            CalculateWeaponSway();
            CalculateAimingIn();
            if (currentBullets>0 && isFire)
            {
                WeaponFire();
            }

            if (fireTimer<60.0f/m_WeaponData.FiringRate)
            {
                fireTimer += Time.deltaTime;
            }
        }
        
        

        private void WeaponFire()
        {
            if (fireTimer<60.0f/m_WeaponData.FiringRate||currentBullets<=0)
            {
                return;
            }


            var position = m_WeaponExData.shootPoint.position;
            if (Physics.Raycast(position,m_WeaponExData.shootPoint.up*-1,out RaycastHit hit,m_WeaponData.EffectiveFiringRange))
            {
                
            }
            
        }

        protected override void OnAttached(EntityLogic childEntity, Transform parentTransform, object userData)
        {
            base.OnAttached(childEntity, parentTransform, userData);
            if (childEntity is WeaponMod entity)
            {
                weaponMods.Add(entity.weaponModData.ModType,entity);
                foreach (var next in entity.modExData.nextModsTransforms)
                {
                    m_WeaponExData.nextModsTransforms.Add(next);
                }

                weaponAttribute.AddModToRefresh(entity);
                return;
            }
        }

        protected override void OnDetached(EntityLogic childEntity, object userData)
        {
            base.OnDetached(childEntity, userData);
            if (childEntity is WeaponMod entity)
            {
                weaponAttribute.RemoveToRefresh(entity);
                foreach (var next in entity.modExData.nextModsTransforms)
                {
                    m_WeaponExData.nextModsTransforms.Remove(next);
                }
                weaponMods.Remove(entity.weaponModData.ModType);
                return;
            }
        }
        

        private void CalculateAimingIn()
        {
            var targetPosition = transform.position;
            if (isAimingIn)
            {
                var cameraHolder = player.m_PlayerExData.cameraTransform.transform;
                targetPosition = cameraHolder.position +
                                 (m_WeaponExData.weaponSwayTransform.position - m_WeaponExData.sightTarget.position) +
                                 (cameraHolder.forward * m_WeaponExData.sightOffset);
            }

            weaponSwayPosition = m_WeaponExData.weaponSwayTransform.position;
            weaponSwayPosition = Vector3.SmoothDamp(weaponSwayPosition, targetPosition, ref weaponSwayPositionVelocity,
                m_WeaponExData.aimingInTime);
            m_WeaponExData.weaponSwayTransform.position = weaponSwayPosition+swayPosition;
        }
        

        public void TriggerJump()
        {
            isGroundTrigger = false;
            weaponAnimator.SetTrigger(Jump);
        }

        protected override void OnAttachTo(EntityLogic parentEntity, Transform parentTransform, object userData)
        {
            base.OnAttachTo(parentEntity, parentTransform, userData);

            Name = Utility.Text.Format("Weapon of {0}", parentEntity.Name);
            CachedTransform.localPosition = Vector3.zero;
        }
        
        

        private void CalculateWeaponRotation()
        {
            targetWeaponRotation.y += (isAimingIn? m_WeaponExData.settings.SwayAmount/2 :m_WeaponExData.settings.SwayAmount) *
                                      (m_WeaponExData.settings.SwayXInverted
                                          ? -player.input_View.x
                                          : player.input_View.x) * Time.deltaTime;
            targetWeaponRotation.x += (isAimingIn? m_WeaponExData.settings.SwayAmount/2 :m_WeaponExData.settings.SwayAmount) *
                                      (m_WeaponExData.settings.SwayYInverted
                                          ? player.input_View.y
                                          : -player.input_View.y) * Time.deltaTime;

            targetWeaponRotation.x = Mathf.Clamp(targetWeaponRotation.x, -m_WeaponExData.settings.SwayClampX,
                m_WeaponExData.settings.SwayClampX);
            targetWeaponRotation.y = Mathf.Clamp(targetWeaponRotation.y, -m_WeaponExData.settings.SwayClampY,
                m_WeaponExData.settings.SwayClampY);
            targetWeaponRotation.z = isAimingIn ? 0 : targetWeaponRotation.y;

            targetWeaponRotation = Vector3.SmoothDamp(targetWeaponRotation, Vector3.zero,
                ref targetWeaponRotationVelocity, m_WeaponExData.settings.SwayResetSmoothing);

            newWeaponRotation = Vector3.SmoothDamp(newWeaponRotation, targetWeaponRotation,
                ref newWeaponRotationVelocity, m_WeaponExData.settings.SwaySmoothing);

            targetWeaponMovementRotation.z = (isAimingIn? m_WeaponExData.settings.MovementSwayX/3 :m_WeaponExData.settings.MovementSwayX) *
                                             (m_WeaponExData.settings.MovementSwayXInverted
                                                 ? -player.input_Movement.x
                                                 : player.input_Movement.x);
            targetWeaponMovementRotation.x = (isAimingIn? m_WeaponExData.settings.MovementSwayY/3 :m_WeaponExData.settings.MovementSwayY) *
                                             (m_WeaponExData.settings.MovementSwayYInverted
                                                 ? -player.input_Movement.y
                                                 : player.input_Movement.y);

            targetWeaponMovementRotation = Vector3.SmoothDamp(targetWeaponMovementRotation, Vector3.zero,
                ref targetWeaponRotationVelocity, m_WeaponExData.settings.MovementSwaySmoothing);
            
            newWeaponMovementRotation = Vector3.SmoothDamp(newWeaponMovementRotation, targetWeaponMovementRotation,
                ref newWeaponRotationVelocity, m_WeaponExData.settings.MovementSwaySmoothing);

            targetRortation = Vector3.Lerp(targetRortation, Vector3.zero, Time.deltaTime * m_WeaponExData.returnAmount);
            currentRotation = Vector3.Slerp(currentRotation, targetRortation, Time.fixedDeltaTime * m_WeaponExData.snappiness);

            transform.localRotation = Quaternion.Euler(newWeaponRotation);
            m_WeaponExData.recoilTransform.localRotation= Quaternion.Euler(currentRotation);
            if (isFire)
            {
                Recoil();
            }

            Back();
        }

        private void SetWeaponAnimations()
        {
            if (isGroundTrigger)
            {
                fallingDelay = 0;
            }
            else
            {
                fallingDelay += Time.deltaTime;
            }

            if (player.isGrounded && !isGroundTrigger && fallingDelay > 0.1f)
            {
                weaponAnimator.SetTrigger(Land);
                isGroundTrigger = true;
            }
            else if (!player.isGrounded && isGroundTrigger)
            {
                weaponAnimator.SetTrigger(Falling);
                isGroundTrigger = false;
            }
            
            weaponAnimator.SetBool(IsWalking,player.isWalking);
            weaponAnimator.SetBool(IsSprinting,player.isSprinting);
            weaponAnimator.SetFloat(WeaponAnimationSpeed,player.weaponAnimationSpeed);
        }

        private void CalculateWeaponSway()
        {
            var targetPosition = LissajousCurve(swayTime, m_WeaponExData.swayAmountA, m_WeaponExData.swayAmountB)/
                                 (isAimingIn? m_WeaponExData.swayScale*2 :m_WeaponExData.swayScale);

            swayPosition = Vector3.Lerp(swayPosition, targetPosition, Time.smoothDeltaTime * m_WeaponExData.swayLerpSpeed);

            swayTime += Time.deltaTime;
            
            if (swayTime > 6.3f)
            {
                swayTime = 0;
            }

            //m_WeaponExData.weaponSwayTransform.localPosition = swayPosition;
        }

        private Vector3 LissajousCurve(float Time, float A, float B)
        {
            return new Vector3(Mathf.Sin(Time), A * Mathf.Sin(B * Time + Mathf.PI));
        }

        public void Recoil()
        {
            targetPosition -= new Vector3(0, 0, m_WeaponExData.kickBackZ);
            targetRortation += new Vector3(m_WeaponExData.recoilX, Random.Range(-m_WeaponExData.recoilY, m_WeaponExData.recoilY), Random.Range(-m_WeaponExData.recoilZ, m_WeaponExData.recoilZ));
        }

        private void Back()
        {
            targetPosition = Vector3.Lerp(targetPosition, initialGunPosition, Time.deltaTime * m_WeaponExData.returnAmount);
            currentPosition = Vector3.Lerp(currentPosition, targetPosition, Time.fixedDeltaTime * m_WeaponExData.snappiness);
            m_WeaponExData.recoilTransform.localPosition= currentPosition;
        }
        
        
    }
}