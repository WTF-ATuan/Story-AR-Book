using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Testing{
	public class InteractSystem : MonoBehaviour{
		[SerializeField] private Button interactButton;
		[SerializeField] private Text debugText;

		[SerializeField] [ReadOnly] private List<string> interactNames = new List<string>();

		public InteractRepository interactRepository;
		[SerializeField] private GameObject paper;


		private void Start(){
			interactRepository = FindObjectOfType<InteractRepository>();
			interactButton.OnClickAsObservable().Subscribe(OnClick);
			interactRepository.RegisterCollisionWithTag("Interact", true, TriggerEnter);
			interactRepository.RegisterCollisionWithTag("Interact", false, TriggerExit);
		}

		private void TriggerEnter(Collider obj, string objID){
			interactNames.Insert(0, objID);
			UpdateUIElement();
		}

		private void TriggerExit(Collider obj, string objID){
			if(interactNames.Contains(objID)){
				interactNames.Remove(objID);
			}

			UpdateUIElement();
		}

		private void UpdateUIElement(){
			interactButton.gameObject.SetActive(interactNames.Count > 0);
			debugText.text = interactNames.First();
		}

		private void OnClick(Unit obj){
			ShowInteractUI();
		}

		private void ShowInteractUI(){
			var currentInteract = interactNames.First();
			if(currentInteract.Equals("Paper")){
				// 報紙的純UI
				paper.SetActive(true);
			}
		}
	}
}