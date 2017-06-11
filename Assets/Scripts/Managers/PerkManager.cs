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

		[SerializeField] private Perk[] perks;

		private PerkWindowController perkWindow;
		private Game game;
		private PlayerShip player;

		private void Awake() {
			perkWindow = FindObjectOfType<PerkWindowController>();
			game = FindObjectOfType<Game>();
			player = FindObjectOfType<PlayerShip>();
		}

		public List<Perk> PickRandomPerks() {
			List<Perk> randomPerks = new List<Perk>();

			for (int i = 0; i < 3; i++) {
				Perk perk = perks[UnityEngine.Random.Range(0, perks.Length)];
				while (randomPerks.Contains(perk))
					perk = perks[UnityEngine.Random.Range(0, perks.Length)];
				randomPerks.Add(perk);
			}

			return randomPerks;
		}

		public bool PurchasePerk(Perk perk) {
			if (player.PurchasePerk(perk.NextCost())) {
				perk.currentCost += perk.cost;
				perk.callback.Invoke();
				return true;
			}
			return false;
		}

		public bool CanPurchasePerk(Perk perk) {
			return player.CanPurchasePerk(perk.NextCost());
		}

		public void DisplayWindow() {
			perkWindow.ShowWindow();
		}

		public void StartPause() {
			StartCoroutine(game.StartPause());
		}

	}

}