using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerPrefsManager {

	const string LVLCOM = "lvlcom";
	const string DIMX = "dimx";
	const string DIMY = "dimy";
	const string CURRENT_LEVEL = "lvlcur";

	const string MUSIC_VOLUME = "musicVolume";
	const string MUSIC_TOGGLE = "musicToggle";
	const string AMBIENCE_VOLUME = "ambienceVolume";
	const string AMBIENCE_TOGGLE = "ambienceToggle";
	const string SFX_VOLUME = "sfxVolume";
	const string SFX_TOGGLE = "sfxToggle";
	
	const string LEVEL_STARS = "lvlStr";

	public static void IncrementLevelsCompleted () {
		PlayerPrefs.SetInt(LVLCOM, GetLevelsCompleted() + 1);
	}

	public static void ResetLevelsCompleted () {
		PlayerPrefs.SetInt(LVLCOM, 0);
	}

	public static int GetLevelsCompleted () {
		return PlayerPrefs.GetInt(LVLCOM, 0);
	}

	public static void SetCurrentLevel (int level) {
		PlayerPrefs.SetInt(CURRENT_LEVEL, level);
	}

	public static int GetCurrentLevel () {
		return PlayerPrefs.GetInt(CURRENT_LEVEL, 0);
	}

	public static void StoreLevelStars (int level, int stars) {
		PlayerPrefs.SetInt(LEVEL_STARS + level, stars);
	}

	public static int GetLevelStars (int level) {
		return PlayerPrefs.GetInt(LEVEL_STARS + level);
	}

	public static void ResetLevelStars (int level) {
		PlayerPrefs.SetInt(LEVEL_STARS + level, 0);
	}

	public static void SetDimX (int x) {
		PlayerPrefs.SetInt(DIMX, x);
	}

	public static int GetDimX () {
		return PlayerPrefs.GetInt(DIMX, 2);
	}

	public static void SetDimY (int y) {
		PlayerPrefs.SetInt(DIMY, y);
	}

	public static int GetDimY () {
		return PlayerPrefs.GetInt(DIMY, 1);
	}

	#region settings
	public static void StoreMusicToggle (bool state) {
		int iState;
		if (state == true) {
			iState = 1;
		}
		else {
			iState = 0;
		}
		PlayerPrefs.SetInt(MUSIC_TOGGLE, iState);
	}

	public static bool GetMusicToggle () {
		int output = PlayerPrefs.GetInt(MUSIC_TOGGLE, 1);
		if (output == 1) {
			return true;
		}
		else {
			return false;
		}
	}

	public static void StoreMusicVolume (float amount) {
		PlayerPrefs.SetFloat(MUSIC_VOLUME, amount);
	}

	public static float GetMusicVolume () {
		return PlayerPrefs.GetFloat(MUSIC_VOLUME, 1);
	}

	public static void StoreAmbienceToggle (bool state) {
		int iState;
		if (state == true) {
			iState = 1;
		}
		else {
			iState = 0;
		}
		PlayerPrefs.SetInt(AMBIENCE_TOGGLE, iState);
	}

	public static bool GetAmbienceToggle () {
		int output = PlayerPrefs.GetInt(AMBIENCE_TOGGLE, 1);
		if (output == 1) {
			return true;
		}
		else {
			return false;
		}
	}

	public static void StoreAmbienceVolume (float amount) {
		PlayerPrefs.SetFloat(AMBIENCE_VOLUME, amount);
	}

	public static float GetAmbienceVolume () {
		return PlayerPrefs.GetFloat(AMBIENCE_VOLUME, 1);
	}

	public static void StoreSfxToggle (bool state) {
		int iState;
		if (state == true) {
			iState = 1;
		}
		else {
			iState = 0;
		}
		PlayerPrefs.SetInt(SFX_TOGGLE, iState);
	}

	public static void StoreSfxVolume (float amount) {
		PlayerPrefs.SetFloat(SFX_VOLUME, amount);

	}

	public static bool GetSfxToggle () {
		int output = PlayerPrefs.GetInt(SFX_TOGGLE, 1);
		if (output == 1) {
			return true;
		}
		else {
			return false;
		}
	}

	public static float GetSfxVolume () {
		return PlayerPrefs.GetFloat(SFX_VOLUME, 1);
	}
	#endregion
}
