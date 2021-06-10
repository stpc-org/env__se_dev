// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyFlareDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRageMath;
using VRageRender.Messages;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_FlareDefinition), null)]
  public class MyFlareDefinition : MyDefinitionBase
  {
    public float Intensity;
    public Vector2 Size;
    public MySubGlare[] SubGlares;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_FlareDefinition builderFlareDefinition = (MyObjectBuilder_FlareDefinition) builder;
      float? intensity = builderFlareDefinition.Intensity;
      this.Intensity = intensity.HasValue ? intensity.GetValueOrDefault() : 1f;
      this.Size = builderFlareDefinition.Size ?? new Vector2(1f, 1f);
      this.SubGlares = new MySubGlare[builderFlareDefinition.SubGlares.Length];
      int index = 0;
      foreach (MySubGlare subGlare in builderFlareDefinition.SubGlares)
      {
        this.SubGlares[index] = subGlare;
        this.SubGlares[index].Color = subGlare.Color.ToLinearRGB();
        ++index;
      }
    }

    private class Sandbox_Definitions_MyFlareDefinition\u003C\u003EActor : IActivator, IActivator<MyFlareDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyFlareDefinition();

      MyFlareDefinition IActivator<MyFlareDefinition>.CreateInstance() => new MyFlareDefinition();
    }
  }
}
