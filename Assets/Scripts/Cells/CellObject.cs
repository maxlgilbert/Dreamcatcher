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
	void Awake () {
		cellNode = new CellNode(x,y);
	}
	// Use this for initialization
	void Start () {
		CellGridManager.Instance.AddCell(this);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
