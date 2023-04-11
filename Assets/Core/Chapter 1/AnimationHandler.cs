using UnityEngine;
using UnityEngine.Events;

namespace Core.Chapter_1{
	public class AnimationHandler : MonoBehaviour{
		public UnityEvent onEvent;
		public UnityEvent onEnding;

		public void AnimationEventTrigger(int id){
			switch(id){
				case 0:
					onEvent?.Invoke();
					break;
				case 1:
					onEnding?.Invoke();
					break;
			}
		}
	}
}