using System;
using System.Collections.Generic;

namespace Nodes.Editor.Core
{
  public class ActionsHistory
  {
    private readonly Stack<HistoryItem> _undo = new Stack<HistoryItem>();
    private readonly Stack<HistoryItem> _redo = new Stack<HistoryItem>();

    public event Action Change;

    public void Execute(Action redo, Action undo, string description)
    {
      _undo.Push(new HistoryItem(redo, undo, description));
      _redo.Clear();
      redo();
      if (Change != null) Change();
    }

    public IEnumerable<HistoryItem> UndoActions { get { return _undo; } }
    public IEnumerable<HistoryItem> RedoActions { get { return _redo; } }

    public string UndoDescription { get { return CanUndo ? _undo.Peek().Description : string.Empty; } }
    public string RedoDescription { get { return CanRedo ? _redo.Peek().Description : string.Empty; } }

    public bool CanUndo { get { return _undo.Count != 0; } }
    public bool CanRedo { get { return _redo.Count != 0; } }

    public void Undo()
    {
      if (CanUndo)
      {
        var item = _undo.Pop();
        _redo.Push(item);
        item.Undo();
        if (Change != null) Change();
      }
    }

    public void Redo()
    {
      if (CanRedo)
      {
        var item = _redo.Pop();
        _undo.Push(item);
        item.Redo();
        if (Change != null) Change();
      }
    }

    public void Dispose()
    {
      Clear();
    }

    public void Clear()
    {
      if (Change != null)
      {
        foreach (var @delegate in Change.GetInvocationList())
        {
          Change -= (Action)@delegate;
        }
      }
    }

    public class HistoryItem
    {
      private readonly Action _undo;
      private readonly Action _redo;
      private readonly string _description;

      public HistoryItem(Action redo, Action undo, string description)
      {
        _undo = undo;
        _redo = redo;
        _description = description;
      }

      public Action Undo { get { return _undo; } }
      public Action Redo { get { return _redo; } }
      public string Description { get { return _description; } }
    }
  }
}
