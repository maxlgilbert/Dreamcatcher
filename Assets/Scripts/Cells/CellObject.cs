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
	public Vector2 location;

	public CellType cellType;
	void Awake () {
		location = new Vector2(gameObject.transform.position.x,gameObject.transform.position.y);
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
		renderer.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
