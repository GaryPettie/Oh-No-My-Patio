using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LevelManager : MonoBehaviour {

	public static LevelManager instance;

	[Header("General")]
	[SerializeField] int mainGameIndex;
	[SerializeField] float autoloadTimer = 0f;

	[Header("Grid Info")]
	public Vector2Int minSize = new Vector2Int(2, 1);
	public Vector2Int maxSize = new Vector2Int(30, 30);
	GridGenerator layout;

	void Awake () {
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else {
			Destroy(this.gameObject);
			Debug.LogWarning("Duplicate Level Manager destroyed on " + gameObject.name);
		}
	}

	void Start () {
		layout = FindObjectOfType<GridGenerator>();
		if (autoloadTimer > 0) {
			Invoke("LoadNextLevel", autoloadTimer);
		}
	}

	void OnApplicationQuit () {
		QuitRequest();
	}

	public int GetActiveScene () {
		return SceneManager.GetActiveScene().buildIndex;
	}

	public void ReloadLevel () {
		StartCoroutine(ProcessLevelRequest(SceneManager.GetActiveScene().buildIndex));
	}

	public void LoadNextLevel () {
		StartCoroutine(ProcessLevelRequest(SceneManager.GetActiveScene().buildIndex + 1));
	}

	public void LoadPreviousLevel () {
		StartCoroutine(ProcessLevelRequest(SceneManager.GetActiveScene().buildIndex - 1));
	}

	public void LoadLevel (int buildIndex) {
		StartCoroutine(ProcessLevelRequest(buildIndex));
	}

	public void QuitRequest () {
		Debug.LogWarning("Request to quit received... Exiting Application.");
		Application.Quit();
	}

	IEnumerator ProcessLevelRequest (int buildIndex) {
		ScaleScreenWipe(buildIndex);	
		ScreenWipe.instance.WipeIn();
		while (ScreenWipe.instance.isRunning) {
			yield return null;
		}
		AsyncOperation operation = SceneManager.LoadSceneAsync(buildIndex);
		while (!operation.isDone) {
			yield return null;
		}
		ScaleScreenWipe(buildIndex);
		ScreenWipe.instance.WipeOut();
	}

	void ScaleScreenWipe (int buildIndex) {
		if (buildIndex == mainGameIndex) {
			if (buildIndex == SceneManager.GetActiveScene().buildIndex) {
				ScreenWipe.instance.ScaleScreenWipe();
			}
		}
		else {
			if (buildIndex == SceneManager.GetActiveScene().buildIndex) {
				ScreenWipe.instance.SetDefaultScale();
			}
		}
	}
}