using _Project.Architecture.Scripts.Runtime.Utilities.SceneManager.Configs;

namespace _Project.Architecture.Scripts.Runtime.Utilities.SceneManager
{
    public sealed class SceneManagerItem : SceneManagerBase
    {
        public override void InitScenesMap()
        {
            sceneConfigMap[BootstrapConfig.SCENE_NAME] = new BootstrapConfig();
            sceneConfigMap[MetaConfig.SCENE_NAME] = new MetaConfig();
            sceneConfigMap[CoreConfig.SCENE_NAME] = new CoreConfig();
        }
    }
}