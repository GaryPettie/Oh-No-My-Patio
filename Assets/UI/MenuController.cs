using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour {

	[SerializeField] GameObject levelCanvas;
	[SerializeField] GameObject optionsCanvas;

	void Start () {
		ShowLevels();
	}

	public void ShowLevels () {
		levelCanvas.SetActive(true);
		optionsCanvas.SetActive(false);
	}

	public void ShowOptions () {
		levelCanvas.SetActive(false);
		optionsCanvas.SetActive(true);
	}

	public void ToggleOptions () {
		levelCanvas.SetActive(!levelCanvas.activeInHierarchy);
		optionsCanvas.SetActive(!optionsCanvas.activeInHierarchy);
	}
}
