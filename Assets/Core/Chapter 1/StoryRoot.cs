﻿using System.Collections.Generic;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace Core.Chapter_1{
	public class StoryRoot : MonoBehaviour{
		[Inject] private readonly UIPresenter _presenter;

		private List<StoryGuideData.StoryData> _currentStoryClone;
		private readonly List<string> _dataRecordList = new List<string>();

		private UIMode _exitMode = UIMode.PlayMode;

		private void Start(){
			_presenter.storyBackGround.OnPointerClickAsObservable().Subscribe(x => NextStory());
		}

		public void Contact(StoryGuideData data, string dataName){
			if(data.interact){
				return;
			}

			ShowStory(data, dataName);
			_exitMode = UIMode.PlayMode;
		}

		public void Interact(StoryGuideData data){
			if(!data.interact){
				return;
			}

			var dataID = data.GetHashCode();
			ShowStory(data, dataID.ToString());
			_exitMode = UIMode.PlayMode;
		}

		public void InteractWithLevel(StoryGuideData data){
			var dataID = data.GetHashCode();
			ShowStory(data, dataID.ToString());
			_exitMode = UIMode.Focus;
		}

		private void ShowStory(StoryGuideData storyData, string objID){
			if(storyData.showOnes && _dataRecordList.Contains(objID)){
				return;
			}

			EventAggregator.Publish(new StoryPresentEvent(objID));
			if(storyData.multiplex){
				var count = _dataRecordList.Count(x => x == objID);
				if(count >= storyData.multiplexStoryContext.Count){
					count = storyData.multiplexStoryContext.Count - 1;
				}

				if(storyData.randomIndex){	
					count = Random.Range(0, storyData.multiplexStoryContext.Count);
				}

				_currentStoryClone = new List<StoryGuideData.StoryData>(storyData.multiplexStoryContext[count].datas);
			}
			else{
				_currentStoryClone = new List<StoryGuideData.StoryData>(storyData.storyContext);
			}

			_presenter.SwitchMode(UIMode.StoryMode);
			_dataRecordList.Add(objID);
			NextStory();
		}

		private void NextStory(){
			if(_currentStoryClone.Count < 1){
				_presenter.SwitchMode(_exitMode);
				return;
			}

			var storyData = _currentStoryClone.First();
			if(storyData.upOrDown){
				_presenter.upStoryText.text = storyData.storyText;
				_presenter.downStoryText.text = string.Empty;
			}
			else{
				_presenter.upStoryText.text = string.Empty;
				_presenter.downStoryText.text = storyData.storyText;
			}

			_currentStoryClone.Remove(storyData);
		}
	}

	public class StoryPresentEvent{
		public string EventID;

		public StoryPresentEvent(string eventID){
			EventID = eventID;
		}
	}
}