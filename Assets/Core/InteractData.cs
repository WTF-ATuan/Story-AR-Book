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
		public AnimationData animationData;


		private List<ValueDropdownItem> GetDataTag(){
			return new List<ValueDropdownItem>{
				new ValueDropdownItem("None", InteractTag.None),
				new ValueDropdownItem("InteractAnimation", InteractTag.InteractAnimation),
				new ValueDropdownItem("Success ", InteractTag.Success),
				new ValueDropdownItem("Failure ", InteractTag.Failure),
				new ValueDropdownItem("Teleport ", InteractTag.Teleport),
			};
		}
	}

	public enum InteractTag{
		None,
		Success,
		Failure,
		Teleport,
		InteractAnimation,
		Condition
	}
}