using UnityEngine;
using UnityEngine.SceneManagement;
#if ENABLE_INPUT_SYSTEM
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

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;


		public Elevator Elevator1;
		public Elevator Elevator2;
		public Elevator Elevator3;

#if ENABLE_INPUT_SYSTEM

		public int TimeoutMinutes = 10;
		int resetCounter = 0;

        private void Start()
        {
			InvokeRepeating("IncResetCounter", 60f, 60f);
        }

		//called every minute
		void IncResetCounter()
		{
			resetCounter++;
			if (resetCounter == TimeoutMinutes)
			{
				GetComponent<CharCtrl>().reset();
			}
		}

        public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
			resetCounter = 0;
		}

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
			resetCounter = 0;
		}

		public void OnJump(InputValue value)
		{
			//disable jumping
			return;

			if (InElevator()) return;
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
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
		
		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}


		bool InElevator()
		{
			if (Elevator1.elevatorState == Elevator.ElevatorState.IN_ELEVATOR || Elevator1.elevatorState == Elevator.ElevatorState.AT_ENTRANCE) return true;
			if (Elevator2.elevatorState == Elevator.ElevatorState.IN_ELEVATOR || Elevator2.elevatorState == Elevator.ElevatorState.AT_ENTRANCE) return true;
			if (Elevator3.elevatorState == Elevator.ElevatorState.IN_ELEVATOR || Elevator3.elevatorState == Elevator.ElevatorState.AT_ENTRANCE) return true;
			return false;
		}


	}

}