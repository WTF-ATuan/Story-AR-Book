using System.Collections.Generic;
using Sirenix.OdinInspector;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Core.Chapter_1{
	public class InteractUI : MonoBehaviour{
		[TitleGroup("Components")] [SerializeField] [Required]
		private Image imageComponent;

		[SerializeField] [Required] private Animation animationComponent;
		[SerializeField] [Required] private AudioSource audioComponent;
		[Inject] private readonly PlayerData _playerData;
		[Inject] private readonly UIPresenter _presenter;
		private readonly List<GameObject> _levelRootList = new List<GameObject>();

		private void Start(){
			_presenter.exitButton.OnPointerClickAsObservable().Subscribe(x => ExitFocusMode());
		}

		public void Interact(InteractAnimationData data){
			if(data.correctAnswer){
				imageComponent.sprite = data.image;
				animationComponent.clip = data.animationClip;
				audioComponent.clip = data.audioClip;
				SetComponentActive(true);
			}
			else{
				CreateOrOpenLevel(data.interactiveRoot);
			}
		}

		private void CreateOrOpenLevel(InteractiveRoot root){
			var existLevel = _levelRootList.Find(x => x.name == root.name);
			if(existLevel){
				existLevel.SetActive(true);
				return;
			}

			var levelRoot = Instantiate(root, transform);
			levelRoot.name = root.name;
			levelRoot.targetFound.AddListener(() => OnLevelPass(root.name));
			_levelRootList.Add(levelRoot.gameObject);
		}

		private void OnLevelPass(string rootName){
			Debug.Log("找到錯誤音效! 給予一個 '通關條件'");
			_playerData.SaveSuccessResult(rootName);
			ExitFocusMode();
		}

		private void SetComponentActive(bool active){
			imageComponent.gameObject.SetActive(active);
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