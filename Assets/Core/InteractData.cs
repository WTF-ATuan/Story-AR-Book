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

		[FoldoutGroup("UI Component")] public Sprite image;
		[FoldoutGroup("UI Component")] public AnimationClip animationClip;
		[FoldoutGroup("UI Component")] public AudioClip audioClip;
		[ShowIf("tag" , InteractTag.Teleport)] public TeleportData teleportData;


		private List<ValueDropdownItem> GetDataTag(){
			return new List<ValueDropdownItem>{
				new ValueDropdownItem("None", InteractTag.None),
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
		Teleport
	}
}