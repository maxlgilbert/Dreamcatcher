using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CellGridManager : MonoBehaviour {
	public AStarNode start;
	public AStarNode goal;
	public int width;
	public int height;
	private CellObject[,] _allCellNodes;
	public Dictionary<Vector2,Dictionary<int,CellType>> timeMapNodes;
	[HideInInspector] public CellAction[,] CellActions;
	[HideInInspector] public List<AStarAction> actions;
	public CellAction left;
	public CellAction upLeft;
	public CellAction up;
	public CellAction upRight;
	public CellAction right;
	public CellAction down;
	public CellAction wait;
	public float yOffset = 1.867725f;

	
	public Material ground;
	public Material obstacle;
	public Material empty;

	private static CellGridManager instance;
	
	public static CellGridManager Instance
	{
		get 
		{
			return instance;
		}
	}
	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake () {
		instance = this;
		CellActions = new CellAction[3,3];
		CellActions[0,1] = left;
		CellActions[0,2] = upLeft;
		CellActions[1,2] = up;
		CellActions[2,2] = upRight;
		CellActions[2,1] = right;
		CellActions[1,0] = down;
		CellActions[1,1] = wait;
		//actions.Add(left);
		//actions.Add(upLeft);
		actions.Add(up);
		actions.Add(upRight);
		actions.Add(right);
		actions.Add(left);
		actions.Add(upLeft);
		//actions.Add(right);
		//actions.Add(down);
		actions.Add(wait);
		_allCellNodes = new CellObject[width,height];
		timeMapNodes = new Dictionary<Vector2, Dictionary<int, CellType>>();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void AddCell (CellObject cellObject) {
		int x = cellObject.cellNode.x;
		int y = cellObject.cellNode.y;
		if (x < width && y < height) {
			_allCellNodes[x,y] = cellObject;
			timeMapNodes[new Vector2(x,y)] = new Dictionary<int, CellType>();
			for (int i =0; i <BeatManager.Instance.totalBeats; i++) {
				timeMapNodes[new Vector2(x,y)][i] = CellType.Empty;
			}
		} else {
			Debug.LogError("Tried adding out of bounds cell!");
		}
	}

	public CellType GetCellTypeAtBeat(CellNode node){
		if (node.beat < BeatManager.Instance.totalBeats) {
			return timeMapNodes[new Vector2(node.x,node.y)][node.beat];
		} else {
			Debug.LogError("Not that many beats in the level!");
			return CellType.Empty;
		}
	}

	
	public void SetCellTypeAtBeat(int x, int y, int beat, CellType cellType){
		if (beat < BeatManager.Instance.totalBeats) {
			timeMapNodes[new Vector2(x,y)][beat] = cellType;
		} else {
			Debug.LogError("Not that many beats in the level!");
		}
	}

	public CellObject GetCell (int x, int y) {
		if (x >= 0 && y >= 0 && x < width && y < height) {
			return _allCellNodes[x,y];
		}
		return null;
	}
}
