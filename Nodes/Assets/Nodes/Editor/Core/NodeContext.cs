using System;
using System.Collections.Generic;
using Nodes.Editor.Data;
using Nodes.Editor.Utils;

namespace Nodes.Editor.Core
{
  public partial class NodeContext
  {
    private readonly NodeCoroutine _nodeCoroutine;
    private readonly NodeProvider _provider;
    private readonly ExternalDataProvider _externalDataProvider;
    private readonly NodeFactory _nodeFactory;
    private readonly ActionsHistory _history;
    private readonly GraphData _graphData;
    private readonly Dictionary<Guid, Node> _nodes;
    private readonly Dictionary<Guid, Socket> _sockets;
    private readonly Dictionary<Guid, Connection> _connections;

    public NodeContext(NodeCoroutine nodeCoroutine, NodeProvider provider, ExternalDataProvider externalDataProvider, GraphData data)
    {
      _nodeCoroutine = nodeCoroutine;
      _provider = provider;
      _externalDataProvider = externalDataProvider;
      _nodes = new Dictionary<Guid, Node>();
      _sockets = new Dictionary<Guid, Socket>();
      _connections = new Dictionary<Guid, Connection>();
      _nodeFactory = new NodeFactory(_externalDataProvider);
      _history = new ActionsHistory();
      _graphData = data ?? new GraphData
      {
        Connections = new ConnectionData[0],
        Nodes = new NodeData[0]
      };
      ParseGraph();
    }

    public ActionsHistory History
    {
      get { return _history; }
    }

    public ICalculateContext Calculate(Node node)
    {
      return node.Calculate(_nodeCoroutine);
    }

    public T CreateNode<T>() where T : Node
    {
      return (T)CreateNode(typeof(T));
    }

    public Node CreateNode(Type type)
    {
      var guid = Guid.NewGuid();

      _history.Execute(() =>
      {
        var sockets = new List<Socket>();
        var node = _nodeFactory.Create(type, guid);
        _nodes.Add(node.Guid, node);

        node.GetSockets(sockets, SocketType.Input);
        node.GetSockets(sockets, SocketType.Output);

        foreach (var socket in sockets)
        {
          _sockets[socket.Guid] = socket;
        }
      }, () =>
      {
        var node = _nodes[guid];
        var sockets = new List<Socket>();
        node.GetSockets(sockets, SocketType.Input);
        node.GetSockets(sockets, SocketType.Output);
        foreach (var socket in sockets)
        {
          _sockets.Remove(socket.Guid);
        }
        _nodes.Remove(guid);
      }, "create node: " + type.FullName);

      return _nodes[guid];
    }

    public void DeleteNode(Guid guid)
    {
      var nodeData = NodeSerializer.Serialize(_nodes[guid]);
      _history.Execute(() =>
      {
        var node = _nodes[guid];
        var sockets = new List<Socket>();
        node.GetSockets(sockets, SocketType.Input);
        node.GetSockets(sockets, SocketType.Output);
        foreach (var socket in sockets)
        {
          _sockets.Remove(socket.Guid);
        }
        _nodes.Remove(guid);

      }, () => { CreateNode(nodeData); },
        "delete node: " + nodeData.Type.FullName);
    }

    public void Connect(SocketIn socketIn, SocketOut socketOut)
    {
      var lastConnection = socketIn.Connection;

      _history.Execute(() =>
      {
        if (lastConnection != null)
        {
          _connections.Remove(lastConnection.Guid);
        }
        var connection = socketIn.Connect(socketOut, Guid.NewGuid());
        _connections.Add(connection.Guid, connection);
      },
      () =>
      {
        if (lastConnection != null)
        {
          _connections.Remove(socketIn.Connection.Guid);
          var connection = socketIn.Connect(lastConnection.Out, lastConnection.Guid);
          _connections.Add(connection.Guid, connection);
        }
      }, "connect sockets");
    }

    public void Disconnect(Socket socket)
    {
      if (!socket.Connected) return;

      var lastConnection = socket.Connection;

      _history.Execute(() =>
        {
          _connections.Remove(socket.Connection.Guid);
          socket.Disconnect();
        },
        () =>
        {
          if (lastConnection != null)
          {
            var connection = lastConnection.In.Connect(lastConnection.Out, lastConnection.Guid);
            _connections.Add(connection.Guid, connection);
          }
        }, "disconnect sockets");
    }

    public GraphData Save()
    {
      SaveGraph();
      return _graphData;
    }

    public void GetNodes<T>(List<T> result, bool inherit = false) where T : Node
    {
      foreach (var node in _nodes.Values)
      {
        if (node.GetType() == typeof(T) || (inherit && node.GetType().IsSubclassOf(typeof(T))))
        {
          result.Add((T)node);
        }
      }
    }
  }
}
