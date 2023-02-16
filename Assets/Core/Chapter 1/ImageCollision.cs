using Sirenix.OdinInspector;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Core.Chapter_1{
	public class ImageCollision : MonoBehaviour{
		public Image trackImage;
		public UnityEvent onDrop;

		private void Start(){
			trackImage.OnEndDragAsObservable().Subscribe(OnTargetEndDrag);
		}

		private void OnTargetEndDrag(PointerEventData obj){
			var image = GetComponent<RectTransform>();
			var rectSize = image.rect.size;
			Debug.Log($"{rectSize}");
			Debug.Log($"{obj.pointerCurrentRaycast.gameObject}");
		}
		[Button]
		public void Test(){
			GetComponent<Image>().color = Color.red;
			var image = GetComponent<RectTransform>();
			var rectSize = image.rect.size;
			Debug.Log($"{rectSize}");
		}
	}
}