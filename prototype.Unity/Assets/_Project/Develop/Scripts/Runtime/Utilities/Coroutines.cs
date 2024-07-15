using System;
using System.Collections;
using UnityEngine;

namespace _Project.Architecture.Scripts.Runtime.Utilities
{
	public sealed class Coroutines : MonoBehaviour
	{
		private static Coroutines instance 
		{ 
			get 
			{
				if (m_instance == null) 
				{
					var go = new GameObject("[COROUTINE MANAGER]");
					m_instance = go.AddComponent<Coroutines>();
					DontDestroyOnLoad(go);
				}

				return m_instance;
			} 
		}

		private static Coroutines m_instance;

		public static Coroutine StartRoutine(IEnumerator enumerator) 
		{
			return instance.StartCoroutine(enumerator);
		}

		public static void StopRoutine(Coroutine routine) 
		{
			try
			{
				instance.StopCoroutine(routine);

			}
			catch (NullReferenceException e)
			{
				Console.WriteLine(e);
			}
			
		}

		public static void StopAllRoutines()
		{
			instance.StopAllCoroutines();
		}
	}
}