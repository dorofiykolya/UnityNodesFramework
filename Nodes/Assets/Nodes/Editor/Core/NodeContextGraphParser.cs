using System.Collections.Generic;
using Nodes.Editor.Data;
using Nodes.Editor.Utils;
using UnityEngine;

namespace Nodes.Editor.Core
{
  public partial class NodeContext
  {
    private void ParseGraph()
    {
      foreach (var nodeData in _graphData.Nodes)
      {
        CreateNode(nodeData);
      }

      foreach (var connectionData in _graphData.Connections)
      {
        var connection = _sockets[connectionData.In.Guid].Connect(_sockets[connectionData.Out.Guid], connectionData.Guid.Guid);
        _connections[connection.Guid] = connection;
      }
    }

    private void CreateNode(NodeData nodeData)
    {
      var sockets = new List<Socket>();

      var node = _nodeFactory.Create(nodeData);
      _nodes.Add(node.Guid, node);
      node.GetSockets(sockets, SocketType.Input);
      node.GetSockets(sockets, SocketType.Output);

      foreach (var socketData in nodeData.Sockets)
      {
        var socket = node.GetSocket(socketData.Name);
        JsonUtility.FromJsonOverwrite(socketData.Data, socket);
        socket.Guid = socketData.Guid.Guid;
      }
      foreach (var socket in sockets)
      {
        _sockets[socket.Guid] = socket;
      }
    }

    private void SaveGraph()
    {
      _graphData.Nodes = new NodeData[_nodes.Count];
      _graphData.Connections = new ConnectionData[_connections.Count];

      var i = 0;
      foreach (var node in _nodes.Values)
      {
        _graphData.Nodes[i] = NodeSerializer.Serialize(node);

        i++;
      }

      i = 0;

      foreach (var connection in _connections.Values)
      {
        _graphData.Connections[i] = new ConnectionData
        {
          Guid = new GuidData(connection.Guid),
          In = new GuidData(connection.In.Guid),
          Out = new GuidData(connection.Out.Guid)
        };

        i++;
      }
    }
  }
}
