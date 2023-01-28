using System;
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
		private Vector3 _teleportTarget;

		private void Start(){
			interactRepository = FindObjectOfType<InteractRepository>();
			interactUI = FindObjectOfType<InteractUI>(true);
			interactButton.OnClickAsObservable().Subscribe(x => Interact());
			//----
			interactRepository.RegisterWithTag("Interact", true, TriggerEnter);
			interactRepository.RegisterWithTag("Interact", false, TriggerExit);
			interactRepository.RegisterWithName("Stairs", true, (obj, id) => TeleportCondition("Stairs Target"));
			interactRepository.RegisterWithName("Stairs Target", true, (obj, id) => TeleportCondition("Stairs"));
			interactRepository.RegisterWithName("Target", TargetCondition);
		}

		private void TeleportCondition(string targetID){
			var target = interactRepository.interactObject.Find(x => x.name == targetID);
			if(!target){
				throw new Exception($"Can,t teleport to {targetID} ");
			}

			interactButton.gameObject.SetActive(true);
			_teleportTarget = target.transform.position;
			_interactState = InteractState.Teleport;
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

		private void TargetCondition(string objID, bool enterOrExit){
			interactButton.gameObject.SetActive(true);
			_interactState = InteractState.Condition;
		}

		private void Interact(){
			if(_interactState == InteractState.Interact){
				ShowInteractUI();
			}

			if(_interactState == InteractState.Condition){
				_successCount = _playerData.GetSuccessCount();
				Debug.Log(_successCount > _targetData.PassCount ? "Pass" : "Not Pass");
			}

			if(_interactState == InteractState.Teleport){
				transform.position = _teleportTarget;
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
		Condition,
		Teleport,
	}
}