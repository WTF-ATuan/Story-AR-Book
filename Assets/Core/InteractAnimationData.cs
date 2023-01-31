using System;
using UnityEngine;

namespace Core{
	[Serializable]
	public class InteractAnimationData{
		public Sprite image;
		public AnimationClip animationClip;
		public AudioClip audioClip;
		public bool correctAnswer = true;
	}
}