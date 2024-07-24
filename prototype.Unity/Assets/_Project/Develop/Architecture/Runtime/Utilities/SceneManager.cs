using System.Collections;
using _Project.Develop.Architecture.Runtime.Loading;
using _Project.Develop.Architecture.Runtime.Utilities.Logging;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

namespace _Project.Develop.Architecture.Runtime.Utilities
{
    public class SceneManager
    {
        private const string LogTag = "SCENE";
        
        public async UniTask LoadScene(int toLoadIndex)
        {
            await FadeInAndWait();
            
            int currentSceneIndex = UnitySceneManager.GetActiveScene().buildIndex;
            bool isSkipEmpty = currentSceneIndex == RuntimeConstants.Scenes.Loading || currentSceneIndex == RuntimeConstants.Scenes.Bootstrap || toLoadIndex == currentSceneIndex;

            if (isSkipEmpty)
            {
                Log.Default.D(LogTag, $"Empty scene skipped. {SceneUtility.GetScenePathByBuildIndex(toLoadIndex)} is loading.");
                
                await LoadSceneWithFade(toLoadIndex);
                
                return;
            }
            
            bool needLoadEmpty = toLoadIndex == RuntimeConstants.Scenes.Meta || toLoadIndex == RuntimeConstants.Scenes.Core || toLoadIndex == RuntimeConstants.Scenes.Loading;

            if (needLoadEmpty)
            {
                Log.Default.D(LogTag, $"{SceneUtility.GetScenePathByBuildIndex(RuntimeConstants.Scenes.Empty)} is loading.");
                await LoadSceneWithFade(RuntimeConstants.Scenes.Empty);
            }
            
            await UniTask.NextFrame();
            
            Log.Default.D(LogTag, $"{SceneUtility.GetScenePathByBuildIndex(toLoadIndex)} is loading.");

            await LoadSceneWithFade(toLoadIndex);
        }
        
        private async UniTask LoadSceneWithFade(int sceneIndex)
        {
            UnitySceneManager.LoadScene(sceneIndex);
            await FadeOutAndWait();
        }

        private async UniTask FadeInAndWait()
        {
            var waitFading = true;
            Fader.instance.FadeIn(() => waitFading = false);
            await UniTask.WaitWhile(() => waitFading);
        }

        private async UniTask FadeOutAndWait()
        {
            var waitFading = true;
            Fader.instance.FadeOut(() => waitFading = false);
            await UniTask.WaitWhile(() => waitFading);
        }
    }
}