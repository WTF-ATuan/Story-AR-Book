using Sirenix.OdinInspector;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Core.Chapter_1{
	public class InteractiveRoot : MonoBehaviour{
		public bool invokeFromEvent = false;
		[Required] [ChildGameObjectsOnly] [HideIf("invokeFromEvent")] public Image target;
		public UnityEvent targetFound;

		private void Start(){
			target.OnPointerClickAsObservable().Subscribe(OnTargetFound);
		}

		private void OnTargetFound(PointerEventData obj){
			targetFound?.Invoke();
			target.gameObject.SetActive(false);
		}
		public void PassLevel(){
			targetFound?.Invoke();
			target.gameObject.SetActive(false);
		}
	}
}