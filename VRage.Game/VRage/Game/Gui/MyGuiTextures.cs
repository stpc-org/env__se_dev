// Decompiled with JetBrains decompiler
// Type: VRage.Game.GUI.MyGuiTextures
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ParallelTasks;
using System;
using System.Collections.Generic;
using System.IO;
using VRage.FileSystem;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Utils;
using VRageRender;
using VRageRender.Messages;

namespace VRage.Game.GUI
{
  public class MyGuiTextures
  {
    private readonly Dictionary<MyStringHash, MyObjectBuilder_GuiTexture> m_textures = new Dictionary<MyStringHash, MyObjectBuilder_GuiTexture>();
    private readonly Dictionary<MyStringHash, MyObjectBuilder_CompositeTexture> m_compositeTextures = new Dictionary<MyStringHash, MyObjectBuilder_CompositeTexture>();
    private List<string> m_texturesToUnload;
    private static MyGuiTextures m_instance;

    public static MyGuiTextures Static => MyGuiTextures.m_instance ?? (MyGuiTextures.m_instance = new MyGuiTextures());

    public void Reload(IEnumerable<string> texturesToPreload = null, bool preload = true)
    {
      this.m_textures.Clear();
      this.m_compositeTextures.Clear();
      if (MyRenderProxy.RenderThread == null)
        return;
      IEnumerable<MyGuiTextureAtlasDefinition> allDefinitions = MyDefinitionManagerBase.Static.GetAllDefinitions<MyGuiTextureAtlasDefinition>();
      if (texturesToPreload != null & preload)
        MyRenderProxy.PreloadTextures(texturesToPreload, TextureType.GUI);
      List<string> stringList = new List<string>();
      if (allDefinitions != null)
      {
        foreach (MyGuiTextureAtlasDefinition textureAtlasDefinition in allDefinitions)
        {
          foreach (KeyValuePair<MyStringHash, MyObjectBuilder_GuiTexture> texture in textureAtlasDefinition.Textures)
          {
            this.m_textures[texture.Key] = texture.Value;
            stringList.Add(texture.Value.Path);
          }
          foreach (KeyValuePair<MyStringHash, MyObjectBuilder_CompositeTexture> compositeTexture in textureAtlasDefinition.CompositeTextures)
            this.m_compositeTextures[compositeTexture.Key] = compositeTexture.Value;
        }
      }
      IEnumerable<string> files = MyFileSystem.GetFiles(Path.Combine(MyFileSystem.ContentPath, "textures\\gui\\icons"), "*", MySearchOption.TopDirectoryOnly);
      stringList.AddRange(files);
      if (preload)
        MyRenderProxy.PreloadTextures((IEnumerable<string>) stringList, TextureType.GUI);
      stringList.Clear();
      List<string> deferredTexturesToLoad = new List<string>();
      Parallel.Start((Action) (() =>
      {
        files = MyFileSystem.GetFiles(Path.Combine(MyFileSystem.ContentPath, "textures\\gui\\icons\\cubes"), "*", MySearchOption.TopDirectoryOnly);
        deferredTexturesToLoad.AddRange(files);
        files = MyFileSystem.GetFiles(Path.Combine(MyFileSystem.ContentPath, "textures\\gui\\icons\\component"), "*", MySearchOption.TopDirectoryOnly);
        deferredTexturesToLoad.AddRange(files);
      }), (Action) (() =>
      {
        if (!preload)
          return;
        MyRenderProxy.PreloadTextures((IEnumerable<string>) deferredTexturesToLoad, TextureType.GUI);
      }));
      files = MyFileSystem.GetFiles(Path.Combine(MyFileSystem.ContentPath, "customworlds"), "*.jpg", MySearchOption.AllDirectories);
      stringList.AddRange(files);
      files = MyFileSystem.GetFiles(Path.Combine(MyFileSystem.ContentPath, "scenarios"), "*.png", MySearchOption.AllDirectories);
      stringList.AddRange(files);
      if (preload)
        MyRenderProxy.PreloadTextures((IEnumerable<string>) stringList, TextureType.GUIWithoutPremultiplyAlpha);
      this.m_texturesToUnload = stringList;
    }

    public void Unload() => MyRenderProxy.DeprioritizeTextures(this.m_texturesToUnload);

    public MyObjectBuilder_GuiTexture GetTexture(MyStringHash hash)
    {
      MyObjectBuilder_GuiTexture builderGuiTexture = (MyObjectBuilder_GuiTexture) null;
      this.m_textures.TryGetValue(hash, out builderGuiTexture);
      return builderGuiTexture;
    }

    public MyObjectBuilder_CompositeTexture GetCompositeTexture(
      MyStringHash hash)
    {
      MyObjectBuilder_CompositeTexture compositeTexture = (MyObjectBuilder_CompositeTexture) null;
      this.m_compositeTextures.TryGetValue(hash, out compositeTexture);
      return compositeTexture;
    }

    public bool TryGetTexture(MyStringHash hash, out MyObjectBuilder_GuiTexture texture) => this.m_textures.TryGetValue(hash, out texture);

    public bool TryGetCompositeTexture(
      MyStringHash hash,
      out MyObjectBuilder_CompositeTexture texture)
    {
      return this.m_compositeTextures.TryGetValue(hash, out texture);
    }
  }
}
