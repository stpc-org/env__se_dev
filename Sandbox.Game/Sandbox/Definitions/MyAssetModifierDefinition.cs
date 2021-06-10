// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyAssetModifierDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRageMath;
using VRageRender.Messages;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_AssetModifierDefinition), null)]
  public class MyAssetModifierDefinition : MyDefinitionBase
  {
    public List<MyObjectBuilder_AssetModifierDefinition.MyAssetTexture> Textures;
    public bool MetalnessColorable;
    public Color? DefaultColor;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_AssetModifierDefinition modifierDefinition = builder as MyObjectBuilder_AssetModifierDefinition;
      this.Textures = modifierDefinition.Textures;
      this.MetalnessColorable = modifierDefinition.MetalnessColorable;
      this.DefaultColor = modifierDefinition.DefaultColor;
    }

    public string GetFilepath(string location, MyTextureType type)
    {
      foreach (MyObjectBuilder_AssetModifierDefinition.MyAssetTexture texture in this.Textures)
      {
        if (texture.Location == location && texture.Type == type)
          return texture.Filepath;
      }
      return (string) null;
    }

    private class Sandbox_Definitions_MyAssetModifierDefinition\u003C\u003EActor : IActivator, IActivator<MyAssetModifierDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyAssetModifierDefinition();

      MyAssetModifierDefinition IActivator<MyAssetModifierDefinition>.CreateInstance() => new MyAssetModifierDefinition();
    }
  }
}
