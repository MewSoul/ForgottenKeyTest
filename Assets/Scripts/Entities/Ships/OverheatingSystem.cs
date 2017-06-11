using DG.Tweening;
using TAOM.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace TAOM.Entities.Ships {

	enum HeatState {
		IN_ORDER,
		OUT_OF_ORDER
	}

	public class OverheatingSystem : MonoBehaviour {

		[SerializeField] private Slider heatSlider;
		[SerializeField] private Image fillSliderImage;
		[SerializeField] private Gradient heatGradient;
		[SerializeField] private Text currentValue;
		[SerializeField] private Text maxValue;

		[SerializeField] private float maxHeatResistance;
		[SerializeField] private float heatAddedPerShot;
		[SerializeField] private float cooldownDelay;

		[SerializeField] private AudioClip stateOKClip;
		[SerializeField] private AudioClip stateKOClip;
		[SerializeField] private float volumeSource;

		private AudioManager audioManager;
		private HeatState currentState;
		private float currentHeat;
		private float nextTimeStartCooling;

		private void Awake() {
			audioManager = FindObjectOfType<AudioManager>();
			currentHeat = 0;
			currentState = HeatState.IN_ORDER;
		}

		private void Update() {
			if (Time.time > nextTimeStartCooling && currentHeat > 0)
				--currentHeat;

			if (currentState.Equals(HeatState.OUT_OF_ORDER) && currentHeat == 0) {
				currentState = HeatState.IN_ORDER;
				audioManager.PlayClip(stateOKClip, volumeSource);
			}

			UpdateSlider();
		}

		#region SYSTEM MANAGEMENT

		public bool IsInOrder() {
			return currentState.Equals(HeatState.IN_ORDER);
		}

		public bool Heat() {
			currentHeat = Mathf.Clamp(currentHeat + heatAddedPerShot, 0, maxHeatResistance);

			if (currentHeat == maxHeatResistance) {
				currentState = HeatState.OUT_OF_ORDER;
				audioManager.PlayClip(stateKOClip, volumeSource);
			}

			nextTimeStartCooling = Time.time + cooldownDelay;

			return true;
		}

		#endregion

		#region DISPLAY

		private void UpdateSlider() {
			heatSlider.DOValue(currentHeat, 0.2f);
			fillSliderImage.color = heatGradient.Evaluate(currentHeat / maxHeatResistance);
			currentValue.text = currentHeat.ToString();
		}

		#endregion

		#region PERKS

		public void IncreaseMaxHeatResistance() {
			//Increase max heat resistance by 25%
			maxHeatResistance *= 1.25f;
			maxValue.text = maxHeatResistance.ToString();
			heatSlider.maxValue = maxHeatResistance;
			Debug.Log("Increase max heat resistance");
		}

		public void DecreaseDelayCooldown() {
			//Decrease delay cooldown by 25%
			cooldownDelay *= 0.75f;
			Debug.Log("Decrease delay cooldown");
		}

		#endregion

	}

}
