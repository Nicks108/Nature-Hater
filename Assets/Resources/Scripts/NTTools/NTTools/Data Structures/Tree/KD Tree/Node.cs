using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NTTools.NTTools.Data_Structures.Tree.KD_Tree
{
    class NodeGameObject
    {
        public GameObject Node;
        public NodeGameObject LeftChildNode = null;
        public NodeGameObject RightChildNode = null;

        public bool HasLeftNode
        {
            get { return this.LeftChildNode != null; }
        }
        public bool HasRightNode
        {
            get { return this.RightChildNode != null; }
        }
        public NodeGameObject(GameObject _node, NodeGameObject LeftChild, NodeGameObject RightChild)
        {
            this.Node = _node;
            this.LeftChildNode = LeftChild;
            this.RightChildNode = RightChild;
        }
    }
}
