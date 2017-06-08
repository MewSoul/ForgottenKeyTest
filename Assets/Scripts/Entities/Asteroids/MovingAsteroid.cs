﻿using System.Collections;
using TAOM.Gameplay;
using UnityEngine;

namespace TAOM.Entities.Asteroids {

	public class MovingAsteroid : AAsteroid {

		private Map map;

		protected override void Awake() {
			base.Awake();
			map = FindObjectOfType<Map>();
		}

		private void Start() {
			transform.LookAt(GameObject.FindGameObjectWithTag("Player").transform);
			rb.velocity = transform.forward * movementSpeed;
		}

		private void OnTriggerEnter(Collider other) {
			if (other.transform.parent.CompareTag("Player") || other.transform.parent.CompareTag("HomeAsteroid")) {
				Explode();
			} else if (other.CompareTag("Wall")) {
				StartCoroutine(CheckDestroy());
			}

		}

		private void Explode() {
			//TODO Spawn explosion + damage
			Die();
		}

		private IEnumerator CheckDestroy() {
			//Trick to check if asteroid is outside of the map area as it spawns outside
			yield return new WaitForSeconds(2f);
			if (!map.IsPositionInsideMapArea(this.transform.position))
				Die();
		}

	}

}