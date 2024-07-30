using UnityEngine.SceneManagement;

namespace _Project.Develop.Architecture.Runtime
{
    public static class RuntimeConstants
    {
        public static class Scenes
        {
            public static readonly int Bootstrap = SceneUtility.GetBuildIndexByScenePath("0.Bootstrap");
            public static readonly int Loading = SceneUtility.GetBuildIndexByScenePath("1.Loading");
            public static readonly int Meta = SceneUtility.GetBuildIndexByScenePath("2.Meta");
            public static readonly int Core = SceneUtility.GetBuildIndexByScenePath("3.Core");
            public static readonly int Empty = SceneUtility.GetBuildIndexByScenePath("4.Empty");
        }
        
        public static class Configs
        {
            public const string ConfigFileName = "MetaConfig";
        }
        
        public static class Prefabs
        {
            public const string Checkpoint = "Prefabs/Meta/Node";
        }
    }
}