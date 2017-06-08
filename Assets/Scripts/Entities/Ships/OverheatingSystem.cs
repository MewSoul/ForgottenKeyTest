using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace TAOM.Entities.Ships {

	enum HeatState {
		IN_ORDER,
		OUT_OF_ORDER
	}

	public class OverheatingSystem : MonoBehaviour {

		[SerializeField] private Slider heatSlider;

		[SerializeField] private float maxHeatResistance;
		[SerializeField] private float heatAddedPerShot;
		[SerializeField] private float cooldownDelay;

		private HeatState currentState;
		private float currentHeat;
		private float nextTimeStartCooling;

		private void Awake() {
			currentHeat = 0;
			currentState = HeatState.IN_ORDER;
		}

		private void Update() {
			if (Time.time > nextTimeStartCooling)
				--currentHeat;

			if (currentState.Equals(HeatState.OUT_OF_ORDER) && currentHeat == 0)
				currentState = HeatState.IN_ORDER;

			UpdateSlider();
		}

		public bool IsInOrder() {
			return currentState.Equals(HeatState.IN_ORDER);
		}

		public bool Heat() {
			currentHeat = Mathf.Clamp(currentHeat + heatAddedPerShot, 0, maxHeatResistance);

			if (currentHeat == maxHeatResistance)
				currentState = HeatState.OUT_OF_ORDER;

			nextTimeStartCooling = Time.time + cooldownDelay;

			return true;
		}

		private void UpdateSlider() {
			heatSlider.DOValue(currentHeat, 0.2f);
		}

	}

}
