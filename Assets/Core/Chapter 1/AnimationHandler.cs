using UnityEngine;
using UnityEngine.Events;

namespace Core.Chapter_1{
	public class AnimationHandler : MonoBehaviour{
		public UnityEvent onEvent;

		public void AnimationEventTrigger(){
			onEvent?.Invoke();
		}
	}
}