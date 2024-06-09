using _Project.Architecture.Scripts.Runtime.Utilities.SceneManager.Configs;

namespace _Project.Architecture.Scripts.Runtime.Utilities.SceneManager
{
	public sealed class SceneManagerItem : SceneManagerBase
	{
		public override void InitScenesMap()
		{
			this.sceneConfigMap[BootstrapConfig.SCENE_NAME] = new BootstrapConfig();
			this.sceneConfigMap[GlobalMapConfig.SCENE_NAME] = new GlobalMapConfig();
			this.sceneConfigMap[FightSceneConfig.SCENE_NAME] = new FightSceneConfig();
		}
	}
}