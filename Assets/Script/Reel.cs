using UnityEngine;
using System.Collections;
using System;

[System.Serializable]
public class ReelData : ISerializationCallbackReceiver
{
	public ReelScreen playScreen;
	public Round gameResult;

	[System.Serializable]
	public struct GameResult
	{
		//public string gameType;
		public Round round;
	}

	[System.Serializable]
	public struct Round
	{
		public ReelScreen startScreen;
		public ReelScreen topScreen;
		public ReelScreen endScreen;
		public Transitions[] transitions;
	}

	[System.Serializable]
	public struct ReelScreen
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
		public Collapse collapse;
	}

	[System.Serializable]
	public struct Collapse
	{
		public Targets[] targets;
	}

	[System.Serializable]
	public struct Targets
	{
		public Target target;
	}

	[System.Serializable]
	public struct Target
	{
		public string symbol;
		public Positions[] positions;
	}

	[System.Serializable]
	public struct Positions
	{
		public int[] p;
	}

	[System.Serializable]
	public struct TwoIntArray
	{
		public int[] symbol;
	}

	public static ReelData CreateFromJSON(string jsonString)
	{
		return JsonUtility.FromJson<ReelData>(jsonString);
	}

	public void OnBeforeSerialize(){
		
	}

	public void OnAfterDeserialize(){

	}

	public void Massage(){
		for(int i=0;i<gameResult.startScreen.reels.Length;i++){
			Array.Reverse (gameResult.startScreen.reels [i].reel);
			Array.Reverse (gameResult.endScreen.reels [i].reel);
			Array.Reverse (gameResult.topScreen.reels [i].reel);
		}

		for (int i = 0; i < gameResult.transitions.Length; i++) {
			for (int j = 0; j < gameResult.transitions [i].collapse.targets.Length; j++) {
				for (int k = 0; k < gameResult.transitions [i].collapse.targets [j].target.positions.Length; k++) {
					for (int y = 0; y < gameResult.transitions [i].collapse.targets [j].target.positions [k].p.Length; y++) {
						if (gameResult.transitions [i].collapse.targets [j].target.positions [k].p [y] == 0) {
							gameResult.transitions [i].collapse.targets [j].target.positions [k].p [y] = 2;
						} else {
							if (gameResult.transitions [i].collapse.targets [j].target.positions [k].p [y] == 2) {
								gameResult.transitions [i].collapse.targets [j].target.positions [k].p [y] = 0;
							}
						}
					}
				}
			}
		}

		playScreen = new ReelScreen ();
		playScreen.reels = new Reels[gameResult.startScreen.reels.Length];
		for (int i = 0; i < gameResult.startScreen.reels.Length; i++) {
			playScreen.reels [i].reel = new string[gameResult.startScreen.reels [i].reel.Length + gameResult.topScreen.reels [i].reel.Length];
			for (int j = 0; j < gameResult.startScreen.reels [i].reel.Length; j++) {
				playScreen.reels [i].reel [j] = gameResult.startScreen.reels [i].reel [j];
			}
			for (int j = 0; j < gameResult.topScreen.reels [i].reel.Length; j++) {
				playScreen.reels [i].reel [j + gameResult.startScreen.reels [i].reel.Length] = gameResult.topScreen.reels [i].reel [j];
			}
		}
	}
}
