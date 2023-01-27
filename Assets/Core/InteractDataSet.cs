using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Core{
	[Serializable]
	public class InteractDataSet{
		[Searchable] [ListDrawerSettings(NumberOfItemsPerPage = 5, ShowItemCount = true)] [SerializeField]
		private List<InteractData> interactDataList;

		public bool CheckCorrect(string objID){
			var interactData = interactDataList.Find(x => x.name == objID);
			if(interactData is null){
				throw new Exception($"Can,t find {objID}");
			}

			return interactData.tag == InteractTag.Success;
		}

		public InteractData FindData(string objID){
			var interactData = interactDataList.Find(x => x.name == objID);
			if(interactData is null){
				throw new Exception($"Can,t find {objID}");
			}

			return interactData;
		}

		#region Editor

		[TitleGroup("Create With Tag")]
		[InlineButton("CreateWithTag", SdfIconType.Tag, "Invoke")]
		[ValueDropdown("GetAllTag")]
		[SerializeField]
		private string tag = "Interact";

		[TitleGroup("Create With GameObjects")]
		[InlineButton("CreateWithGameObject", SdfIconType.Box, "Invoke")]
		[SerializeField]
		private GameObject[] gameObjects;

		private void CreateWithTag(){
			var foundWithTag = GameObject.FindGameObjectsWithTag(tag);
			foreach(var gameObject in foundWithTag){
				var interactData = new InteractData{
					name = gameObject.name,
					instanceID = gameObject.GetInstanceID(),
				};
				if(!interactDataList.Exists(x => x.name == interactData.name)){
					interactDataList.Add(interactData);
				}
			}
		}

		private void CreateWithGameObject(){
			foreach(var gameObject in gameObjects){
				var interactData = new InteractData{
					name = gameObject.name,
					instanceID = gameObject.GetInstanceID()
				};
				if(!interactDataList.Exists(x => x.name == interactData.name)){
					interactDataList.Add(interactData);
				}
			}

			gameObjects = Array.Empty<GameObject>();
		}

		private List<ValueDropdownItem> GetAllTag(){
			return new List<ValueDropdownItem>{
				new ValueDropdownItem("Interact", "Interact"),
				new ValueDropdownItem("Story", "Story"),
			};
		}

		[Serializable]
		public class InteractData{
			[ReadOnly] [HideInInspector] public string name;

			[TitleGroup("$name", "$instanceID", TitleAlignments.Split, boldTitle: true)] [ValueDropdown("GetDataTag")]
			public InteractTag tag = InteractTag.None;

			[ReadOnly] [HideInInspector] public int instanceID;

			[FoldoutGroup("UI Component")] public Sprite image;
			[FoldoutGroup("UI Component")] public AnimationClip animationClip;
			[FoldoutGroup("UI Component")] public RectTransform buttonTransform;
			[FoldoutGroup("UI Component")] public AudioClip audioClip;


			private List<ValueDropdownItem> GetDataTag(){
				return new List<ValueDropdownItem>{
					new ValueDropdownItem("None", InteractTag.None),
					new ValueDropdownItem("Success ", InteractTag.Success),
					new ValueDropdownItem("Failure ", InteractTag.Failure),
				};
			}
		}

		#endregion
	}

	public enum InteractTag{
		None,
		Success,
		Failure,
	}
}