using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Core.Chapter_1{
	public class Slidable : MonoBehaviour, IPointerDownHandler, IPointerUpHandler{
		private Vector2 _pointerDownPos;
		private Vector2 _pointerUpPos;
		public UnityEvent slideLeft;
		public UnityEvent slideRight;

		[HideLabel] [HorizontalGroup] public int invokeCount = 2;
		[HorizontalGroup] [HideLabel] public UnityEvent afterInvokeCount;

		private int _invokeTimes;

		public void OnPointerDown(PointerEventData eventData){
			_pointerDownPos = eventData.position;
		}

		public void OnPointerUp(PointerEventData eventData){
			_pointerUpPos = eventData.position;
			CalculateSlide();
		}

		private void CalculateSlide(){
			var pointerOffset = _pointerUpPos - _pointerDownPos;
			if(pointerOffset.magnitude < 10){
				return;
			}

			if(pointerOffset.x > 0){
				slideRight?.Invoke();
			}
			else{
				slideLeft?.Invoke();
			}

			_invokeTimes++;
			if(_invokeTimes == invokeCount){
				afterInvokeCount.Invoke();
			}
		}
	}
}