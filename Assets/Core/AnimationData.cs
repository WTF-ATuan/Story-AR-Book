using System;
using UnityEngine;

namespace Core{
	[Serializable]
	public class AnimationData{
		public Sprite image;
		public AnimationClip animationClip;
		public AudioClip audioClip;
		public bool correctAnswer = true;
	}
}