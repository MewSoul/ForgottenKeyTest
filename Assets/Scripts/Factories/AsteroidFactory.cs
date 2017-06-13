using System.Collections;
using TAOM.Entities.Asteroids;
using TAOM.Gameplay;
using UnityEngine;

namespace TAOM.Factories {

	public class AsteroidFactory : MonoBehaviour {

		[SerializeField] private GameObject asteroidPrefab;

		[SerializeField] private float delayMinBetweenAsteroidSpawn;
		[SerializeField] private float delayMaxBetweenAsteroidSpawn;

		private Map map;
		private Transform asteroidParent;
		private HomeAsteroid homeAsteroid;

		private void Awake() {
			map = FindObjectOfType<Map>();
			asteroidParent = GameObject.FindGameObjectWithTag("_Asteroids").transform;
			homeAsteroid = GameObject.FindGameObjectWithTag("HomeAsteroid").GetComponent<HomeAsteroid>();
		}

		public IEnumerator StartAsteroidWave() {
			while (true) {
				yield return new WaitForSeconds(UnityEngine.Random.Range(delayMinBetweenAsteroidSpawn, delayMaxBetweenAsteroidSpawn));
				Instantiate(asteroidPrefab, map.GetRandomPositionBorderMap(), Quaternion.identity, asteroidParent);
			}
		}

		public void ExplodeAllAsteroids(bool destroyHomeAsteroid = false) {
			foreach (GameObject asteroid in GameObject.FindGameObjectsWithTag("Asteroid"))
				asteroid.GetComponent<MovingAsteroid>().Die();

			if (destroyHomeAsteroid)
				homeAsteroid.Die();
		}

		#region PERKS

		public void DecreaseDelayMaxAsteroidSpawn() {
			//Decrease by 25% the delay max between each asteroid spawn
			delayMaxBetweenAsteroidSpawn *= 0.75f;
		}

		#endregion

	}

}