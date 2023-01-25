using System.Collections.Generic;
using System.Linq;
using Core.Chapter_1;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Core.Testing{
	public class InteractSystem : MonoBehaviour{
		[SerializeField] private Button interactButton;
		[SerializeField] private Text debugText;

		[SerializeField] [ReadOnly] private List<string> interactNames = new List<string>();

		public InteractRepository interactRepository;
		[SerializeField] private InteractUI interactUI;


		private void Start(){
			interactRepository = FindObjectOfType<InteractRepository>();
			interactUI = FindObjectOfType<InteractUI>(true);
			interactButton.OnClickAsObservable().Subscribe(x => OnClick());
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
			if(interactNames.Count > 0)
				debugText.text = interactNames.First();
		}

		private void OnClick(){
			ShowInteractUI();
			interactButton.gameObject.SetActive(false);
		}

		private void ShowInteractUI(){
			var currentInteract = interactNames.First();
			interactUI.ChangeUIData(currentInteract);
		}
	}
}