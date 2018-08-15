using Nodes.Editor.Core;
using UnityEditor;

namespace Nodes.Editor.Utils
{
  public class EditorNodeTicker
  {
    public static NodeTicker Create()
    {
      var ticker = new NodeTicker();
      EditorApplication.update += ticker.Tick;
      return ticker;
    }
  }
}
