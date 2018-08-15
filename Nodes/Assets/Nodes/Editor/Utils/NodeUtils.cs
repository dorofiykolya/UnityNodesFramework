using Nodes.Editor.Core;
using Nodes.Editor.Data;

namespace Nodes.Editor.Utils
{
  public static class NodeUtils
  {
    public static bool CanConnect(Socket first, Socket second)
    {
      if (first == second) return false;
      if (first.Type == second.Type) return false;
      if (first.Type == SocketType.None) return false;
      if (second.Type == SocketType.None) return false;


      return true;
    }
  }
}
