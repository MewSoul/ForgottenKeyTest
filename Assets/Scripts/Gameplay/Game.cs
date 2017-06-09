using System.Collections;
using TAOM.Entities.Ships;
using TAOM.Factories;
using TAOM.UI;
using UnityEngine;

namespace TAOM.Gameplay {

	enum GameState {
		WAVE_IN_PROGRESS,
		WAVE_DONE,
		PAUSED,
		GAME_OVER
	}

	public class Game : MonoBehaviour {

		[SerializeField] private float delayMinBetweenAsteroidSpawn;
		[SerializeField] private float delayMaxBetweenAsteroidSpawn;
		[SerializeField] private float delayBetweenWaves;

		private ShipFactory shipFactory;
		private AsteroidFactory asteroidFactory;
		private Score score;
		private GameOverController gameOverController;
		private GameState gameState;
		private int currentWave;

		private void Awake() {
			shipFactory = FindObjectOfType<ShipFactory>();
			asteroidFactory = FindObjectOfType<AsteroidFactory>();
			score = FindObjectOfType<Score>();
			gameOverController = FindObjectOfType<GameOverController>();
			gameState = GameState.PAUSED;
			currentWave = 0;
		}

		private void Start() {
			StartGame();
		}

		private void StartGame() {
			//	StartEnemyWave();
			StartAsteroidFlow();
		}

		#region ENEMY WAVES

		private void StartEnemyWave() {
			Debug.Log("WAVE STARTED");
			gameState = GameState.WAVE_IN_PROGRESS;
			StartCoroutine(shipFactory.StartEnemyWave(currentWave, () => OnWaveComplete()));
		}

		public void OnWaveComplete() {
			Debug.Log("WAVE DONE");
			++currentWave;
			gameState = GameState.WAVE_DONE;
		}

		private void CheckPauseBeforeNextWave() {
			if (gameState.Equals(GameState.WAVE_DONE) && shipFactory.SpawnedEnemies.Count == 0)
				StartCoroutine(StartPause());
		}

		private IEnumerator StartPause() {
			Debug.Log("START PAUSE");
			score.WaveCompleted();
			gameState = GameState.PAUSED;
			yield return new WaitForSeconds(delayBetweenWaves);
			StartEnemyWave();
		}

		public void NotifyEnemyDeath(EnemyShip destroyedEnemy) {
			score.EnemyKilled();
			shipFactory.RemoveDestroyedEnemy(destroyedEnemy);
			CheckPauseBeforeNextWave();
		}

		#endregion

		#region ASTEROIDS

		private void StartAsteroidFlow() {
			StartCoroutine(asteroidFactory.StartAsteroidWave());
		}

		#endregion

		public void NotifyPlayerDamaged() {
			score.ResetCombo();
		}

		public void GameOver() {
			gameState = GameState.GAME_OVER;
			StopAllCoroutines();
			gameOverController.DisplayGameOverWindow();
		}

	}

}
