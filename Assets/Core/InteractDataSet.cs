using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Core{
	[Serializable]
	public class InteractDataSet{
		[SerializeField] [FoldoutGroup("Replace")] [ValueDropdown("GetData")]
		private string replaceID;

		[SerializeField] [FoldoutGroup("Replace")]
		private GameObject selectedObject;

		[SerializeField] [FoldoutGroup("Replace")]
		private string replaceShowsName;
		
		[Searchable] [ListDrawerSettings(NumberOfItemsPerPage = 3, ShowItemCount = true)] [SerializeField]
		private List<InteractData> interactDataList;

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

		private List<ValueDropdownItem> GetData(){
			return interactDataList.Select(data => new ValueDropdownItem(data.name, data.name)).ToList();
		}

		private List<ValueDropdownItem> GetDataReference(){
			return interactDataList.Select(data => new ValueDropdownItem(data.name, data)).ToList();
		}

		[Button]
		private void Sort(){
			interactDataList = interactDataList.OrderBy(x => x.tag).ToList();
		}

		[Button, FoldoutGroup("Replace")]
		private void Replace(){
			var foundData = FindData(replaceID);
			foundData.name = selectedObject.name;
			foundData.instanceID = selectedObject.GetInstanceID();
			foundData.showsName = replaceShowsName;
			replaceID = string.Empty;
			selectedObject = null;
			replaceShowsName = string.Empty;
		}

		#endregion
	}
}