using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ReelPanel : MonoBehaviour {
	private string spinResultJsonString;
	private ConnectRequest spinRequest;
	public GameObject symbol_gameobject;
	public GameObject[,] symbol = new GameObject[3,5];

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
				symbol[j,i] = Instantiate (symbol_gameobject);
				symbol[j,i].transform.SetParent (this.transform);
				symbol [j, i].GetComponent<SymbolBehavior> ().SetCurrentPos (j + 3);
			}
		}
		resetSymbolPosition ();
	}

	private void resetSymbolPosition(){
		for (int i = 0; i < 5; i++) {
			for (int j = 0; j < 3; j++) {
				symbol[j,i].transform.localPosition = new Vector3 (i*250, 750 + j*250 + (i*5+j)*25, 0);
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

		Reel reel = Reel.CreateFromJSON (spinResultJsonString);
		initImage (reel);
		DropSymbol ();
	}

	public void initImage(Reel reel){
		for (int i = 0; i < 5; i++) {
			for (int j = 0; j < 3; j++) {
				symbol [j, i].GetComponent<Image> ().overrideSprite = Resources.Load<Sprite> (reel.result.gameResult.start.reels[i].reel[j].ToString());
			}
		}
	}

	private void DropSymbol(){
		for (int i = 0; i < 5; i++) {
			for (int j = 0; j < 3; j++) {
				symbol [j, i].GetComponent<SymbolBehavior> ().SetTargetPos (j, (int)symbol [j, i].transform.localPosition.y, 250 * j);
				//symbol [j, i].GetComponent<SymbolBehavior> ().SetTargetPos (j);
			}
		}
	}
}
