using UnityEngine;
using System.Collections.Generic;

public class ReelScreen{
	public Reels[] symbols_line = new Reels[5];

	public struct Reels{
		public List<GameObject> reel_symbol;
	}

	public GameObject getLineSymbol(int linePos, int symbolPos){
		return symbols_line [linePos].reel_symbol [symbolPos];
	}

	public void addSymbolToLine(int linePos, GameObject symbol_object){
		if (symbols_line [linePos].reel_symbol == null) {
			symbols_line [linePos].reel_symbol = new List<GameObject> ();
		}
		symbols_line [linePos].reel_symbol.Add (symbol_object);
	}

	public void destroySymbol(int linePos, int symbolPos){
		symbols_line [linePos].reel_symbol.RemoveAt (symbolPos);
	}
}
