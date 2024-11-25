using System;
using UnityEngine;

namespace _Project.Develop.Architecture.Runtime.Loading
{
    public class Fader : MonoBehaviour
    {
        private static Fader _instance;
        [SerializeField] private Animator _animator;
        private Action _fadeInCallback;
        private Action _fadeOutCallback;

        public static Fader instance
        {
            get
            {
                if (_instance == null)
                {
                    var prefab = Resources.Load<Fader>("Screenloader/Fader");
                    _instance = Instantiate(prefab);
                    DontDestroyOnLoad(_instance.gameObject);
                }

                return _instance;
            }
        }

        public bool isFading { get; private set; }

        public void FadeIn(Action fadeInCallback)
        {
            if (isFading)
                return;
            isFading = true;
            _fadeInCallback = fadeInCallback;
            _animator.SetBool("Faded", true);
        }

        public void FadeOut(Action fadeOutCallback)
        {
            if (isFading)
                return;
            isFading = true;
            _fadeOutCallback = fadeOutCallback;
            _animator.SetBool("Faded", false);
        }

        private void Handle_FadeInAnimationOver()
        {
            _fadeInCallback.Invoke();
            _fadeInCallback = null;
            isFading = false;
        }

        private void Handle_FadeOutAnimationOver()
        {
            _fadeOutCallback.Invoke();
            _fadeOutCallback = null;
            isFading = false;
        }
    }
}