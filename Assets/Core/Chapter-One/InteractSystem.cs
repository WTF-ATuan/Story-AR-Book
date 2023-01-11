using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Testing{
	public class InteractSystem : MonoBehaviour{
		[SerializeField] private Button interactButton;

		private void Start(){
			interactButton.OnClickAsObservable().Subscribe(OnClick);
		}

		private void OnClick(Unit obj){
			
		}

		private void OnTriggerEnter(Collider other){
			if(!other.CompareTag("Interact")){
				return;
			}

			interactButton.gameObject.SetActive(true);
		}

		private void OnTriggerExit(Collider other){
			if(!other.CompareTag("Interact")){
				return;
			}

			interactButton.gameObject.SetActive(false);
		}
	}
}