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
			Register();
		}

		private void Register(){
			interactButton.OnClickAsObservable().Subscribe(x => Interact());
			interactRepository.RegisterWithTag("Interact", InteractCondition);
			interactRepository.RegisterWithName("Stairs", true, (obj, id) => TeleportCondition("Stairs Target"));
			interactRepository.RegisterWithName("Stairs Target", true, (obj, id) => TeleportCondition("Stairs"));
			interactRepository.RegisterWithName("Target", TargetCondition);
		}

		private void InteractCondition(string objID, bool exitOrEnter){
			if(exitOrEnter){
				interactNames.Insert(0, objID);
			}
			else{
				if(interactNames.Contains(objID)){
					interactNames.Remove(objID);
				}
			}

			UpdateUIElement();
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


		private void TargetCondition(string objID, bool enterOrExit){
			interactButton.gameObject.SetActive(true);
			_interactState = InteractState.Condition;
		}

		private void Interact(){
			switch(_interactState){
				case InteractState.Interact:{
					var currentInteract = interactNames.First();
					interactUI.ChangeUIData(currentInteract);
					break;
				}
				case InteractState.Condition:
					_successCount = _playerData.GetSuccessCount();
					Debug.Log(_successCount > _targetData.PassCount ? "Pass" : "Not Pass");
					break;
				case InteractState.Teleport:
					transform.position = _teleportTarget;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}


			interactButton.gameObject.SetActive(false);
		}

		private void UpdateUIElement(){
			interactButton.gameObject.SetActive(interactNames.Count > 0);
			_interactState = InteractState.Interact;
			if(interactNames.Count > 0)
				debugText.text = interactNames.First();
		}
	}

	public enum InteractState{
		Interact,
		Condition,
		Teleport,
	}
}