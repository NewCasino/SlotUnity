using UnityEngine;
using System.Collections;
using System;

public class ReelData
{
	public ReelScreen playScreen;
	//public Round gameResult;
	public Cosmos cosmos;

	public struct Cosmos
	{
		public GameResult gameResults;
	}
		
	public struct GameResult
	{
		public ReelScreen startScreen;
		public ReelScreen topScreen;
		public ReelScreen endScreen;
		public Transitions[] transitions;
	}
		
	public struct ReelScreen
	{
		public Reels[] reels;
	}
		
	public struct Reels
	{
		public string[] reel;
	}
		
	public struct Transitions
	{
		public Collapse collapse;
	}

	public struct Collapse
	{
		public Targets[] targets;
	}
		
	public struct Targets
	{
		public Target target;
	}
		
	public struct Target
	{
		public string symbol;
		public Positions[] positions;
	}
		
	public struct Positions
	{
		public int[] p;
	}
		
	public struct TwoIntArray
	{
		public int[] symbol;
	}

	public void Massage(){
		for(int i=0;i<cosmos.gameResults.startScreen.reels.Length;i++){
			if (cosmos.gameResults.startScreen.reels != null) {
				Array.Reverse (cosmos.gameResults.startScreen.reels [i].reel);
			}
			if (cosmos.gameResults.endScreen.reels != null) {
				Array.Reverse (cosmos.gameResults.endScreen.reels [i].reel);
			}
			if (cosmos.gameResults.topScreen.reels != null) {
				//Array.Reverse (cosmos.gameResults.topScreen.reels [i].reel);
			} else {
				Debug.Log ("fuck");
			}
		}

		for (int i = 0; i < cosmos.gameResults.transitions.Length; i++) {
			for (int j = 0; j < cosmos.gameResults.transitions [i].collapse.targets.Length; j++) {
				for (int k = 0; k < cosmos.gameResults.transitions [i].collapse.targets [j].target.positions.Length; k++) {
					//for (int y = 0; y < cosmos.gameResult.transitions [i].collapse.targets [j].target.positions [k].p.Length; y++) {
					if (cosmos.gameResults.transitions [i].collapse.targets [j].target.positions [k].p [1] == 0) {
						cosmos.gameResults.transitions [i].collapse.targets [j].target.positions [k].p [1] = 2;
						} else {
						if (cosmos.gameResults.transitions [i].collapse.targets [j].target.positions [k].p [1] == 2) {
							cosmos.gameResults.transitions [i].collapse.targets [j].target.positions [k].p [1] = 0;
							}
						}
					//}
				}
			}
		}

		playScreen = new ReelScreen ();
		playScreen.reels = new Reels[cosmos.gameResults.startScreen.reels.Length];

		for (int i = 0; i < cosmos.gameResults.startScreen.reels.Length; i++) {
			int topScreenLength = 0;
			if (cosmos.gameResults.topScreen.reels != null) {
				topScreenLength = cosmos.gameResults.topScreen.reels [i].reel.Length;
			}
			playScreen.reels [i].reel = new string[cosmos.gameResults.startScreen.reels [i].reel.Length + topScreenLength];
			for (int j = 0; j < cosmos.gameResults.startScreen.reels [i].reel.Length; j++) {
				playScreen.reels [i].reel [j] = cosmos.gameResults.startScreen.reels [i].reel [j];
			}
			for (int j = 0; j < topScreenLength; j++) {
				playScreen.reels [i].reel [j + cosmos.gameResults.startScreen.reels [i].reel.Length] = cosmos.gameResults.topScreen.reels [i].reel [j];
			}
		}
	}
}
