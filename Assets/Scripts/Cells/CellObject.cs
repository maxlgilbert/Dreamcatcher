using UnityEngine;
using System.Collections;

public class CellObject : MonoBehaviour {
	public int x;
	public int y;
	public CellNode cellNode;
	// Use this for initialization
	void Start () {
		cellNode = new CellNode(x,y);
		CellGridManager.Instance.AddCell(cellNode);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
