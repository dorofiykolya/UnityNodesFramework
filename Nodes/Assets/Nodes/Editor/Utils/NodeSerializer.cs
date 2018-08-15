using Nodes.Editor.Core;
using Nodes.Editor.Data;
using UnityEngine;

namespace Nodes.Editor.Utils
{
  public class NodeSerializer
  {
    public static NodeData Serialize(Node node)
    {
      var sockets = node.Sockets;
      var socketData = new SocketData[sockets.Length];

      var index = 0;
      foreach (var socket in sockets)
      {
        socketData[index] = new SocketData
        {
          Guid = new GuidData(socket.Guid),
          Type = socket.GetType(),
          Data = JsonUtility.ToJson(socket),
          Name = socket.Name
        };
        index++;
      }

      return new NodeData
      {
        Guid = new GuidData(node.Guid),
        Node = JsonUtility.ToJson(node),
        Type = node.GetType(),
        Sockets = socketData
      };

    }
  }
}
