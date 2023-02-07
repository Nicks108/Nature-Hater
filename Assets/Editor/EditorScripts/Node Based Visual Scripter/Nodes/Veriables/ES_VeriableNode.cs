using UnityEngine;
using System.Collections;

namespace NTTools.NodeScripter
{
    public class ES_VeriableNode : Node
    {
       public ES_VeriableNode(string name, int id, Vector2 pos):base(name, id, pos)
       {
           OutputPositons = new Node[1];
       }
    }
}
