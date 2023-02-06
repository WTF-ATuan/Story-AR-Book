using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Core.Chapter_1{
	public class Clickable : MonoBehaviour, IPointerClickHandler{
		public UnityEvent onClick;
		public UnityEvent<bool> switchClick;
		private bool _flagSwitch;
		public void OnPointerClick(PointerEventData eventData){
			onClick?.Invoke();
			switchClick?.Invoke(_flagSwitch);
			_flagSwitch = !_flagSwitch;
		}
	}
}