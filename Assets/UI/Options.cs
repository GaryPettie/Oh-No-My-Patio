using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour {

	[SerializeField] Toggle musicToggle;
	[SerializeField] Slider musicVolume;
	[SerializeField] Toggle sfxToggle;
	[SerializeField] Slider sfxVolume;
	[SerializeField] Toggle ambienceToggle;
	[SerializeField] Slider ambienceVolume;
	[SerializeField] Toggle resetToggle;
	[SerializeField] AudioSource sfxAudio;

	bool[] originalToggles = new bool [3];
	float[] originalVolumes = new float [3];

	LevelSelector levelSelector;

	void Awake () {
		originalToggles[0] = PlayerPrefsManager.GetMusicToggle();
		originalToggles[1] = PlayerPrefsManager.GetSfxToggle();
		originalToggles[2] = PlayerPrefsManager.GetAmbienceToggle(); ;

		originalVolumes[0] = PlayerPrefsManager.GetMusicVolume();
		originalVolumes[1] = PlayerPrefsManager.GetSfxVolume(); 
		originalVolumes[2] = PlayerPrefsManager.GetAmbienceVolume();

		musicToggle.isOn = originalToggles[0];
		sfxToggle.isOn = originalToggles[1];
		ambienceToggle.isOn = originalToggles[2];
		musicVolume.value = originalVolumes[0];
		sfxVolume.value = originalVolumes[1];
		ambienceVolume.value = originalVolumes[2];

		levelSelector = FindObjectOfType<LevelSelector>();
	}

	public void ToggleMusic () {
		MusicManager.instance.ToggleMusic(musicToggle.isOn);
	}

	public void ChangeMusicVolume () {
		MusicManager.instance.SetMusicVolume(musicVolume.value);
	}

	public void ChangeSfxVolume () {
		if (sfxToggle.isOn && !sfxAudio.isPlaying) {
			sfxAudio.volume = sfxVolume.value;
			sfxAudio.Play();
		}
	}

	public void ToggleAmbience () {
		MusicManager.instance.ToggleAmbience(ambienceToggle.isOn);
	}

	public void ChangeAmbienceVolume () {
		MusicManager.instance.SetAmbienceVolume(ambienceVolume.value);
	}

	public void Revert () {
		musicToggle.isOn = originalToggles[0];
		musicVolume.value = originalVolumes[0];
		sfxToggle.isOn = originalToggles[1];
		sfxVolume.value = originalVolumes[1];
		ambienceToggle.isOn = originalToggles[2];
		ambienceVolume.value = originalVolumes[2];
		resetToggle.isOn = false;
	}

	public void Save () {
		PlayerPrefsManager.StoreMusicToggle(musicToggle.isOn);
		PlayerPrefsManager.StoreMusicVolume(musicVolume.value);
		PlayerPrefsManager.StoreSfxToggle(sfxToggle.isOn);
		PlayerPrefsManager.StoreSfxVolume(sfxVolume.value);
		PlayerPrefsManager.StoreAmbienceToggle(ambienceToggle.isOn);
		PlayerPrefsManager.StoreAmbienceVolume(ambienceVolume.value);

		if (resetToggle.isOn) {
			ResetLevelData();
			resetToggle.isOn = false;
		}
	}

	void ResetLevelData () {
		PlayerPrefsManager.ResetLevelsCompleted();
		PlayerPrefsManager.SetDimX(LevelManager.instance.minSize.x);
		PlayerPrefsManager.SetDimY(LevelManager.instance.minSize.y);
		levelSelector.LockLevels();
	}
}
