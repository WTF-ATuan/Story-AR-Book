using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Core{
	[CreateAssetMenu(fileName = "GameSetting", menuName = "Setting/GameSetting", order = 0)]
	public class GameSetting : ScriptableObjectInstaller<GameSetting>{
		[EnumToggleButtons] [HideLabel] public PlatformType platform = PlatformType.ArBuild;
		public SoundDataSet soundDataSet;
		public PlayerMoveData playerMoveData;

		public override void InstallBindings(){
			Container.Bind<PlatformType>().FromInstance(platform).NonLazy();
			Container.Bind<PlayerMoveData>().FromInstance(playerMoveData).NonLazy();
			Container.Bind<SoundDataSet>().FromInstance(soundDataSet).NonLazy();
		}
	}

	public enum PlatformType{
		ArBuild,
		MobileBuild,
		Editor,
	}
}