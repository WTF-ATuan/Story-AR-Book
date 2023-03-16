using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Core.Chapter_1{
	public class Shackable : MonoBehaviour{
		
		[HideLabel] [HorizontalGroup] public int invokeCount = 2;
		[HorizontalGroup] [HideLabel] public UnityEvent afterInvokeCount;

		private int _invokeTimer = 0;
		private float _countTimer;
		private void Update(){
			if(Input.acceleration.magnitude > 2){
				if(_countTimer <= 0.25f){
					return;
				}

				_invokeTimer++;
				if(_invokeTimer >= invokeCount){
					afterInvokeCount?.Invoke();
				}
			}

			_countTimer += Time.deltaTime;
		}
	}
}