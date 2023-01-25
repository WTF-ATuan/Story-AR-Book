using System.Collections.Generic;
using System.Linq;
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
		[SerializeField] private GameObject news;
		[Inject] private readonly InteractDataSet _interactDataSet;


		private void Start(){
			interactRepository = FindObjectOfType<InteractRepository>();
			interactButton.OnClickAsObservable().Subscribe(x => OnClick());
			interactRepository.RegisterCollisionWithTag("Interact", true, TriggerEnter);
			interactRepository.RegisterCollisionWithTag("Interact", false, TriggerExit);
		}

		private void TriggerEnter(Collider obj, string objID){
			interactNames.Insert(0, objID);
			UpdateUIElement();
		}

		private void TriggerExit(Collider obj, string objID){
			if(interactNames.Contains(objID)){
				interactNames.Remove(objID);
			}

			UpdateUIElement();
		}

		private void UpdateUIElement(){
			interactButton.gameObject.SetActive(interactNames.Count > 0);
			if(interactNames.Count > 0)
				debugText.text = interactNames.First();
		}

		private void OnClick(){
			ShowInteractUI();
		}

		private void ShowInteractUI(){
			var currentInteract = interactNames.First();
			if(!currentInteract.Equals("News")) return;
			// 報紙的純UI
			news.SetActive(true);
			var button = news.GetComponentInChildren<Button>();
			button.OnClickAsObservable().Subscribe(x => OnInteractButtonClick());
			//關閉按鈕UI
			interactButton.gameObject.SetActive(false);
		}

		private void OnInteractButtonClick(){
			if(_interactDataSet.CheckCorrect(interactNames.First())){
				Debug.Log($"找到錯誤音效! 給予一個 '通關條件' ");
			}
			else{
				Debug.Log($"這是正確的音效");
			}
			news.SetActive(false);
		}
	}
}