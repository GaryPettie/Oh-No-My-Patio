using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonLock : MonoBehaviour {

	[SerializeField] float m_lockDuration = 5f;
	[SerializeField] List<Button> additionalButtonsToLock = new List<Button>();

	public void Start () {
		TempLockButton(2f);
	}

	public void TempLockButton () {
		StartCoroutine(LockButton(m_lockDuration));
	}

	public void TempLockButton (float lockDuration) {
		StartCoroutine(LockButton(lockDuration));
	}

	public void AddButtonsToLock (List<Button> buttonList) {
		for (int i = 0; i < buttonList.Count; i++) {
			additionalButtonsToLock.Add(buttonList[i]);
		}
	}

	IEnumerator LockButton (float duration) {
		Button button = GetComponent<Button>();
		button.interactable = false;

		for (int i = 0; i < additionalButtonsToLock.Count; i++) {
			additionalButtonsToLock[i].interactable = false;
		}

		float currentTime = 0;
		while (currentTime < duration) {
			currentTime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		button.interactable = true;

		for (int i = 0; i < additionalButtonsToLock.Count; i++) {
			additionalButtonsToLock[i].interactable = true;
		}
	}
}
