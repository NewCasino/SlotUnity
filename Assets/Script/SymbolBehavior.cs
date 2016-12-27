using UnityEngine;
using System.Collections;

public class SymbolBehavior : MonoBehaviour {

	private int currentPos = -1;
	private int targetPos = -1;

	private int currentTime = 0;
	private int dropDistance = 25;
	private int boxDistance = 250;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (currentTime <= 0) return;

		currentTime--;
		gameObject.transform.localPosition = gameObject.transform.localPosition + Vector3.down * dropDistance;
	}

	public void SetTargetPos(int targetPos, int cPos = 0, int tPos = 0){
		this.targetPos = targetPos;
		if (tPos - cPos != 0) {
			this.currentTime = (int)((cPos - tPos) / this.dropDistance);
		} else {
			this.currentTime = (this.currentPos - this.targetPos) * (this.boxDistance / this.dropDistance);
		}
		this.currentPos = this.targetPos;
	}

	public void SetDistance(int dropDistance, int boxDistance){
		this.dropDistance = dropDistance;
		this.boxDistance = boxDistance;
	}

	public void SetCurrentPos(int currentPos){
		this.currentPos = currentPos;
	}

//	public void SetIndividual(int cPos, int tPos, int targetPos){
//		currentTime = (tPos - cPos) / dropDistance;
//		this.targetPos = this.currentPos = targetPos;
//	}
}
