using UnityEngine;

namespace TAOM.Entities.Ships {

	public class CollectibleMagnet : MonoBehaviour {

		private const float MAGNET_RADIUS = 15f;
		private const float MAX_PULL_FORCE = 5f;

		private int collectibleLayer;

		private void Awake() {
			collectibleLayer = 1 << LayerMask.NameToLayer("Collectibles");
		}

		private void Update() {
			foreach (Collider collider in Physics.OverlapSphere(this.transform.position, MAGNET_RADIUS, collectibleLayer)) {
				Vector3 forceDirection = transform.position - collider.transform.position;
				float distance = Vector3.Distance(this.transform.position, collider.transform.position);

				//The closer, the stronger the pull is
				float pullForce = MAX_PULL_FORCE - (distance * MAX_PULL_FORCE / MAGNET_RADIUS);
				pullForce *= pullForce;

				collider.transform.parent.position += forceDirection.normalized * pullForce * Time.fixedDeltaTime;
			}

		}

	}

}