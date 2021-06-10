// Decompiled with JetBrains decompiler
// Type: VRage.Game.Definitions.MySafeZoneTexturesDefinition
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;
using VRage.Utils;
using VRageRender.Messages;

namespace VRage.Game.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_SafeZoneTexturesDefinition), null)]
  public class MySafeZoneTexturesDefinition : MyDefinitionBase
  {
    public MyTextureChange Texture;
    public MyStringHash DisplayTextId;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_SafeZoneTexturesDefinition texturesDefinition = (MyObjectBuilder_SafeZoneTexturesDefinition) builder;
      this.Texture = new MyTextureChange()
      {
        ColorMetalFileName = texturesDefinition.Alphamask,
        NormalGlossFileName = texturesDefinition.NormalGloss
      };
      this.DisplayTextId = MyStringHash.GetOrCompute(texturesDefinition.DisplayTextId);
    }

    private class VRage_Game_Definitions_MySafeZoneTexturesDefinition\u003C\u003EActor : IActivator, IActivator<MySafeZoneTexturesDefinition>
    {
      object IActivator.CreateInstance() => (object) new MySafeZoneTexturesDefinition();

      MySafeZoneTexturesDefinition IActivator<MySafeZoneTexturesDefinition>.CreateInstance() => new MySafeZoneTexturesDefinition();
    }
  }
}
