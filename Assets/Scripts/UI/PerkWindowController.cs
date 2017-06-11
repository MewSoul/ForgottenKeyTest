using System.Collections.Generic;
using TAOM.Gameplay;
using TAOM.Managers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TAOM.UI {

	public class PerkWindowController : MonoBehaviour {

		[SerializeField] private GameObject window;
		[SerializeField] private PerkSlot[] slots;
		[SerializeField] private Button closeButton;
		[SerializeField] private AudioClip hoverButtonClip;
		[SerializeField] private AudioClip purchaseSuccessClip;
		[SerializeField] private AudioClip purchaseFailureClip;
		[SerializeField] private AudioClip showWindowClip;
		[SerializeField] private AudioClip hideWindowClip;
		[SerializeField] private AudioClip buttonCloseClip;
		[SerializeField] private float volumeSource;

		private InputManager inputManager;
		private PerkManager perkManager;
		private AudioManager audioManager;
		private List<Perk> chosenPerks;

		//Specific for controller support
		private int currentSlotSelected;

		private void Awake() {
			inputManager = FindObjectOfType<InputManager>();
			perkManager = FindObjectOfType<PerkManager>();
			audioManager = FindObjectOfType<AudioManager>();
		}

		private void Update() {
			if (window.activeSelf) {
				UpdateSlotSelection();
				UpdateConfirm();
			}
		}

		#region DISPLAY

		public void ShowWindow() {
			currentSlotSelected = 0;

			if (inputManager.InputType.Equals(InputType.CONTROLLER))
				slots[currentSlotSelected].GetComponent<Button>();

			chosenPerks = perkManager.PickRandomPerks();

			for (int i = 0; i < 3; i++)
				slots[i].SetData(chosenPerks[i].title, chosenPerks[i].lore, chosenPerks[i].NextCost());

			window.SetActive(true);
			audioManager.PlayClip(showWindowClip, volumeSource);
		}

		private void HideWindow() {
			window.SetActive(false);
			DisableHighlight(currentSlotSelected);
			perkManager.StartPause();
			audioManager.PlayClip(hideWindowClip, volumeSource);
		}

		private void UpdateSlotSelection() {
			Direction direction = inputManager.InputDirection();

			if (direction == 0)
				return;

			currentSlotSelected += (int) direction;

			//Cycle through slots and close button
			if (currentSlotSelected > 3)
				currentSlotSelected = 0;
			if (currentSlotSelected < 0)
				currentSlotSelected = 3;

			HighlightButton(currentSlotSelected);
		}

		private void UpdateConfirm() {
			if (inputManager.InputConfirm()) {
				if (currentSlotSelected < 3)
					OnClickPerkButton(currentSlotSelected);
				else
					OnClickCloseButton();
			}
		}

		public void HighlightButton(int index) {
			DisableHighlight(currentSlotSelected);

			//Force save slot in case hover done by mouse
			currentSlotSelected = index;

			if (index < 3) {
				slots[index].GetComponent<Button>().Select();
				slots[index].GetComponent<Outline>().enabled = true;
			} else {
				closeButton.Select();
				closeButton.GetComponent<Outline>().enabled = true;
			}
			audioManager.PlayClip(hoverButtonClip, volumeSource);
		}

		public void DisableHighlight(int index) {
			EventSystem.current.SetSelectedGameObject(null);
			if (index < 3)
				slots[index].GetComponent<Outline>().enabled = false;
			else
				closeButton.GetComponent<Outline>().enabled = false;
		}

		#endregion

		#region CALLBACKS

		public void OnClickPerkButton(int perkIndex) {
			if (perkManager.PurchasePerk(chosenPerks[perkIndex])) {
				audioManager.PlayClip(purchaseSuccessClip, volumeSource);
				HideWindow();
			} else
				audioManager.PlayClip(purchaseFailureClip, volumeSource);

		}

		public void OnClickCloseButton() {
			audioManager.PlayClip(buttonCloseClip, volumeSource);
			HideWindow();
		}

		#endregion

	}

}