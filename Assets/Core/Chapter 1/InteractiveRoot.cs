using Sirenix.OdinInspector;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Core.Chapter_1{
	public class InteractiveRoot : MonoBehaviour{
		[Required] [ChildGameObjectsOnly] public Image target;
		public UnityEvent targetFound;

		private void Start(){
			target.OnPointerClickAsObservable().Subscribe(OnTargetFound);
		}

		private void OnTargetFound(PointerEventData obj){
			targetFound?.Invoke();
			target.gameObject.SetActive(false);
		}
	}
}