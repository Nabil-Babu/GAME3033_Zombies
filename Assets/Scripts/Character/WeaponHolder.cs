using Parent;
using Character.UI;
using UnityEngine.InputSystem;
using UnityEngine;
using Weapons;

namespace Character
{
    public class WeaponHolder : MonoBehaviour
    {
         [Header("Weapon To Spawn"), SerializeField]
        private GameObject weaponToSpawn;

        [SerializeField] private Transform weaponSocketLocation;

        private Transform _gripIKLocation;
        private bool _wasFiring = false;
        private bool _firingPressed = false;
        
        //Components
        public PlayerController Controller => _playerController;
        private PlayerController _playerController;
        
        private CrosshairScript _playerCrosshair;
        private Animator _playerAnimator;
        
        //Ref
        private Camera ViewCamera;
        private WeaponComponent EquippedWeapon;
        
        private static readonly int AimHorizontalHash = Animator.StringToHash("AimHorizontal");
        private static readonly int AimVerticalHash = Animator.StringToHash("AimVertical");
        private static readonly int IsFiringHash = Animator.StringToHash("IsFiring");
        private static readonly int IsReloadingHash = Animator.StringToHash("IsReloading");
        private static readonly int WeaponTypeHash = Animator.StringToHash("WeaponType");


        private new void Awake()
        {
            //base.Awake();
            
            _playerAnimator = GetComponent<Animator>();
            _playerController = GetComponent<PlayerController>();
            if (_playerController)
            {
                _playerCrosshair = _playerController.CrossHair;
            }
            
            ViewCamera = Camera.main;
        }

        // Start is called before the first frame update
        void Start()
        {
            GameObject spawnedWeapon = Instantiate(weaponToSpawn, weaponSocketLocation.position, weaponSocketLocation.rotation, weaponSocketLocation);
            if (!spawnedWeapon) return;
            
            EquippedWeapon = spawnedWeapon.GetComponent<WeaponComponent>();
            if (!EquippedWeapon) return;
            
            EquippedWeapon.Initialize(this, _playerCrosshair);
            
            _gripIKLocation = EquippedWeapon.GripLocation;
            //_playerAnimator.SetInteger(WeaponTypeHash, (int)EquippedWeapon.WeaponInformation.WeaponType);
        }

        private void OnAnimatorIK(int layerIndex)
        {
            _playerAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
            _playerAnimator.SetIKPosition(AvatarIKGoal.LeftHand, _gripIKLocation.position);
        }
        
        // private void OnFire(InputAction.CallbackContext pressed)
        // {
        //     _firingPressed = pressed.ReadValue<float>() == 1f ? true : false;
        //     
        //     if (_firingPressed)
        //         StartFiring();
        //     else
        //         StopFiring();
        //     
        // }

        // private void StartFiring()
        // {
        //     //TODO: Weapon Seems to be reloading after no bullets left
        //     if (EquippedWeapon.WeaponInformation.BulletsAvailable <= 0 &&
        //         EquippedWeapon.WeaponInformation.BulletsInClip <= 0) return;
        //
        //     _playerController.isFiring = true;
        //     _playerAnimator.SetBool(IsFiringHash, true);
        //     EquippedWeapon.StartFiringWeapon();
        // }
        //
        // private void StopFiring()
        // {
        //     _playerController.isFiring = false;
        //     _playerAnimator.SetBool(IsFiringHash, false);
        //     EquippedWeapon.StopFiringWeapon();
        // }

        
        // private void OnReload(InputValue button)
        // {
        //     StartReloading();
        // }

        // public void StartReloading()
        // {
        //     if (EquippedWeapon.WeaponInformation.BulletsAvailable <= 0 && _playerController.isFiring)
        //     {
        //         StopFiring();
        //         return;
        //     }
        //
        //     _playerController.isReloading = true;
        //     _playerAnimator.SetBool(IsReloadingHash, true);
        //     EquippedWeapon.StartReloading();
        //     
        //     InvokeRepeating(nameof(StopReloading), 0, .1f);
        // }
        
        // private void StopReloading()
        // {
        //     if (_playerAnimator.GetBool(IsReloadingHash)) return;
        //     
        //     _playerController.isReloading = false;
        //     EquippedWeapon.StopReloading();
        //     CancelInvoke(nameof(StopReloading));
        //     
        //     if (!_wasFiring || !_firingPressed) return;
        //     
        //     StartFiring();
        //     _wasFiring = false;
        // }
        
    private void OnLook(InputValue obj)
    {
        Vector3 independentMousePosition = ViewCamera.ScreenToViewportPoint(_playerCrosshair.CurrentAimPosition);
        
        _playerAnimator.SetFloat(AimHorizontalHash, independentMousePosition.x);
        _playerAnimator.SetFloat(AimVerticalHash, independentMousePosition.y);
    }
    //     
    //     private new void OnEnable()
    //     {
    //         base.OnEnable();
    //         GameInput.ThirdPerson.Look.performed += OnLook;
    //         //GameInput.ThirdPerson.Fire.performed += OnFire;
    //         
    //     }
    //     
    //     private new void OnDisable()
    //     {
    //         base.OnDisable();
    //         GameInput.ThirdPerson.Look.performed -= OnLook;
    //         //GameInput.ThirdPerson.Fire.performed -= OnFire;
    //     }
     }
}
