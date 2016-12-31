using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using LitJson;

public class ReelPanel : MonoBehaviour {
	private int burstSymbolNum = 0;
	private int currentTransition = 0;
	private ReelData reelData;
	private ReelScreen reelScreen = new ReelScreen();
	private string spinResultJsonString;
	private ConnectRequest spinRequest;
	public GameObject symbol_gameobject;
	private IEnumerator iter;

	//private string json = "{\"result\":{\"gameType\":\"cascadingReels\",\"gameResult\":{\"start\":{\"reels\":[{\"reel\":[3,1,1]},{\"reel\":[5,1,4]},{\"reel\":[1,6,7]},{\"reel\":[6,5,4]},{\"reel\":[2,9,4]}]},\"transitions\":[{\"transition\":{\"collapse\":{\"reels\":[{\"reel\":[2,3]},{\"reel\":[2]},{\"reel\":[1]}]},\"drop\":{\"reels\":[{\"reel\":[6,4]},{\"reel\":[1]},{\"reel\":[4]}]}}},{\"transition\":{\"collapse\":{\"reels\":[{\"reel\":[3]},{\"reel\":[2]},{\"reel\":[3]},{\"reel\":[3]},{\"reel\":[3]}]},\"drop\":{\"reels\":[{\"reel\":[4]},{\"reel\":[2]},{\"reel\":[3]},{\"reel\":[8]},{\"reel\":[1]}]}}}],\"end\":{\"reels\":[{\"reel\":[3,6,4]},{\"reel\":[5,1,2]},{\"reel\":[6,7,3]},{\"reel\":[6,5,8]},{\"reel\":[2,9,1]}]}}}}";
	//private string json = "{\"result\":{\"gameType\":\"cascadingReels\",\"gameResult\":{\"start\":{\"reels\":[{\"reel\":[1,1,3]},{\"reel\":[4,1,5]},{\"reel\":[7,6,1]},{\"reel\":[4,5,6]},{\"reel\":[4,9,2]}]},\"transitions\":[{\"transition\":{\"collapse\":{\"reels\":[{\"reel\":[1,2]},{\"reel\":[2]},{\"reel\":[3]}]},\"drop\":{\"reels\":[{\"reel\":[6,4]},{\"reel\":[1]},{\"reel\":[4]}]}}},{\"transition\":{\"collapse\":{\"reels\":[{\"reel\":[1]},{\"reel\":[2]},{\"reel\":[1]},{\"reel\":[1]},{\"reel\":[1]}]},\"drop\":{\"reels\":[{\"reel\":[4]},{\"reel\":[2]},{\"reel\":[3]},{\"reel\":[8]},{\"reel\":[1]}]}}}],\"end\":{\"reels\":[{\"reel\":[4,6,3]},{\"reel\":[2,1,5]},{\"reel\":[3,7,6]},{\"reel\":[8,5,6]},{\"reel\":[1,9,2]}]}}}}";
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void InitSymbol(){
		for (int i = 0; i < reelData.playScreen.reels.Length; i++) {
			for (int j = 0; j < reelData.playScreen.reels [i].reel.Length; j++) {
				GameObject newSymbol = Instantiate (symbol_gameobject);
				reelScreen.addSymbolToLine (i, newSymbol);
				reelScreen.getLineSymbol (i, j).transform.SetParent (this.transform);
				int[] pos = { i, j };
				reelScreen.getLineSymbol (i, j).GetComponent<SymbolBehavior> ().SetCurrentPos (pos);
				reelScreen.getLineSymbol (i, j).name = i.ToString () + j.ToString ();
			}
		}
		resetSymbolPosition ();
	}

	private void DestoryAllSymbol(){
		reelScreen = null;
		reelScreen = new ReelScreen ();
		foreach (Transform child in gameObject.transform) {
			//DestoryAllSymbol (child);
			//Destroy (child);
			Destroy(child.gameObject);
		}
	}

