using System;

namespace Nodes.Editor.Data
{
  [Serializable]
  public class ConnectionData
  {
    public GuidData Guid;
    public GuidData In;
    public GuidData Out;
  }
}
