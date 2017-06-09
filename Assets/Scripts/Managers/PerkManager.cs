using System;
using System.Collections.Generic;
using TAOM.Entities.Ships;
using TAOM.UI;
using UnityEngine;
using UnityEngine.Events;

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

		public Perk[] perks;

		private List<Perk> chosenPerks;
		private Game game;
		private PlayerShip player;

		private void Awake() {
			game = FindObjectOfType<Game>();
			player = FindObjectOfType<PlayerShip>();
		}

		public void ApplyRandomPerk() {
			PurchasePerk(perks[UnityEngine.Random.Range(0, perks.Length)]);
		}

		private bool PurchasePerk(Perk perk) {
			if (player.PurchasePerk(perk.NextCost())) {
				perk.currentCost += perk.cost;
				perk.callback.Invoke();
				return true;
			}
			return false;
		}

		public void DisplayWindow() {
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

		public void OnClickPerkButton(int perkIndex) {
			if (PurchasePerk(chosenPerks[perkIndex]))
				StartPause();
		}

		public void OnClickCloseButton() {
			StartPause();
		}

		private void StartPause() {
			window.SetActive(false);
			StartCoroutine(game.StartPause());
		}
	}

}