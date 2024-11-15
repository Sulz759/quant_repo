using System.Collections.Generic;
using _Project.Develop.Architecture.Runtime.Utilities.Logging;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.Develop.Architecture.Runtime.Core.Railway
{
    public class RailwayView: MonoBehaviour
    {
        public readonly List<Checkpoint> Checkpoints = new ();

        [SerializeField] private List<WayView> ways;
        [SerializeField] private List<GameObject> pointObjects;
        

        public void Initialize()
        {
            for (int i = 0; i < pointObjects.Count; i++)
            {
                Checkpoints.Add(new Checkpoint(i, pointObjects[i]));
            }
            Log.Battle.D(Checkpoints.Count.ToString());
        }

        public List<WayView> GetWays()
        {
            return ways;
        }
    }

    public class Checkpoint
    {
        public int Index;
        public GameObject Object;
        

        public Checkpoint(int index, GameObject obj)
        {
            Index = index;
            Object = obj;
            
        }
    }
}