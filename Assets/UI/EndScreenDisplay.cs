using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScreenDisplay : MonoBehaviour {

	[SerializeField] Canvas canvas;
	[SerializeField] Text message;
	PlayerInput player;
	bool hasUpdated = false; 

	int stars = 1;

	void Start () {
		player = FindObjectOfType<PlayerInput>();
		canvas.enabled = false;
	}

	void Update () {
		if (player.HasWon() && player.GetPlayerMoves() > 0 && !hasUpdated && player.nodeClicked) {
			canvas.enabled = true;
			
			int level = PlayerPrefsManager.GetCurrentLevel();
			int stars = player.GetStars();

			if (stars > PlayerPrefsManager.GetLevelStars(level)) {
				PlayerPrefsManager.StoreLevelStars(level, stars);
			}

			if (PlayerPrefsManager.GetLevelsCompleted() <= level) {
				PlayerPrefsManager.IncrementLevelsCompleted();
			}

			message.text = "Won in " + player.GetPlayerMoves() + " moves.  " + stars + " stars!";
			hasUpdated = true;
		}
	}
}
