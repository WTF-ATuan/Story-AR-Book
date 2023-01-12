using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Testing{
	public class InteractSystem : MonoBehaviour{
		[SerializeField] private Button interactButton;
		[SerializeField] private Text debugText;
		
		private void Start(){
			interactButton.OnClickAsObservable().Subscribe(OnClick);
		}

		private void Update(){
			debugText.text = transform.position.ToString();
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