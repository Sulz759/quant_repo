using UnityEngine;

namespace _Project.Architecture
{
    public class Bootstrap: MonoBehaviour
    {
        private void Awake()
        {
            Application.targetFrameRate = 60;
            Game.Run();
        }

        public void StartNewGame()
        {
            Game.sceneManager.scene.BeginRepositories();
        }
    }
}