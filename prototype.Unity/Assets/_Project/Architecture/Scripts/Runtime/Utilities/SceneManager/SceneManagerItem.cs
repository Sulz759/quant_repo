using _Project.Architecture.Scripts.Runtime.Utilities.SceneManager.Configs;

namespace _Project.Architecture.Scripts.Runtime.Utilities.SceneManager
{
	public sealed class SceneManagerItem : SceneManagerBase
	{
		public override void InitScenesMap()
		{
			this.sceneConfigMap[BootstrapConfig.SCENE_NAME] = new BootstrapConfig();
			this.sceneConfigMap[MetaConfig.SCENE_NAME] = new MetaConfig();
			this.sceneConfigMap[CoreConfig.SCENE_NAME] = new CoreConfig();
		}
	}
}