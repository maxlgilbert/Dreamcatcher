using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStarAction : MonoBehaviour {
	public int numberAvailable;
	public string identification;
	protected List<AStarNode> _possibleNeighbors;

	void Awake () {
		_possibleNeighbors = new List<AStarNode>();
	}
	public virtual string GetActionText() {
		return gameObject.name;
	}

	public virtual List<AStarNode> TryAction(AStarNode curr) {
		return null;
	}

	public virtual bool Equals(AStarAction other) {
		return (identification == other.identification);
	}
}
