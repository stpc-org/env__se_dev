// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyEmissiveBlock
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Game.Entities.Cube;
using System;
using VRage.Game;
using VRage.Network;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Entities
{
  [MyCubeBlockType(typeof (MyObjectBuilder_EmissiveBlock))]
  public class MyEmissiveBlock : MyCubeBlock
  {
    private const string EMISSIVE_PART = "EmissiveColorable";

    public event Action<MyEmissiveBlock> OnModelChanged;

    public void SetEmissivity(float emissivity) => this.SetEmissiveParts("EmissiveColorable", (Color) MyColorPickerConstants.HSVOffsetToHSV(this.Render.ColorMaskHsv).HsvToRgb(), emissivity);

    public override void OnModelChange()
    {
      base.OnModelChange();
      Action<MyEmissiveBlock> onModelChanged = this.OnModelChanged;
      if (onModelChanged == null)
        return;
      onModelChanged(this);
    }

    private class Sandbox_Game_Entities_MyEmissiveBlock\u003C\u003EActor : IActivator, IActivator<MyEmissiveBlock>
    {
      object IActivator.CreateInstance() => (object) new MyEmissiveBlock();

      MyEmissiveBlock IActivator<MyEmissiveBlock>.CreateInstance() => new MyEmissiveBlock();
    }
  }
}
