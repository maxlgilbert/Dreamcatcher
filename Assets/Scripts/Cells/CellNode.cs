using UnityEngine;
using System.Collections;

public class CellNode : AStarNode {
	public int x;
	public int y;
	private Vector2 _position;

	public CellNode (int _x, int _y) {
		x = _x;
		y = _y;
		_position = new Vector2(x,y);
	}
	public override float distance (AStarNode other)
	{
		CellNode otherCellNode = other as CellNode;
		return Vector2.Distance(_position,new Vector2(otherCellNode.x,otherCellNode.y));
	}

	public override float estimate (AStarNode other)
	{
		CellNode otherCellNode = other as CellNode;
		return Mathf.Abs(x-otherCellNode.x);
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
