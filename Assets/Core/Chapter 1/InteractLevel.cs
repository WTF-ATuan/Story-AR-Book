using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Core.Chapter_1{
	public class InteractLevel : MonoBehaviour{
		[TitleGroup("Components")]
		[SerializeField] [Required] private Animator animationComponent;
		[SerializeField] [Required] private AudioSource audioComponent;
		[Inject] private readonly PlayerData _playerData;
		[Inject] private readonly UIPresenter _presenter;
		private readonly List<GameObject> _levelRootList = new List<GameObject>();

		private void Start(){
			_presenter.exitButton.OnPointerClickAsObservable().Subscribe(x => ExitFocusMode());
		}

		public void Interact(InteractAnimationData data){
			if(data.correctAnswer){
				SetAnimation(data.animationClip);
				audioComponent.clip = data.audioClip;
				SetComponentActive(true);
			}
			else{
				CreateOrOpenLevel(data.interactiveRoot);
			}
		}

		private void SetAnimation(AnimationClip clip){
			var overrideController = new AnimatorOverrideController(animationComponent.runtimeAnimatorController);
			var animations = overrideController.animationClips.Select(animationClip =>
					new KeyValuePair<AnimationClip, AnimationClip>(animationClip, clip)).ToList();
			overrideController.ApplyOverrides(animations);
			animationComponent.runtimeAnimatorController = overrideController;
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
			ExitFocusMode();
		}

		private void SetComponentActive(bool active){
			animationComponent.gameObject.SetActive(active);
			audioComponent.gameObject.SetActive(active);
		}

		private void ExitFocusMode(){
			_levelRootList.ForEach(x => x.SetActive(false));
			_presenter.SwitchMode(UIMode.PlayMode);
			SetComponentActive(false);
		}
	}
}