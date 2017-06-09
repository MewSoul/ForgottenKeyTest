using System.Collections;
using TAOM.Entities.Ships;
using TAOM.Factories;
using TAOM.UI;
using UnityEngine;

namespace TAOM.Gameplay {

	public enum GameState {
		WAVE_IN_PROGRESS,
		WAVE_DONE,
		PERK,
		PAUSED,
		GAME_OVER
	}

	public class Game : MonoBehaviour {

		[SerializeField] private float delayMinBetweenAsteroidSpawn;
		[SerializeField] private float delayMaxBetweenAsteroidSpawn;
		[SerializeField] private float delayBetweenWaves;
		[SerializeField] private float delayBetweenPerkWindow;

		private ShipFactory shipFactory;
		private AsteroidFactory asteroidFactory;
		private Score score;
		private PerkManager perkManager;
		private GameOverController gameOverController;
		public GameState GameState { get; private set; }
		private int currentWave;
		private Coroutine asteroidCoroutine;

		private void Awake() {
			shipFactory = FindObjectOfType<ShipFactory>();
			asteroidFactory = FindObjectOfType<AsteroidFactory>();
			score = FindObjectOfType<Score>();
			perkManager = FindObjectOfType<PerkManager>();
			gameOverController = FindObjectOfType<GameOverController>();
			GameState = GameState.PAUSED;
			currentWave = 0;
		}

		private void Start() {
			StartGame();
		}

		private void StartGame() {
			StartEnemyWave();
			StartAsteroidFlow();
		}

		#region ENEMY WAVES

		private void StartEnemyWave() {
			Debug.Log("WAVE STARTED");
			GameState = GameState.WAVE_IN_PROGRESS;
			StartCoroutine(shipFactory.StartEnemyWave(currentWave, () => OnWaveComplete()));
		}

		public void OnWaveComplete() {
			Debug.Log("WAVE DONE");
			++currentWave;
			GameState = GameState.WAVE_DONE;
		}

		private void CheckPauseBeforeNextWave() {
			Debug.Log("CHECK WAVE, STATE=" + GameState + " COUNT=" + shipFactory.SpawnedEnemies.Count);
			if (GameState.Equals(GameState.WAVE_DONE) && shipFactory.SpawnedEnemies.Count == 0) {
				StopAsteroidFlow();
				StartCoroutine(DisplayPerkWindow());
			}
		}

		public void NotifyEnemyDeath(EnemyShip destroyedEnemy) {
			score.EnemyKilled();
			shipFactory.RemoveDestroyedEnemy(destroyedEnemy);
			CheckPauseBeforeNextWave();
		}

		#endregion

		#region BREAKS

		private IEnumerator DisplayPerkWindow() {
			Debug.Log("DISPLAY PERK WINDOW");
			GameState = GameState.PERK;
			score.WaveCompleted();
			yield return new WaitForSeconds(delayBetweenPerkWindow);
			perkManager.DisplayWindow();
		}

		public IEnumerator StartPause() {
			Debug.Log("START PAUSE");
			GameState = GameState.PAUSED;
			yield return new WaitForSeconds(delayBetweenWaves);
			StartEnemyWave();
			StartAsteroidFlow();
		}

		#endregion

		#region ASTEROIDS

		private void StartAsteroidFlow() {
			asteroidCoroutine = StartCoroutine(asteroidFactory.StartAsteroidWave());
		}

		private void StopAsteroidFlow() {
			StopCoroutine(asteroidCoroutine);
		}

		#endregion

		public void NotifyPlayerDamaged() {
			score.ResetCombo();
		}

		public void GameOver() {
			GameState = GameState.GAME_OVER;
			StopAllCoroutines();
			gameOverController.DisplayGameOverWindow();
		}

	}

}
