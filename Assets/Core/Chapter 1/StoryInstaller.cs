using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Core.Chapter_1{
	public class StoryInstaller : MonoInstaller<StoryInstaller>{
		[SerializeField, ReadOnly] private InteractRepository interactRepository;
		[SerializeField, ReadOnly] private InteractLevel interactLevel;
		[SerializeField, ReadOnly] private UIPresenter uiPresenter;
		[SerializeField, ReadOnly] private StoryRoot storyRoot;
		[SerializeField, ReadOnly] private OutlineRepository outlineRepository;


		public override void InstallBindings(){
			GetSceneData();
			Container.Bind<PlayerData>().AsSingle().NonLazy();
			Container.Bind<InteractRepository>().FromInstance(interactRepository).AsSingle();
			Container.Bind<InteractLevel>().FromInstance(interactLevel).AsSingle();
			Container.Bind<UIPresenter>().FromInstance(uiPresenter).AsSingle();
			Container.Bind<StoryRoot>().FromInstance(storyRoot).AsSingle();
			Container.Bind<OutlineRepository>().FromInstance(outlineRepository).AsSingle();
		}

		private void GetSceneData(){
			interactRepository = FindObjectOfType<InteractRepository>(true);
			interactLevel = FindObjectOfType<InteractLevel>(true);
			uiPresenter = FindObjectOfType<UIPresenter>(true);
			storyRoot = FindObjectOfType<StoryRoot>();
			outlineRepository = FindObjectOfType<OutlineRepository>();
		}
	}
}