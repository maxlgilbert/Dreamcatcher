using UnityEngine;
using System.Collections;

public class CellNode : AStarNode {
	public int x;
	public int y;
	private Vector2 _position;
	public int beat;

	public CellNode (int _x, int _y) {
		actions = CellGridManager.Instance.actions;
		x = _x;
		y = _y;
		_position = new Vector2(x,y);
		beat = 0;
	}
	public override float distance (AStarNode other)
	{
		CellNode otherCellNode = other as CellNode;
		return Vector2.Distance(_position,new Vector2(otherCellNode.x,otherCellNode.y));
	}

	public override float estimate (AStarNode other)
	{
		CellNode otherCellNode = other as CellNode;
		return Vector2.Distance (_position, new Vector2 (otherCellNode.x, otherCellNode.y));// + this.beat;
	}

	public override bool Equals (AStarNode other)
	{
		CellNode otherCellNode = other as CellNode;
		return (x==otherCellNode.x && y == otherCellNode.y);
	}

	public override string ToString ()
	{
		return _position.ToString();
	}
}
