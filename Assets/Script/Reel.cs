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
		public GameResult gameResult;
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
		for(int i=0;i<cosmos.gameResult.startScreen.reels.Length;i++){
			Array.Reverse (cosmos.gameResult.startScreen.reels [i].reel);
			Array.Reverse (cosmos.gameResult.endScreen.reels [i].reel);
			Array.Reverse (cosmos.gameResult.topScreen.reels [i].reel);
		}

		for (int i = 0; i < cosmos.gameResult.transitions.Length; i++) {
			for (int j = 0; j < cosmos.gameResult.transitions [i].collapse.targets.Length; j++) {
				for (int k = 0; k < cosmos.gameResult.transitions [i].collapse.targets [j].target.positions.Length; k++) {
					for (int y = 0; y < cosmos.gameResult.transitions [i].collapse.targets [j].target.positions [k].p.Length; y++) {
						if (cosmos.gameResult.transitions [i].collapse.targets [j].target.positions [k].p [y] == 0) {
							cosmos.gameResult.transitions [i].collapse.targets [j].target.positions [k].p [y] = 2;
						} else {
							if (cosmos.gameResult.transitions [i].collapse.targets [j].target.positions [k].p [y] == 2) {
								cosmos.gameResult.transitions [i].collapse.targets [j].target.positions [k].p [y] = 0;
							}
						}
					}
				}
			}
		}

		playScreen = new ReelScreen ();
		playScreen.reels = new Reels[cosmos.gameResult.startScreen.reels.Length];
		for (int i = 0; i < cosmos.gameResult.startScreen.reels.Length; i++) {
			playScreen.reels [i].reel = new string[cosmos.gameResult.startScreen.reels [i].reel.Length + cosmos.gameResult.topScreen.reels [i].reel.Length];
			for (int j = 0; j < cosmos.gameResult.startScreen.reels [i].reel.Length; j++) {
				playScreen.reels [i].reel [j] = cosmos.gameResult.startScreen.reels [i].reel [j];
			}
			for (int j = 0; j < cosmos.gameResult.topScreen.reels [i].reel.Length; j++) {
				playScreen.reels [i].reel [j + cosmos.gameResult.startScreen.reels [i].reel.Length] = cosmos.gameResult.topScreen.reels [i].reel [j];
			}
		}
	}
}
