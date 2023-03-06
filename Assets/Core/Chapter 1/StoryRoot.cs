using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace Core.Chapter_1{
	public class StoryRoot : MonoBehaviour{
		[Inject] private readonly UIPresenter _presenter;

		private List<StoryGuideData.StoryData> _currentStoryClone;
		private readonly List<string> _idList = new List<string>();

		private void Start(){
			_presenter.storyBackGround.OnPointerClickAsObservable().Subscribe(x => NextStory());
		}

		public void ShowStory(StoryGuideData storyData, string objID){
			if(storyData.showOnes && _idList.Contains(objID)){
				return;
			}

			_presenter.SwitchMode(UIMode.StoryMode);
			_currentStoryClone = new List<StoryGuideData.StoryData>(storyData.storyContext);
			_idList.Add(objID);
			NextStory();
		}

		private void NextStory(){
			if(_currentStoryClone.Count < 1){
				_presenter.SwitchMode(UIMode.PlayMode);
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
}