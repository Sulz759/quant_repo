﻿using UnityEngine;

namespace _Project.Develop.Architecture.Runtime.Meta.Nodes
{
    public class NodeView : MonoBehaviour
    {
        public string type;
        [SerializeField] private SpriteRenderer spriteRenderer;
        private NodeConfig _nodeConfig;
        private NodeType _nodeType;


        public void Initialize(NodeConfig nodeConfig, NodeType nodeType)
        {
            _nodeConfig = nodeConfig;
            _nodeType = nodeType;
            GetNodeType();
        }

        private void GetNodeType()
        {
            _nodeType.Initialize();
            type = _nodeType.GetType().ToString();
            spriteRenderer.color = _nodeType.color;
            transform.rotation = Quaternion.Euler(90, 0, 0);

            //Log.Meta.D($"{_nodeType.GetType().ToString()}");
        }
    }
}