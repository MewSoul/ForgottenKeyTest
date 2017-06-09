using UnityEngine;
using UnityEngine.UI;

namespace TAOM.UI {

	public class PerkSlot : MonoBehaviour {

		[SerializeField] private Text titleText;
		[SerializeField] private Text loreText;
		[SerializeField] private Text costText;

		public void SetData(string title, string lore, int cost) {
			titleText.text = title;
			loreText.text = lore;
			costText.text = "Cost: " + cost;
		}

	}

}
