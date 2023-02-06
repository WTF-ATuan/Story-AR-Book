using Core.Chapter_One;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Core{
	[CreateAssetMenu(fileName = "GameSetting", menuName = "Setting/GameSetting", order = 0)]
	public class GameSetting : ScriptableObjectInstaller<GameSetting>{
		[VerticalGroup("Platform")] [EnumToggleButtons] [HideLabel]
		public PlatformType platform = PlatformType.ArBuild;

		public PlayerMoveData playerMoveData;
		public InteractDataSet interactDataSet;

		public override void InstallBindings(){
			Container.Bind<PlayerMoveData>().FromInstance(playerMoveData).NonLazy();
			Container.Bind<InteractDataSet>().FromInstance(interactDataSet).NonLazy();
		}

		[Button(SdfIconType.Building)]
		[VerticalGroup("Platform")]
		private void Build(){
			var switcher = FindObjectOfType<PlatformSwitcher>();
			switcher.SwitchPlatform(platform);
		}
	}

	public enum PlatformType{
		ArBuild,
		MobileBuild,
		Editor,
	}
}