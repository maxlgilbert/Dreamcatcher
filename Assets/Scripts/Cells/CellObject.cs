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
	[HideInInspector] public CellObject returnCell;
	[HideInInspector] public Vector2 location;
	public bool transitionCell;

	public CellType cellType;
	private CellGrid _cellGrid;
	void Awake () {
		location = new Vector2(gameObject.transform.position.x,gameObject.transform.position.y);

	}
	// Use this for initialization
	void Start () {
		_cellGrid = gameObject.GetComponentInParent<CellGrid> () as CellGrid;
		int xOffset = _cellGrid.width * _cellGrid.xIndex;
		int yOffset = _cellGrid.height * _cellGrid.yIndex;
		x += xOffset;
		y += yOffset;
		cellNode = new CellNode(x,y);
		CellGridManager.Instance.AddCell(this);
		if (cellType == CellType.Ground) {
			//renderer.material = CellGridManager.Instance.ground;
			Vector3 location = gameObject.transform.position;
			location.y -= 1.867725f;
			Platform newGround = Instantiate(LevelManager.Instance.ground, location,Quaternion.identity) as Platform;
			newGround.currentCell = this;
		} else if (cellType == CellType.Obstacle){
			Vector3 location = gameObject.transform.position;
			location.y -= .867725f;
			Instantiate(LevelManager.Instance.obstacle, location,Quaternion.identity);
		} else {
			renderer.material = CellGridManager.Instance.empty;
		}
		renderer.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
