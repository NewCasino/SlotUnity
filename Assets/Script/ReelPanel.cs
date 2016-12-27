using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ReelPanel : MonoBehaviour {
	private int burstSymbolNum = 0;
	private int currentTransition = 0;
	private Reel reel;
	private string spinResultJsonString;
	private ConnectRequest spinRequest;
	public GameObject symbol_gameobject;
	public GameObject[,] symbol = new GameObject[5,3];
	private IEnumerator iter;

	private string json = "{\"result\":{\"gameType\":\"cascadingReels\",\"gameResult\":{\"start\":{\"reels\":[{\"reel\":[3,1,1]},{\"reel\":[5,1,4]},{\"reel\":[1,6,7]},{\"reel\":[6,5,4]},{\"reel\":[2,9,4]}]},\"transitions\":[{\"transition\":{\"collapse\":{\"reels\":[{\"reel\":[2,3]},{\"reel\":[2]},{\"reel\":[1]}]},\"drop\":{\"reels\":[{\"reel\":[6,4]},{\"reel\":[1]},{\"reel\":[4]}]}}},{\"transition\":{\"collapse\":{\"reels\":[{\"reel\":[3]},{\"reel\":[2]},{\"reel\":[3]},{\"reel\":[3]},{\"reel\":[3]}]},\"drop\":{\"reels\":[{\"reel\":[4]},{\"reel\":[2]},{\"reel\":[3]},{\"reel\":[8]},{\"reel\":[1]}]}}}],\"end\":{\"reels\":[{\"reel\":[3,6,4]},{\"reel\":[5,1,2]},{\"reel\":[6,7,3]},{\"reel\":[6,5,8]},{\"reel\":[2,9,1]}]}}}}";

	// Use this for initialization
	void Start () {
		InitSymbol ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void InitSymbol(){
		for (int i = 0; i < 5; i++) {
			for (int j = 0; j < 3; j++) {
				symbol[i,j] = Instantiate (symbol_gameobject);
				symbol[i,j].transform.SetParent (this.transform);
				int[] pos = { j + 3, i };
				symbol[i,j].GetComponent<SymbolBehavior> ().SetCurrentPos (pos);
			}
		}
		resetSymbolPosition ();
	}

	private void resetSymbolPosition(){
		for (int i = 0; i < 5; i++) {
			for (int j = 0; j < 3; j++) {
				symbol[i,j].transform.localPosition = new Vector3 (i*250, 750 + j*250 + (i*5+j)*25, 0);
			}
		}
	}

	private void reset(){
		
	}

	public void Spin(){
		resetSymbolPosition ();
		GetSpinResult ();
	}

	public void GetSpinResult(){
		spinRequest = new ConnectRequest (ConnectRequest.SPIN);
		StartCoroutine (spinRequest.WaitForRequest (spinRequest.GetWWW(), stopSpin));
	}

	public void stopSpin(){
		spinResultJsonString = spinRequest.GetResult ();
		spinRequest = null;

		//reel = Reel.CreateFromJSON (spinResultJsonString);
		reel = Reel.CreateFromJSON (json);
		InitImage (reel);
		DropSymbol ();
		Invoke("BurstSymbol", 1.0f);
		//Invoke ("BurstSymbol", 1.0f);
		//StartCoroutine(WaitUntil(DropSymbol ()));
	}

	public void InitImage(Reel reel){
		for (int i = 0; i < 5; i++) {
			for (int j = 0; j < 3; j++) {
				symbol [i, j].GetComponent<Image> ().overrideSprite = Resources.Load<Sprite> (reel.result.gameResult.start.reels[i].reel[j].ToString());
			}
		}
	}

	private bool DropSymbol(){
		for (int i = 0; i < 5; i++) {
			for (int j = 0; j < 3; j++) {
				symbol [i, j].GetComponent<SymbolBehavior> ().SetTargetPos (j, (int)symbol [i, j].transform.localPosition.y, 250 * j);
				//symbol [j, i].GetComponent<SymbolBehavior> ().SetTargetPos (j);
			}
		}

		return true;
	}


	public void DoNextIter(){
		burstSymbolNum--;
		if (burstSymbolNum <= 0) {
			burstSymbolNum = 0;
			iter.MoveNext ();
		}
	}

	private void BurstSymbol(){
		if (currentTransition >= reel.result.gameResult.transitions.Length) {
			currentTransition = 0;
			return;
		}

		for (int i = 0; i < reel.result.gameResult.transitions [currentTransition].transition.collapse.reels.Length; i++) {
			for (int j = 0; j < reel.result.gameResult.transitions [currentTransition].transition.collapse.reels [i].reel.Length; j++) {
				symbol [i, reel.result.gameResult.transitions [currentTransition].transition.collapse.reels [i].reel[j]-1].GetComponent<SymbolBehavior> ().SetToFlash (true);
				burstSymbolNum++;
			}
		}

		iter = Drop ();
	}
		
	private IEnumerator Drop(){
		//Drop Current Symbol To bottom
		for (int i = 0; i < 5; i++) {
			int currentPos = -1;
			for (int j = 0; j < 3; j++) {
				if (symbol [i, j] != null) {
					if (currentPos >= 0) {
						symbol [i, j - currentPos - 1] = symbol [i, j];
						symbol [i, j].GetComponent<SymbolBehavior> ().SetTargetPos (j-currentPos-1);
					}
				} else {
					currentPos++;
				}
			}
		}

		//add Symbol
		int reesLength = reel.result.gameResult.transitions [currentTransition].transition.collapse.reels.Length;
		for (int k = 0; k < reesLength; k++) {
			int reelLength = reel.result.gameResult.transitions [currentTransition].transition.collapse.reels [k].reel.Length;
			for (int z = 0; z < reelLength; z++) {
				symbol[k,3-reelLength+z] = Instantiate (symbol_gameobject);
				symbol[k,3-reelLength+z].transform.SetParent (this.transform);
				int[] pos = { z + 3, k };
				symbol[k,3-reelLength+z].GetComponent<SymbolBehavior> ().SetCurrentPos (pos);
				symbol[k,3-reelLength+z].transform.localPosition = new Vector3 (k*250, 750 + z*250, 0);
				symbol [k, 3-reelLength+z].GetComponent<Image> ().overrideSprite = Resources.Load<Sprite> (reel.result.gameResult.transitions[currentTransition].transition.drop.reels[k].reel[z].ToString());
				symbol [k, 3 - reelLength + z].GetComponent<SymbolBehavior> ().SetTargetPos (3 - reelLength + z);
			}
		}

		this.currentTransition++;
		Invoke ("BurstSymbol", 1.0f);
		yield return 1;
	}

	public void DestroySymbol(int[] pos){
		symbol [pos[1], pos[0]] = null;
	}
}
