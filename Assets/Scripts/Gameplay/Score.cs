using UnityEngine;
using UnityEngine.UI;

namespace TAOM.Gameplay {

	public class Score : MonoBehaviour {

		[SerializeField] private Text scoreText;
		[SerializeField] private Text comboText;
		[SerializeField] private int valueEnemyKilled;
		[SerializeField] private int valueWaveCompleted;

		private int currentScore;
		private int currentCombo;

		private void Awake() {
			currentScore = 0;
			currentCombo = 1;
		}

		public void EnemyKilled() {
			currentScore += valueEnemyKilled * currentCombo;
			UpdateScoreText();
			IncreaseCombo();
		}

		public void WaveCompleted() {
			currentScore += valueWaveCompleted;
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
			scoreText.text = currentScore.ToString();
		}

		private void UpdateComboText() {
			comboText.text = "x" + currentCombo;
		}

	}

}