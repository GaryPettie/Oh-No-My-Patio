using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIContoller : MonoBehaviour {

	[SerializeField] float autoCompletionDelay = 1f;
	[SerializeField] int levelSelectIndex;
	[SerializeField] GameObject Instructions;

	AudioSource audio;
	GridGenerator grid;

	bool isInstructionsShowing;

	void Start () {
		audio = GetComponent<AudioSource>();
		audio.volume = PlayerPrefsManager.GetSfxVolume();

		grid = FindObjectOfType<GridGenerator>();
	}

	public void ReloadLevel () {
		LevelManager.instance.ReloadLevel(); ;
	}

	public void SolveAndReload () {
		grid.GiveUp(autoCompletionDelay);
		StartCoroutine(ReloadAfterWait());
	}

	public void PlayButtonAudio () {
		if (PlayerPrefsManager.GetSfxToggle()) {
			audio.Play();
		}
	}

	public void GoToLevelSelect () {
		StartCoroutine(WaitForClipEnd(audio.clip.length));
		LevelManager.instance.LoadLevel(levelSelectIndex);
	}

	public void ShowInstructions () {
		isInstructionsShowing = !isInstructionsShowing;
		Instructions.SetActive(isInstructionsShowing);
	}

	IEnumerator ReloadAfterWait () {
		while (!grid.IsComplete()) {
			yield return new WaitForEndOfFrame();
		}
		LevelManager.instance.ReloadLevel();
	}

	IEnumerator WaitForClipEnd (float time) {
		if (!PlayerPrefsManager.GetSfxToggle()) {
			time = 0;
		}
		yield return new WaitForSeconds(time);
		LevelManager.instance.LoadNextLevel();
	}
}
