using System;
using System.Collections.Generic;

namespace _Project.Develop.Architecture.Runtime.Meta.Nodes
{

    public static class Node
    {
        public static readonly Dictionary<int, INodeType> Type = new Dictionary<int, INodeType>()
        {
            {0, new EnemyNode()},
            {1, new EliteEnemyNode()},
            {2, new ShopNode()},
            {3, new RestNode()},
            {4, new UnknownNode()}
        };
    }
    public class EnemyNode : INodeType
    {
        public void Run()
        {
            throw new System.NotImplementedException();
        }
    }

    public class EliteEnemyNode : EnemyNode
    {
        
    }

    public class ShopNode : INodeType
    {
        public void Run()
        {
            throw new NotImplementedException();
        }
    }
    
    public class RestNode : INodeType
    {
        public void Run()
        {
            throw new NotImplementedException();
        }
    }
    
    public class UnknownNode : INodeType
    {
        public void Run()
        {
            throw new NotImplementedException();
        }
    }

    public interface INodeType
    {
        void Run();
    }
}