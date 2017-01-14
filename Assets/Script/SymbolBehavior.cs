using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SymbolBehavior : MonoBehaviour {

	private string myName = "";
	private int[] myPos;
	private int LightingTime = 8;
	private int currentLight = 20;
	private bool canFlash = false;
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
		CheckDropSymbol ();
		CheckFlash ();
	}

	private void CheckDropSymbol(){
		if (currentTime <= 0) return;

		currentTime--;
		//gameObject.transform.localPosition = gameObject.transform.localPosition + Vector3.down * dropDistance;
	}

	private void CheckFlash(){
		if (canFlash == false) {
			gameObject.GetComponent<Image> ().enabled = true;
			return;
		}

		Lightning ();
	}

	public void SetTargetPos(int targetPos, int cPos = 0, int tPos = 0){
		return;
		this.targetPos = targetPos;
		if (tPos - cPos != 0) {
			this.currentTime = (int)((cPos - tPos) / this.dropDistance);
		} else {
			this.currentTime = (this.currentPos - this.targetPos) * (this.boxDistance / this.dropDistance);
		}
		this.myPos[1] = this.currentPos = this.targetPos;
	}

	public void SetCurrentPos(int[] myPos){
		this.myPos = myPos;
		this.currentPos = myPos[1];
	}

	public void SetMyName(string myName){
		this.myName = myName;
	}

	public int GetSymbolPos(){
		return myPos [1];
	}

	private void Lightning(){
		if (currentLight <= 0) {
			//gameObject.GetComponent<Image> ().enabled = !gameObject.GetComponent<Image> ().enabled;
			currentLight = 20;
			LightingTime--;
			if (LightingTime <= 0) {
				
			}
		} else {
			currentLight--;
		}
	}

	public void SetToFlash(bool canFlash){
		this.canFlash = canFlash;
	}

	public void DestroyMyself(){
		Destroy (this.gameObject);
	}

	public void playAni(){
		gameObject.GetComponent<Animator> ().Play (myName);
		gameObject.GetComponent<Animator> ().StopPlayback ();
	}
}
