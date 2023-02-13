using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Core.Chapter_1{
	public class Clickable : MonoBehaviour, IPointerClickHandler{
		public UnityEvent<bool> switchClick;
		public UnityEvent<bool> switchNegativeClick;
		private bool _flagSwitch;

		public void OnPointerClick(PointerEventData eventData){
			switchClick?.Invoke(_flagSwitch);
			switchNegativeClick?.Invoke(!_flagSwitch);
			_flagSwitch = !_flagSwitch;
		}
	}
}