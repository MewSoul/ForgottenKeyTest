using TAOM.Managers;
using UnityEngine;

namespace TAOM.Entities.Ships {

	public class Crosshair : MonoBehaviour {

		private InputManager inputManager;
		private RectTransform rectTransform;

		private void Awake() {
			inputManager = FindObjectOfType<InputManager>();
			rectTransform = GetComponent<RectTransform>();
		}

		private void Update() {
			if (inputManager.InputType.Equals(InputType.MOUSE)) {
				Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				pos.y = 3;
				rectTransform.position = pos;
			} else {
				//TODO if enough time, compute position so crosshair stays inside camera view
				rectTransform.localPosition = new Vector3(0, 10, -3);
			}
		}

	}

}