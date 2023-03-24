using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Core.Chapter_1{
	public class GoalChecker : MonoBehaviour{
		[Inject] private readonly PlayerData _playerData;
		public UnityEvent<bool> taskCompleted;

		private void OnEnable(){
			var successID = _playerData.GetSuccessID();
			taskCompleted?.Invoke(successID.Count >= 5);
		}
	}
}