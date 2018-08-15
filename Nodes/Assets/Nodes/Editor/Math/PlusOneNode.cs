using Nodes.Editor.Core;
using UnityEngine;

namespace Nodes.Editor.Math
{
  [Tag("Math")]
  [Title("Math", Name = "Plus 1", Description = "int + 1")]
  public class PlusOneNode : Node
  {
    [Socket(Color = 0xFFFF0000)]
    public SocketIn<TypeMatcher<int>> In;

    [Socket(Color = 0xFF00FF00)]
    public SocketOut<TypeMatcher<int>> Out;

    [Inspector]
    [SerializeField]
    private bool _invert;

    public override void Calculate(ICalculateContext context)
    {
      int inValue = In.Connected ? In.GetValue<int>() : default(int);
      Out.SetValue(inValue + 1);
      context.Success();
    }
  }
}
