using System.Collections.Generic;
using _Project.Develop.Architecture.Runtime.Meta.Nodes;
using UnityEngine;

namespace _Project.Develop.Architecture.Runtime.Meta.Tests
{
    public class ShuffleBagTest : MonoBehaviour
    {
        public List<NodeView> nodes;
        public NodeFactory factory;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                foreach (var node in nodes) Destroy(node.gameObject);

                nodes = factory.CreateNodes();
            }
        }
    }
}