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

		[Inject] private readonly PlayerData _playerData;
		[Inject] private readonly TargetData _targetData;
		private int _successCount;
		private InteractState _interactState;

		private void Start(){
			interactRepository = FindObjectOfType<InteractRepository>();
			interactUI = FindObjectOfType<InteractUI>(true);
			interactButton.OnClickAsObservable().Subscribe(x => OnClick());
			interactRepository.RegisterWithTag("Interact", true, TriggerEnter);
			interactRepository.RegisterWithTag("Interact", false, TriggerExit);
			interactRepository.RegisterWithName("Target", TargetCondition);
		}

		private void TargetCondition(Collider obj, bool enterOrExit){
			interactButton.gameObject.SetActive(true);
			_interactState = InteractState.Condition;
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
			_interactState = InteractState.Interact;
			if(interactNames.Count > 0)
				debugText.text = interactNames.First();
		}

		private void OnClick(){
			if(_interactState == InteractState.Interact){
				ShowInteractUI();
			}

			if(_interactState == InteractState.Condition){
				_successCount = _playerData.GetSuccessCount();
				Debug.Log(_successCount > _targetData.PassCount ? "Pass" : "Not Pass");
			}

			interactButton.gameObject.SetActive(false);
		}

		private void ShowInteractUI(){
			var currentInteract = interactNames.First();
			interactUI.ChangeUIData(currentInteract);
		}
	}

	public enum InteractState{
		Interact,
		Condition
	}
}