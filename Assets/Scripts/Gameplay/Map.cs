using UnityEngine;

namespace TAOM.Gameplay {

	public class Map : MonoBehaviour {

		[SerializeField] private GameObject wallPrefab;

		private Rect cameraRect;

		private void Awake() {
			SetUpWallsGameArea();
		}

		private void SetUpWallsGameArea() {
			var bottomLeft = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
			var topRight = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

			Transform wallParent = GameObject.FindGameObjectWithTag("_Walls").transform;

			GameObject wallLeft = Instantiate(wallPrefab, wallParent);
			wallLeft.transform.position = new Vector3(bottomLeft.x - 0.5f, 0, 0);
			wallLeft.transform.localScale = new Vector3(1, 1, topRight.z - bottomLeft.z);

			GameObject wallRight = Instantiate(wallPrefab, wallParent);
			wallRight.transform.position = new Vector3(topRight.x + 0.5f, 0, 0);
			wallRight.transform.localScale = new Vector3(1, 1, topRight.z - bottomLeft.z);

			GameObject wallTop = Instantiate(wallPrefab, wallParent);
			wallTop.transform.position = new Vector3(0, 0, topRight.z + 0.5f);
			wallTop.transform.localScale = new Vector3(topRight.x - bottomLeft.x, 1, 1);

			GameObject wallBottom = Instantiate(wallPrefab, wallParent);
			wallBottom.transform.position = new Vector3(0, 0, bottomLeft.z - 0.5f);
			wallBottom.transform.localScale = new Vector3(topRight.x - bottomLeft.x, 1, 1);
		}

	}

}
