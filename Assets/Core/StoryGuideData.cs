﻿using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Core{
	[Serializable]
	public class StoryGuideData{
		public bool interact;
		public bool showOnes;
		public bool multiplex;

		[ListDrawerSettings(NumberOfItemsPerPage = 5, ElementColor = "GetColor")]
		[InfoBox("if Enable Multiplex, the data will added to multiplexStoryContext", VisibleIf = "@multiplex")]
		public List<StoryData> storyContext;

		[ShowIf("@multiplex")] public List<MultipleStoryData> multiplexStoryContext;
		[ShowIf("@multiplex")] public bool randomIndex;

		private Color GetColor(){
			return Color.gray;
		}


		[Button("Add Story"), ShowIf("@multiplex")]
		private void AddStory(){
			multiplexStoryContext ??= new List<MultipleStoryData>();
			var storyDatas = storyContext.Select(storyData => new StoryData
					{ storyText = storyData.storyText, upOrDown = storyData.upOrDown, }).ToList();
			multiplexStoryContext.Add(new MultipleStoryData(storyDatas));
		}
		
		[Serializable]
		public class StoryData{
			[ValueDropdown("GetUpDown")] public bool upOrDown;
			[TextArea] public string storyText;

			private List<ValueDropdownItem> GetUpDown(){
				return new List<ValueDropdownItem>{
					new ValueDropdownItem("Up", true),
					new ValueDropdownItem("Down", false),
				};
			}
		}

		[Serializable]
		public class MultipleStoryData{
			public List<StoryData> datas;

			public MultipleStoryData(List<StoryData> datas){
				this.datas = datas;
			}
		}
	}
}