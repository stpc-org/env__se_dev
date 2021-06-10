// Decompiled with JetBrains decompiler
// Type: VRage.Game.Definitions.MyGuiTextureAtlasDefinition
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System.Collections.Generic;
using System.IO;
using VRage.FileSystem;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Library.Filesystem;
using VRage.Library.Utils;
using VRage.Network;
using VRage.Utils;

namespace VRage.Game.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_GuiTextureAtlasDefinition), null)]
  public class MyGuiTextureAtlasDefinition : MyDefinitionBase
  {
    public readonly Dictionary<MyStringHash, MyObjectBuilder_GuiTexture> Textures = new Dictionary<MyStringHash, MyObjectBuilder_GuiTexture>((IEqualityComparer<MyStringHash>) MyStringHash.Comparer);
    public readonly Dictionary<MyStringHash, MyObjectBuilder_CompositeTexture> CompositeTextures = new Dictionary<MyStringHash, MyObjectBuilder_CompositeTexture>((IEqualityComparer<MyStringHash>) MyStringHash.Comparer);

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_GuiTextureAtlasDefinition textureAtlasDefinition = builder as MyObjectBuilder_GuiTextureAtlasDefinition;
      this.Textures.Clear();
      this.CompositeTextures.Clear();
      foreach (MyObjectBuilder_GuiTexture texture in textureAtlasDefinition.Textures)
      {
        bool flag = true;
        if (this.Context.IsBaseGame && ContentIndex.TryGetImageSize(texture.Path, out texture.SizePx.X, out texture.SizePx.Y))
        {
          texture.Path = Path.Combine(MyFileSystem.ContentPath, texture.Path);
          this.Textures.Add(texture.SubtypeId, texture);
          flag = false;
        }
        else
        {
          if (this.Context.IsBaseGame)
            texture.Path = Path.Combine(MyFileSystem.ContentPath, texture.Path);
          string lower = texture.Path.ToLower();
          if (lower.EndsWith(".dds"))
          {
            MyImageHeaderUtils.DDS_HEADER header;
            if (MyImageHeaderUtils.Read_DDS_HeaderData(texture.Path, out header))
            {
              texture.SizePx.X = (int) header.dwWidth;
              texture.SizePx.Y = (int) header.dwHeight;
              this.Textures.Add(texture.SubtypeId, texture);
              flag = false;
            }
          }
          else if (lower.EndsWith(".png"))
          {
            if (MyImageHeaderUtils.Read_PNG_Dimensions(texture.Path, out texture.SizePx.X, out texture.SizePx.Y))
            {
              this.Textures.Add(texture.SubtypeId, texture);
              flag = false;
            }
          }
          else
          {
            MyLog.Default.WriteLine("GuiTextures.sbc");
            MyLog.Default.WriteLine("Unsupported texture format! Texture: " + texture.Path);
          }
        }
        if (flag)
        {
          MyLog.Default.WriteLine("GuiTextures.sbc");
          MyLog.Default.WriteLine("Failed to parse texture header! Texture: " + texture.Path);
        }
      }
      foreach (MyObjectBuilder_CompositeTexture compositeTexture in textureAtlasDefinition.CompositeTextures)
        this.CompositeTextures.Add(compositeTexture.SubtypeId, compositeTexture);
    }

    private class VRage_Game_Definitions_MyGuiTextureAtlasDefinition\u003C\u003EActor : IActivator, IActivator<MyGuiTextureAtlasDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyGuiTextureAtlasDefinition();

      MyGuiTextureAtlasDefinition IActivator<MyGuiTextureAtlasDefinition>.CreateInstance() => new MyGuiTextureAtlasDefinition();
    }
  }
}
