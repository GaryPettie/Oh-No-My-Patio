using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {

	[SerializeField] string type = "NULL";
	[SerializeField] List<Sprite> sprites = new List<Sprite>();
	[SerializeField] float rotationAngle = 90f;
	[SerializeField] float rotationSpeed = 2f;
	[SerializeField] int[] exits = { 0, 0, 0, 0 };
	float newRotation;
	Vector2Int coords;


	GridGenerator layout;


	void Awake () {
		SpriteRenderer rend = GetComponent<SpriteRenderer>();
		if (sprites.Count == 0) {
			sprites.Add(rend.sprite);
		}
		rend.sprite = sprites[Random.Range(0, sprites.Count)];
		

		SetCoords();
		gameObject.name = coords.ToString();
		layout = FindObjectOfType<GridGenerator>();

		AudioSource sfx = GetComponent<AudioSource>();
		if (PlayerPrefsManager.GetSfxToggle()) {
			sfx.enabled = false;
		}
		else {
			sfx.enabled = true;
			sfx.volume = PlayerPrefsManager.GetSfxVolume();
		}
	}


	void Update () {
		if (transform.eulerAngles.z != newRotation) {
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(0, 0, newRotation)), rotationSpeed * Time.deltaTime);
		}
	}

	public string GetNodeType () {
		return type;
	}

	public Vector2Int GetCoords () {
		return coords;
	}

	public int GetExit (int exitIndex) {
		return exits[exitIndex];
	}

	public int[] GetAllExits () {
		return exits;
	}

	public int GetExitCount () {
		int exitCount = 0;
		for (int i = 0; i < exits.Length; i++) {
			exitCount += exits[i];
		}
		return exitCount;
	}

	public void RotatePiece (int rotations) {
		newRotation += (rotationAngle * rotations) % 360;
		for (int i = 0; i < rotations; i++) {
			RotateExits();
		}
	}

	void RotateExits () {
		int temp = exits[0];

		for (int i = 0; i < exits.Length - 1; i++) {
			exits[i] = exits[i + 1];
		}

		exits[exits.Length - 1] = temp;
	}

	void SetCoords () {
		coords.x = (int)transform.localPosition.x;
		coords.y = (int)transform.localPosition.y;
	}
}
