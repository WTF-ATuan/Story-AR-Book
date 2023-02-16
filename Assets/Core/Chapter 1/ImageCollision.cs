using Sirenix.OdinInspector;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Core.Chapter_1{
	public class ImageCollision : MonoBehaviour{
		[Required] public Image trackImage;
		public float distance = 150;
		public UnityEvent<bool> onDrop;

		private void Start(){
			trackImage.OnEndDragAsObservable().Subscribe(OnTargetEndDrag);
		}

		private void OnTargetEndDrag(PointerEventData obj){
			var rect = GetComponent<RectTransform>();
			var targetPosition = obj.pointerDrag.GetComponent<RectTransform>().position;
			onDrop?.Invoke(Vector3.Distance(targetPosition, rect.position) < distance);
		}

		[Button]
		public void Test(){
			GetComponent<Image>().color = Color.red;
		}
	}
}