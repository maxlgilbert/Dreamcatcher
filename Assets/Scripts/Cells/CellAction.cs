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
		CellObject neighborObject = CellGridManager.Instance.GetCell(x,y);
		CellNode neighborNode;
		if (neighborObject != null) {
			neighborNode = new CellNode(x,y);
			neighborNode.beat = currCellNode.beat+1;
		} else {
			return neighborList;
		}
		while (neighborObject != null && CellGridManager.Instance.GetCellTypeAtBeat(neighborNode) == CellType.Empty) {
			y--;
			neighborObject = CellGridManager.Instance.GetCell(x,y);
			neighborNode = new CellNode(x,y);
			neighborNode.beat = currCellNode.beat+1;
		}
		if (neighborObject != null && CellGridManager.Instance.GetCellTypeAtBeat(neighborNode) == CellType.Ground) {
			//neighborList.Add(neighbor.cellNode);
			//CellNode newNeighbor = new CellNode(neighbor.x,neighbor.y);
			//newNeighbor.beat = currCellNode.beat+1;
			neighborList.Add(neighborNode);
		}
		return neighborList;
	} 

	public static CellAction GetAction (CellNode from, CellNode to) {
		int dirX = to.x - from.x+1;
		int dirY = to.y - from.y+1;
		if (dirY<1) {
			dirY = 1;
		}
		return CellGridManager.Instance.CellActions[dirX,dirY];
	}
}
