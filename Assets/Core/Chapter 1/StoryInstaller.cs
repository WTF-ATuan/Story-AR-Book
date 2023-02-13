using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Core.Chapter_1{
	public class StoryInstaller : MonoInstaller<StoryInstaller>{
		[SerializeField, ReadOnly] private InteractRepository interactRepository;
		[SerializeField, ReadOnly] private InteractUI interactUI;
		[SerializeField, ReadOnly] private UIPresenter uiPresenter;


		public override void InstallBindings(){
			GetSceneData();
			Container.Bind<PlayerData>().AsSingle().NonLazy();
			Container.Bind<InteractRepository>().FromInstance(interactRepository).AsSingle();
			Container.Bind<InteractUI>().FromInstance(interactUI).AsSingle();
			Container.Bind<UIPresenter>().FromInstance(uiPresenter).AsSingle();
		}

		private void GetSceneData(){
			interactRepository = FindObjectOfType<InteractRepository>(true);
			interactUI = FindObjectOfType<InteractUI>(true);
			uiPresenter = FindObjectOfType<UIPresenter>(true);
		}
	}
}