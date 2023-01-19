using UnityEngine;
using Zenject;

namespace Core.Chapter_One{
	public class InteractEventHandler : MonoBehaviour{
		// Handle All Tag Of Interact
		[Inject] private InteractDataSet _interactDataSet;

		public void HandleEventTag(InteractTag eventTag){
			
		}
	}
}