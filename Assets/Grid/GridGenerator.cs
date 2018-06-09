using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(PlayerInput))]
public class GridGenerator : MonoBehaviour {

	[SerializeField] bool randomlyGenerate = true;
	[SerializeField] GameObject[] nodePrefabs;

	[HideInInspector] public Vector2Int dimensions;
	[HideInInspector] public int totalConnections = 0;
	[HideInInspector] public int currentConnections = 0;

	Dictionary<Vector2Int, Node> graph = new Dictionary<Vector2Int, Node>();
	Dictionary<Vector2Int, int[]> solutionGraph = new Dictionary<Vector2Int, int[]>();

	Node[] nodes;
	PlayerInput playerInput;

	bool isComplete = false;

	int minMoves = 0;

	void Start () {
		dimensions.x = PlayerPrefsManager.GetDimX();
		dimensions.y = PlayerPrefsManager.GetDimY();
		playerInput = GetComponent<PlayerInput>();


		if (randomlyGenerate) {
			GenerateGraph();
		}

		LoadNodes();
		currentConnections = playerInput.CheckAllConnections();
		Solve();
		do {
			Shuffle();
			currentConnections = playerInput.CheckAllConnections();
		}
		while (currentConnections == totalConnections);

		Debug.Log(minMoves);
	}

	public int GetMinMoves () {
		return minMoves;
	}

	public Dictionary<Vector2Int, Node> GetGraph () {
		return graph;
	}

	public void Solve () {
		for (int y = 0; y < dimensions.y; y++) {
			for (int x = 0; x < dimensions.x; x++) {
				Vector2Int coords = new Vector2Int(x, y);
				Node currentNode;
				int[] currentExits;
				int[] solutionExits;

				graph.TryGetValue(coords, out currentNode);
				solutionGraph.TryGetValue(coords, out solutionExits);

				if (currentNode != null) {
					currentExits = currentNode.GetAllExits();

					while (!currentExits.SequenceEqual(solutionExits)) {
						playerInput.RotateAndCheck(currentNode);
					}
				}
			}
		}
	}

	public void GiveUp (float completionDelay) {
		StartCoroutine(AutoComplete(completionDelay));
	}

	IEnumerator AutoComplete (float completionDelay) {
		for (int y = 0; y < dimensions.y; y++) {
			for (int x = 0; x < dimensions.x; x++) {
				Vector2Int coords = new Vector2Int(x, y);
				Node currentNode;
				int[] currentExits;
				int[] solutionExits;

				graph.TryGetValue(coords, out currentNode);
				solutionGraph.TryGetValue(coords, out solutionExits);

				if (currentNode != null) {
					currentExits = currentNode.GetAllExits();

					while (!currentExits.SequenceEqual(solutionExits)) {
						playerInput.RotateAndCheck(currentNode);
						yield return new WaitForEndOfFrame();
					}
				}
			}
		}
		yield return new WaitForSeconds(completionDelay);
		isComplete = true;
	}

	public bool IsComplete () {
		return isComplete;
	}

	void GenerateGraph () {
		for (int y = 0; y < dimensions.y; y++) {
			for (int x = 0; x < dimensions.x; x++) {

				Vector2Int coords = new Vector2Int(x, y);
				int[] currentExits = { 0, 0, 0, 0 };
				int[] leftExits = { 0, 0, 0, 0 };
				int[] bottomExits = { 0, 0, 0, 0 };
				int exitCount = 0;

				solutionGraph.TryGetValue(new Vector2Int(coords.x - 1, coords.y), out leftExits);
				solutionGraph.TryGetValue(new Vector2Int(coords.x, coords.y - 1), out bottomExits);

				if (x != 0) {
					currentExits[3] = leftExits[1];

				}
				if (x != dimensions.x - 1) {
					if (dimensions.x < 3) {
						currentExits[1] = Random.Range(1, 2);
					}
					else {
						currentExits[1] = Random.Range(0, 2);
					}
				}

				if (y != 0) {
					currentExits[2] = bottomExits[0];
				}
				if (y != dimensions.y - 1) {
					if (dimensions.y < 3) {
						currentExits[0] = Random.Range(1, 2);
					}
					else {
						currentExits[0] = Random.Range(0, 2);
					}
				}

				solutionGraph.Add(coords, currentExits);

				exitCount = currentExits[0] + currentExits[1] + currentExits[2] + currentExits[3];

				InstantiateNodes(coords, exitCount, currentExits);
			}
		}
	}

	private void InstantiateNodes (Vector2Int coords, int exitCount, int[] currentExits) {
		GameObject instantiatedNode;

		switch (exitCount) {
			case 0:
				instantiatedNode = Instantiate(nodePrefabs[0], new Vector3(coords.x, coords.y, 0), Quaternion.identity, transform);
				break;
			case 1:
				instantiatedNode = Instantiate(nodePrefabs[1], new Vector3(coords.x, coords.y, 0), Quaternion.identity, transform);
				break;
			case 2:
				if (currentExits[0] != currentExits[2]) {
					instantiatedNode = Instantiate(nodePrefabs[2], new Vector3(coords.x, coords.y, 0), Quaternion.identity, transform);
				}
				else {
					instantiatedNode = Instantiate(nodePrefabs[3], new Vector3(coords.x, coords.y, 0), Quaternion.identity, transform);
				}
				break;
			case 3:
				instantiatedNode = Instantiate(nodePrefabs[4], new Vector3(coords.x, coords.y, 0), Quaternion.identity, transform);
				break;
			case 4:
				instantiatedNode = Instantiate(nodePrefabs[5], new Vector3(coords.x, coords.y, 0), Quaternion.identity, transform);
				break;
			default:
				break;
		}
	}

	void LoadNodes () {
		nodes = FindObjectsOfType<Node>();

		for (int i = 0; i < nodes.Length; i++) {
			if (graph.ContainsKey(nodes[i].GetCoords())) {
				Destroy(nodes[i].gameObject);
			}
			else {
				Vector2Int currentNodeCoords = nodes[i].GetCoords();
				graph.Add(currentNodeCoords, nodes[i]);
				totalConnections += nodes[i].GetExitCount();

				if (dimensions.x < currentNodeCoords.x + 1) {
					dimensions.x = currentNodeCoords.x + 1;
				}
				if (dimensions.y < currentNodeCoords.y + 1) {
					dimensions.y = currentNodeCoords.y + 1;
				}
			}
		}

		totalConnections /= 2;
	}

	void Shuffle () {
		foreach (KeyValuePair<Vector2Int, Node> entry in graph) {
			if (entry.Value.GetComponent<Node>().GetNodeType() != "NULL" && entry.Value.GetComponent<Node>().GetNodeType() != "CROSS") {
				int rotations = Random.Range(1, 4);
				entry.Value.RotatePiece(rotations);
				minMoves += (4 - rotations);
			}
		}
	}
}
