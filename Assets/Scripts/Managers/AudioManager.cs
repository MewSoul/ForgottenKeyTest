using UnityEngine;

namespace TAOM.Managers {

	public class AudioManager : MonoBehaviour {

		private Transform clipParent;

		private void Awake() {
			clipParent = GameObject.FindGameObjectWithTag("_Clips").transform;
		}

		public void PlayClip(AudioClip clip, float volumeSource, float pitch = 1) {
			//Done so can play sound even when object destroyed right away
			GameObject tempGO = new GameObject("Clip");
			tempGO.transform.position = this.transform.position;

			AudioSource source = tempGO.AddComponent<AudioSource>();
			source.transform.parent = clipParent;
			source.clip = clip;
			source.volume = volumeSource;
			source.pitch = pitch;
			source.Play();

			Destroy(tempGO, clip.length);
		}

	}

}