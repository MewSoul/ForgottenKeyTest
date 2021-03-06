﻿using TAOM.Gameplay;
using UnityEngine;

namespace TAOM.Entities.Ships {

	public class AShip : AEntity {

		[SerializeField] private Projectile projectilePrefab;
		[SerializeField] private Transform projectileSpawn;
		[SerializeField] protected float projectileDamage;
		[SerializeField] private float projectileSpeed;
		[SerializeField] protected float shootDelay;
		[SerializeField] private AudioClip shootClip;

		protected Game game;
		private Transform projectileParent;
		private float nextTimeShoot;
		protected float projectileScale;

		protected override void Awake() {
			base.Awake();
			game = FindObjectOfType<Game>();
			projectileParent = GameObject.FindGameObjectWithTag("_Projectiles").transform;
			projectileScale = 1f;
		}

		protected virtual void Start() {
			nextTimeShoot = Time.time;
		}

		protected bool Fire() {
			if (Time.time > nextTimeShoot && game.CanDoActions()) {
				nextTimeShoot = Time.time + shootDelay;

				audioManager.PlayClip(shootClip, volumeSource);

				Projectile instance = Instantiate(projectilePrefab, projectileSpawn.position + RandomOffsetProjectile(), Quaternion.identity, projectileParent);
				instance.transform.localScale = new Vector3(projectileScale, projectileScale, projectileScale);
				instance.GetComponent<Projectile>().SetData(this.transform.rotation.eulerAngles.y, projectileSpeed, projectileDamage);

				//Doesn't collide with own ship when projectile bigger because of the perk
				Physics.IgnoreCollision(instance.GetComponent<Collider>(), this.GetComponentInChildren<Collider>());
				return true;
			}
			return false;
		}

		private Vector3 RandomOffsetProjectile() {
			return new Vector3(Random.Range(-0.5f, 0.5f), 0, Random.Range(-0.5f, 0.5f));
		}

	}
}
