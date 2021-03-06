﻿using System.Collections;
using TAOM.Entities.Ships;
using TAOM.Managers;
using UnityEngine;

namespace TAOM.Gameplay {

	public class Collectible : MonoBehaviour {

		private const float DISTANCE_MAGNET = 10f;

		[SerializeField] private float rotationSpeed;
		[SerializeField] private float lifeTime;
		[SerializeField] private AudioClip pickupClip;
		[SerializeField] private float volumeSource;
		[SerializeField] private float pitchSource;

		private AudioManager audioManager;
		private Rigidbody rb;
		private float currentLifeTime;
		private Coroutine flashCoroutine;
		private Behaviour halo;

		private void Awake() {
			audioManager = FindObjectOfType<AudioManager>();
			rb = GetComponent<Rigidbody>();
			rb.angularVelocity = Random.insideUnitSphere * rotationSpeed;
			flashCoroutine = null;
			halo = (Behaviour) GetComponent("Halo");
		}

		private void Start() {
			currentLifeTime = 0;
		}

		private void Update() {
			currentLifeTime += Time.deltaTime;

			if (currentLifeTime >= lifeTime) {
				StopCoroutine(flashCoroutine);
				Destroy(this.gameObject);
			} else if (flashCoroutine == null && currentLifeTime >= lifeTime - 2)
				flashCoroutine = StartCoroutine(FlashModel());
		}

		private IEnumerator FlashModel() {
			while (true) {
				halo.enabled = false;
				yield return new WaitForSeconds(0.05f);
				halo.enabled = true;
				yield return new WaitForSeconds(0.1f);
			}
		}

		private void OnTriggerEnter(Collider other) {
			if (other.transform.parent.CompareTag("Player")) {
				other.gameObject.GetComponentInParent<PlayerShip>().IncreaseNbCollectible();
				audioManager.PlayClip(pickupClip, volumeSource, pitchSource);
				Destroy(this.gameObject);
			}
		}

	}

}