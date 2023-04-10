using UniRx;
using UnityEngine;
using UnityEngine.Events;

namespace Core.Chapter_1{
	public class StoryPresentHandler : MonoBehaviour{
		public string storyID;
		public UnityEvent onPresent;

		private void Start(){
			EventAggregator.OnEvent<StoryPresentEvent>().Subscribe(Present);
		}

		private void Present(StoryPresentEvent obj){
			if(obj.EventID.Equals(storyID)){
				onPresent?.Invoke();
			}
		}
	}
}