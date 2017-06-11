using DG.Tweening;
using UnityEngine;

namespace TAOM.Entities.Ships {

	public class EnemyShip : AShip {

		[SerializeField] private float shootDistancePlayer;
		[SerializeField] private float shootDistanceHomeAsteroid;
		[SerializeField] private float delayBetweenShots;

		private GameObject homeAsteroid;
		private GameObject player;
		private float timeNextShot;

		protected override void Awake() {
			base.Awake();
			homeAsteroid = GameObject.FindGameObjectWithTag("HomeAsteroid");
			player = GameObject.FindGameObjectWithTag("Player");
		}

		protected override void Start() {
			base.Start();

			timeNextShot = Time.time + delayBetweenShots;

			transform.LookAt(homeAsteroid.transform);
			rb.velocity = transform.forward * movementSpeed;
		}

		private void Update() {
			if (player.gameObject != null && Vector3.Distance(this.transform.position, player.transform.position) < shootDistancePlayer)
				this.transform.DOLookAt(player.transform.position, 0.5f);
			else if (homeAsteroid.gameObject != null)
				this.transform.DOLookAt(homeAsteroid.transform.position, 0.5f);

			if (Time.time > timeNextShot && IsCloseToTargetsToShoot() && Fire()) {
				timeNextShot = Time.time + delayBetweenShots;
			}
		}

		private bool IsCloseToTargetsToShoot() {
			return ((player.gameObject != null && Vector3.Distance(this.transform.position, player.transform.position) < shootDistancePlayer) ||
				(homeAsteroid.gameObject != null && Vector3.Distance(this.transform.position, homeAsteroid.transform.position) < shootDistanceHomeAsteroid));
		}

		public override void Die() {
			base.Die();
			DOTween.Kill(this.transform);
			game.NotifyEnemyDeath(this);
		}

	}

}