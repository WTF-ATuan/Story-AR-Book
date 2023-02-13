using System;
using UnityEngine;

namespace Core.Chapter_1{
	public class Teleport{
		private readonly Transform _controlObject;
		private readonly InteractRepository _interactRepository;


		public Teleport(Transform controlObject, InteractRepository interactRepository){
			_controlObject = controlObject;
			_interactRepository = interactRepository;
		}

		public void Interact(TeleportData data){
			var targetObjID = data.target;
			var target = _interactRepository.interactObject.Find(x => x.name == targetObjID);
			if(!target){
				throw new Exception($"Can,t teleport to {targetObjID} ");
			}
			_controlObject.position = target.transform.position;
		}
	}
}