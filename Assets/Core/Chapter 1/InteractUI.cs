using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Core.Chapter_1{
	public class InteractUI : MonoBehaviour{
		[TitleGroup("Data Holder")] [ReadOnly] [SerializeField]
		private Sprite image;

		[ReadOnly] [SerializeField] private AnimationClip animationClip;
		[ReadOnly] [SerializeField] private RectTransform buttonTransform;
		[ReadOnly] [SerializeField] private AudioClip audioClip;

		[TitleGroup("Components")] [SerializeField] [Required]
		private Image imageComponent;

		[SerializeField] [Required] private Animator animationComponent;
		[SerializeField] [Required] private Button buttonComponent;
		[SerializeField] [Required] private AudioSource audioComponent;


		private AnimatorOverrideController _overrideController;

		[Inject] private readonly InteractDataSet _interactDataSet;
		private string _currentObjID;

		private void Start(){
			image = imageComponent.sprite;
			animationClip = animationComponent.runtimeAnimatorController.animationClips.First();
			buttonTransform = buttonComponent.GetComponent<RectTransform>();
			audioClip = audioComponent.clip;
			_overrideController = new AnimatorOverrideController(animationComponent.runtimeAnimatorController);
			buttonComponent.OnClickAsObservable().Subscribe(x => OnInteractButtonClick());
		}

		[Button]
		public void ChangeUIData(string objID){
			var interactData = _interactDataSet.FindData(objID);
			image = interactData.image;
			animationClip = interactData.animationClip;
			buttonTransform = interactData.buttonTransform;
			audioClip = interactData.audioClip;
			_currentObjID = objID;
			SetDataToComponent();
			SetComponentActive(true);
		}

		private void SetComponentActive(bool active){
			imageComponent.gameObject.SetActive(active);
			animationComponent.gameObject.SetActive(active);
			buttonComponent.gameObject.SetActive(active);
			audioComponent.gameObject.SetActive(active);
		}

		private void SetDataToComponent(){
			imageComponent.sprite = image;
			var anims = _overrideController.animationClips
					.Select(a => new KeyValuePair<AnimationClip, AnimationClip>(a, animationClip)).ToList();
			_overrideController.ApplyOverrides(anims);
			animationComponent.runtimeAnimatorController = _overrideController;
			//Button .....
			audioComponent.clip = audioClip;
		}

		private void OnInteractButtonClick(){
			if(_interactDataSet.CheckCorrect(_currentObjID)){
				Debug.Log($"找到錯誤音效! 給予一個 '通關條件' ");
			}
			else{
				Debug.Log($"這是正確的音效");
			}

			SetComponentActive(false);
		}
	}
}