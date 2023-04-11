using System;
using System.Collections.Generic;
using System.Linq;
using Core.Chapter_1;
using UniRx;
using UnityEngine;
using Zenject;

namespace Core.Testing{
	public class InteractSystem : MonoBehaviour{
		[Inject] private InteractRepository _interactRepository;
		[Inject] private InteractLevel _interactLevel;
		[Inject] private readonly StoryRoot _storyRoot;
		[Inject] private readonly InteractDataSet _interactDataSet;
		[Inject] private readonly UIPresenter _presenter;
		[Inject] private readonly OutlineRepository _outline;

		private Teleport _teleport;
		private InteractTag _interactTag;
		private InteractData _currentInteractData;
		private readonly List<string> _interactNames = new List<string>();

		private void Start(){
			_presenter.SubscribeFindIcon(Interact);
			_interactRepository.RegisterAll(UpdateInteract);
			EventAggregator.OnEvent<StoryEvent>()
					.Subscribe(x => {
						var interactData = _interactDataSet.FindData(x.EventID);
						CompareData(interactData);
					});
			_teleport = new Teleport(transform, _interactRepository);
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
				var data = _interactDataSet.FindData(_interactNames.First());
				CompareData(data);
				_presenter.ModifyNameTag(data.showsName);
				_outline.ActiveOutline(data.name);
			}
			else{
				_presenter.ModifyNameTag("無", false);
				_outline.DisableAllOutline();
			}

			_presenter.SetFindState(_interactNames.Count > 0);
		}

		private void CompareData(InteractData data){
			if(data.tag == InteractTag.StoryGuide
			   || data.storyGuide && data.onContact){
				_storyRoot.Contact(data.storyGuideData, data.name);
			}

			_interactTag = data.tag;
			_currentInteractData = data;
		}

		private void Interact(){
			switch(_interactTag){
				case InteractTag.InteractAnimation:{
					_interactLevel.Interact(_currentInteractData.interactAnimationData);
					if(!_currentInteractData.storyGuide) return;

					if(_currentInteractData.onFinish){
						_interactLevel.OnCurrentLevelPass += () => {
							_storyRoot.InteractWithLevel(_currentInteractData.storyGuideData);
							EventAggregator.Publish(new StoryPresentEvent(_currentInteractData.name));
						};
					}
					else{
						if(_currentInteractData.onContact) return;
						_storyRoot.InteractWithLevel(_currentInteractData.storyGuideData);
					}

					break;
				}
				case InteractTag.Teleport:
					_teleport.Interact(_currentInteractData.teleportData);
					break;
				case InteractTag.StoryGuide:
					_storyRoot.Interact(_currentInteractData.storyGuideData);
					break;

				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
}