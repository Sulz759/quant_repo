using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Develop.Architecture.Runtime.Meta.Nodes
{

    public static class Node
    {
        public static readonly Dictionary<int, NodeType> Type = new Dictionary<int, NodeType>()
        {
            {0, new EnemyNode()},
            {1, new EliteEnemyNode()},
            {2, new ShopNode()},
            {3, new RestNode()},
            {4, new UnknownNode()}
        };
    }
    public class EnemyNode : NodeType
    {
        public override void Initialize()
        {
            color = Color.yellow; // for test
        }
    }

    public class EliteEnemyNode : EnemyNode
    {
        public override void Initialize()
        {
            color = Color.red;  // for test
        }
    }

    public class ShopNode : NodeType
    {
        public override void Initialize()
        {
            color = Color.blue;  // for test
        }
    }
    
    public class RestNode : NodeType
    {
        public override void Initialize()
        {
            color = Color.green;  // for test
        }
    }
    
    public class UnknownNode : NodeType
    {
        public override void Initialize()
        {
            color = Color.gray;  // for test
        }
    }

    public abstract class NodeType
    {
        public Color color;
        
        public abstract void Initialize();
    }
}