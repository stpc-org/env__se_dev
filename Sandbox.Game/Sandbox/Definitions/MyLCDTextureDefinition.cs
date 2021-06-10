// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyLCDTextureDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_LCDTextureDefinition), null)]
  public class MyLCDTextureDefinition : MyDefinitionBase
  {
    public string TexturePath;
    public string SpritePath;
    public string LocalizationId;
    public bool Selectable;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      if (!(builder is MyObjectBuilder_LCDTextureDefinition textureDefinition))
        return;
      this.TexturePath = textureDefinition.TexturePath;
      this.SpritePath = textureDefinition.SpritePath;
      this.LocalizationId = textureDefinition.LocalizationId;
      this.Selectable = textureDefinition.Selectable;
    }

    private class Sandbox_Definitions_MyLCDTextureDefinition\u003C\u003EActor : IActivator, IActivator<MyLCDTextureDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyLCDTextureDefinition();

      MyLCDTextureDefinition IActivator<MyLCDTextureDefinition>.CreateInstance() => new MyLCDTextureDefinition();
    }
  }
}
