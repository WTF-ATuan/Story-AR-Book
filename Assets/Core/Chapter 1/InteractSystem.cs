using System;
using Core.Chapter_1;
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

		private void Start(){
			interactButton.OnClickAsObservable().Subscribe(x => Interact());
			interactRepository.RegisterAll(CompareData);
			_teleport = new Teleport(transform, interactRepository);
		}


		private void CompareData(string objID, bool enterOrExit){
			var interactData = _interactDataSet.FindData(objID);
			_interactTag = interactData.tag;
			switch(interactData.tag){
				case InteractTag.InteractAnimation:{
					_interactUI.SetData(interactData, enterOrExit);
					interactButton.gameObject.SetActive(_interactUI.InteractObjectExist());
					break;
				}
				case InteractTag.Teleport:{
					_teleport.SetData(interactData.teleportData);
					interactButton.gameObject.SetActive(true);
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
	}
}