using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Chapter_1{
	public class UIPresenter : MonoBehaviour{
		[TabGroup("Play Mode")] public Image findIcon;
		[TabGroup("Play Mode")] public Image nameTag;
		[TabGroup("Play Mode")] public Image setting;
		[TabGroup("Interact Focus")] public Image interactBackGround;
		[TabGroup("Interact Focus")] public GameObject interactController;

		[Button]
		public void SwitchMode(UIMode mode){
			switch(mode){
				case UIMode.PlayMode:
					findIcon.gameObject.SetActive(true);
					nameTag.gameObject.SetActive(true);
					interactBackGround.gameObject.SetActive(false);
					setting.transform.DOMoveX(-150, 0.5f);
					break;
				case UIMode.Focus:
					findIcon.gameObject.SetActive(false);
					nameTag.gameObject.SetActive(false);
					interactBackGround.gameObject.SetActive(true);
					setting.transform.DOMoveX(1, 0.5f);
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
			}
		}

	}

	public enum UIMode{
		PlayMode,
		Focus
	}
}