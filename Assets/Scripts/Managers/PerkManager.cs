using System;
using System.Collections.Generic;
using TAOM.Entities.Ships;
using TAOM.Managers;
using TAOM.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TAOM.Gameplay {

	[Serializable]
	public class Perk {
		public string title;
		[Multiline] public string lore;
		public UnityEvent callback;
		public int cost;
		[HideInInspector] public int currentCost = 0;

		public int NextCost() {
			return currentCost + cost;
		}
	}

	public class PerkManager : MonoBehaviour {

		[SerializeField] private GameObject window;
		[SerializeField] private PerkSlot[] slots;
		[SerializeField] private Button closeButton;

		public Perk[] perks;

		private List<Perk> chosenPerks;
		private Game game;
		private InputManager inputManager;
		private PlayerShip player;

		//Specific for controller support
		private int currentSlotSelected;

		private void Awake() {
			game = FindObjectOfType<Game>();
			inputManager = FindObjectOfType<InputManager>();
			player = FindObjectOfType<PlayerShip>();
		}

		private void Update() {
			if (window.activeSelf) {
				UpdateSlotSelection();
				UpdateConfirm();
			}
		}

		#region DISPLAY

		private void UpdateSlotSelection() {
			Direction direction = inputManager.InputDirection();
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

		public void DisplayWindow() {
			currentSlotSelected = 0;

			if (inputManager.InputType.Equals(InputType.CONTROLLER)) {
				slots[currentSlotSelected].GetComponent<Button>();
			}

			chosenPerks = PickRandomPerks();

			for (int i = 0; i < 3; i++)
				slots[i].SetData(chosenPerks[i].title, chosenPerks[i].lore, chosenPerks[i].NextCost());

			window.SetActive(true);
		}

		private List<Perk> PickRandomPerks() {
			List<Perk> randomPerks = new List<Perk>();

			for (int i = 0; i < 3; i++) {
				Perk perk = perks[UnityEngine.Random.Range(0, perks.Length)];
				while (randomPerks.Contains(perk))
					perk = perks[UnityEngine.Random.Range(0, perks.Length)];
				randomPerks.Add(perk);
			}

			return randomPerks;
		}

		private void HighlightButton(int index) {
			EventSystem.current.SetSelectedGameObject(null);
			if (index < 3)
				slots[index].GetComponent<Button>().Select();
			else
				closeButton.Select();
		}

		#endregion

		#region CALLBACKS

		public void OnClickPerkButton(int perkIndex) {
			if (PurchasePerk(chosenPerks[perkIndex]))
				StartPause();
		}

		public void OnClickCloseButton() {
			StartPause();
		}

		#endregion

		private bool PurchasePerk(Perk perk) {
			if (player.PurchasePerk(perk.NextCost())) {
				perk.currentCost += perk.cost;
				perk.callback.Invoke();
				return true;
			}
			return false;
		}

		private void StartPause() {
			window.SetActive(false);
			StartCoroutine(game.StartPause());
		}

	}

}