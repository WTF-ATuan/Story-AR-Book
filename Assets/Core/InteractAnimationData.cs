using System;
using Core.Chapter_1;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Core{
	[Serializable]
	public class InteractAnimationData{
		[OnValueChanged("ClearField")] public bool correctAnswer = true;
		[ShowIf("$correctAnswer")] public AnimationClip animationClip;
		[ShowIf("$correctAnswer")] public AudioClip audioClip;
		[HideIf("$correctAnswer")] public InteractiveRoot interactiveRoot;

		private void ClearField(){
			if(correctAnswer){
				interactiveRoot = null;
			}
			else{
				animationClip = null;
				audioClip = null;
			}
		}
	}
}