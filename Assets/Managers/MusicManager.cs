using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour {

	[Header("Music")]
	[SerializeField] AudioClip[] levelMusic;

	[Header("Ambience")]
	[SerializeField] AudioClip[] levelAmbience;

	[Header("Fade Settings")]
	[SerializeField] bool fadeIn = false;
	[SerializeField] float fadeStep = 0.01f;

	int startingVolume;

	public static MusicManager instance;
	static AudioSource musicAudio;
	static AudioSource ambienceAudio;


	void Awake () {
		if (instance == null) {
			instance = this;
			musicAudio = gameObject.AddComponent<AudioSource>();
			ambienceAudio = gameObject.AddComponent<AudioSource>();

			DontDestroyOnLoad(gameObject);
		}
		else {
			Destroy(this.gameObject);
			Debug.LogWarning("Duplicate Music Manager destroyed on " + gameObject.name);
		}
	}

	void Start () {
		musicAudio.enabled = PlayerPrefsManager.GetMusicToggle();
		musicAudio.volume = PlayerPrefsManager.GetMusicVolume();

		ambienceAudio.enabled = PlayerPrefsManager.GetAmbienceToggle();
		ambienceAudio.volume = PlayerPrefsManager.GetAmbienceVolume();

		PlayLevelMusic(0);
		PlayLevelAmbience(0);
	}

	void OnLevelWasLoaded (int buildIndex) {
		PlayLevelMusic(buildIndex);
		PlayLevelAmbience(buildIndex);
	}

	void PlayLevelMusic (int buildIndex) {
		AudioClip thisLevelMusic = levelMusic[buildIndex];

		if (thisLevelMusic) {
			musicAudio.clip = thisLevelMusic;
			musicAudio.loop = true;
			if (fadeIn) {
				float volume = musicAudio.volume;
				musicAudio.volume = 0;
				StartCoroutine(FadeIn(musicAudio, volume));
			}
			musicAudio.Play();
		}
	}

	void PlayLevelAmbience (int buildIndex) {
		AudioClip thisLevelAmbience = levelAmbience[buildIndex];

		if (thisLevelAmbience) {
			ambienceAudio.clip = thisLevelAmbience;
			ambienceAudio.loop = true;
			if (fadeIn) {
				float volume = ambienceAudio.volume;
				ambienceAudio.volume = 0;
				StartCoroutine(FadeIn(ambienceAudio, volume));
			}
			ambienceAudio.Play();
		}
	}

	public void ToggleMusic (bool state) {
		if (state) {
			UnmuteMusic();
		}
		else {
			MuteMusic();
		}
	}

	public void ToggleAmbience (bool state) {
		if (state) {
			UnmuteAmbience();
		}
		else {
			MuteAmbience();
		}
	}

	public void SetMusicVolume (float volume) {
		musicAudio.volume = volume;
	}

	public void SetAmbienceVolume (float volume) {
		ambienceAudio.volume = volume;
	}

	public void MuteMusic () {
		musicAudio.enabled = false;
	}

	public void UnmuteMusic () {
		musicAudio.enabled = true;
	}

	public void MuteAmbience () {
		ambienceAudio.enabled = false;
	}

	public void UnmuteAmbience () {
		ambienceAudio.enabled = true;
	}

	IEnumerator FadeIn(AudioSource audio, float volume) {
		while (audio.volume < volume) {
			audio.volume += fadeStep;
			yield return null;
		}
	}
}
