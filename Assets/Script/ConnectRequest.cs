using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ConnectRequest{

	//public GameObject reelPanel;

	private const string DOMAIN_NAME = "https://cosmos.skygeario.com/";
	public const string SPIN = DOMAIN_NAME + "spin";
	private WWW www;

	private string results = "";

	public ConnectRequest(string url){
		www = new WWW (url);
	}

	public string GetResult(){
		return results;
	}

	public WWW GetWWW(){
		return www;
	}

	public IEnumerator WaitForRequest(WWW www,System.Action onComplete) {
		yield return www;
		if (www.error == null) {
			results = www.text;
			onComplete ();
		} else {
			Debug.Log (www.error);
		}
	}
}
