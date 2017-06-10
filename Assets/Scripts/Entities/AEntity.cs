using System.Collections;
using TAOM.Managers;
using UnityEngine;

namespace TAOM.Entities {

	public abstract class AEntity : MonoBehaviour {

		[SerializeField] protected float lifePoints;
		[SerializeField] protected float movementSpeed;
		[SerializeField] private int dropRateCollectible;
		[SerializeField] private GameObject collectiblePrefab;
		[SerializeField] private GameObject explosionPrefab;
		[SerializeField] private AudioClip explosionClip;
		[SerializeField] protected float volumeSource;

		protected AudioManager audioManager;
		private Renderer meshRenderer;
		protected Rigidbody rb;
		protected float maxLifePoints;
		private Transform explosionParent;

		protected virtual void Awake() {
			audioManager = FindObjectOfType<AudioManager>();
			meshRenderer = GetComponentInChildren<Renderer>();
			rb = GetComponent<Rigidbody>();
			maxLifePoints = lifePoints;
			explosionParent = GameObject.FindGameObjectWithTag("_Explosions").transform;
		}

		public virtual void Damage(float damagePoint) {
			lifePoints -= damagePoint;

			if (lifePoints <= 0)
				Die();
			else
				StartCoroutine(FlashModel());
		}

		public virtual void Die() {
			DropCollectible();
			if (explosionPrefab != null)
				Instantiate(explosionPrefab, this.transform.position, Quaternion.identity, explosionParent);

			audioManager.PlayClip(explosionClip, volumeSource);

			Destroy(this.gameObject);
		}

		private IEnumerator FlashModel() {
			foreach (Material material in meshRenderer.materials)
				material.color = Color.red;
			yield return new WaitForSeconds(.1f);
			foreach (Material material in meshRenderer.materials)
				material.color = Color.white;
		}

		private void DropCollectible() {
			if (Random.Range(0, 100) < dropRateCollectible) {
				Transform collectibleParent = GameObject.FindGameObjectWithTag("_Collectibles").transform;
				Instantiate(collectiblePrefab, this.transform.position, Quaternion.identity, collectibleParent);
			}
		}

	}

}
