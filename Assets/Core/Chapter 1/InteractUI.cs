using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Core.Chapter_1{
	public class InteractUI : MonoBehaviour{
		[TitleGroup("Data Holder")] [ReadOnly] [SerializeField]
		private Sprite image;

		[ReadOnly] [SerializeField] private AnimationClip animationClip;
		[ReadOnly] [SerializeField] private AudioClip audioClip;

		[TitleGroup("Components")] [SerializeField] [Required]
		private Image imageComponent;

		[SerializeField] [Required] private Animation animationComponent;
		[SerializeField] [Required] private Button buttonComponent;
		[SerializeField] [Required] private AudioSource audioComponent;
		[Inject] private readonly PlayerData _playerData;

		private InteractData _data;

		[TitleGroup("Debug")] [SerializeField] [ReadOnly]
		private List<string> interactNames = new List<string>();

		[SerializeField] private Text debugText;

		private readonly List<GameObject> _levelRootList = new List<GameObject>();

		private void Start(){
			image = imageComponent.sprite;
			animationClip = animationComponent.clip;
			audioClip = audioComponent.clip;
			buttonComponent.OnClickAsObservable().Subscribe(x => OnInteractButtonClick());
		}


		public void SetData(InteractData data, bool enterOrExit){
			var animationData = data.interactAnimationData;
			image = animationData.image;
			animationClip = animationData.animationClip;
			audioClip = animationData.audioClip;
			_data = data;

			UpdateDebugUI(data, enterOrExit);
		}

		public void Interact(){
			if(_data.interactAnimationData.correctAnswer){
				imageComponent.sprite = image;
				animationComponent.clip = animationClip;
				audioComponent.clip = audioClip;
				SetComponentActive(true);
			}
			else{
				CreateLevel(_data.interactAnimationData.interactiveRoot);
			}
		}

		private void CreateLevel(InteractiveRoot root){
			var levelRoot = Instantiate(root, transform);
			levelRoot.name = root.name;
			levelRoot.targetFound.AddListener(() => OnLevelPass(root.name));
			_levelRootList.Add(levelRoot.gameObject);
		}

		private void OnLevelPass(string rootName){
			var foundRoot = _levelRootList.Find(x => x.name == rootName);
			Debug.Log("找到錯誤音效! 給予一個 '通關條件'");
			_playerData.SaveSuccessResult(_data.name);
			foundRoot.SetActive(false);
		}

		public bool InteractObjectExist(){
			return interactNames.Count > 0;
		}

		private void UpdateDebugUI(InteractData data, bool enterOrExit){
			var objID = data.name;
			if(enterOrExit){
				interactNames.Insert(0, objID);
			}
			else{
				if(interactNames.Contains(objID)){
					interactNames.Remove(objID);
				}
			}

			if(interactNames.Count > 0)
				debugText.text = interactNames.First();
		}

		private void SetComponentActive(bool active){
			imageComponent.gameObject.SetActive(active);
			animationComponent.gameObject.SetActive(active);
			buttonComponent.gameObject.SetActive(active);
			audioComponent.gameObject.SetActive(active);
		}

		private void OnInteractButtonClick(){
			Debug.Log("這是正確的音效，沒有問題");
			SetComponentActive(false);
		}
	}
}