using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core.Chapter_1.Interactive.Puzzle{
	public class PuzzleRoot : MonoBehaviour{
		public List<IPuzzle> puzzles;
		[SerializeField] private List<PuzzleData> _puzzleCheckList = new List<PuzzleData>();

		private void Start(){
			puzzles = GetComponentsInChildren<IPuzzle>().ToList();
			puzzles.ForEach(x => x.StateChanged += OnPuzzleComplete);
			puzzles.ForEach(x => { _puzzleCheckList.Add(new PuzzleData(x, false)); });
		}
		private void OnPuzzleComplete(PuzzleData data){
			if(!_puzzleCheckList.Exists(x => x.Puzzle.Equals(data.Puzzle))) return;
			var puzzleData = _puzzleCheckList.Find(x => x.Puzzle.Equals(data.Puzzle));
			puzzleData.Complete = data.Complete;
			if(!_puzzleCheckList.Exists(x => x.Complete == false)){
				Debug.Log($"Pass Puzzle");
			}
		}
	}
}