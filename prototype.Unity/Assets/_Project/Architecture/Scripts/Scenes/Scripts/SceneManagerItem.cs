namespace _Project.Architecture
{
	public sealed class SceneManagerItem : SceneManagerBase
	{
		public override void InitScenesMap()
		{
			this.sceneConfigMap[SampleSceneConfig.SCENE_NAME] = new SampleSceneConfig();
		}
	}
}