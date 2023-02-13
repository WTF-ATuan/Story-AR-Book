using System;
using System.Collections.Generic;
using System.Linq;
using Core.Chapter_1;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace Core.Testing{
	public class InteractSystem : MonoBehaviour{
		[Inject] private InteractRepository _interactRepository;
		[Inject] private InteractUI _interactUI;
		[Inject] private readonly InteractDataSet _interactDataSet;
		[Inject] private readonly UIPresenter _presenter;

		private Teleport _teleport;
		private InteractTag _interactTag;
		private InteractData _currentInteractData;
		private readonly List<string> _interactNames = new List<string>();

		private void Start(){
			_presenter.findIcon.OnPointerClickAsObservable().Subscribe(x => Interact());
			_interactRepository.RegisterAll(CompareData);
			_interactRepository.RegisterAll(UpdateInteract);
			_teleport = new Teleport(transform, _interactRepository);
		}


		private void CompareData(string objID, bool enterOrExit){
			var interactData = _interactDataSet.FindData(objID);
			_interactTag = interactData.tag;
			_currentInteractData = interactData;
		}

		private void Interact(){
			switch(_interactTag){
				case InteractTag.InteractAnimation:{
					_interactUI.Interact(_currentInteractData.interactAnimationData);
					break;
				}
				case InteractTag.Teleport:
					_teleport.Interact(_currentInteractData.teleportData);
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

			if(_interactNames.Count > 0){
				_presenter.ModifyNameTag(_interactNames.First());
			}

			_presenter.SetFindState(_interactNames.Count > 0);
		}
	}
}