	private void resetSymbolPosition(){
		for (int i = 0; i < reelData.playScreen.reels.Length; i++) {
			for (int j = 0; j < reelData.playScreen.reels [i].reel.Length; j++) {
				reelScreen.getLineSymbol (i, j).transform.localPosition = new Vector3 (i * 250, 750 + j * 250 + (i * 5 + j) * 25, 0);
			}
		}
	}

	private void reset(){
		
	}

	public void Spin(){
		DestoryAllSymbol ();
		GetSpinResult ();
	}

	public void GetSpinResult(){
		spinRequest = new ConnectRequest (ConnectRequest.SPIN);
		StartCoroutine (spinRequest.WaitForRequest (spinRequest.GetWWW(), stopSpin));
	}

	public void stopSpin(){
		spinResultJsonString = spinRequest.GetResult ();
		spinRequest = null;

		reelData = JsonMapper.ToObject<ReelData>(spinResultJsonString);
		reelData.Massage ();

		InitSymbol();
		InitImage ();
		DropSymbol ();
		Invoke("BurstSymbol", 1.0f);
	}

	public void InitImage(){
		for (int i = 0; i < reelData.playScreen.reels.Length; i++) {
			for (int j = 0; j < reelData.playScreen.reels[i].reel.Length; j++) {
				reelScreen.getLineSymbol(i,j).GetComponent<Image> ().overrideSprite = Resources.Load<Sprite> (reelData.playScreen.reels[i].reel[j].ToString());
			}
		}
	}

	private bool DropSymbol(){
		for (int i = 0; i < reelData.playScreen.reels.Length; i++) {
			for (int j = 0; j < reelData.playScreen.reels [i].reel.Length; j++) {
				reelScreen.getLineSymbol(i,j).GetComponent<SymbolBehavior> ().SetTargetPos (j, (int)reelScreen.getLineSymbol(i,j).transform.localPosition.y, 250 * j);
			}
		}

		return true;
	}


	public void DoNextIter(){
		burstSymbolNum--;
		if (burstSymbolNum <= 0) {
			burstSymbolNum = 0;
			DestroySymbol ();
			iter.MoveNext ();
		}
	}

	private void BurstSymbol(){
		if (currentTransition >= reelData.cosmos.gameResult.transitions.Length) {
			currentTransition = 0;
			return;
		}

		for (int i = 0; i < reelData.cosmos.gameResult.transitions [currentTransition].collapse.targets[0].target.positions.Length; i++) {
			for (int j = 0; j < reelData.cosmos.gameResult.transitions [currentTransition].collapse.targets[0].target.positions[i].p.Length; j++) {
				reelScreen.getLineSymbol(i, reelData.cosmos.gameResult.transitions [currentTransition].collapse.targets[0].target.positions[i].p[j]).GetComponent<SymbolBehavior> ().SetToFlash (true);
				burstSymbolNum++;
			}
		}

		iter = Drop ();
	}
		
	private IEnumerator Drop(){
		for (int i = 0; i < reelScreen.symbols_line.Length; i++) {
			for (int j = 0; j < reelScreen.symbols_line[i].reel_symbol.Count; j++) {
				if (reelScreen.getLineSymbol (i, j).GetComponent<SymbolBehavior> ().GetSymbolPos () != j) {
					reelScreen.getLineSymbol (i, j).GetComponent<SymbolBehavior> ().SetTargetPos (j);
				}
			}
		}

		this.currentTransition++;
		Invoke ("BurstSymbol", 1.0f);
		yield return 1;
	}

	public void DestroySymbol(){
		for (int i = 0; i < reelData.cosmos.gameResult.transitions [currentTransition].collapse.targets[0].target.positions.Length; i++) {
			for (int j = 0; j < reelData.cosmos.gameResult.transitions [currentTransition].collapse.targets[0].target.positions[i].p.Length; j++) {
				reelScreen.destroySymbol (i, reelData.cosmos.gameResult.transitions [currentTransition].collapse.targets[0].target.positions[i].p[j]);
			}
		}
	}
}
