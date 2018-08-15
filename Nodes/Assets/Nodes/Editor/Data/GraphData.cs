using System;

namespace Nodes.Editor.Data
{
  [Serializable]
  public class GraphData
  {
    public NodeData[] Nodes;
    public ConnectionData[] Connections;
  }
}
