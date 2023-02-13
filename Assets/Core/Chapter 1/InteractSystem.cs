using System;
using System.Collections.Generic;
using System.Linq;
using Core.Chapter_1;
using Sirenix.OdinInspector;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Core.Testing{
	public class InteractSystem : MonoBehaviour{
		[SerializeField] private Image interactButton;
		[Inject] public InteractRepository interactRepository;
		[Inject] private InteractUI _interactUI;
		[Inject] private readonly InteractDataSet _interactDataSet;
		private Teleport _teleport;
		private InteractTag _interactTag;
		
		private readonly List<string> _interactNames = new List<string>();
		[SerializeField] private Text nameTag;

		private void Start(){
			interactButton.OnPointerClickAsObservable().Subscribe(x => Interact());
			interactRepository.RegisterAll(CompareData);
			interactRepository.RegisterAll(UpdateInteract);
			_teleport = new Teleport(transform, interactRepository);
		}


		private void CompareData(string objID, bool enterOrExit){
			var interactData = _interactDataSet.FindData(objID);
			_interactTag = interactData.tag;
			switch(interactData.tag){
				case InteractTag.InteractAnimation:{
					_interactUI.SetData(interactData);
					break;
				}
				case InteractTag.Teleport:{
					_teleport.SetData(interactData.teleportData);
					break;
				}
			}
		}

		private void Interact(){
			switch(_interactTag){
				case InteractTag.InteractAnimation:{
					_interactUI.Interact();
					break;
				}
				case InteractTag.Teleport:
					_teleport.Interact();
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private void UpdateInteract(string objID, bool enterOrExit){
			if(enterOrExit){
				_interactNames.Insert(0, objID);
			}
			else{
				if(_interactNames.Contains(objID)){
					_interactNames.Remove(objID);
				}
			}

			var activeObj = interactButton.transform.GetChild(0).gameObject;
			if(_interactNames.Count > 0){
				nameTag.text = _interactNames.First();
				activeObj.SetActive(true);
			}
			else{
				activeObj.SetActive(false);
			}
		}
	}
}