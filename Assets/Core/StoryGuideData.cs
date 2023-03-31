using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Core{
	[Serializable]
	public class StoryGuideData{
		public bool interact;
		public bool showOnes;
		public bool multiplex;

		[ListDrawerSettings(NumberOfItemsPerPage = 5, ElementColor = "GetColor")]
		[InfoBox("if Enable Multiplex, the data will added to multiplexStoryContext", VisibleIf = "@multiplex")]
		public List<StoryData> storyContext;

		[ShowIf("@multiplex"), ReadOnly]
		[ListDrawerSettings(NumberOfItemsPerPage = 1)]
		[SerializeReference]
		public List<List<StoryData>> multiplexStoryContext = new List<List<StoryData>>();

		private Color GetColor(){
			return Color.gray;
		}


		[Button("Add Story"), ShowIf("@multiplex")]
		private void AddStory(){
			multiplexStoryContext.Add(storyContext);
		}

		[Button("Remove Story"), ShowIf("@multiplex")]
		private void RemoveStory(){
			multiplexStoryContext.Remove(multiplexStoryContext.Last());
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
	}
}