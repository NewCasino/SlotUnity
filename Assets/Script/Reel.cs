using UnityEngine;
using System.Collections;

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
		public int[] reel;
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
		public Drop drop;
	}

	[System.Serializable]
	public struct Collapse
	{
		public Reels[] reels;
	}

	[System.Serializable]
	public struct Drop
	{
		public Reels[] reels;
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

}
