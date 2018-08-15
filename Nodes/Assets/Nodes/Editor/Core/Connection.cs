using System;

namespace Nodes.Editor.Core
{
  public class Connection
  {
    public Guid Guid { get; private set; }
    public SocketIn In { get; private set; }
    public SocketOut Out { get; private set; }

    public Connection(Guid guid, SocketIn inSocket, SocketOut outSocket)
    {
      Guid = guid;
      In = inSocket;
      Out = outSocket;
    }
  }
}
