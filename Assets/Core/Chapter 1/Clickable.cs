using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Core.Chapter_1{
	public class Clickable : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler{
		public bool showMore = true;
		public UnityEvent<bool> switchClick;
		public UnityEvent<bool> switchNegativeClick;
		[ShowIf("$showMore")] public UnityEvent pointDown;
		[ShowIf("$showMore")] public UnityEvent pointUp;
		private bool _flagSwitch;

		public void OnPointerClick(PointerEventData eventData){
			switchClick?.Invoke(_flagSwitch);
			switchNegativeClick?.Invoke(!_flagSwitch);
			_flagSwitch = !_flagSwitch;
		}

		public void OnPointerDown(PointerEventData eventData){
			pointDown?.Invoke();
		}

		public void OnPointerUp(PointerEventData eventData){
			pointUp?.Invoke();
		}
	}
}