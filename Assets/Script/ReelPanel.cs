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
	public GameObject Floor;
	public GameObject SpinButton;
	private IEnumerator iter;

	//private string json = "{\"result\":{\"gameType\":\"cascadingReels\",\"gameResult\":{\"start\":{\"reels\":[{\"reel\":[3,1,1]},{\"reel\":[5,1,4]},{\"reel\":[1,6,7]},{\"reel\":[6,5,4]},{\"reel\":[2,9,4]}]},\"transitions\":[{\"transition\":{\"collapse\":{\"reels\":[{\"reel\":[2,3]},{\"reel\":[2]},{\"reel\":[1]}]},\"drop\":{\"reels\":[{\"reel\":[6,4]},{\"reel\":[1]},{\"reel\":[4]}]}}},{\"transition\":{\"collapse\":{\"reels\":[{\"reel\":[3]},{\"reel\":[2]},{\"reel\":[3]},{\"reel\":[3]},{\"reel\":[3]}]},\"drop\":{\"reels\":[{\"reel\":[4]},{\"reel\":[2]},{\"reel\":[3]},{\"reel\":[8]},{\"reel\":[1]}]}}}],\"end\":{\"reels\":[{\"reel\":[3,6,4]},{\"reel\":[5,1,2]},{\"reel\":[6,7,3]},{\"reel\":[6,5,8]},{\"reel\":[2,9,1]}]}}}}";
	//private string json = "{\"result\":{\"gameType\":\"cascadingReels\",\"gameResult\":{\"start\":{\"reels\":[{\"reel\":[1,1,3]},{\"reel\":[4,1,5]},{\"reel\":[7,6,1]},{\"reel\":[4,5,6]},{\"reel\":[4,9,2]}]},\"transitions\":[{\"transition\":{\"collapse\":{\"reels\":[{\"reel\":[1,2]},{\"reel\":[2]},{\"reel\":[3]}]},\"drop\":{\"reels\":[{\"reel\":[6,4]},{\"reel\":[1]},{\"reel\":[4]}]}}},{\"transition\":{\"collapse\":{\"reels\":[{\"reel\":[1]},{\"reel\":[2]},{\"reel\":[1]},{\"reel\":[1]},{\"reel\":[1]}]},\"drop\":{\"reels\":[{\"reel\":[4]},{\"reel\":[2]},{\"reel\":[3]},{\"reel\":[8]},{\"reel\":[1]}]}}}],\"end\":{\"reels\":[{\"reel\":[4,6,3]},{\"reel\":[2,1,5]},{\"reel\":[3,7,6]},{\"reel\":[8,5,6]},{\"reel\":[1,9,2]}]}}}}";
	private string json = "{\"cosmos\":{\"gameResults\":{\"startScreen\":{\"reels\":[{\"reel\":[\"SYML_04\",\"SYML_03\",\"SYML_02\"]},{\"reel\":[\"SYML_05\",\"SYML_04\",\"SYML_03\"]},{\"reel\":[\"SYML_05\",\"SYML_03\",\"SYML_03\"]},{\"reel\":[\"SYML_06\",\"SYML_04\",\"SYML_06\"]},{\"reel\":[\"SYML_01\",\"SYML_05\",\"SYML_06\"]}]},\"endScreen\":{\"reels\":[{\"reel\":[\"SYML_00\",\"SYML_02\",\"SYML_02\"]},{\"reel\":[\"SYML_04\",\"SYML_06\",\"SYML_06\"]},{\"reel\":[\"SYML_00\",\"SYML_04\",\"SYML_01\"]},{\"reel\":[\"SYML_03\",\"SYML_06\",\"SYML_06\"]},{\"reel\":[\"SYML_01\",\"SYML_05\",\"SYML_06\"]}]},\"topScreen\":{\"reels\":[{\"reel\":[\"SYML_00\",\"SYML_05\",\"SYML_02\",\"SYML_00\"]},{\"reel\":[\"SYML_06\",\"SYML_05\",\"SYML_06\",\"SYML_00\",\"SYML_04\"]},{\"reel\":[\"SYML_04\",\"SYML_04\",\"SYML_00\",\"SYML_05\",\"SYML_01\",\"SYML_04\",\"SYML_00\"]},{\"reel\":[\"SYML_03\"]},{\"reel\":[]}]},\"transitions\":[{\"collapse\":{\"targets\":[{\"target\":{\"symbol\":\"SYML_03\",\"positions\":[{\"p\":[0,1]},{\"p\":[1,2]},{\"p\":[2,2]},{\"p\":[2,1]}]}}]}},{\"collapse\":{\"targets\":[{\"target\":{\"symbol\":\"SYML_04\",\"positions\":[{\"p\":[0,1]},{\"p\":[1,2]},{\"p\":[2,1]},{\"p\":[2,0]},{\"p\":[3,1]}]}}]}},{\"collapse\":{\"targets\":[{\"target\":{\"symbol\":\"SYML_05\",\"positions\":[{\"p\":[0,0]},{\"p\":[1,2]},{\"p\":[1,0]},{\"p\":[2,2]},{\"p\":[2,0]}]}}]}},{\"collapse\":{\"targets\":[{\"target\":{\"symbol\":\"SYML_00\",\"positions\":[{\"p\":[0,1]},{\"p\":[1,0]},{\"p\":[2,2]}]}}]}}],\"wins\":{\"wins\":{\"totalWinAmt\":145,\"results\":[{\"result\":{\"symbol\":\"SYML_03\",\"payOut\":{\"pay\":20,\"desc\":\"Big Win! wins 20\"}}},{\"result\":{\"symbol\":\"SYML_04\",\"payOut\":{\"pay\":100,\"desc\":\"\"}}},{\"result\":{\"symbol\":\"SYML_05\",\"payOut\":{\"pay\":20,\"desc\":\"Big Win! wins 20\"}}},{\"result\":{\"symbol\":\"SYML_00\",\"payOut\":{\"pay\":5,\"desc\":\"Happy! wins 5\"}}}]}}},\"gameProgress\":[{\"result\":{\"reels\":[{\"reel\":[\"SYML_04\",\"SYML_03\",\"SYML_02\"]},{\"reel\":[\"SYML_05\",\"SYML_04\",\"SYML_03\"]},{\"reel\":[\"SYML_05\",\"SYML_03\",\"SYML_03\"]},{\"reel\":[\"SYML_06\",\"SYML_04\",\"SYML_06\"]},{\"reel\":[\"SYML_01\",\"SYML_05\",\"SYML_06\"]}]},\"winEvals\":{\"hasWin\":true,\"reelScreen\":\"SYML_04:SYML_03:SYML_02|SYML_05:SYML_04:SYML_03|SYML_05:SYML_03:SYML_03|SYML_06:SYML_04:SYML_06|SYML_01:SYML_05:SYML_06\",\"results\":[{\"result\":{\"symbol\":\"SYML_00\",\"isWin\":false,\"winMatching\":null,\"payOut\":null}},{\"result\":{\"symbol\":\"SYML_01\",\"isWin\":false,\"winMatching\":null,\"payOut\":null}},{\"result\":{\"symbol\":\"SYML_02\",\"isWin\":false,\"winMatching\":null,\"payOut\":null}},{\"result\":{\"symbol\":\"SYML_03\",\"isWin\":true,\"winMatching\":[\"SYML_04:SYML_03:SYML_02|SYML_05:SYML_04:SYML_03|SYML_05:SYML_03:SYML_03\"],\"payOut\":{\"pay\":20,\"desc\":\"Big Win! wins 20\"}}},{\"result\":{\"symbol\":\"SYML_04\",\"isWin\":false,\"winMatching\":null,\"payOut\":null}},{\"result\":{\"symbol\":\"SYML_05\",\"isWin\":false,\"winMatching\":null,\"payOut\":null}},{\"result\":{\"symbol\":\"SYML_06\",\"isWin\":false,\"winMatching\":null,\"payOut\":null}}]}},{\"result\":{\"reels\":[{\"reel\":[\"SYML_00\",\"SYML_04\",\"SYML_02\"]},{\"reel\":[\"SYML_06\",\"SYML_05\",\"SYML_04\"]},{\"reel\":[\"SYML_04\",\"SYML_04\",\"SYML_05\"]},{\"reel\":[\"SYML_06\",\"SYML_04\",\"SYML_06\"]},{\"reel\":[\"SYML_01\",\"SYML_05\",\"SYML_06\"]}]},\"winEvals\":{\"hasWin\":true,\"reelScreen\":\"SYML_00:SYML_04:SYML_02|SYML_06:SYML_05:SYML_04|SYML_04:SYML_04:SYML_05|SYML_06:SYML_04:SYML_06|SYML_01:SYML_05:SYML_06\",\"results\":[{\"result\":{\"symbol\":\"SYML_00\",\"isWin\":false,\"winMatching\":null,\"payOut\":null}},{\"result\":{\"symbol\":\"SYML_01\",\"isWin\":false,\"winMatching\":null,\"payOut\":null}},{\"result\":{\"symbol\":\"SYML_02\",\"isWin\":false,\"winMatching\":null,\"payOut\":null}},{\"result\":{\"symbol\":\"SYML_03\",\"isWin\":false,\"winMatching\":null,\"payOut\":null}},{\"result\":{\"symbol\":\"SYML_04\",\"isWin\":true,\"winMatching\":[\"SYML_00:SYML_04:SYML_02|SYML_06:SYML_05:SYML_04|SYML_04:SYML_04:SYML_05|SYML_06:SYML_04:SYML_06\"],\"payOut\":{\"pay\":100,\"desc\":\"\"}}},{\"result\":{\"symbol\":\"SYML_05\",\"isWin\":false,\"winMatching\":null,\"payOut\":null}},{\"result\":{\"symbol\":\"SYML_06\",\"isWin\":false,\"winMatching\":null,\"payOut\":null}}]}},{\"result\":{\"reels\":[{\"reel\":[\"SYML_05\",\"SYML_00\",\"SYML_02\"]},{\"reel\":[\"SYML_05\",\"SYML_06\",\"SYML_05\"]},{\"reel\":[\"SYML_05\",\"SYML_00\",\"SYML_05\"]},{\"reel\":[\"SYML_03\",\"SYML_06\",\"SYML_06\"]},{\"reel\":[\"SYML_01\",\"SYML_05\",\"SYML_06\"]}]},\"winEvals\":{\"hasWin\":true,\"reelScreen\":\"SYML_05:SYML_00:SYML_02|SYML_05:SYML_06:SYML_05|SYML_05:SYML_00:SYML_05|SYML_03:SYML_06:SYML_06|SYML_01:SYML_05:SYML_06\",\"results\":[{\"result\":{\"symbol\":\"SYML_00\",\"isWin\":false,\"winMatching\":null,\"payOut\":null}},{\"result\":{\"symbol\":\"SYML_01\",\"isWin\":false,\"winMatching\":null,\"payOut\":null}},{\"result\":{\"symbol\":\"SYML_02\",\"isWin\":false,\"winMatching\":null,\"payOut\":null}},{\"result\":{\"symbol\":\"SYML_03\",\"isWin\":false,\"winMatching\":null,\"payOut\":null}},{\"result\":{\"symbol\":\"SYML_04\",\"isWin\":false,\"winMatching\":null,\"payOut\":null}},{\"result\":{\"symbol\":\"SYML_05\",\"isWin\":true,\"winMatching\":[\"SYML_05:SYML_00:SYML_02|SYML_05:SYML_06:SYML_05|SYML_05:SYML_00:SYML_05\"],\"payOut\":{\"pay\":20,\"desc\":\"Big Win! wins 20\"}}},{\"result\":{\"symbol\":\"SYML_06\",\"isWin\":false,\"winMatching\":null,\"payOut\":null}}]}},{\"result\":{\"reels\":[{\"reel\":[\"SYML_02\",\"SYML_00\",\"SYML_02\"]},{\"reel\":[\"SYML_00\",\"SYML_06\",\"SYML_06\"]},{\"reel\":[\"SYML_04\",\"SYML_01\",\"SYML_00\"]},{\"reel\":[\"SYML_03\",\"SYML_06\",\"SYML_06\"]},{\"reel\":[\"SYML_01\",\"SYML_05\",\"SYML_06\"]}]},\"winEvals\":{\"hasWin\":true,\"reelScreen\":\"SYML_02:SYML_00:SYML_02|SYML_00:SYML_06:SYML_06|SYML_04:SYML_01:SYML_00|SYML_03:SYML_06:SYML_06|SYML_01:SYML_05:SYML_06\",\"results\":[{\"result\":{\"symbol\":\"SYML_00\",\"isWin\":true,\"winMatching\":[\"SYML_02:SYML_00:SYML_02|SYML_00:SYML_06:SYML_06|SYML_04:SYML_01:SYML_00\"],\"payOut\":{\"pay\":5,\"desc\":\"Happy! wins 5\"}}},{\"result\":{\"symbol\":\"SYML_01\",\"isWin\":false,\"winMatching\":null,\"payOut\":null}},{\"result\":{\"symbol\":\"SYML_02\",\"isWin\":false,\"winMatching\":null,\"payOut\":null}},{\"result\":{\"symbol\":\"SYML_03\",\"isWin\":false,\"winMatching\":null,\"payOut\":null}},{\"result\":{\"symbol\":\"SYML_04\",\"isWin\":false,\"winMatching\":null,\"payOut\":null}},{\"result\":{\"symbol\":\"SYML_05\",\"isWin\":false,\"winMatching\":null,\"payOut\":null}},{\"result\":{\"symbol\":\"SYML_06\",\"isWin\":false,\"winMatching\":null,\"payOut\":null}}]}},{\"result\":{\"reels\":[{\"reel\":[\"SYML_00\",\"SYML_02\",\"SYML_02\"]},{\"reel\":[\"SYML_04\",\"SYML_06\",\"SYML_06\"]},{\"reel\":[\"SYML_00\",\"SYML_04\",\"SYML_01\"]},{\"reel\":[\"SYML_03\",\"SYML_06\",\"SYML_06\"]},{\"reel\":[\"SYML_01\",\"SYML_05\",\"SYML_06\"]}]},\"winEvals\":{\"hasWin\":false,\"reelScreen\":\"SYML_00:SYML_02:SYML_02|SYML_04:SYML_06:SYML_06|SYML_00:SYML_04:SYML_01|SYML_03:SYML_06:SYML_06|SYML_01:SYML_05:SYML_06\",\"results\":[{\"result\":{\"symbol\":\"SYML_00\",\"isWin\":false,\"winMatching\":null,\"payOut\":null}},{\"result\":{\"symbol\":\"SYML_01\",\"isWin\":false,\"winMatching\":null,\"payOut\":null}},{\"result\":{\"symbol\":\"SYML_02\",\"isWin\":false,\"winMatching\":null,\"payOut\":null}},{\"result\":{\"symbol\":\"SYML_03\",\"isWin\":false,\"winMatching\":null,\"payOut\":null}},{\"result\":{\"symbol\":\"SYML_04\",\"isWin\":false,\"winMatching\":null,\"payOut\":null}},{\"result\":{\"symbol\":\"SYML_05\",\"isWin\":false,\"winMatching\":null,\"payOut\":null}},{\"result\":{\"symbol\":\"SYML_06\",\"isWin\":false,\"winMatching\":null,\"payOut\":null}}]}}]}}";
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

	private void DropAllSymbol(){
		int childNum = 0;
		foreach (Transform child in gameObject.transform) {
			if (child.tag != "Floor") {
				childNum++;
			}
		}
		if (childNum > 1) {
			for (int i = 0; i < reelData.playScreen.reels.Length; i++) {
				Invoke ("MoveRightFloor", i * 0.08f);
			}
			Invoke ("setFloorFalse", 1.0f);
			//Floor.SetActive (false);
		}

	}

	private void MoveRightFloor(){
		Floor.transform.position = Floor.transform.position + Vector3.right * 250;
	}

	private void setFloorFalse(){
		Floor.transform.position = Floor.transform.position + Vector3.left * 1250;
		Floor.SetActive (false);
	}

	private void DestoryAllSymbol(){
		reelScreen = null;
		reelScreen = new ReelScreen ();
		foreach (Transform child in gameObject.transform) {
			if (child.tag != "Floor") {
				Destroy (child.gameObject);
			}
		}
		Floor.SetActive (true);
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
		SpinButton.GetComponent<Button> ().enabled = false;
		DropAllSymbol ();
		Invoke ("DestoryAllSymbol", 1.0f);
		//Invoke ("GetSpinResult", 1.1f);
		Invoke ("stopSpin", 1.1f);
	}

	public void GetSpinResult(){
		//spinRequest = new ConnectRequest (ConnectRequest.SPIN);
		//StartCoroutine (spinRequest.WaitForRequest (spinRequest.GetWWW(), stopSpin));
	}

	public void stopSpin(){
		//spinResultJsonString = spinRequest.GetResult ();
		spinResultJsonString = json;
		Debug.Log (spinResultJsonString);
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
			DestroyData ();
			iter.MoveNext ();
		}
	}

	private void BurstSymbol(){
		if (currentTransition >= reelData.cosmos.gameResults.transitions.Length) {
			currentTransition = 0;
			SpinButton.GetComponent<Button> ().enabled = true;
			return;
		}

		burstSymbolNum = reelData.cosmos.gameResults.transitions [currentTransition].collapse.targets.Length;

		StartCoroutine (playOneByOneAni());

		iter = Drop ();
	}

	private IEnumerator playOneByOneAni(){
		int targetsLength = reelData.cosmos.gameResults.transitions [currentTransition].collapse.targets.Length;
		for (int j = 0; j < targetsLength; j++) {
			for (int i = 0; i < reelData.cosmos.gameResults.transitions [currentTransition].collapse.targets [j].target.positions.Length; i++) {
				int p0 = reelData.cosmos.gameResults.transitions [currentTransition].collapse.targets [j].target.positions [i].p [0];
				reelScreen.getLineSymbol (p0, reelData.cosmos.gameResults.transitions [currentTransition].collapse.targets [j].target.positions [i].p [1]).GetComponent<SymbolBehavior> ().SetToFlash (true);
			}
			yield return new WaitForSeconds (2);
			for (int i = 0; i < reelData.cosmos.gameResults.transitions [currentTransition].collapse.targets [j].target.positions.Length; i++) {
				int p0 = reelData.cosmos.gameResults.transitions [currentTransition].collapse.targets [j].target.positions [i].p [0];
				reelScreen.getLineSymbol (p0, reelData.cosmos.gameResults.transitions [currentTransition].collapse.targets [j].target.positions [i].p [1]).GetComponent<SymbolBehavior> ().SetToFlash (false);
			}
			DoNextIter ();
		}
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
		bool[,] destroyArray = new bool[reelData.cosmos.gameResults.startScreen.reels.Length, reelData.cosmos.gameResults.startScreen.reels [0].reel.Length];
		for (int i = 0; i < reelData.cosmos.gameResults.startScreen.reels.Length; i++) {
			for (int j = 0; j < reelData.cosmos.gameResults.startScreen.reels [0].reel.Length; j++) {
				destroyArray [i, j] = false;
			}
		}
		for (int j = 0; j < reelData.cosmos.gameResults.transitions [currentTransition].collapse.targets.Length; j++) {
			for (int i = 0; i < reelData.cosmos.gameResults.transitions [currentTransition].collapse.targets [j].target.positions.Length; i++) {
				int p0 = reelData.cosmos.gameResults.transitions [currentTransition].collapse.targets [j].target.positions [i].p [0];
				destroyArray [p0, reelData.cosmos.gameResults.transitions [currentTransition].collapse.targets [j].target.positions [i].p [1]] = true;
			}
		}
		for (int i = 0; i < reelData.cosmos.gameResults.startScreen.reels.Length; i++) {
			for (int j = reelData.cosmos.gameResults.startScreen.reels [0].reel.Length-1;j>=0; j--) {
				if (destroyArray [i, j] == true) {
					reelScreen.getLineSymbol (i,j).GetComponent<SymbolBehavior> ().DestroyMyself ();
				}
			}
		}
	}

	public void DestroyData(){
		bool[,] destroyArray = new bool[reelData.cosmos.gameResults.startScreen.reels.Length, reelData.cosmos.gameResults.startScreen.reels [0].reel.Length];
		for (int i = 0; i < reelData.cosmos.gameResults.startScreen.reels.Length; i++) {
			for (int j = 0; j < reelData.cosmos.gameResults.startScreen.reels [0].reel.Length; j++) {
				destroyArray [i, j] = false;
			}
		}
		for (int j = 0; j < reelData.cosmos.gameResults.transitions [currentTransition].collapse.targets.Length; j++) {
			for (int i = 0; i < reelData.cosmos.gameResults.transitions [currentTransition].collapse.targets [j].target.positions.Length; i++) {
				int p0 = reelData.cosmos.gameResults.transitions [currentTransition].collapse.targets [j].target.positions [i].p [0];
				destroyArray [p0, reelData.cosmos.gameResults.transitions [currentTransition].collapse.targets [j].target.positions [i].p [1]] = true;
			}
		}
		for (int i = 0; i < reelData.cosmos.gameResults.startScreen.reels.Length; i++) {
			for (int j = reelData.cosmos.gameResults.startScreen.reels [0].reel.Length-1;j>=0; j--) {
				if (destroyArray [i, j] == true) {
					reelScreen.destroySymbol (i,j);
				}
			}
		}
	}
}
