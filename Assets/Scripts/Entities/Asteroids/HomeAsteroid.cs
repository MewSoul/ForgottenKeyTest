using TAOM.Gameplay;
using UnityEngine;

namespace TAOM.Entities.Asteroids {

	public class HomeAsteroid : AAsteroid {

		private Game game;

		protected override void Awake() {
			base.Awake();
			game = FindObjectOfType<Game>();
		}

		public override void Die() {
			base.Die();
			game.GameOver();
		}

		#region PERKS

		public void AddLife() {
			//Add 2 life points
			lifePoints = Mathf.Clamp(lifePoints + 3, lifePoints + 3, maxLifePoints);
			Debug.Log("Add 2 life points");
		}

		#endregion

	}

}