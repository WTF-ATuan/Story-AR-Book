using Sirenix.OdinInspector;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace Core.Chapter_1{
	public class CompletePresenter : MonoBehaviour{
		[FoldoutGroup("Cheat")] public Image settingButton;
		[FoldoutGroup("Cheat")] public RectTransform cheatButton;
		[FoldoutGroup("Cheat")] public Image stageOne;
		[FoldoutGroup("Cheat")] public Image stageTwo;

		[Inject] private PlayerData _playerData;

		private void Start(){
			settingButton.OnPointerClickAsObservable().Subscribe(x => {
				cheatButton.gameObject.SetActive(!cheatButton.gameObject.activeSelf);
			});
			stageOne.OnPointerClickAsObservable().Subscribe(CompleteStageOne);
			stageTwo.OnPointerClickAsObservable().Subscribe(CompleteStageTwo);
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
	}
}