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
	public CellObject returnCell;

	public CellType cellType;
	void Awake () {
	}
	// Use this for initialization
	void Start () {
		cellNode = new CellNode(x,y);
		CellGridManager.Instance.AddCell(this);
		if (cellType == CellType.Ground) {
			renderer.material = CellGridManager.Instance.ground;
		} else if (cellType == CellType.Obstacle){
			renderer.material = CellGridManager.Instance.obstacle;
		} else {
			renderer.material = CellGridManager.Instance.empty;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
