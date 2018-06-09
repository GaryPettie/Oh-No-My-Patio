using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class ScreenWipe : MonoBehaviour {

	public static ScreenWipe instance;
	[SerializeField] float wipeSpeed;
	Image screen;
	ParticleSystem particles; 
	
	public bool isRunning;

	void Awake () {
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad(gameObject);

			screen = GetComponentInChildren<Image>();
			particles = GetComponentInChildren<ParticleSystem>();
		}
		else {
			Destroy(this.gameObject);
			Debug.LogWarning("Duplicate Screen Wipe destroyed on " + gameObject.name);
		}
	}

	public void SetDefaultScale () {
		ScreenWipe.instance.transform.position = new Vector3(0, 0, -0.1f);
		ScreenWipe.instance.transform.localScale = new Vector3(0.0125f, 0.0125f, 1);
		ScreenWipe.instance.GetComponentInChildren<ParticleSystem>().transform.localScale = new Vector3(1, 1, 1);
	}

	public void ScaleScreenWipe () {
		float wipeScaleMultiplier = 0.0018f;
		float wipeParticleScaleMultipler = 0.15f;

		Vector2Int dimensions = new Vector2Int(PlayerPrefsManager.GetDimX(), PlayerPrefsManager.GetDimY());
		float xPos = dimensions.x / 2;
		float yPos = dimensions.y / 2;

		if (dimensions.x % 2 == 0) {
			xPos -= 0.5f;
		}
		if (dimensions.y % 2 == 0) {
			yPos -= 0.5f;
		}

		transform.position = new Vector3(xPos, yPos, -0.1f);
		transform.localScale = new Vector3(dimensions.x * wipeScaleMultiplier, dimensions.x * wipeScaleMultiplier, 1);
		particles.transform.localScale = new Vector3(dimensions.x * wipeParticleScaleMultipler, dimensions.x * wipeParticleScaleMultipler, dimensions.x * wipeParticleScaleMultipler);
	}

	public void WipeIn () {
		screen.fillAmount = 0;
		screen.rectTransform.localScale = new Vector3(1, 1, 1);
		StartCoroutine(Wipe(true));
	}

	public void WipeOut () {
		screen.fillAmount = 1;
		screen.rectTransform.localScale = new Vector3(-1, 1, 1);
		StartCoroutine(Wipe(false));
	}

	public IEnumerator Wipe (bool isWipingIn) {
		isRunning = true;
		particles.Play();
		if (isWipingIn) {
			while (screen.fillAmount < 1) {
				screen.fillAmount += (1f / wipeSpeed) * Time.deltaTime;
				yield return new WaitForEndOfFrame();
			}
		}
		else {
			while (screen.fillAmount > 0) {
				screen.fillAmount -= (1f / wipeSpeed) * Time.deltaTime;
				yield return new WaitForEndOfFrame();
			}
		}
		
		isRunning = false;
	}
}
