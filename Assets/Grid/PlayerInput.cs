using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {

	[SerializeField] int[] starThreshold = {95, 85, 70};
	int playerMoves;
	GridGenerator grid;
	int stars = 0;
	public bool nodeClicked; 

	void Start () {
		grid = GetComponent<GridGenerator>();
		playerMoves = 0;
	}

	void Update () {
		if (Input.GetMouseButtonDown(0) && !HasWon()) {
			nodeClicked = GetClickedNode();
		}
	}
	
	public bool HasWon () {
		return (grid.currentConnections == grid.totalConnections);
	}

	public int GetPlayerMoves () {
		return playerMoves;
	}

	public int GetStars () {
		CheckStars();
		return stars;
	}

	void CheckStars () {
		float completionScore = (100f / playerMoves) * grid.GetMinMoves();

		if (completionScore > starThreshold[0]) {
			stars = 3;
		}
		else if (completionScore > starThreshold[1]) {
			stars = 2;
		}
		else if (completionScore > starThreshold[2]) {
			stars = 1;
		}
	}

	bool GetClickedNode () {
		RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

		if (hit.collider != null) {
			Node clickedNode = hit.collider.GetComponent<Node>();
			Debug.Log(clickedNode.GetNodeType());
			if (clickedNode.GetNodeType() != "NULL" && clickedNode.GetNodeType() != "CROSS") {
				Debug.Log(clickedNode.GetNodeType() == "NULL");
				if (PlayerPrefsManager.GetSfxToggle()) {
					AudioSource.PlayClipAtPoint(clickedNode.GetComponent<AudioSource>().clip, Camera.main.transform.position, PlayerPrefsManager.GetSfxVolume());
				}
				if (clickedNode != null) {
					playerMoves++;
					RotateAndCheck(clickedNode);
					return true;
				}
			}
		}
		return false;
	}

	public void RotateAndCheck (Node node) {
		int diff = -CheckNodeConnection(node.GetCoords());
		node.RotatePiece(1);
		diff += CheckNodeConnection(node.GetCoords());
		grid.currentConnections += diff;
	}

	public int CheckNodeConnection (Vector2Int coords) {
		int value = 0;
		Node currentNode;
		Node topNode;
		Node rightNode;
		Node bottomNode;
		Node leftNode;

		grid.GetGraph().TryGetValue(coords, out currentNode);
		grid.GetGraph().TryGetValue(new Vector2Int(coords.x, coords.y + 1), out topNode);
		grid.GetGraph().TryGetValue(new Vector2Int(coords.x + 1, coords.y), out rightNode);
		grid.GetGraph().TryGetValue(new Vector2Int(coords.x, coords.y - 1), out bottomNode);
		grid.GetGraph().TryGetValue(new Vector2Int(coords.x - 1, coords.y), out leftNode);

		if (currentNode != null && topNode != null) {
			if (currentNode.GetExit(0) == 1 && topNode.GetExit(2) == 1) {
				value++;
			}
		}
		if (currentNode != null && rightNode != null) {
			if (currentNode.GetExit(1) == 1 && rightNode.GetExit(3) == 1) {
				value++;
			}
		}
		if (currentNode != null && bottomNode != null) {
			if (currentNode.GetExit(2) == 1 && bottomNode.GetExit(0) == 1) {
				value++;
			}
		}
		if (currentNode != null && leftNode != null) {
			if (currentNode.GetExit(3) == 1 && leftNode.GetExit(1) == 1) {
				value++;
			}
		}

		return value;
	}

	public int CheckAllConnections () {
		if (grid == null) {
			grid = GetComponent<GridGenerator>();
		}

		int value = 0;
		for (int y = 0; y < grid.dimensions.y; y++) {
			for (int x = 0; x < grid.dimensions.x; x++) {
				Node currentNode;
				Node topNode;
				Node rightNode;
				grid.GetGraph().TryGetValue(new Vector2Int(x, y), out currentNode);
				grid.GetGraph().TryGetValue(new Vector2Int(x, y + 1), out topNode);
				grid.GetGraph().TryGetValue(new Vector2Int(x + 1, y), out rightNode);

				if (currentNode != null && topNode != null) {
					if (currentNode.GetExit(0) == 1 && topNode.GetExit(2) == 1) {
						value++;
					}
				}
				if (currentNode != null && rightNode != null) {
					if (currentNode.GetExit(1) == 1 && rightNode.GetExit(3) == 1) {
						value++;
					}
				}

			}
		}
		return value;
	}
}
