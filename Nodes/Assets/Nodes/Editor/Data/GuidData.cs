using System;
using UnityEngine;

namespace Nodes.Editor.Data
{
  [Serializable]
  public class GuidData : ISerializationCallbackReceiver
  {
    [NonSerialized]
    private Guid _guid;

    [SerializeField]
    private string _guidSerialized;

    public GuidData()
    {
      _guid = Guid.NewGuid();
    }

    public GuidData(Guid guid)
    {
      _guid = guid;
    }

    public Guid Guid
    {
      get { return _guid; }
    }

    public virtual void OnBeforeSerialize()
    {
      _guidSerialized = _guid.ToString();
    }

    public virtual void OnAfterDeserialize()
    {
      if (!string.IsNullOrEmpty(_guidSerialized))
      {
        _guid = new Guid(_guidSerialized);
      }
      else
      {
        _guid = Guid.NewGuid();
      }
    }
  }
}
