using System;
using UnityEngine.Events;

namespace Core.Chapter_1.Interactive.Puzzle{
	public interface IPuzzle{
		Action<PuzzleData> StateChanged{ get; set; }
	}
	public class PuzzleData{
		public readonly IPuzzle Puzzle;
		public bool Complete;

		public PuzzleData(IPuzzle puzzle, bool complete){
			Puzzle = puzzle;
			Complete = complete;
		}
	}
}