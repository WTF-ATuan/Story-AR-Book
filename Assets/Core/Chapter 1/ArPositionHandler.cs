using System;
using UnityEngine;
using Vuforia;

namespace Core.Chapter_1{
	[RequireComponent(typeof(AreaTargetBehaviour))]
	[RequireComponent(typeof(DefaultAreaTargetEventHandler))]
	public class ArPositionHandler : MonoBehaviour{
		private AreaTargetBehaviour _targetBehaviour;
		private DefaultAreaTargetEventHandler _areaTargetEventHandler;
		private float _startY;

		private void Start(){
			_targetBehaviour = GetComponent<AreaTargetBehaviour>();
			_startY = _targetBehaviour.transform.position.y;
		}

		private void Update(){
			_targetBehaviour.transform.position = new Vector3(_targetBehaviour.transform.position.x, _startY,
				_targetBehaviour.transform.position.z);
		}
	}
}