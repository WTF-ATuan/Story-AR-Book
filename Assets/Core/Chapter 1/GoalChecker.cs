using System;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Core.Chapter_1{
	public class GoalChecker : MonoBehaviour{
		[Inject] private readonly PlayerData _playerData;
		[Inject] private readonly UIPresenter _presenter;

		private void Start(){
			_playerData.ResultSaved += UpdateGoalUI;
		}

		private void UpdateGoalUI(){
			var successIDList = _playerData.GetSuccessID();
			foreach(var image in successIDList.SelectMany(id =>
							_presenter.completePartList.Where(image => id.Contains(image.name)))){
				image.gameObject.SetActive(true);
			}
		}
	}
}