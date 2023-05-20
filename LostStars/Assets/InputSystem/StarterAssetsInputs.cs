using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;
		public bool isCrouching;
		public bool isFiring1;
		public bool isFiring2;
		public bool activate;
		public bool menuOpen;
		public bool swapWeapon;
		public bool reload;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

		public void Update()
		{

		}

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
		public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}

		public void OnCrouch(InputValue value)
		{
			ToggleCrouch();
		}

		public void OnPrimaryFire(InputValue value)
		{
			Fire1(value.isPressed);
		}

		public void OnSecondaryFire(InputValue value)
		{
			Fire2(value.isPressed);
		}
		public void OnActivate(InputValue value)
		{
			Activate();
		}

		public void OnMenu(InputValue value)
		{
			ToggleMenu(value.isPressed);
		}

		public void OnReload(InputValue value)
		{
			Reload();
		}

		public void OnSwapWeapon(InputValue value)
		{
			SwapWeapon();
		}
#endif


		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}

		public void ToggleCrouch()
		{
			isCrouching = !isCrouching;
		}

		public void Fire1(bool newFire1State)
        {
			isFiring1 = newFire1State;
        }

		public void Fire2(bool newFire2State)
		{
			isFiring2 = newFire2State;
		}

		public void Activate()
		{
			activate = true;
		}

		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}

		private void ToggleMenu(bool newMenuState)
		{
			menuOpen = newMenuState;
		}

		public void Reload()
		{
			reload = true;
		}

		public void SwapWeapon()
		{
			swapWeapon = true;
		}
	}
	
}