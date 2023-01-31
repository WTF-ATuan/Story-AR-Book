using System;
using UnityEngine;

namespace Core.Chapter_1{
	public class Teleport{
		private readonly Transform _controlObject;
		private Vector3 _teleportTarget;
		private readonly InteractRepository _interactRepository;


		public Teleport(Transform controlObject, InteractRepository interactRepository){
			_controlObject = controlObject;
			_interactRepository = interactRepository;
		}

		public void SetData(TeleportData data){
			var targetObjID = data.target;
			var target = _interactRepository.interactObject.Find(x => x.name == targetObjID);
			if(!target){
				throw new Exception($"Can,t teleport to {targetObjID} ");
			}

			_teleportTarget = target.transform.position;
		}

		public void Interact(){
			_controlObject.position = _teleportTarget;
		}
	}
}