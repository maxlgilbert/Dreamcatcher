using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStar {

	/// <summary>
	/// List of nodes ordered by fValue
	/// </summary>
	private iSSortedList _sortedNodes;

	private List<AStarNode> _visitedNodes;

	/// <summary>
	/// The max depth aStar searches before giving up.
	/// </summary>
	private int _maxDepth;

	public bool reachedGoal;

	/// <summary>
	/// Initializes a new instance of the <see cref="AStar"/> class.
	/// </summary>
	/// <param name="maxDepth">Max depth.</param>
	public AStar (int maxDepth) {
		_maxDepth = maxDepth;
		_sortedNodes = new iSSortedList();
		_visitedNodes = new List<AStarNode> ();
	}

	/// <summary>
	/// Finds the path with aStar.
	/// </summary>
	/// <returns>The path.</returns>
	/// <param name="start">Start.</param>
	/// <param name="goal">Goal.</param>
	public List<AStarNode> FindPath (AStarNode start, AStarNode goal) {
		// Set curr to be the starting node
		AStarNode curr = start;
		curr.visited = true;
		int depth = 0;
		// Initialize the "closest to goal" node
		AStarNode closestNode = start;
		reachedGoal = true;
		// Search through curr's neighbors and and set curr to best guess.
		// If curr becomes the goal, stop, else keep looking through neighbors.
		while (depth < _maxDepth && !curr.Equals(goal)) {
			// Add the current node to visited nodes
			_visitedNodes.Add(curr);
			for (int i = 0; i < curr.GetNeighbors().Count; i++) {
				AStarNode neighbor = curr.GetNeighbors()[i];
				// Check if node is active or has been visited.
				if (neighbor.traversable) {
					bool visited = false;
					for (int j = 0; j < _visitedNodes.Count; j++) {
						if (neighbor.Equals(_visitedNodes[j])) {
							visited = true;
						}
					}
					if (!visited) { //TODO is this okay?
						// Get projected actual distance travelled and estimated distance to goal.
						float gValue = curr.distanceTraveled + curr.distance(neighbor);
						float hValue = neighbor.estimate(goal);
						// If node has not beem visited or the new combined fValue is lower
						// add the node (or update its values) to the sorted list.
						if (!neighbor.visited || neighbor.fValue  > gValue + hValue) {
							neighbor.fValue = gValue + hValue;
							neighbor.parent = curr;
							neighbor.visited = true;
							neighbor.distanceTraveled = gValue;
							_sortedNodes.Add(neighbor);
							if (hValue > closestNode.hValue) {
								closestNode = neighbor;
							}
						}
					}
				}
			}
			// Get the node with lowest fValue (best guess).
			if (_sortedNodes.Count() > 0) {
				curr = _sortedNodes.Pop();
			} else {
				return RetrievePath(closestNode);
			}
			depth++;
		}
		if (!curr.Equals(goal)) {
			return RetrievePath(closestNode);
		}
		return RetrievePath(curr);
	}

	private List<AStarNode> RetrievePath (AStarNode node) {
		// Create the list to return.
		List<AStarNode> path = new List<AStarNode>();
		AStarNode curr = node;
		path.Add(curr);
		int depth = 0;
		// Populate the list with the destination node's parents.
		while (depth < _maxDepth && curr.parent != null) {
			curr = curr.parent;
			path.Add(curr);
			depth ++;
		}
		// Return the reverse of the path.
		path.Reverse();
		return path;
	}

	/// <summary>
	/// Reset this instance of aStar.
	/// </summary>
	public void Reset () {
		_sortedNodes = new iSSortedList();
		_visitedNodes = new List<AStarNode>();
	}
}
