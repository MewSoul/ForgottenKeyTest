using TAOM.Managers;
using UnityEngine;

namespace TAOM.Entities.Ships {

	public class Projectile : MonoBehaviour {

		[SerializeField] private AudioClip hitClip;
		[SerializeField] private float volumeSource;

		private AudioManager audioManager;
		private Rigidbody rb;
		private float projectileDamage;

		private void Awake() {
			audioManager = FindObjectOfType<AudioManager>();
			rb = GetComponent<Rigidbody>();
		}

		public void SetData(float rotation, float speed, float damage) {
			this.transform.rotation = Quaternion.Euler(0, rotation, 0);
			this.projectileDamage = damage;
			rb.velocity = transform.forward * speed;
		}

		private void OnTriggerEnter(Collider other) {
			AEntity entity = other.gameObject.GetComponentInParent<AEntity>();
			if (entity != null) {
				entity.Damage(projectileDamage);
				audioManager.PlayClip(hitClip, volumeSource);
			}
			Destroy(this.gameObject);
		}

	}

}
