using System;
using Nodes.Editor.Data;
using Nodes.Editor.Utils;
using UnityEngine;

namespace Nodes.Editor.Core
{
  public class NodeFactory
  {
    private readonly ExternalDataProvider _externalDataProvider;

    public NodeFactory(ExternalDataProvider externalDataProvider)
    {
      _externalDataProvider = externalDataProvider;
    }

    public Node Create(NodeData data)
    {
      var node = Create(data.Type, data.Guid.Guid);
      JsonUtility.FromJsonOverwrite(data.Node, node);
      return node;
    }

    public Node Create(Type nodeType, Guid guid)
    {
      var node = (Node)Activator.CreateInstance(nodeType);
      MethodInvoker<Node, PostCreateAttribute>.Invoke(node, _externalDataProvider, guid);
      return node;
    }

    public T Create<T>(Guid guid) where T : Node
    {
      var node = Activator.CreateInstance<T>();
      MethodInvoker<Node, PostCreateAttribute>.Invoke(node, _externalDataProvider, guid);
      return node;
    }
  }
}
