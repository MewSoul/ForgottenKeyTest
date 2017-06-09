using UnityEngine;

namespace TAOM.Entities {

	public abstract class AEntity : MonoBehaviour {

		[SerializeField] private float lifePoints;
		[SerializeField] protected float movementSpeed;
		[SerializeField] private int dropRateCollectible;
		[SerializeField] private GameObject collectiblePrefab;

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
			DropCollectible();
			Destroy(this.gameObject);
		}

		private void DropCollectible() {
			if (Random.Range(0, 100) < dropRateCollectible) {
				Transform collectibleParent = GameObject.FindGameObjectWithTag("_Collectibles").transform;
				Instantiate(collectiblePrefab, this.transform.position, Quaternion.identity, collectibleParent);
			}
		}

	}

}
