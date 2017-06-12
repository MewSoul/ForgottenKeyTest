using System.Collections;
using UnityEngine;
using XInputDotNetPure;

namespace TAOM.Managers {

	public enum Direction {
		LEFT = -1,
		NONE = 0,
		RIGHT = 1
	}

	public class InputManager : MonoBehaviour {

		public InputType InputType { get; private set; }

		private Vector3 mousePositionToWorld;

		private Direction previousInput;
		private Direction currentInput;
		private bool previousConfirm;

		#region MOVEMENTS

		public Direction InputDirection() {
			Direction directionToReturn = Direction.NONE;
			float h = Input.GetAxis("Horizontal");

			if (h > 0.5f)
				currentInput = Direction.RIGHT;
			else if (h < -0.5f)
				currentInput = Direction.LEFT;
			else
				currentInput = Direction.NONE;

			//Force to release input otherwise it will cycle too much
			if (previousInput.Equals(Direction.NONE))
				directionToReturn = currentInput;

			previousInput = currentInput;
			return directionToReturn;
		}

		public Vector3 InputMovement() {
			float h = Input.GetAxis("Horizontal");
			float v = Input.GetAxis("Vertical");
			return new Vector3(h, 0f, v);
		}

		public Vector3 InputRotation() {
			//Check for mouse input first
			float h = Input.GetAxisRaw("Aim X Mouse");
			float v = Input.GetAxisRaw("Aim Y Mouse");

			if (Mathf.Abs(h) > 0f || Mathf.Abs(v) > 0f) {
				InputType = InputType.MOUSE;
				mousePositionToWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				return mousePositionToWorld;
			}

			//Check for controller input if no input from mouse
			h = Input.GetAxisRaw("Aim X");
			v = Input.GetAxisRaw("Aim Y");

			if (Mathf.Abs(h) >= 0.5f || Mathf.Abs(v) > 0.5f) {
				InputType = InputType.CONTROLLER;
				return new Vector3(h, 0f, v);
			}

			//If no input from controller and previous one was from mouse, to target the same position
			if (InputType.Equals(InputType.MOUSE))
				return mousePositionToWorld;

			return Vector3.zero;
		}

		#endregion

		#region ACTIONS

		public bool InputFire() {
			return Input.GetAxis("Fire1") >= 1f;
		}

		public bool InputConfirm() {
			bool valueToReturn = false;
			bool currentConfirm = Input.GetAxis("Submit") >= 1;

			//Done so force the player to press and release the input
			if (previousConfirm == false && currentConfirm)
				valueToReturn = true;

			previousConfirm = currentConfirm;
			return valueToReturn;
		}


		#endregion

		#region CONTROLLER VIBRATIONS

		public void VibrateController(float powerLeft, float powerRight, float duration) {
			StopCoroutine("StopVibration");
			GamePad.SetVibration(0, powerLeft, powerRight);
			StartCoroutine(StopVibration(duration));
		}

		private IEnumerator StopVibration(float timeToWait) {
			yield return new WaitForSeconds(timeToWait);
			GamePad.SetVibration(0, 0, 0);
		}

		#endregion

	}

}