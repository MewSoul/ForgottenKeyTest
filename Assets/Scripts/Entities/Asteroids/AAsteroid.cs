using UnityEngine;

namespace TAOM.Entities.Asteroids {

	public abstract class AAsteroid : AEntity {

		[SerializeField] private float rotationSpeed;

		protected override void Awake() {
			rb = GetComponent<Rigidbody>();
			rb.angularVelocity = Random.insideUnitSphere * rotationSpeed;
		}

	}

}