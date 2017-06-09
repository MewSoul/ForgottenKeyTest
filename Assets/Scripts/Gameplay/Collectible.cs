using TAOM.Entities.Ships;
using UnityEngine;

namespace TAOM.Gameplay {

	public class Collectible : MonoBehaviour {

		private const float DISTANCE_MAGNET = 10f;

		[SerializeField] private float rotationSpeed;

		private Rigidbody rb;

		private void Awake() {
			rb = GetComponent<Rigidbody>();
			rb.angularVelocity = Random.insideUnitSphere * rotationSpeed;
		}

		private void OnTriggerEnter(Collider other) {
			if (other.transform.parent.CompareTag("Player")) {
				other.gameObject.GetComponentInParent<PlayerShip>().IncreaseNbCollectible();
				Destroy(this.gameObject);
			}
		}


	}

}