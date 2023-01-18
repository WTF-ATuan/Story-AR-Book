using System;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Core.Testing{
	public class InteractSystem : MonoBehaviour{
		[SerializeField] private Button interactButton;
		[SerializeField] private Text debugText;

		public InteractRepository interactRepository;

		private void Start(){
			interactButton.OnClickAsObservable().Subscribe(OnClick);
			interactRepository = FindObjectOfType<InteractRepository>();
			interactRepository.RegisterCollisionWithTag("Interact", true, TriggerEnter);
			interactRepository.RegisterCollisionWithTag("Interact", false, TriggerExit);
			interactRepository.RegisterCollision("Stairs", (obj, condition) => Teleport(true, condition));
			// interactRepository.RegisterCollision("Stairs Target", (obj, condition) => Teleport(false, condition));
			interactRepository.RegisterCollision("Exit", (obj, condition) => Exit());
		}

		private void Teleport(bool toTarget, bool enterOrExit){
			var target = interactRepository.interactObject.Find(x => x.name == "Stairs Target");
			transform.position = target.transform.position;
		}
		private void Exit(){
			debugText.text = "逃脫成功";
			debugText.color = Color.red;
			debugText.fontStyle = FontStyle.Bold;
		}

		private void TriggerEnter(Collider obj, string objID){
			interactButton.gameObject.SetActive(true);
			debugText.text = objID;
		}

		private void TriggerExit(Collider obj, string objID){
			interactButton.gameObject.SetActive(false);
		}

		private void OnClick(Unit obj){
			//需要一個可以設定 答案的資料庫，去比對答案
		}
	}
}