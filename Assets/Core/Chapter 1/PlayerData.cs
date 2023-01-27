using System.Collections.Generic;
using UnityEngine;

namespace Core.Chapter_1{
	public class PlayerData{
		private int _successCount;
		private readonly List<string> _successIDFinder = new List<string>();

		public void SaveSuccessResult(string currentObjID){
			if(_successIDFinder.Contains(currentObjID)) return;
			_successIDFinder.Add(currentObjID);
			_successCount += 1;
			Debug.Log($"_successCount = {_successCount}");
		}

		public int GetSuccessCount(){
			return _successCount;
		}
	}
}