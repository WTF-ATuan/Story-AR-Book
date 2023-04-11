using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Core.Chapter_1{
	public class Slidable : MonoBehaviour, IPointerDownHandler, IDragHandler{
		private Vector2 _pointerDownPos;
		private Vector2 _pointerUpPos;

		[HideLabel] [HorizontalGroup] public int invokeCount = 2;
		[HorizontalGroup] [HideLabel] public UnityEvent afterInvokeCount;

		private float _invokeTimes;
		private bool _isInvoke;

		public void OnPointerDown(PointerEventData eventData){
			_pointerDownPos = eventData.position;
		}

		public void OnDrag(PointerEventData eventData){
			var pointerOffset = eventData.position - _pointerDownPos;
			if(pointerOffset.magnitude < 10){
				return;
			}

			if(pointerOffset.x < 0){
				if(transform.eulerAngles.z < 0){
					_invokeTimes += Time.deltaTime;
				}

				transform.DOLocalRotate(new Vector3(0, 0, 20), 0.25f);
			}
			else{
				if(transform.eulerAngles.z > 0){
					_invokeTimes += Time.deltaTime;
				}

				transform.DOLocalRotate(new Vector3(0, 0, -20), 0.25f);
			}

			if(_invokeTimes > invokeCount && !_isInvoke){
				afterInvokeCount?.Invoke();
				_isInvoke = true;
			}
		}
	}
}