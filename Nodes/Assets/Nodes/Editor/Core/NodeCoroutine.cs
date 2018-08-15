using System;
using System.Collections;
using UnityEngine;

namespace Nodes.Editor.Core
{
  public class NodeCoroutine
  {
    private readonly NodeTicker _ticker;

    public NodeCoroutine(NodeTicker ticker)
    {
      _ticker = ticker;
    }

    public IDisposable StartCoroutine(IEnumerator enumerator)
    {
      var keep = new KeepCoroutine();
      var coroutine = _ticker.StartCoroutine(enumerator);
      if (coroutine != null)
      {
        keep.Action = () => { _ticker.StopCoroutine(coroutine); };
      }
      return keep;
    }

    private class KeepCoroutine : IDisposable
    {
      public Action Action;

      public void Dispose()
      {
        if (Action != null)
        {
          Action();
        }
      }
    }
  }
}
