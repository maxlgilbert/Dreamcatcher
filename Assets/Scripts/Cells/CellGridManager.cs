using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CellGridManager : MonoBehaviour {
	public AStarNode start;
	public AStarNode goal;
	public int width;
	public int height;
	private CellNode[,] _allCellNodes;
	[HideInInspector] public List<CellAction> CellActions;
	public CellAction left;
	public CellAction upLeft;
	public CellAction up;
	public CellAction upRight;
	public CellAction right;
	public CellAction wait;

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
		CellActions = new List<CellAction>();
		CellActions.Add(left);
		CellActions.Add(upLeft);
		CellActions.Add(up);
		CellActions.Add(upRight);
		CellActions.Add(right);
		CellActions.Add(wait);
		_allCellNodes = new CellNode[width,height];
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void AddCell (CellNode cellNode) {
		int x = cellNode.x;
		int y = cellNode.y;
		if (x < width && y < height) {
			_allCellNodes[x,y] = cellNode;
		} else {
			Debug.LogError("Tried adding out of bounds cell!");
		}
	}

	public CellNode GetCell (int x, int y) {
		if (x >= 0 && y >= 0 && x < width && y < height) {
			return _allCellNodes[x,y];
		}
		return null;
	}
}
