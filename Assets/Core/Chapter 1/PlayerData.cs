using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Chapter_1{
	public class PlayerData{
		private readonly List<string> _successIDFinder = new List<string>();
		public Action ResultSaved;

		public void SaveSuccessResult(string currentObjID){
			if(_successIDFinder.Contains(currentObjID)) return;
			_successIDFinder.Add(currentObjID);
			ResultSaved?.Invoke();
		}

		public List<string> GetSuccessID(){
			return _successIDFinder;
		}
	}
}