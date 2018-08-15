using Nodes.Editor.Core;

namespace Nodes.Editor.Math
{
  [Tag("Math")]
  [Title("Math")]
  public class InputValueNode : Node
  {
    [Socket]
    public SocketOut<TypeMatcher<int>> Out;

    public int Value;

    public override void Calculate(ICalculateContext context)
    {
      Out.SetValue(Value);
      context.Success();
    }
  }
}
