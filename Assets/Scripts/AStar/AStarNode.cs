using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Base class for a node that AStar can search paths along.
/// Inherit and overrid functions and fields to implement
/// </summary>
public class AStarNode {

	/// <summary>
	/// Whether or not this node is free.
	/// </summary>
	public bool traversable = true;

	/// <summary>
	/// Minimum currently projected distance travelled at this node.
	/// </summary>
	public float distanceTraveled = .0f;

	/// <summary>
	/// Total AStar score.
	/// </summary>
	public float fValue = 0.0f;

	public float hValue = 0.0f;
	
	/// <summary>
	/// The parent node.
	/// </summary>
	public AStarNode parent;

	public List<string> parentActions;

	/// <summary>
	/// Whether AStar has seen this node or not.
	/// </summary>
	public bool visited = false;

	public List<AStarAction> actions;

	protected List<AStarNode> _neighbors;

	public AStarNode () {
		parentActions = new List<string>();
	}

	public void UpdateNeighbors () {
		_neighbors = new List<AStarNode>();
		if (actions != null) {
			foreach (AStarAction action in actions) {
				List<AStarNode> possibleNeighbors = action.TryAction(this);
				if (possibleNeighbors != null) {
					_neighbors.AddRange(possibleNeighbors);
				}
				/*foreach (AStarNode possibleNeighbor in possibleNeighbors) {
					if (possibleNeighbor != null) {
						int numActions = 0;
						for (int i  = 0; i < this.parentActions.Count; i++) {
							possibleNeighbor.parentActions.Add(this.parentActions[i]);
							if (this.parentActions[i] == action.GetActionText()) {
								numActions++;
							}
						}
						if (numActions < action.numberAvailable) {
							possibleNeighbor.parentActions.Add(action.GetActionText());
							_neighbors.Add(possibleNeighbor);
						}
					}
				}*/
			}
		}
	}
	
	/// <summary>
	/// Function to get neighbors.
	/// </summary>
	/// <returns>The neighbors.</returns>
	public List<AStarNode> GetNeighbors (){
		UpdateNeighbors();
		return _neighbors;
	}

	/// <summary>
	/// Determines whether the specified <see cref="AStarNode"/> is equal to the current <see cref="AStarNode"/>.
	/// </summary>
	/// <param name="other">The <see cref="AStarNode"/> to compare with the current <see cref="AStarNode"/>.</param>
	/// <returns><c>true</c> if the specified <see cref="AStarNode"/> is equal to the current <see cref="AStarNode"/>; otherwise, <c>false</c>.</returns>
	public virtual bool Equals (AStarNode other) {
		return true;//(this.gameObject.transform.position == other.gameObject.transform.position);
	}

	/// <summary>
	/// Distance from this node to another.
	/// </summary>
	/// <param name="other">Other.</param>
	public virtual float distance (AStarNode other) {
		return 0.0f;//Vector3.Distance(this.gameObject.transform.position,other.gameObject.transform.position);
	}

	/// <summary>
	/// Estimated distance from this node to another.
	/// </summary>
	/// <param name="other">Other.</param>
	public virtual float estimate (AStarNode other) {
		return 0.0f;//Vector3.Distance(this.gameObject.transform.position,other.gameObject.transform.position);
	}

	/// <summary>
	/// Resets this node.
	/// </summary>
	public virtual void clear() {
		distanceTraveled = 0.0f;
		fValue = 0.0f;
		parent = null;
		visited = false;
	}
}
