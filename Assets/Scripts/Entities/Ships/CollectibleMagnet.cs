using System.Collections;
using UnityEngine;

namespace TAOM.Entities.Ships {

	public class CollectibleMagnet : MonoBehaviour {

		private const float BOOST_RADIUS = 150f;
		private const float MAGNET_RADIUS = 15f;
		private const float BOOST_FORCE = 15f;
		private const float PULL_FORCE = 5f;

		private int collectibleLayer;
		private float currentRadius;
		private float currentForce;

		private void Awake() {
			collectibleLayer = 1 << LayerMask.NameToLayer("Collectibles");
			currentRadius = MAGNET_RADIUS;
			currentForce = PULL_FORCE;
		}

		private void Update() {
			foreach (Collider collider in Physics.OverlapSphere(this.transform.position, currentRadius, collectibleLayer)) {
				Vector3 forceDirection = transform.position - collider.transform.position;
				float distance = Vector3.Distance(this.transform.position, collider.transform.position);

				//The closer, the stronger the pull is
				float pullForce = currentForce - (distance * currentForce / currentRadius);
				pullForce *= pullForce;

				collider.transform.parent.position += forceDirection.normalized * pullForce * Time.fixedDeltaTime;
			}

		}

		public IEnumerator BoostMagnet() {
			//Boost magnet when you can't move so you can collect everything before perk purchase
			currentRadius = BOOST_RADIUS;
			currentForce = BOOST_FORCE;
			yield return new WaitForSeconds(3f);
			currentRadius = MAGNET_RADIUS;
			currentForce = PULL_FORCE;
		}

	}

}