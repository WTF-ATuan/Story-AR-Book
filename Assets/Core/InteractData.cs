using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Core{
	[Serializable]
	public class InteractData{
		[ReadOnly] [HideInInspector] public string name;

		[TitleGroup("$name", "$instanceID", TitleAlignments.Split, boldTitle: true)] [ValueDropdown("GetDataTag")]
		public InteractTag tag = InteractTag.None;

		[ReadOnly] [HideInInspector] public int instanceID;

		[ShowIf("tag", InteractTag.Teleport)] [BoxGroup("Teleport"), HideLabel]
		public TeleportData teleportData;

		[ShowIf("tag", InteractTag.InteractAnimation)] [BoxGroup("InteractAnimation"), HideLabel]
		public InteractAnimationData interactAnimationData;

		[ShowIf("tag", InteractTag.InteractAnimation)]
		public bool storyGuide;

		[ShowIf("IsShowStoryGuide")] [BoxGroup("StoryGuide"), HideLabel]
		public StoryGuideData storyGuideData;

		private List<ValueDropdownItem> GetDataTag(){
			return new List<ValueDropdownItem>{
				new ValueDropdownItem("None", InteractTag.None),
				new ValueDropdownItem("InteractAnimation", InteractTag.InteractAnimation),
				new ValueDropdownItem("Teleport ", InteractTag.Teleport),
				new ValueDropdownItem("StoryGuide", InteractTag.StoryGuide)
			};
		}

		private bool IsShowStoryGuide(){
			if(tag == InteractTag.InteractAnimation){
				return storyGuide;
			}

			return tag == InteractTag.StoryGuide;

		}
	}

	public enum InteractTag{
		None,
		Teleport,
		InteractAnimation,
		StoryGuide,
	}
}