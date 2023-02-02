using UnityEngine;
using UnityEngine.EventSystems;

namespace Core{
	public class Dragable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler{
		public Transform movingObject;
		private bool _isPointerOver = false;

		public void OnPointerEnter(PointerEventData eventData){
			_isPointerOver = true;
		}

		public void OnPointerExit(PointerEventData eventData){
			_isPointerOver = false;
		}

		public void OnDrag(PointerEventData eventData){
			if(!_isPointerOver){
				return;
			}

			if(movingObject){
				movingObject.position = Input.mousePosition;
			}
			else{
				transform.position = Input.mousePosition;
			}
		}
	}
}