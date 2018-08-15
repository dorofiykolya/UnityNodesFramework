using Nodes.Editor.Core;
using UnityEngine;

namespace Nodes.Editor.Math
{
  [Tag("Math")]
  [Title("Math")]
  public class MasterNode : Node
  {
    [Socket(Color = 0xFFFF0000)]
    public SocketIn<TypeMatcher<int>> In;

    [SerializeField]
    public int Result;

    public override void Calculate(ICalculateContext context)
    {
      Result = In.Connected ? In.GetValue<int>() : Result;
      context.Success();
    }
  }
}
