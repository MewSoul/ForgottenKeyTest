using UnityEngine;

namespace TAOM.Entities {

	public abstract class AEntity : MonoBehaviour {

		[SerializeField] private float lifePoints;
		[SerializeField] protected float movementSpeed;

		protected Rigidbody rb;

		protected virtual void Awake() {
			rb = GetComponent<Rigidbody>();
		}

		public virtual void Damage(float damagePoint) {
			lifePoints -= damagePoint;

			if (lifePoints <= 0)
				Die();
		}

		protected virtual void Die() {
			Destroy(this.gameObject);
		}

	}

}
