using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public enum CellType {
	Empty,
	Ground,
	Obstacle,
	MovingGround,
	MovingObstacle
}
public class CellObject : MonoBehaviour {
	public int x;
	public int y;
	public CellNode cellNode;
	[HideInInspector] public CellObject returnCell;
	[HideInInspector] public Vector2 location;
	public bool transitionCell;
	public int puzzleUnit;
	[HideInInspector] public bool transitionAICell;
	public List<Vector2> pathActions = new List<Vector2>();
	public CellType cellType;
	private CellGrid _cellGrid;
	public bool returnToCheckpoint;
	public bool checkPoint;
	void Awake () {
		location = new Vector2(gameObject.transform.position.x,gameObject.transform.position.y);
		transitionAICell = transitionCell;

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
			newGround.transform.parent = gameObject.transform;
		} else if (cellType == CellType.MovingGround){
			//renderer.material = CellGridManager.Instance.ground;
			Vector3 location = gameObject.transform.position;
			location.y -= 1.867725f;
			MovingPlatform newGround = Instantiate(LevelManager.Instance.movingGround, location,Quaternion.identity) as MovingPlatform;
			newGround.currentCell = this;
			newGround.yOffset = -1.867725f;
			newGround.transform.parent = gameObject.transform;
			newGround.InitializePath(pathActions);
		} else if (cellType == CellType.Obstacle){
			Vector3 location = gameObject.transform.position;
			location.y -= .867725f;
			GameObject obstacle = Instantiate(LevelManager.Instance.obstacle, location,Quaternion.identity) as GameObject;
			obstacle.transform.parent = gameObject.transform;
		} else if (cellType == CellType.MovingObstacle){
			Vector3 location = gameObject.transform.position;
			location.y -= .867725f;
			MovingPlatform newObstacle = Instantiate(LevelManager.Instance.movingObstacle, location,Quaternion.identity) as MovingPlatform;
			newObstacle.currentCell = this;
			newObstacle.yOffset = -.867725f;
			newObstacle.transform.parent = gameObject.transform;
			newObstacle.InitializePath(pathActions);
		} else {
			renderer.material = CellGridManager.Instance.empty;
		}
		renderer.enabled = false;
		if (transitionCell) {
			LevelManager.Instance.puzzleUnits[this.puzzleUnit].transitionCell = this;
		}

		if (checkPoint) {
			LevelManager.Instance.puzzleUnits[this.puzzleUnit].checkPointCell = this;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
