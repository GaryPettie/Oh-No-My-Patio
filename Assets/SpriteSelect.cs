using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteSelect : MonoBehaviour {

	[SerializeField] Sprite[] sprites;

	void Start () {
		if (sprites.Length != 0) {
			GetComponent<Image>().sprite = sprites[Random.Range(0, sprites.Length)];
		}	
	}	
}
