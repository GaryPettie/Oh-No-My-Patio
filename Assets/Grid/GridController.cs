using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GridController : MonoBehaviour {

	GridGenerator layout;
	UIContoller ui;

	void Start () {
		ui = FindObjectOfType<UIContoller>();
		layout = FindObjectOfType<GridGenerator>();
	}

	public void ReloadGrid () {
		LevelManager.instance.ReloadLevel();
	}

	public void LoadPreviousLevel () {
		int x = PlayerPrefsManager.GetDimX();
		int y = PlayerPrefsManager.GetDimY();

		if (x != y && x > LevelManager.instance.minSize.x) {
			x--;
		}
		else if (y > LevelManager.instance.minSize.y) {
			y--;
		}

		PlayerPrefsManager.SetDimX(x);
		PlayerPrefsManager.SetDimY(y);
		PlayerPrefsManager.SetCurrentLevel(PlayerPrefsManager.GetCurrentLevel() - 1);

		ui.PlayButtonAudio();
		ui.ReloadLevel();
	}

	public void LoadNextLevel () {
		int x = PlayerPrefsManager.GetDimX();
		int y = PlayerPrefsManager.GetDimY();

		if (x == y && x < LevelManager.instance.maxSize.x) {
			x++;
		}
		else if (y < LevelManager.instance.maxSize.y) {
			y++;
		}

		PlayerPrefsManager.SetDimX(x);
		PlayerPrefsManager.SetDimY(y);
		PlayerPrefsManager.SetCurrentLevel(PlayerPrefsManager.GetCurrentLevel() + 1);

		ui.PlayButtonAudio();
		ui.ReloadLevel();
	}
}
