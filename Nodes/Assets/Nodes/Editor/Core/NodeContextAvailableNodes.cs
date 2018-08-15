using System;
using System.Collections.Generic;
using Nodes.Editor.Data;

namespace Nodes.Editor.Core
{
  public partial class NodeContext
  {
    public void GetAvailableNodes(Socket socket, List<Type> result)
    {
      if (socket.Type == SocketType.Input)
      {
        foreach (var availableType in _provider.AvailableTypes)
        {
          var node = _provider.GetDescription(availableType);
          var added = false;
          var sockets = node.GetSockets(SocketType.Output);
          foreach (var nodeSocket in sockets)
          {
            foreach (var type in nodeSocket.Matcher.Types)
            {
              if (socket.Match(type))
              {
                added = true;
                result.Add(availableType);
                break;
              }
            }
            if (added) break;
          }
        }
      }
      else if (socket.Type == SocketType.Output)
      {
        foreach (var availableType in _provider.AvailableTypes)
        {
          var node = _provider.GetDescription(availableType);
          var added = false;
          var sockets = node.GetSockets(SocketType.Input);
          foreach (var nodeSocket in sockets)
          {
            foreach (var type in nodeSocket.Matcher.Types)
            {
              if (nodeSocket.Matcher.Match(type))
              {
                added = true;
                result.Add(availableType);
                break;
              }
            }
            if (added) break;
          }
        }
      }
    }

    public void GetAvailableNodes(Socket socket, List<Node> result)
    {
      if (socket.Type == SocketType.Input)
      {
        foreach (var node in _nodes.Values)
        {
          var sockets = node.GetSockets(SocketType.Output);
          foreach (var nodeSocket in sockets)
          {
            var added = true;
            foreach (var type in nodeSocket.MatchTypes)
            {
              if (!socket.Match(type))
              {
                added = false;
                break;
              }
            }

            if (added)
            {
              result.Add(node);
            }
            if (added) break;
          }
        }
      }
      else if (socket.Type == SocketType.Output)
      {
        foreach (var node in _nodes.Values)
        {
          var sockets = node.GetSockets(SocketType.Input);
          foreach (var nodeSocket in sockets)
          {
            var added = true;
            foreach (var type in socket.MatchTypes)
            {
              if (!nodeSocket.Match(type))
              {
                added = false;
                break;
              }
            }

            if (added)
            {
              result.Add(node);
            }
            if (added) break;
          }
        }
      }
    }
  }
}
