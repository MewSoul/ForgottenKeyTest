using UnityEngine;
using UnityEngine.UI;

namespace TAOM.UI {

	public class PerkSlot : MonoBehaviour {

		[SerializeField] private Text titleText;
		[SerializeField] private Text loreText;
		[SerializeField] private Text costText;
		[SerializeField] private Color canPurchaseColor;
		[SerializeField] private Color canNotPurchaseColor;

		public void SetData(string title, string lore, int cost, bool canPurchase) {
			titleText.text = title;
			loreText.text = lore;
			costText.text = "Cost: " + cost;
			if (canPurchase)
				costText.color = canPurchaseColor;
			else
				costText.color = canNotPurchaseColor;
		}

	}

}
