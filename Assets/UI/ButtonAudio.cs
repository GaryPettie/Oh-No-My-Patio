using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAudio : MonoBehaviour {

	public void Play() {
		if (PlayerPrefsManager.GetSfxToggle()) {
			AudioSource audio = GetComponent<AudioSource>();
			audio.volume = PlayerPrefsManager.GetSfxVolume();
			AudioSource.PlayClipAtPoint(audio.clip, Camera.main.transform.position, PlayerPrefsManager.GetSfxVolume());
		}
	}
}
