using System;
using System.Collections;
using System.Collections.Generic;
using TAOM.Entities.Ships;
using TAOM.Gameplay;
using UnityEngine;

namespace TAOM.Factories {

	public class ShipFactory : MonoBehaviour {

		[SerializeField] private EnemyShip[] enemyShipPrefabs;

		[SerializeField] private int baseMinEnemyToSpawn;
		[SerializeField] private int baseMaxEnemyToSpawn;
		[SerializeField] private int nbEnemyMorePerWaveDone;

		[SerializeField] private float delayMinBetweenEnemySpawn;
		[SerializeField] private float delayMaxBetweenEnemySpawn;

		private Map map;
		private Transform enemyShipParent;
		public List<EnemyShip> SpawnedEnemies { get; private set; }
		private PlayerShip playerShip;
		private int spawnRateEliteEnemies;

		private void Awake() {
			map = FindObjectOfType<Map>();
			enemyShipParent = GameObject.FindGameObjectWithTag("_Enemies").transform;
			SpawnedEnemies = new List<EnemyShip>();
			playerShip = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerShip>();
			spawnRateEliteEnemies = 0;
		}

		public IEnumerator StartEnemyWave(int waveNb, Action onWaveComplete) {
			int nbEnemyToSpawn = UnityEngine.Random.Range(baseMinEnemyToSpawn, baseMinEnemyToSpawn) + waveNb * nbEnemyMorePerWaveDone;

			for (int i = 0; i < nbEnemyToSpawn; i++) {
				yield return new WaitForSeconds(UnityEngine.Random.Range(delayMinBetweenEnemySpawn, delayMaxBetweenEnemySpawn));
				SpawnEnemyShip();
			}

			spawnRateEliteEnemies += 10;
			if (spawnRateEliteEnemies > 100)
				spawnRateEliteEnemies = 100;

			onWaveComplete();
		}

		private void SpawnEnemyShip() {
			int value = UnityEngine.Random.Range(0, 100);
			EnemyShip enemyToSpawn = (spawnRateEliteEnemies < value) ? enemyShipPrefabs[0] : enemyShipPrefabs[1];
			SpawnedEnemies.Add(Instantiate(enemyToSpawn, map.GetRandomPositionBorderMap(), Quaternion.identity, enemyShipParent));
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