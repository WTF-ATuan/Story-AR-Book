using System;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Chapter_1{
	public class UIPresenter : MonoBehaviour{
		[TitleGroup("Play Mode")] public Image findIcon;
		[TitleGroup("Play Mode")] public Image nameTagView;
		[TitleGroup("Play Mode")] public Text nameTag;
		[TitleGroup("Play Mode")] public Image setting;
		[TitleGroup("Play Mode")] public List<Image> completePartList;
		[TitleGroup("Interact Focus")] public Image interactBackGround;

		[Button]
		public void SwitchMode(UIMode mode){
			switch(mode){
				case UIMode.PlayMode:
					findIcon.gameObject.SetActive(true);
					nameTagView.gameObject.SetActive(true);
					interactBackGround.gameObject.SetActive(false);
					setting.transform.DOMoveX(-150, 0.5f);
					break;
				case UIMode.Focus:
					findIcon.gameObject.SetActive(false);
					nameTagView.gameObject.SetActive(false);
					interactBackGround.gameObject.SetActive(true);
					setting.transform.DOMoveX(1, 0.5f);
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
			}
		}

		public void ModifyNameTag(string message){
			nameTag.text = message;
		}

		public void SetFindState(bool active){
			var activeObj = findIcon.transform.GetChild(0);
			activeObj.gameObject.SetActive(active);
		}
	}

	public enum UIMode{
		PlayMode,
		Focus
	}
}