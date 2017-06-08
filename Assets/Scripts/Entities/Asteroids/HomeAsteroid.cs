using TAOM.Gameplay;

namespace TAOM.Entities.Asteroids {

	public class HomeAsteroid : AAsteroid {

		private Game game;

		protected override void Awake() {
			base.Awake();
			game = FindObjectOfType<Game>();
		}

		protected override void Die() {
			base.Die();
			game.GameOver();
		}

	}

}