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
		[SerializeField] private Text debugText;

		[SerializeField] [ReadOnly] private List<string> interactNames = new List<string>();

		public InteractRepository interactRepository;
		[SerializeField] private InteractUI interactUI;

		[Inject] private readonly PlayerData _playerData;
		[Inject] private readonly InteractDataSet _interactDataSet;


		private Vector3 _teleportTarget;
		private InteractTag _interactTag;

		private void Start(){
			interactRepository = FindObjectOfType<InteractRepository>();
			interactUI = FindObjectOfType<InteractUI>(true);
			Register();
		}

		private void Register(){
			interactButton.OnClickAsObservable().Subscribe(x => Interact());
			interactRepository.RegisterWithTag("Interact", CompareData);
		}

		private void CompareData(string objID, bool enterOrExit){
			var interactData = _interactDataSet.FindData(objID);
			_interactTag = interactData.tag;
			switch(interactData.tag){
				case InteractTag.Teleport:{
					var targetObjID = interactData.teleportData.target;
					var target = interactRepository.interactObject.Find(x => x.name == targetObjID);
					if(!target){
						throw new Exception($"Can,t teleport to {targetObjID} ");
					}

					interactButton.gameObject.SetActive(true);
					_teleportTarget = target.transform.position;
					break;
				}
				case InteractTag.InteractAnimation:{
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
					interactUI.SetData(interactData.animationData);
					break;
				}
			}
		}

		private void TargetCondition(string objID, bool enterOrExit){
			interactButton.gameObject.SetActive(true);
		}

		private void Interact(){
			switch(_interactTag){
				case InteractTag.InteractAnimation:{
					interactUI.ShowUI();
					break;
				}
				case InteractTag.Condition:
					var successCount = _playerData.GetSuccessCount();
					Debug.Log(successCount > 0 ? "Pass" : "Not Pass");
					debugText.text = successCount > 0 ? "通關成功" : "未找齊所有物件";
					break;
				case InteractTag.Teleport:
					transform.position = _teleportTarget;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}


			interactButton.gameObject.SetActive(false);
		}
	}
}