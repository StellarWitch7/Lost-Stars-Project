using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class ShipInputs : MonoBehaviour
{
	[Header("Ship Input Values")]
	public Vector2 look;
	public bool boost;
	public bool isFiring1;
	public bool isFiring2;
	public bool up;
	public bool down;
	public bool right;
	public bool left;
	public bool rollRight;
	public bool rollLeft;
	public float speed;
	public bool exit;
	public bool isParked = false;

	[Header("Mouse Cursor Settings")]
	public bool cursorLocked = true;
	public bool cursorInputForLook = true;

	public void Update()
	{

	}

#if ENABLE_INPUT_SYSTEM

	public void OnLook(InputValue value)
	{
		if (cursorInputForLook)
		{
			LookInput(value.Get<Vector2>());
		}
	}

	public void OnBoost(InputValue value)
	{
		BoostInput(value.isPressed);
	}

	public void OnPrimaryFire(InputValue value)
	{
		Fire1(value.isPressed);
	}

	public void OnSecondaryFire(InputValue value)
	{
		Fire2(value.isPressed);
	}

	public void OnUp(InputValue value)
	{
		UpInput(value.isPressed);
	}

	public void OnDown(InputValue value)
	{
		DownInput(value.isPressed);
	}

	public void OnRight(InputValue value)
	{
		RightInput(value.isPressed);
	}

	public void OnLeft(InputValue value)
	{
		LeftInput(value.isPressed);
	}

	public void OnRollRight(InputValue value)
	{
		RollRightInput(value.isPressed);
	}

	public void OnRollLeft(InputValue value)
	{
		RollLeftInput(value.isPressed);
	}

	public void OnControlSpeed(InputValue value)
	{
		ControlSpeedInput(value.Get<float>());
	}

	public void OnExitShip(InputValue value)
	{
		ExitShip(value.isPressed);
	}

	public void OnParkShip(InputValue value)
	{
		TogglePark();
	}

#endif

	public void LookInput(Vector2 v)
	{
		look = v;
	}

	public void BoostInput(bool v)
	{
		boost = v;
	}

	public void UpInput(bool v)
	{
		up = v;
	}

	public void DownInput(bool v)
	{
		down = v;
	}

	public void RightInput(bool v)
	{
		right = v;
	}

	public void LeftInput(bool v)
	{
		left = v;
	}

	public void Fire1(bool v)
	{
		isFiring1 = v;
	}

	public void Fire2(bool v)
	{
		isFiring2 = v;
	}

	private void OnApplicationFocus(bool hasFocus)
	{
		SetCursorState(cursorLocked);
	}

	private void RollRightInput(bool v)
	{
		rollRight = v;
	}

	private void RollLeftInput(bool v)
	{
		rollLeft = v;
	}

	private void ControlSpeedInput(float v)
	{
		if (v >= 1)
		{
			speed += 3;
		}
		else if (v <= -1)
		{
			speed -= 3;
		}
	}

	private void ExitShip(bool v)
	{
		exit = v;
	}

	private void SetCursorState(bool newState)
	{
		Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
	}

	private void TogglePark()
	{
		isParked = !isParked;
	}
}