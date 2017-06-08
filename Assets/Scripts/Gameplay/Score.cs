using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TAOM.Gameplay {

	public class Score : MonoBehaviour {

		[SerializeField] private Text scoreText;
		[SerializeField] private Text comboText;
		[SerializeField] private int valueEnemyKilled;
		[SerializeField] private int valueWaveCompleted;

		private dreamloLeaderBoard leaderBoard;
		public int CurrentScore { get; private set; }
		private int currentCombo;

		private void Awake() {
			leaderBoard = dreamloLeaderBoard.GetSceneDreamloLeaderboard();
			CurrentScore = 0;
			currentCombo = 1;
		}

		public void EnemyKilled() {
			CurrentScore += valueEnemyKilled * currentCombo;
			UpdateScoreText();
			IncreaseCombo();
		}

		public void WaveCompleted() {
			CurrentScore += valueWaveCompleted;
			UpdateScoreText();
		}

		public void IncreaseCombo() {
			++currentCombo;
			UpdateComboText();
		}

		public void ResetCombo() {
			currentCombo = 0;
		}

		private void UpdateScoreText() {
			scoreText.text = CurrentScore.ToString();
		}

		private void UpdateComboText() {
			comboText.text = "x" + currentCombo;
		}

		public void SubmitScore(string playerName) {
			leaderBoard.AddScore(playerName, CurrentScore);
		}

		public List<dreamloLeaderBoard.Score> RetrieveLeaderBoard() {
			return leaderBoard.ToListHighToLow();
		}

	}

}