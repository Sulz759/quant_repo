using _Project.Architecture.Scripts.Runtime;
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

        private void Start()
        {
            Debug.Log("Bootstrap is started");
        }
    }
}