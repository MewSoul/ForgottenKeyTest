using UnityEngine;

namespace TAOM.Entities.Ships {

	public class EnemyShip : AShip {

		[SerializeField] private float delayBetweenShots;

		private float timeNextShot;

		protected override void Start() {
			base.Start();

			timeNextShot = Time.time + delayBetweenShots;

			if (movementSpeed > 0) {
				transform.LookAt(GameObject.FindGameObjectWithTag("HomeAsteroid").transform);
				rb.velocity = transform.forward * movementSpeed;
			}
		}

		private void Update() {
			if (Time.time > timeNextShot && Fire()) {
				timeNextShot = Time.time + delayBetweenShots;
			}
		}

		protected override void Die() {
			base.Die();
			game.NotifyEnemyDeath(this);
		}

	}

}