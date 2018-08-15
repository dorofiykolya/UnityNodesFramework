using System;
using System.Collections;
using System.Collections.Generic;

namespace Nodes.Editor.Core
{
  public interface ICalculateContext
  {
    void Success();
    void Fail();
    IDisposable StartCoroutine(IEnumerator enumerator);
  }

  public class CalculateContext : ICalculateContext
  {
    private readonly NodeCoroutine _coroutine;
    private readonly CalculateContext _parent;
    private readonly Node _node;
    private readonly List<CalculateContext> _children;
    private CalculateContextStatus _status;

    public CalculateContext(NodeCoroutine coroutine, CalculateContext parent, Node node)
    {
      _children = new List<CalculateContext>();
      _coroutine = coroutine;
      _parent = parent;
      _node = node;
      if (_parent != null)
      {
        _parent._children.Add(this);
      }
    }

    public CalculateContext Parent
    {
      get { return _parent; }
    }

    public Node Node
    {
      get { return _node; }
    }

    public bool IsFail
    {
      get
      {
        if (_status == CalculateContextStatus.Fail) return true;
        foreach (var child in _children)
        {
          if (child.IsFail) return true;
        }

        return false;
      }
    }

    public void Process()
    {
      if (_status != CalculateContextStatus.None) throw new InvalidOperationException();
      _status = CalculateContextStatus.InProcess;
    }

    public void Success()
    {
      if (_status != CalculateContextStatus.InProcess) throw new InvalidOperationException();
      _status = CalculateContextStatus.Success;
    }

    public void Fail()
    {
      if (_status != CalculateContextStatus.InProcess) throw new InvalidOperationException();
      _status = CalculateContextStatus.Fail;
    }

    public IDisposable StartCoroutine(IEnumerator enumerator)
    {
      return _coroutine.StartCoroutine(enumerator);
    }
  }
}
