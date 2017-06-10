using UnityEngine;

namespace TAOM.Misc {

	public class AutoDestructParticles : MonoBehaviour {

		private ParticleSystem ps;

		private void Awake() {
			ps = GetComponent<ParticleSystem>();
		}

		void Update() {
			if (!ps.IsAlive())
				Destroy(this.gameObject);
		}

	}

}
