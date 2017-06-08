using DG.Tweening;
using TAOM.Gameplay;
using TAOM.Managers;
using UnityEngine;

namespace TAOM.Entities.Ships {

	public class PlayerShip : AShip {

		private const float SHAKE_DURATION = 0.1f;
		private const float SHAKE_POWER = 0.5f;

		private InputManager inputManager;
		private Game game;
		private OverheatingSystem overheatingSystem;
		private Vector3 movement;
		private Vector3 rotation;

		protected override void Awake() {
			base.Awake();
			inputManager = FindObjectOfType<InputManager>();
			game = FindObjectOfType<Game>();
			overheatingSystem = GetComponent<OverheatingSystem>();
		}

		private void Update() {
			movement = inputManager.InputMovement();
			rotation = inputManager.InputRotation();

			CheckIsFiring();
		}

		private void FixedUpdate() {
			Move();
			Rotate();
		}

		#region MOVEMENTS

		private void Move() {
			rb.velocity = movement * movementSpeed;
		}

		private void Rotate() {
			if (rotation == Vector3.zero)
				return;

			switch (InputManager.InputType) {
				case InputType.CONTROLLER:
					if (rotation != Vector3.zero)
						transform.DORotate(Quaternion.LookRotation(rotation).eulerAngles, 0.2f);
					break;

				case InputType.MOUSE:
				default:
					rotation.y = 0;
					transform.LookAt(rotation);
					break;
			}
		}

		#endregion

		#region ACTIONS

		private void CheckIsFiring() {
			if (inputManager.InputFire() && overheatingSystem.IsInOrder() && Fire()) {
				overheatingSystem.Heat();
				Camera.main.DOShakePosition(SHAKE_DURATION, SHAKE_POWER);
				inputManager.VibrateController(0.2f, 0.2f, 0.2f);
			}

		}

		#endregion

		public override void Damage(float damagePoint) {
			base.Damage(damagePoint);
		}

		protected override void Die() {
			base.Die();
			game.GameOver();
		}

	}

}