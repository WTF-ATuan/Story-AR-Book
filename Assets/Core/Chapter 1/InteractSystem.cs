using System;
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

		[Inject] public InteractRepository interactRepository;
		[Inject] private InteractUI _interactUI;
		[Inject] private readonly PlayerData _playerData;
		[Inject] private readonly InteractDataSet _interactDataSet;

		private Teleport _teleport;
		private InteractTag _interactTag;

		private void Start(){
			interactButton.OnClickAsObservable().Subscribe(x => Interact());
			interactRepository.RegisterAll(CompareData);
			_teleport = new Teleport(transform, interactRepository);
		}


		private void CompareData(string objID, bool enterOrExit){
			var interactData = _interactDataSet.FindData(objID);
			_interactTag = interactData.tag;
			switch(interactData.tag){
				case InteractTag.Teleport:{
					_teleport.SetData(interactData.teleportData);
					interactButton.gameObject.SetActive(true);
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
					_interactUI.SetData(interactData.animationData);
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
				case InteractTag.Condition:
					var successCount = _playerData.GetSuccessCount();
					Debug.Log(successCount > 0 ? "Pass" : "Not Pass");
					debugText.text = successCount > 0 ? "通關成功" : "未找齊所有物件";
					break;
				case InteractTag.Teleport:
					_teleport.Interact();
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}


			interactButton.gameObject.SetActive(false);
		}
	}
}