using UnityEngine;
using System.Collections;

public class CellNode : AStarNode {
	public int x;
	public int y;
	private Vector2 _position;
	public int beat;
	public bool waitNode = false;

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
		return Vector2.Distance(_position,new Vector2(otherCellNode.x,otherCellNode.y));// + 5.0f*Mathf.Abs(this.beat - otherCellNode.beat);
	}

	public override float estimate (AStarNode other)
	{
		CellNode otherCellNode = other as CellNode;
		float dist = Vector2.Distance (_position, new Vector2 (otherCellNode.x, otherCellNode.y));
		if (dist <0.1f) {
			//return 5.0f;
		}
		return dist;
	}

	public override bool Equals (AStarNode other)
	{
		CellNode otherCellNode = other as CellNode;
		if (waitNode == otherCellNode.waitNode) {
			return (x==otherCellNode.x && y == otherCellNode.y);
		} else {
			return (x==otherCellNode.x && y == otherCellNode.y);// && this.beat == otherCellNode.beat);
		}
	}

	public override string ToString ()
	{
		return _position.ToString();
	}
}
