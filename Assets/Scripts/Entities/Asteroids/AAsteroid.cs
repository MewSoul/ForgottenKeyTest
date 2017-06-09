using UnityEngine;

namespace TAOM.Entities.Asteroids {

	public abstract class AAsteroid : AEntity {

		[SerializeField] private float rotationSpeed;

		protected override void Awake() {
			base.Awake();
			rb.angularVelocity = Random.insideUnitSphere * rotationSpeed;
		}

	}

}