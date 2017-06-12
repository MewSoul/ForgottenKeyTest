using DG.Tweening;
using TAOM.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace TAOM.Entities.Ships {

	public class PlayerShip : AShip {

		private const float SHAKE_DURATION = 0.1f;
		private const float SHAKE_POWER = 0.5f;

		[SerializeField] private Slider lifeSlider;
		[SerializeField] private Text currentLifeText;
		[SerializeField] private Text maxLifeText;
		[SerializeField] private Text currentNbCollectibleText;

		private OverheatingSystem overheatingSystem;
		private Vector3 movement;
		private Vector3 rotation;
		private int currentNbCollectible;

		private float currentSpeed;
		private Vector3 previousDirection;

		protected override void Awake() {
			base.Awake();
			overheatingSystem = GetComponentInChildren<OverheatingSystem>();
			currentNbCollectible = 0;
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

		public override void Damage(float damagePoint) {
			base.Damage(damagePoint);
			currentLifeText.text = lifePoints.ToString();
			lifeSlider.value = lifePoints;
			game.NotifyPlayerDamaged();
			inputManager.VibrateController(0.3f, 0.3f, 0.2f);
		}

		public override void Die() {
			base.Die();
			game.GameOver();
		}

		#region MOVEMENTS

		private void Move() {
			if (game.CanDoActions() && movement != Vector3.zero) {
				rb.velocity = movement * movementSpeed;
				previousDirection = movement;
				currentSpeed = movementSpeed;
			} else if (currentSpeed > 0) {
				//Smooth way to stop down the spaceship rather than stopping immediately
				currentSpeed -= 0.25f;
				if (currentSpeed < 0)
					currentSpeed = 0;
				rb.velocity = previousDirection * currentSpeed;
			}
		}

		private void Rotate() {
			if (rotation == Vector3.zero)
				return;

			switch (inputManager.InputType) {
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
				Camera.main.DOShakePosition(SHAKE_DURATION, SHAKE_POWER * projectileScale);
			}

		}

		#endregion

		#region COLLECTIBLES

		public void IncreaseNbCollectible() {
			++currentNbCollectible;
			currentNbCollectibleText.text = "x" + currentNbCollectible;
			Sequence sequence = DOTween.Sequence();
			sequence.Append(currentNbCollectibleText.transform.DOScale(1.3f, 0.1f));
			sequence.Append(currentNbCollectibleText.transform.DOScale(1f, 0.1f));
		}

		#endregion

		#region PERKS

		public void IncreaseFiringRate() {
			//Increase firing rate  by 25%
			shootDelay *= 0.75f;
			Debug.Log("Increase firing rate!");
		}

		public void IncreaseProjectileDamage() {
			//Increase damage by 25%
			projectileDamage *= 1.25f;
			projectileScale *= 1.33f;
			Debug.Log("Increase projectile damage!");
		}

		public void IncreaseMovementSpeed() {
			//Increase speed by 10%
			movementSpeed *= 1.1f;
			Debug.Log("Increase movement speed!");
		}

		public bool PurchasePerk(int cost) {
			Debug.Log("CURRENT NB=" + currentNbCollectible);
			Debug.Log("COST=" + cost);
			currentNbCollectible -= cost;
			currentNbCollectibleText.text = "x" + currentNbCollectible;
			return true;
		}

		public bool CanPurchasePerk(int cost) {
			return (cost <= currentNbCollectible);
		}

		#endregion

	}

}