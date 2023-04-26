using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Core.Chapter_1.Interactive.Puzzle{
	public class PuzzleRoot : MonoBehaviour{
		public List<IPuzzle> puzzles;
		private readonly List<PuzzleData> _puzzleCheckList = new List<PuzzleData>();
		public UnityEvent onPassPuzzle;

		private void Start(){
			puzzles = GetComponentsInChildren<IPuzzle>().ToList();
			puzzles.ForEach(x => x.StateChanged += OnPuzzleComplete);
			puzzles.ForEach(x => { _puzzleCheckList.Add(new PuzzleData(x, false)); });
		}

		private void OnPuzzleComplete(PuzzleData data){
			if(!_puzzleCheckList.Exists(x => x.Puzzle.Equals(data.Puzzle))) return;
			var puzzleData = _puzzleCheckList.Find(x => x.Puzzle.Equals(data.Puzzle));
			puzzleData.Complete = data.Complete;
			if(_puzzleCheckList.Exists(x => x.Complete == false)) return;
			onPassPuzzle?.Invoke();
			puzzles.ForEach(x => x.StateChanged -= OnPuzzleComplete);
		}
	}
}