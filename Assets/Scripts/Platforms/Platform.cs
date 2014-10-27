using UnityEngine;
using System.Collections;

public class Platform : MonoBehaviour {

	[HideInInspector] public CellObject currentCell;
	
	public CellObject returnCell;

	public CellType cellType;
	// Use this for initialization
	void Start () {
		BeatManager.Instance.Beat += BeatHandler;
		//currentCell = gameObject.GetComponentInParent<CellObject>() as CellObject;
		if (currentCell == null) {
			//currentCell = gameObject.GetComponent<CellObject>() as CellObject;
		}
		if (currentCell != null) {
			currentCell.cellType = cellType;
			currentCell.returnCell = this.returnCell;
		} else {
			Debug.LogError("No parent cell specified!");
		}
		if (currentCell.transitionCell) {
			renderer.material = CellGridManager.Instance.ground;
		} else if (cellType == CellType.Obstacle){
			renderer.material = CellGridManager.Instance.obstacle;
		} else {
			renderer.material = CellGridManager.Instance.empty;
		}
		Initialize();
	}
	protected virtual void Initialize(){
		//Debug.LogWarning("Didn't override initialize!");
	}

	protected virtual void SetCellTypes() {
		for (int i =0; i <BeatManager.Instance.totalBeats; i++) {
			CellGridManager.Instance.SetCellTypeAtBeat(currentCell.x,currentCell.y,i,currentCell.cellType);
		}
	}
	// Update is called once per frame
	void Update () {
	
	}
	
	protected virtual void BeatHandler (BeatManager beatManager) {
		if (BeatManager.Instance.beatNumber == -1) {
			SetCellTypes();
		}
		//Debug.Log("Beat fired!");
	}
}
