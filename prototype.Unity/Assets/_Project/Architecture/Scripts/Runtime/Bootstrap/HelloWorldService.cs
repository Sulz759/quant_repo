using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace _Project.Architecture.Scripts.Runtime.Bootstrap
{
    public class HelloWorldService
    {
        public void Hello()
        {
            Debug.Log("Hello world");
        }
    }

    public class HelloScreen : MonoBehaviour
    {
        public Button HelloButton;
    }
}