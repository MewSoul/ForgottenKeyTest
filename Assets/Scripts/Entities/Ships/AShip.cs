using UnityEngine;

namespace TAOM.Entities.Ships {

	public class AShip : AEntity {

		[SerializeField] private Projectile projectilePrefab;
		[SerializeField] private Transform projectileSpawn;
		[SerializeField] private float projectileDamage;
		[SerializeField] private float projectileSpeed;
		[SerializeField] private float shootDelay;

		private Transform projectileParent;
		private float nextTimeShoot;

		private void Start() {
			projectileParent = GameObject.FindGameObjectWithTag("_Projectiles").transform;
			nextTimeShoot = Time.time;
		}

		protected bool Fire() {
			if (Time.time > nextTimeShoot) {
				nextTimeShoot = Time.time + shootDelay;

				Projectile instance = Instantiate(projectilePrefab, projectileSpawn.position, Quaternion.identity, projectileParent);
				instance.GetComponent<Projectile>().SetData(this.transform.rotation.eulerAngles.y, projectileSpeed, projectileDamage);

				return true;

			}
			return false;
		}

	}
}
