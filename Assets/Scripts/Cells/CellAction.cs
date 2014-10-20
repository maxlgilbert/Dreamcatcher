using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CellAction : AStarAction {
	public int directionX;
	public int directionY;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public override List<AStarNode> TryAction (AStarNode curr)
	{
		CellNode currCellNode = curr as CellNode;
		int x = currCellNode.x + directionX;
		int y = currCellNode.y + directionY;
		List<AStarNode> neighborList = new List<AStarNode>();
		CellObject neighbor = CellGridManager.Instance.GetCell(x,y);
		if (neighbor != null && neighbor.cellType != CellType.Obstacle) {
			neighborList.Add(neighbor.cellNode);
		}
		return neighborList;
	} 

	public static CellAction GetAction (CellNode from, CellNode to) {
		int dirX = to.x - from.x+1;
		int dirY = to.y - from.y+1;
		return CellGridManager.Instance.CellActions[dirX,dirY];
	}
}
