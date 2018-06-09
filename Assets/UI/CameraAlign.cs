using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAlign : MonoBehaviour {

	[SerializeField] GameObject canvasContainer;
	Vector2Int dimensions;
	float xPos, yPos;

	float cameraScaleMultiplier = 0.7125f;
	float canvasScaleMultiplier = 0.1425f;
	float wipeScaleMultiplier = 0.0018f;
	float wipeParticleScaleMultipler = 0.15f;

	void Start () {
		SetXYPos();
		AlignCamera();
		AlignCanvas();
	}

	void SetXYPos () {
		dimensions = new Vector2Int(PlayerPrefsManager.GetDimX(), PlayerPrefsManager.GetDimY());
		xPos = dimensions.x / 2;
		yPos = dimensions.y / 2;
		if (dimensions.x % 2 == 0) {
			xPos -= 0.5f;
		}
		if (dimensions.y % 2 == 0) {
			yPos -= 0.5f;
		}
	}

	void AlignCamera () {
		Camera.main.transform.position = new Vector3(xPos, yPos, -10);
		Camera.main.orthographicSize = dimensions.x * cameraScaleMultiplier;
	}

	void AlignCanvas () {
		canvasContainer.transform.position = new Vector3(xPos, yPos, 0);
		canvasContainer.transform.localScale = new Vector3(dimensions.x * canvasScaleMultiplier, dimensions.x * canvasScaleMultiplier, 1);
	}
}
