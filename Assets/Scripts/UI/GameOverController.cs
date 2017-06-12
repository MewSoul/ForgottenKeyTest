using DG.Tweening;
using System.Collections.Generic;
using TAOM.Gameplay;
using TAOM.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TAOM.UI {

	enum WindowState {
		SUBMIT_SCORE,
		LOADING_SCORES,
		DONE
	}

	public class GameOverController : MonoBehaviour {

		private const int NB_MAX_SCORE_DISPLAY = 5;

		[SerializeField] private GameObject window;
		[SerializeField] private Text scoreText;
		[SerializeField] private InputField playerNameInput;
		[SerializeField] private Button submitButton;
		[SerializeField] private Text leaderboardText;
		[SerializeField] private Button replayButton;
		[SerializeField] private AudioClip hoverButtonClip;
		[SerializeField] private AudioClip buttonCloseClip;
		[SerializeField] private float volumeSource;

		private InputManager inputManager;
		private AudioManager audioManager;
		private Score score;
		private WindowState currentState;

		private void Awake() {
			inputManager = FindObjectOfType<InputManager>();
			audioManager = FindObjectOfType<AudioManager>();
			score = FindObjectOfType<Score>();
			currentState = WindowState.SUBMIT_SCORE;
		}

		private void Update() {
			if (window.activeSelf)
				UpdateConfirm();
			if (currentState.Equals(WindowState.LOADING_SCORES))
				DisplayLeaderboard();
		}

		private void UpdateConfirm() {
			if (inputManager.InputConfirm()) {
				if (currentState.Equals(WindowState.SUBMIT_SCORE))
					OnClickSubmitButton();
				else
					OnClickReplayButton();
			}
		}

		#region DISPLAYS

		public void DisplayGameOverWindow() {
			scoreText.text = score.CurrentScore.ToString();
			window.transform.localScale = Vector3.zero;
			window.SetActive(true);
			window.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack);
		}

		public void HideSubmitComponents() {
			playerNameInput.gameObject.SetActive(false);
			submitButton.gameObject.SetActive(false);
		}

		public void DisplayLeaderboard() {
			List<dreamloLeaderBoard.Score> leaderboard = score.RetrieveLeaderBoard();

			if (leaderboard == null)
				return;

			int position = 1;
			leaderboardText.text = "";
			foreach (dreamloLeaderBoard.Score currentScore in leaderboard) {
				leaderboardText.text += position + " - " + currentScore.playerName + " " + currentScore.score + "\n";
				++position;
				if (position > NB_MAX_SCORE_DISPLAY)
					break;
			}

			currentState = WindowState.DONE;
		}

		private void DisplayReplayButton() {
			replayButton.gameObject.SetActive(true);
		}

		public void HighlightButton() {
			if (currentState.Equals(WindowState.SUBMIT_SCORE))
				submitButton.GetComponent<Outline>().enabled = true;
			else
				replayButton.GetComponent<Outline>().enabled = true;
			audioManager.PlayClip(hoverButtonClip, volumeSource);
		}

		public void DisableHighlight() {
			if (currentState.Equals(WindowState.SUBMIT_SCORE))
				submitButton.GetComponent<Outline>().enabled = false;
			else
				replayButton.GetComponent<Outline>().enabled = false;
		}

		#endregion

		#region CALLBACKS

		public void OnClickSubmitButton() {
			if (playerNameInput.text.Length == 0)
				return;
			audioManager.PlayClip(buttonCloseClip, volumeSource);
			score.SubmitScore(playerNameInput.text);
			currentState = WindowState.LOADING_SCORES;
			HideSubmitComponents();
			leaderboardText.text = "Loading scores...";
			DisplayReplayButton();
		}

		public void OnClickReplayButton() {
			audioManager.PlayClip(buttonCloseClip, volumeSource);
			SceneManager.LoadScene(0);
		}

		#endregion

	}

}