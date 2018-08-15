using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Nodes.Editor.Core
{
  public class NodeTicker
  {
    private readonly Dictionary<Ticker, IEnumerator> _map = new Dictionary<Ticker, IEnumerator>();

    public Ticker StartCoroutine(IEnumerator enumerator)
    {
      var ticker = new Ticker();
      _map[ticker] = enumerator;
      return ticker;
    }

    public void StopCoroutine(Ticker ticker)
    {
      _map.Remove(ticker);
    }

    public void Tick()
    {
      foreach (var pair in _map.ToList())
      {
        if (!pair.Value.MoveNext())
        {
          _map.Remove(pair.Key);
        }
      }
    }

    public class Ticker
    {
      private static int _index;

      public int Id { get; private set; }

      public Ticker()
      {
        Id = ++_index;
      }
    }
  }
}
