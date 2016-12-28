using UnityEngine;
using System.Collections;
using System;

[System.Serializable]
public class Reel
{
	public Result result;

	[System.Serializable]
	public struct Result
	{
		public string gameType;
		public GameResult gameResult;
	}

	[System.Serializable]
	public struct GameResult
	{
		public FullScreen start;
		public Transitions[] transitions;
		public FullScreen end;
	}

	[System.Serializable]
	public struct FullScreen
	{
		public Reels[] reels;
	}

	[System.Serializable]
	public struct Reels
	{
		public string[] reel;
	}

	[System.Serializable]
	public struct Transitions
	{
		public Transition transition;
	}

	[System.Serializable]
	public struct Transition
	{
		public Collapse collapse;
		//public Drop drop;
	}

	[System.Serializable]
	public struct Collapse
	{
		public CollapseReels[] reels;
	}

	[System.Serializable]
	public struct Drop
	{
		public Reels[] reels;
	}

	[System.Serializable]
	public struct CollapseReels
	{
		public int[] reel;
	}

	[System.Serializable]
	public struct TwoIntArray
	{
		public int[] symbol;
	}

	public static Reel CreateFromJSON(string jsonString)
	{
		return JsonUtility.FromJson<Reel>(jsonString);
	}

	public void Massage(){
		for(int i=0;i<result.gameResult.start.reels.Length;i++){
			Array.Reverse (result.gameResult.start.reels [i].reel);
			Array.Reverse (result.gameResult.end.reels [i].reel);
		}

		for (int i = 0; i < result.gameResult.transitions.Length; i++) {
			for(int j=0;j<result.gameResult.transitions[i].transition.collapse.reels.Length;j++){
				for(int k=0;k<result.gameResult.transitions[i].transition.collapse.reels[j].reel.Length;k++){
					if (result.gameResult.transitions [i].transition.collapse.reels [j].reel [k] == 1) {
						result.gameResult.transitions [i].transition.collapse.reels [j].reel [k] = 3;
					} else {
						if (result.gameResult.transitions [i].transition.collapse.reels [j].reel [k] == 3) {
							result.gameResult.transitions [i].transition.collapse.reels [j].reel [k] = 1;
						}
					}
				}
			}
		}
	}
}
