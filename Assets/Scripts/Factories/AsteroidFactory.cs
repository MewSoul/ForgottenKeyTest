using System.Collections;
using TAOM.Gameplay;
using UnityEngine;

namespace TAOM.Factories {

	public class AsteroidFactory : MonoBehaviour {

		[SerializeField] private GameObject asteroidPrefab;

		[SerializeField] private float delayMinBetweenAsteroidSpawn;
		[SerializeField] private float delayMaxBetweenAsteroidSpawn;

		private Map map;
		private Transform asteroidParent;

		private void Awake() {
			map = FindObjectOfType<Map>();
			asteroidParent = GameObject.FindGameObjectWithTag("_Asteroids").transform;
		}

		public IEnumerator StartAsteroidWave() {
			while (true) {
				Instantiate(asteroidPrefab, map.GetRandomPositionBorderMap(), Quaternion.identity, asteroidParent);
				yield return new WaitForSeconds(UnityEngine.Random.Range(delayMinBetweenAsteroidSpawn, delayMaxBetweenAsteroidSpawn));
			}
		}

	}

}