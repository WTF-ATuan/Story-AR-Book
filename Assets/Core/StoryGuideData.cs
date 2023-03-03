using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Core{
	[Serializable]
	public class StoryGuideData{
		public bool interact;
		public bool showOnes;

		[ListDrawerSettings(NumberOfItemsPerPage = 5, ElementColor = "GetColor")]
		public List<StoryData> storyContext;


		private Color GetColor(){
			return Color.gray;
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