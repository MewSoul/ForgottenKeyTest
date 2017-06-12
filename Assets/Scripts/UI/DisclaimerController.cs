using DG.Tweening;
using TAOM.Gameplay;
using TAOM.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace TAOM.UI {

	public class DisclaimerController : MonoBehaviour {

		[SerializeField] private GameObject window;
		[SerializeField] private Outline outline;
		[SerializeField] private AudioClip hoverButtonClip;
		[SerializeField] private AudioClip buttonCloseClip;
		[SerializeField] private float volumeSource;

		private InputManager inputManager;
		private AudioManager audioManager;
		private Game game;

		private void Awake() {
			inputManager = FindObjectOfType<InputManager>();
			audioManager = FindObjectOfType<AudioManager>();
			game = FindObjectOfType<Game>();
		}

		private void Update() {
			if (window.activeSelf)
				UpdateConfirm();
		}

		private void UpdateConfirm() {
			if (inputManager.InputConfirm()) {
				OnClickCloseButton();
			}
		}

		#region DISPLAY

		public void ShowWindow() {
			PlayerPrefs.SetInt("Disclaimer", 0);
			PlayerPrefs.Save();
			window.transform.localScale = Vector3.zero;
			window.SetActive(true);
			window.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack);
		}

		public void HideWindow() {
			window.transform.DOScale(0f, 0.5f).SetEase(Ease.InBack).OnComplete(() => window.SetActive(false));
		}

		public void HighlightButton() {
			outline.enabled = true;
			audioManager.PlayClip(hoverButtonClip, volumeSource);
		}

		public void DisableHighlight() {
			outline.enabled = false;
		}

		#endregion

		#region CALLBACKS

		public void OnClickCloseButton() {
			audioManager.PlayClip(buttonCloseClip, volumeSource);
			HideWindow();
			game.StartGame();
		}

		#endregion

	}

}