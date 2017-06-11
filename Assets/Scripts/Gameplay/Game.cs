using DG.Tweening;
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
		private CollectibleMagnet collectibleMagnet;
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
			collectibleMagnet = FindObjectOfType<CollectibleMagnet>();

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
				asteroidFactory.ExplodeAllAsteroids();
				StartCoroutine(collectibleMagnet.BoostMagnet());
				StartCoroutine(DisplayPerkWindow());
			}
		}

		public void NotifyEnemyDeath(EnemyShip destroyedEnemy) {
			if (GameState.Equals(GameState.GAME_OVER))
				return;

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

		public bool CanDoActions() {
			return !GameState.Equals(GameState.PERK) && !GameState.Equals(GameState.GAME_OVER);
		}

		public void NotifyPlayerDamaged() {
			score.ResetCombo();
		}

		public void GameOver() {
			if (GameState.Equals(GameState.GAME_OVER))
				return;

			Debug.Log("GAME OVER");

			GameState = GameState.GAME_OVER;
			StopAllCoroutines();

			asteroidFactory.ExplodeAllAsteroids(true);
			shipFactory.DestroyAllShips();

			Camera.main.DOShakePosition(2f, 4f, 5, 360);
			StartCoroutine(WaitBeforeDisplayGameOver());
		}

		private IEnumerator WaitBeforeDisplayGameOver() {
			yield return new WaitForSeconds(2.2f);
			GameObject.FindGameObjectWithTag("InGameCanvas").SetActive(false);
			gameOverController.DisplayGameOverWindow();
		}

	}

}
