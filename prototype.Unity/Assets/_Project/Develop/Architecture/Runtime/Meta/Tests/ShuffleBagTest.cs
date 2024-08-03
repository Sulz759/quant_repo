using System;
using System.Collections.Generic;
using _Project.Develop.Architecture.Runtime.Meta.Nodes;
using _Project.Develop.Architecture.Runtime.Utilities.Logging;
using UnityEngine;

namespace _Project.Develop.Architecture.Runtime.Meta.Tests
{
    public class ShuffleBagTest: MonoBehaviour
    {
        public NodeFactory factory;
        public List<NodeView> nodes;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                foreach (var node in nodes)
                {
                    Destroy(node.gameObject);
                }

                nodes = factory.CreateNodes();
                
                Log.Battle.D("Create");
            }
        }
    }
}