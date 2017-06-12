using TAOM.Gameplay;
using UnityEngine;
using UnityEngine.UI;

namespace TAOM.Entities.Asteroids {

	public class HomeAsteroid : AAsteroid {

		private Game game;

		[SerializeField] private Slider lifeSlider;
		[SerializeField] private Text currentLifeText;
		[SerializeField] private Text maxLifeText;

		protected override void Awake() {
			base.Awake();
			game = FindObjectOfType<Game>();
		}

		public override void Damage(float damagePoint) {
			base.Damage(damagePoint);
			UpdateUIData();
			inputManager.VibrateController(0.3f, 0.3f, 0.2f);
		}

		public override void Die() {
			base.Die();
			game.GameOver();
			inputManager.VibrateController(0.6f, 0.6f, 1f);
		}

		private void UpdateUIData() {
			currentLifeText.text = lifePoints.ToString();
			lifeSlider.value = lifePoints;
		}

		#region PERKS

		public void AddLife() {
			//Add 2 life points
			lifePoints = Mathf.Clamp(lifePoints + 3, lifePoints + 3, maxLifePoints);
			currentLifeText.text = lifePoints.ToString();
			lifeSlider.value = lifePoints;
			Debug.Log("Add 2 life points");
		}

		#endregion

	}

}