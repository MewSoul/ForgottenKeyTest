using System;
using System.Collections;
using System.Collections.Generic;
using TAOM.Entities.Ships;
using TAOM.Gameplay;
using UnityEngine;

namespace TAOM.Factories {

	public class ShipFactory : MonoBehaviour {

		[SerializeField] private EnemyShip enemyShipPrefab;

		[SerializeField] private int baseMinEnemyToSpawn;
		[SerializeField] private int baseMaxEnemyToSpawn;
		[SerializeField] private int nbEnemyMorePerWaveDone;

		[SerializeField] private float delayMinBetweenEnemySpawn;
		[SerializeField] private float delayMaxBetweenEnemySpawn;

		private Map map;
		private Transform enemyShipParent;
		public List<EnemyShip> SpawnedEnemies { get; private set; }
		private PlayerShip playerShip;

		private void Awake() {
			map = FindObjectOfType<Map>();
			enemyShipParent = GameObject.FindGameObjectWithTag("_Enemies").transform;
			SpawnedEnemies = new List<EnemyShip>();
			playerShip = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerShip>();
		}

		public IEnumerator StartEnemyWave(int waveNb, Action onWaveComplete) {
			int nbEnemyToSpawn = UnityEngine.Random.Range(baseMinEnemyToSpawn, baseMinEnemyToSpawn) + waveNb * nbEnemyMorePerWaveDone;

			for (int i = 0; i < nbEnemyToSpawn; i++) {
				yield return new WaitForSeconds(UnityEngine.Random.Range(delayMinBetweenEnemySpawn, delayMaxBetweenEnemySpawn));
				SpawnEnemyShip();
			}

			onWaveComplete();
		}

		private void SpawnEnemyShip() {
			SpawnedEnemies.Add(Instantiate(enemyShipPrefab, map.GetRandomPositionBorderMap(), Quaternion.identity, enemyShipParent));
		}

		public void RemoveDestroyedEnemy(EnemyShip destroyedEnemy) {
			SpawnedEnemies.Remove(destroyedEnemy);
		}

		public void DestroyAllShips() {
			foreach (EnemyShip ship in SpawnedEnemies) {
				ship.Die();
			}
			if (playerShip.gameObject != null)
				playerShip.Die();
		}

	}

}