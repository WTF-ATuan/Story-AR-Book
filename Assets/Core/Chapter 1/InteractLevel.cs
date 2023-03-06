using System.Collections.Generic;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace Core.Chapter_1{
	public class InteractLevel : MonoBehaviour{
		[Inject] private readonly PlayerData _playerData;
		[Inject] private readonly UIPresenter _presenter;
		[SerializeField] private List<GameObject> startLoadingList;
		
		private List<GameObject> _levelRootList = new List<GameObject>();

		private void Start(){
			_presenter.exitButton.OnPointerClickAsObservable().Subscribe(x => ExitFocusMode());
			_levelRootList = _levelRootList.Concat(startLoadingList).ToList();
		}

		public void Interact(InteractAnimationData data){
			if(data.correctAnswer){
				_presenter.SetAnimation(data.animationClip);
				_presenter.SetAudio(data.audioClip);
				_presenter.SetLevelActive(true);
			}
			else{
				CreateOrOpenLevel(data.interactiveRoot);
			}
			_presenter.SwitchMode(UIMode.Focus);
		}

		private void CreateOrOpenLevel(InteractiveRoot root){
			var existLevel = _levelRootList.Find(x => x.name == root.name);
			if(existLevel){
				existLevel.SetActive(true);
				return;
			}

			var levelRoot = Instantiate(root, transform);
			levelRoot.name = root.name;
			levelRoot.targetFound.AddListener(() => PassLevel(root.name));
			_levelRootList.Add(levelRoot.gameObject);
		}

		private void PassLevel(string rootName){
			_playerData.SaveSuccessResult(rootName);
		}

		private void ExitFocusMode(){
			_levelRootList.ForEach(x => x.SetActive(false));
			_presenter.SetLevelActive(false);
			_presenter.SwitchMode(UIMode.PlayMode);
		}
	}
}