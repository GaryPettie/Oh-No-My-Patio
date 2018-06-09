using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour {

	[SerializeField] GameObject levelButtonPrefab;
	[SerializeField] int levelCount;

	List<Button> buttonList = new List<Button>();

	void Start () {
		int levelsUnlocked = PlayerPrefsManager.GetLevelsCompleted();

		for (int i = 0; i < levelCount; i++) {
			int level = i + 1;
			
			GameObject levelButtonGO = Instantiate(levelButtonPrefab,transform);
			Button levelButton = levelButtonGO.GetComponent<Button>();
			ButtonComponents buttonComponents = levelButtonGO.GetComponent<ButtonComponents>();
			buttonList.Add(levelButton);

			int x = Mathf.CeilToInt(0.5f * (level - 1) + 1.5f);
			int y = x;

			if (level % 2 != 0) {
				y = x - 1;
			}

			string coords = "(" + x + "," + y + ")";

			levelButtonGO.name = level+ ": " + coords; 

			buttonComponents.infoText.text = level + "\n" + coords;

			if (i <= levelsUnlocked) {
				buttonComponents.lockscreen.enabled = false;
				levelButton.onClick.AddListener(delegate { StoreSelectedLevel(x, y); });
				levelButton.onClick.AddListener(LoadLevel);
			}

			int stars = PlayerPrefsManager.GetLevelStars(level);

			for (int j = 0; j < buttonComponents.stars.transform.childCount; j++) {
				if (j + 1 > stars) {
					buttonComponents.stars.transform.GetChild(j).GetComponent<Image>().enabled = false;
				}
			}
		}
		for (int i = 0; i < buttonList.Count; i++) {
			buttonList[i].GetComponent<ButtonLock>().AddButtonsToLock(buttonList);
		}
	}

	void StoreSelectedLevel (int x, int y) {
		PlayerPrefsManager.SetCurrentLevel(x + y - 2);
		PlayerPrefsManager.SetDimX(x);
		PlayerPrefsManager.SetDimY(y);
	}

	void LoadLevel () {
		LevelManager.instance.LoadNextLevel();
	}

	public void LockLevels () {
		for (int i = 0; i < buttonList.Count; i++) {
			PlayerPrefsManager.ResetLevelStars(i);
			ButtonComponents components = buttonList[i].GetComponent<ButtonComponents>();
			Button levelButton = buttonList[i].GetComponent<Button>();

			if (i > 0) {
				levelButton.onClick.RemoveAllListeners();
				components.lockscreen.enabled = true;
			}

			for (int j = 0; j < components.stars.transform.childCount; j++) {
				components.stars.transform.GetChild(j).GetComponent<Image>().enabled = false;
			}
		}
	}
}
