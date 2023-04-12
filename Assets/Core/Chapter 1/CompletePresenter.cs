﻿using Sirenix.OdinInspector;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using Vuforia;
using Zenject;
using Image = UnityEngine.UI.Image;

namespace Core.Chapter_1{
	public class CompletePresenter : MonoBehaviour{
		[FoldoutGroup("Cheat")] public Image settingButton;
		[FoldoutGroup("Cheat")] public RectTransform cheatButton;
		[FoldoutGroup("Cheat")] public Image stageOne;
		[FoldoutGroup("Cheat")] public Image stageTwo;
		[FoldoutGroup("Cheat")] public Image resetAR;

		[Inject] private PlayerData _playerData;

		private void Start(){
			settingButton.OnPointerClickAsObservable().Subscribe(x => {
				cheatButton.gameObject.SetActive(!cheatButton.gameObject.activeSelf);
			});
			stageOne.OnPointerClickAsObservable().Subscribe(CompleteStageOne);
			stageTwo.OnPointerClickAsObservable().Subscribe(CompleteStageTwo);
			resetAR.OnPointerClickAsObservable().Subscribe(ResetAreaTarget);
		}

		private void CompleteStageOne(PointerEventData obj){
			_playerData.SaveSuccessResult("Radio");
			_playerData.SaveSuccessResult("Wardrobe");
			_playerData.SaveSuccessResult("News");
			_playerData.SaveSuccessResult("Lamp");
			_playerData.SaveSuccessResult("Fan");
			cheatButton.gameObject.SetActive(false);
		}

		private void CompleteStageTwo(PointerEventData obj){
			_playerData.SaveSuccessResult("Backpack (Puzzle)");
			_playerData.SaveSuccessResult("Clock (Puzzle)");
			_playerData.SaveSuccessResult("Famliy (Puzzle)");
			_playerData.SaveSuccessResult("Note (Puzzle)");
			_playerData.SaveSuccessResult("Park (Puzzle)");
			_playerData.SaveSuccessResult("ToyBear (Puzzle)");
			_playerData.SaveSuccessResult("Uniform (Puzzle)");
			cheatButton.gameObject.SetActive(false);
		}

		private void ResetAreaTarget(PointerEventData obj){
			var areaTargetBehaviour = FindObjectOfType<AreaTargetBehaviour>();
			areaTargetBehaviour.gameObject.SetActive(false);
			areaTargetBehaviour.gameObject.SetActive(true);
			cheatButton.gameObject.SetActive(false);
		}
	}
}