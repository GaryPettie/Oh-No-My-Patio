using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugLevels : MonoBehaviour {

	public void SetFirstLevel () {
		PlayerPrefsManager.SetDimX(LevelManager.instance.minSize.x);
		PlayerPrefsManager.SetDimY(LevelManager.instance.minSize.y);
	}

	public void SetLastLevel () {
		PlayerPrefsManager.SetDimX(LevelManager.instance.maxSize.x);
		PlayerPrefsManager.SetDimY(LevelManager.instance.maxSize.y);
	}
}
