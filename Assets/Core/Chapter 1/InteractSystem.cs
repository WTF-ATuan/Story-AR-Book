﻿using System;
using System.Collections.Generic;
using System.Linq;
using Core.Chapter_1;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Core.Testing{
	public class InteractSystem : MonoBehaviour{
		[SerializeField] private Button interactButton;

		[Inject] public InteractRepository interactRepository;
		[Inject] private InteractUI _interactUI;
		[Inject] private readonly InteractDataSet _interactDataSet;

		private Teleport _teleport;
		private InteractTag _interactTag;
		
		[TitleGroup("Debug")] [SerializeField] [ReadOnly]
		private List<string> interactNames = new List<string>();

		[SerializeField] private Text debugText;

		private void Start(){
			interactButton.OnClickAsObservable().Subscribe(x => Interact());
			interactRepository.RegisterAll(CompareData);
			interactRepository.RegisterAll(UpdateInteract);
			_teleport = new Teleport(transform, interactRepository);
		}


		private void CompareData(string objID, bool enterOrExit){
			var interactData = _interactDataSet.FindData(objID);
			_interactTag = interactData.tag;
			switch(interactData.tag){
				case InteractTag.InteractAnimation:{
					_interactUI.SetData(interactData);
					break;
				}
				case InteractTag.Teleport:{
					_teleport.SetData(interactData.teleportData);
					break;
				}
			}
		}

		private void Interact(){
			switch(_interactTag){
				case InteractTag.InteractAnimation:{
					_interactUI.Interact();
					break;
				}
				case InteractTag.Teleport:
					_teleport.Interact();
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}


			interactButton.gameObject.SetActive(false);
		}
		private void UpdateInteract(string objID, bool enterOrExit){
			if(enterOrExit){
				interactNames.Insert(0, objID);
			}
			else{
				if(interactNames.Contains(objID)){
					interactNames.Remove(objID);
				}
			}
			interactButton.gameObject.SetActive(interactNames.Count > 0);
			if(interactNames.Count > 0)
				debugText.text = interactNames.First();
		}

	}
}