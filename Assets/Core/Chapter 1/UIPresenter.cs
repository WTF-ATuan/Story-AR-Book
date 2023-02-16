﻿using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Sirenix.OdinInspector;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Chapter_1{
	public class UIPresenter : MonoBehaviour{
		[FoldoutGroup("Play Mode")] public Image findIcon;
		[FoldoutGroup("Play Mode")] public Image nameTagView;
		[FoldoutGroup("Play Mode")] public Text nameTag;
		[FoldoutGroup("Play Mode")] public Image setting;
		[FoldoutGroup("Interact Focus")] public Image interactBackGround;
		[FoldoutGroup("Interact Focus")] public Image exitButton;
		[FoldoutGroup("Interact Focus")] public Animator levelAnimator;
		[FoldoutGroup("Interact Focus")] public AudioSource levelAudio;
		[FoldoutGroup("Goal")] public List<Image> completePartList;

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

		public void SubscribeFindIcon(Action callback){
			var children = findIcon.GetComponentInChildren<Image>();
			children.OnPointerClickAsObservable().Subscribe(x => callback());
		}

		public void ModifyNameTag(string message, bool showNameTag = true){
			nameTag.text = message;
			nameTagView.gameObject.SetActive(showNameTag);
		}

		public void SetFindState(bool active){
			var activeObj = findIcon.transform.GetChild(0);
			activeObj.gameObject.SetActive(active);
		}

		public void SetAnimation(AnimationClip clip){
			var overrideController = new AnimatorOverrideController(levelAnimator.runtimeAnimatorController);
			var animations = overrideController.animationClips.Select(animationClip =>
					new KeyValuePair<AnimationClip, AnimationClip>(animationClip, clip)).ToList();
			overrideController.ApplyOverrides(animations);
			levelAnimator.runtimeAnimatorController = overrideController;
		}

		public void SetAudio(AudioClip clip){
			levelAudio.clip = clip;
		}

		public void SetLevelActive(bool active){
			levelAnimator.gameObject.SetActive(active);
			levelAudio.gameObject.SetActive(active);
		}
	}

	public enum UIMode{
		PlayMode,
		Focus
	}
}