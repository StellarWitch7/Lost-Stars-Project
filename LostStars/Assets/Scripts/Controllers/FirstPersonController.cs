using System;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	[RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
	[RequireComponent(typeof(PlayerInput))]
#endif
	public class FirstPersonController : MonoBehaviour
	{
		[Header("Player")]
		[Tooltip("Move speed of the character in m/s")]
		public float MoveSpeed = 4.0f;
		[Tooltip("Sprint speed of the character in m/s")]
		public float SprintSpeed = 6.0f;
		[Tooltip("Crouch speed of the character in m/s")]
		public float CrouchSpeed = 2.0f;
		[Tooltip("Rotation speed of the character")]
		public float RotationSpeed = 1.0f;
		[Tooltip("Acceleration and deceleration")]
		public float SpeedChangeRate = 10.0f;
		[Tooltip("Position of eyes when crouching")]
		public Vector3 CrouchingEyesPosition = new Vector3(0, 0.75f);

		[Space(10)]
		[Tooltip("The height the player can jump")]
		public float JumpHeight = 1.2f;
		[Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
		public float Gravity = -15.0f;

		[Space(10)]
		[Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
		public float JumpTimeout = 0.1f;
		[Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
		public float FallTimeout = 0.15f;

		[Header("Player Grounded")]
		[Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
		public bool Grounded = true;
		[Tooltip("Useful for rough ground")]
		public float GroundedOffset = -0.14f;
		[Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
		public float GroundedRadius = 0.5f;
		[Tooltip("What layers the character uses as ground")]
		public LayerMask GroundLayers;

		[Header("Cinemachine")]
		[Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
		public GameObject CinemachineCameraTarget;
		[Tooltip("How far in degrees can you move the camera up")]
		public float TopClamp = 90.0f;
		[Tooltip("How far in degrees can you move the camera down")]
		public float BottomClamp = -90.0f;

		// cinemachine
		private float _pitch;
		private float _cinemachineTargetPitch;
		private Vector3 _defaultEyesPosition;

		// player
		private float _speed;
		private float _rotationVelocity;
		private float _verticalVelocity;
		private float _terminalVelocity = 53.0f;
		private float _defaultHeight;
		private float _crouchingHeight;
		private float _maxHealth = 100;
		private float _healthCurrent = 60;
		private float _maxEnergy = 1000;
		private float _energyCurrent = 800;
		private float _timeSinceLastRegen;
		private float _timeSinceLastEnergyDrain;
		private float _velocity;
		private float _cameraRecoilCurrent;
		private float _cameraRecoilMax = 7;
		private bool _regenAllowed = true;
		private bool _cameraRotAllowed = false;
		private bool _moveAllowed = false;
		private bool _castingAllowed = true;
		private Vector3 _lastPos;
		public float TimeBetweenRegens = 1;
		public float NaturalRegenAmount = 1;
		public float TimeBetweenDrains = 1;
		public float NaturalDrainAmount = 0.25f;
		public CargoActivateable ItemPickedUp;
		public bool IsCarrying;
		public GameObject ControlledVehicle;
		public WeaponHolder WeaponHolder;
		public InventoryController InventoryController;
		public Vector3 PickupPoint;
		public Rigidbody Rigidbody;

		// timeout deltatime
		private float _jumpTimeoutDelta;
		private float _fallTimeoutDelta;

	
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
		private PlayerInput _playerInput;
#endif
		private CharacterController _controller;
		private StarterAssetsInputs _input;
		private GameObject _mainCamera;
		private HudController _hudController;
		private MenuController _menuController;
		private GameObject _hud;
		private GameObject _menu;

		private const float _threshold = 0.01f;

		private bool IsCurrentDeviceMouse
		{
			get
			{
				#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
				return _playerInput.currentControlScheme == "KeyboardMouse";
				#else
				return false;
				#endif
			}
		}

		private void Awake()
		{
			// get a reference to our main camera
			if (_mainCamera == null)
			{
				_mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
			}
		}

		private void Start()
		{
			_hud = GameObject.Find("HUD");
			_menu = GameObject.Find("MenuUI");
			this.WeaponHolder = GameObject.Find("WeaponHolder").GetComponent<WeaponHolder>();
			_controller = GetComponent<CharacterController>();
			_input = GetComponent<StarterAssetsInputs>();
			this.InventoryController = _menu.GetComponentInChildren<InventoryController>();
			_hudController = _hud.GetComponent<HudController>();
			_menuController = _menu.GetComponent<MenuController>();
			_hudController.SetPlayer(this.gameObject);
			_menuController.SetPlayer(this.gameObject);
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
			_playerInput = GetComponent<PlayerInput>();
#else
			Debug.LogError( "Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif

			// reset our timeouts on start
			_jumpTimeoutDelta = JumpTimeout;
			_fallTimeoutDelta = FallTimeout;
			_defaultEyesPosition = CinemachineCameraTarget.transform.localPosition;
			_defaultHeight = _controller.height;
			_crouchingHeight = _defaultHeight - (_defaultEyesPosition.y - CrouchingEyesPosition.y);
			IsCarrying = false;
		}

		private void Update()
		{
			//Calculates velocity
			_velocity = Vector3.Distance(gameObject.transform.position, _lastPos) / Time.deltaTime;

			//Sets last position for next velocity calculation
			_lastPos = gameObject.transform.position;

			//Some more stuff
			_hudController.SetBottomLabel("");
			PickupPoint = _mainCamera.transform.position + _mainCamera.transform.TransformDirection(new Vector3(0, 0, 4));
			AdjustHeight();
			if (_moveAllowed)
			{
				JumpAndGravity();
				GroundedCheck();
				Move();
			}
			Look();

			//Weapon Controls
			if (_input.swapWeapon)
			{
				SwitchWeapon();
			}
			if (WeaponHolder.HeldWeapon != null)
			{
				if (!_input.isCrouching && _velocity > 0.1)
				{
					var spreadFactor = WeaponHolder.HeldWeapon.GetComponent<WeaponObject>().SpreadFactor;
					AddGunSpread(Time.deltaTime * (spreadFactor + _velocity));
				}

				if (_input.reload)
				{
					ReloadGun();
				}

				if (_input.isFiring1)
				{
					PrimaryFire();
				}
				else
				{
					TriggerReleased();
				}

				if (_input.isFiring2)
				{
					SecondaryFire();
				}
				else
				{

				}
			}
			else
			{
				_hudController.ClearAmmoLabel();
			}

			//Regen
			if (_regenAllowed)
			{
				if (_timeSinceLastRegen > TimeBetweenRegens)
				{
					AddHealth(NaturalRegenAmount);
					_timeSinceLastRegen = 0;
				}
			}

			_timeSinceLastRegen += Time.deltaTime;

			//Exhaustion
			if (_timeSinceLastEnergyDrain > TimeBetweenDrains)
			{
				DepleteEnergy(NaturalDrainAmount);
				_timeSinceLastEnergyDrain = 0;

				if (_energyCurrent <= 0)
				{
					_castingAllowed = false;
					Horrors();
				}
				else
				{
					_castingAllowed = true;
				}
			}

			_timeSinceLastEnergyDrain += Time.deltaTime;

			//Input Resets
			_input.activate = false;
			_input.reload = false;
			_input.swapWeapon = false;
		}

		public void ApplyRecoil(float amount)
		{
			_cameraRecoilCurrent += amount;
		}

		private void Look()
		{
			Vector3 fwd = _mainCamera.transform.TransformDirection(Vector3.forward);
			if (Physics.Raycast(_mainCamera.transform.position, fwd, out RaycastHit hitInfo, 10))
			{
				GameObject targetObject = hitInfo.transform.gameObject;

				if (targetObject != null)
				{
					Activateable activateable = targetObject.GetComponent<Activateable>();
					if (activateable != null)
					{
						_hudController.SetBottomLabel(activateable.ActionText);

						if (_input.activate)
						{
							activateable.Activate(this.gameObject);
						}
					}
				}
			}
		}

		public void SwitchWeapon()
		{
			if (InventoryController.PlayerInventory.EquippedIsPrimary && InventoryController.PlayerInventory.SecondaryWeapon != null)
			{
				WeaponHolder.EquippedWeapon = InventoryController.PlayerInventory.SecondaryWeapon;
				InventoryController.PlayerInventory.EquippedIsPrimary = false;
			}
			else if (InventoryController.PlayerInventory.PrimaryWeapon != null)
			{
				WeaponHolder.EquippedWeapon = InventoryController.PlayerInventory.PrimaryWeapon;
				InventoryController.PlayerInventory.EquippedIsPrimary = true;
			}

			if (InventoryController.PlayerInventory.PrimaryWeapon == null && InventoryController.PlayerInventory.SecondaryWeapon == null)
			{
				WeaponHolder.EquippedWeapon = null;
			}

			if (WeaponHolder.EquippedWeapon != null)
			{
				WeaponHolder.SetWeaponChanged();
			}

			WeaponHolder.SpawnWeapon();

			if (WeaponHolder.HeldWeapon != null)
			{
				RefreshAmmoUi(WeaponHolder.HeldWeapon.GetComponent<WeaponObject>().AmmoLeft, 
					WeaponHolder.HeldWeapon.GetComponent<WeaponObject>().AmmoCapacity);
			}
		}

		public void ReloadGun()
		{
			WeaponHolder.HeldWeapon.GetComponent<WeaponObject>().StartReloading();
		}

		public void TriggerReleased()
		{
			WeaponHolder.HeldWeapon.GetComponent<WeaponObject>().SetTriggerHeld(false);
		}

		public void PrimaryFire()
		{
			if (WeaponHolder.HeldWeapon != null)
			{
				WeaponHolder.HeldWeapon.GetComponent<WeaponObject>().PrimaryFire(_mainCamera.transform.position,
				_mainCamera.transform.TransformDirection(Vector3.forward)); //Object reference not set to an instance of an object
			}
		}

		public void SecondaryFire()
		{
			if (WeaponHolder.HeldWeapon != null)
			{
				WeaponHolder.HeldWeapon.GetComponent<WeaponObject>().SecondaryFire(_mainCamera.transform.position,
				_mainCamera.transform.TransformDirection(Vector3.forward));
			}
		}

		public void ToggleInputs(bool b)
		{
			_cameraRotAllowed = b;
			_moveAllowed = b;
		}

		public void DisableInput()
		{
			_playerInput.enabled = false;
		}

		public void RefreshAmmoUi(int ammoLeft, int ammoCap)
		{
			_hudController.SetAmmoAmountLabel(ammoLeft, ammoCap);
		}

		private void AdjustHeight()
        {
            if (_input.isCrouching)
            {
                _controller.height = _crouchingHeight;
            }
            else
            {
                _controller.height = _defaultHeight;
            }
        }

		public void AddHealth(float amount)
		{
			_healthCurrent += amount;

			if (_healthCurrent > _maxHealth)
			{
				_healthCurrent = _maxHealth;
			}

			_hudController.UpdateHealth(_healthCurrent, _maxHealth);
		}

		public void RemoveHealth(float amount)
		{
			_healthCurrent -= amount;

			if (_healthCurrent < 0)
			{
				_healthCurrent = 0;
			}

			_hudController.UpdateHealth(_healthCurrent, _maxHealth);
		}

		public void GainEnergy(float amount)
		{
			_energyCurrent += amount;

			if (_energyCurrent > _maxEnergy)
			{
				_energyCurrent = _maxEnergy;
			}

			_hudController.UpdateEnergy(_energyCurrent, _maxEnergy);
		}

		public void DepleteEnergy(float amount)
		{
			_energyCurrent -= amount;

			if (_energyCurrent < 0)
			{
				_energyCurrent = 0;
			}

			_hudController.UpdateEnergy(_energyCurrent, _maxEnergy);
		}

		public float GetMaxHealth()
		{
			return _maxHealth;
		}

		public float GetCurrentHealth()
		{
			return _healthCurrent;
		}

		public void AddGunSpread(float amount)
		{
			if (WeaponHolder.HeldWeapon != null)
			{
				WeaponHolder.HeldWeapon.GetComponent<WeaponObject>().Spread += amount;
			}
		}

		public void SetUiSpread(float current, float min, float max)
		{
			_hudController.Crosshair.MinSpread = min;
			_hudController.Crosshair.MaxSpread = max;
			_hudController.Crosshair.CurrentSpread = current;
		}

		public void Horrors()
		{

		}

		public void SetReloadingGraphic(bool b)
		{
			_hudController.SetReloadingGraphic(b);
		}

		private void LateUpdate()
		{
			if (_cameraRotAllowed)
			{
				CameraRotation();
			}
		}

		private void GroundedCheck()
		{
			// set sphere position, with offset
			Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
			Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);
		}

		private void CameraRotation()
		{
			// if there is an input
			if (_input.look.sqrMagnitude >= _threshold)
			{
				//Don't multiply mouse input by Time.deltaTime
				float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;
				
				_pitch += _input.look.y * RotationSpeed * deltaTimeMultiplier;
				_rotationVelocity = _input.look.x * RotationSpeed * deltaTimeMultiplier;
			}

			//recoil control
			_cameraRecoilCurrent = _cameraRecoilCurrent > 0 ? _cameraRecoilCurrent : 0;
			_cameraRecoilCurrent = _cameraRecoilCurrent < _cameraRecoilMax ? _cameraRecoilCurrent : _cameraRecoilMax;
			_cinemachineTargetPitch = _pitch - _cameraRecoilCurrent;
			_cameraRecoilCurrent -= 5 * Time.deltaTime;

			// clamp our pitch rotation
			_cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

			// Update Cinemachine camera target pitch
			CinemachineCameraTarget.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0.0f, 0.0f);

			// if there is an input
			if (_input.look.sqrMagnitude >= _threshold)
			{
				// rotate the player left and right
				transform.Rotate(Vector3.up * _rotationVelocity);
			}
		}

		public void OpenInventory(GameObject target)
		{
			_menuController.EnterMenu();
			_menuController.OpenStorageTarget(target);
		}

        private void Move()
		{
			// set target speed based on move speed, sprint speed and if sprint is pressed

			float targetSpeed = MoveSpeed;
			if (_input.isCrouching)
			{
				targetSpeed = CrouchSpeed;
			}
			else if (_input.sprint)
			{
				targetSpeed = SprintSpeed;
			}

			// a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

			// note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
			// if there is no input, set the target speed to 0
			if (_input.move == Vector2.zero) targetSpeed = 0.0f;

			// a reference to the players current horizontal velocity
			float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

			float speedOffset = 0.1f;
			float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

			// accelerate or decelerate to target speed
			if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
			{
				// creates curved result rather than a linear one giving a more organic speed change
				// note T in Lerp is clamped, so we don't need to clamp our speed
				_speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);

				// round speed to 3 decimal places
				_speed = Mathf.Round(_speed * 1000f) / 1000f;
			}
			else
			{
				_speed = targetSpeed;
			}

			// normalise input direction
			Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

			// note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
			// if there is a move input rotate player when the player is moving
			if (_input.move != Vector2.zero)
			{
				// move
				inputDirection = transform.right * _input.move.x + transform.forward * _input.move.y;
			}

			// move the player
			_controller.Move(inputDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
		}

		private void JumpAndGravity()
		{
			if (Grounded)
			{
				// reset the fall timeout timer
				_fallTimeoutDelta = FallTimeout;

				// stop our velocity dropping infinitely when grounded
				if (_verticalVelocity < 0.0f)
				{
					_verticalVelocity = -2f;
				}

				// Jump
				if (_input.jump && _jumpTimeoutDelta <= 0.0f)
				{
					// the square root of H * -2 * G = how much velocity needed to reach desired height
					_verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
				}

				// jump timeout
				if (_jumpTimeoutDelta >= 0.0f)
				{
					_jumpTimeoutDelta -= Time.deltaTime;
				}
			}
			else
			{
				// reset the jump timeout timer
				_jumpTimeoutDelta = JumpTimeout;

				// fall timeout
				if (_fallTimeoutDelta >= 0.0f)
				{
					_fallTimeoutDelta -= Time.deltaTime;
				}

				// if we are not grounded, do not jump
				_input.jump = false;
			}

			// apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
			if (_verticalVelocity < _terminalVelocity)
			{
				_verticalVelocity += Gravity * Time.deltaTime;
			}
		}

		private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
		{
			if (lfAngle < -360f) lfAngle += 360f;
			if (lfAngle > 360f) lfAngle -= 360f;
			return Mathf.Clamp(lfAngle, lfMin, lfMax);
		}

		private void OnDrawGizmosSelected()
		{
			Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
			Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

			if (Grounded) Gizmos.color = transparentGreen;
			else Gizmos.color = transparentRed;

			// when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
			Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z), GroundedRadius);
		}
	}
}