using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

namespace Core.Chapter_1{
	[RequireComponent(typeof(VideoPlayer))]
	public class VideoEventHandler : MonoBehaviour{
		[SerializeField] private UnityEvent finishVideo;
		private VideoPlayer _videoPlayer;

		private void Start(){
			_videoPlayer = GetComponent<VideoPlayer>();
			_videoPlayer.loopPointReached += OnVideoFinish;
		}

		private void OnVideoFinish(VideoPlayer source){
			finishVideo?.Invoke();
		}
	}
}