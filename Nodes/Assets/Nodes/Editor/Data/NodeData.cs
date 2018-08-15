using System;
using UnityEngine;

namespace Nodes.Editor.Data
{
  [Serializable]
  public class NodeData
  {
    private Type _type;
    [SerializeField]
    private string _typeSerialized;

    public string Node;
    public Type Type
    {
      get
      {
        if (_type == null)
        {
          foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
          {
            _type = assembly.GetType(_typeSerialized);
            if (_type != null)
            {
              break;
            }
          }
        }

        return _type;
      }
      set
      {
        _type = value;
        _typeSerialized = value.FullName;
      }
    }

    public GuidData Guid;
    public SocketData[] Sockets;
  }
}
