using UnityEngine;
using System.Collections;
public enum CellType {
	Empty,
	Ground,
	Obstacle
}
public class CellObject : MonoBehaviour {
	public int x;
	public int y;
	public CellNode cellNode;

	public CellType cellType;
	// Use this for initialization
	void Start () {
		cellNode = new CellNode(x,y);
		CellGridManager.Instance.AddCell(this);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
