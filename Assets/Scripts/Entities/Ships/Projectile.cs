using UnityEngine;

namespace TAOM.Entities.Ships {

	public class Projectile : MonoBehaviour {

		private Rigidbody rb;
		private float projectileDamage;

		private void Awake() {
			rb = GetComponent<Rigidbody>();
		}

		public void SetData(float rotation, float speed, float damage) {
			this.transform.rotation = Quaternion.Euler(0, rotation, 0);
			this.projectileDamage = damage;
			rb.velocity = transform.forward * speed;
		}

		private void OnTriggerEnter(Collider other) {
			AEntity entity = other.gameObject.GetComponentInParent<AEntity>();
			if (entity != null)
				entity.Damage(projectileDamage);
			Destroy(this.gameObject);
		}

	}

}
