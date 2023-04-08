using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Core.Chapter_1{
	public class PuzzleChecker : MonoBehaviour{
		[Inject] private readonly PlayerData _playerData;
		[SerializeField] private GameObject[] activePart;

		private void OnEnable(){
			OnPuzzlePass(_playerData.GetSuccessID());
		}

		private void OnPuzzlePass(List<string> passIDList){
			foreach(var part in from passID in passIDList
					from part in activePart
					where part.name == passID
					select part){
				part.SetActive(true);
			}
		}
	}
}