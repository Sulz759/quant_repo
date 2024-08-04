using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Develop.Architecture.Runtime.Meta.Nodes
{

    public static class Node
    {
        public static IReadOnlyList<NodeType> Types = new NodeType[5]
        {
            new EnemyNode(),
            new EliteEnemyNode(),
            new ShopNode(),
            new RestNode(),
            new UnknownNode()
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