using System;
using Core.Chapter_1;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Core{
	[Serializable]
	public class InteractAnimationData{
		public bool correctAnswer = true;
		public Sprite image;
		public AnimationClip animationClip;
		public AudioClip audioClip;
		[HideIf("$correctAnswer")] public InteractiveRoot interactiveRoot;
	}
}