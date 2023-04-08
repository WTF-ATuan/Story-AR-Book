﻿using System;
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

		[TitleGroup("$name", "$instanceID", TitleAlignments.Split, boldTitle: true)]
		public string showsName;

		[ShowIf("tag", InteractTag.Teleport)] [BoxGroup("Teleport"), HideLabel]
		public TeleportData teleportData;

		[ShowIf("tag", InteractTag.InteractAnimation)] [BoxGroup("InteractAnimation"), HideLabel]
		public InteractAnimationData interactAnimationData;

		[ShowIf("tag", InteractTag.InteractAnimation)]
		public bool storyGuide;

		[ShowIf("tag", InteractTag.InteractAnimation)] [ShowIf("@storyGuide")]
		public bool onFinish;

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

	public class StoryEvent{
		public readonly string EventID;

		public StoryEvent(string eventID){
			EventID = eventID;
		}
	}
}