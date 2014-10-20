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
	}
	// Use this for initialization
	void Start () {
		cellNode = new CellNode(x,y);
		CellGridManager.Instance.AddCell(this);
		if (cellType == CellType.Ground || cellType == CellType.Empty) {
			renderer.material = CellGridManager.Instance.ground;
		} else {
			renderer.material = CellGridManager.Instance.obstacle;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
