using UnityEngine;

namespace TAOM.Gameplay {

	public class Map : MonoBehaviour {

		[SerializeField] private GameObject wallPrefab;

		private Rect mapAreaRect;
		private Rect spawnAreaRect;

		private void Awake() {
			ComputeGameArea();
			SetUpWallsGameArea();
		}

		#region GAME AREA SETUP

		private void ComputeGameArea() {
			Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
			Vector3 topRight = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

			mapAreaRect = new Rect(bottomLeft.x, bottomLeft.z, topRight.x - bottomLeft.x, topRight.z - bottomLeft.z);
			spawnAreaRect = new Rect(bottomLeft.x * 1.1f, bottomLeft.z * 1.1f, (topRight.x - bottomLeft.x) * 1.1f, (topRight.z - bottomLeft.z) * 1.1f);
		}

		private void SetUpWallsGameArea() {
			Transform wallParent = GameObject.FindGameObjectWithTag("_Walls").transform;

			GameObject wallLeft = Instantiate(wallPrefab, wallParent);
			wallLeft.transform.position = new Vector3(mapAreaRect.xMin - 0.5f, 0, 0);
			wallLeft.transform.localScale = new Vector3(1, 1, mapAreaRect.yMax - mapAreaRect.yMin);

			GameObject wallRight = Instantiate(wallPrefab, wallParent);
			wallRight.transform.position = new Vector3(mapAreaRect.xMax + 0.5f, 0, 0);
			wallRight.transform.localScale = new Vector3(1, 1, mapAreaRect.yMax - mapAreaRect.yMin);

			GameObject wallTop = Instantiate(wallPrefab, wallParent);
			wallTop.transform.position = new Vector3(0, 0, mapAreaRect.yMax + 0.5f);
			wallTop.transform.localScale = new Vector3(mapAreaRect.xMax - mapAreaRect.xMin, 1, 1);

			GameObject wallBottom = Instantiate(wallPrefab, wallParent);
			wallBottom.transform.position = new Vector3(0, 0, mapAreaRect.yMin - 0.5f);
			wallBottom.transform.localScale = new Vector3(mapAreaRect.xMax - mapAreaRect.xMin, 1, 1);
		}

		#endregion

		#region UTILS

		public Vector3 GetRandomPositionBorderMap() {
			Vector3 position = Vector3.zero;

			while (mapAreaRect.Contains(position)) {
				position = new Vector3(Random.Range(spawnAreaRect.xMin, spawnAreaRect.xMax), 0,
					Random.Range(spawnAreaRect.yMin, spawnAreaRect.yMax));
			}

			return position;
		}

		public bool IsPositionInsideMapArea(Vector3 position) {
			return mapAreaRect.Contains(position);
		}

		#endregion

	}

}
