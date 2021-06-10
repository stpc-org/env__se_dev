// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiScreenDebugSbs
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using VRage;
using VRage.FileSystem;
using VRage.Game;
using VRage.Network;
using VRage.ObjectBuilders;
using VRageMath;

namespace Sandbox.Game.Gui
{
  [MyDebugScreen("Game", "Sbs")]
  [StaticEventOwner]
  internal class MyGuiScreenDebugSbs : MyGuiScreenDebugBase
  {
    private const float TWO_BUTTON_XOFFSET = 0.05f;

    public MyGuiScreenDebugSbs()
      : base()
      => this.RecreateControls(true);

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.m_scale = 0.7f;
      this.AddCaption("Sbs Recreating", new Vector4?(Color.Yellow.ToVector4()));
      this.AddShareFocusHint();
      this.m_currentPosition = -this.m_size.Value / 2f + new Vector2(0.02f, 0.1f);
      this.AddVerticalSpacing(0.01f * this.m_scale);
      this.AddButton("Generate World sbsB5", (Action<MyGuiControlButton>) (x => MyGuiScreenDebugSbs.RegenerateWorlds(false)));
      this.AddButton("ReGenerate World sbsB5", (Action<MyGuiControlButton>) (x => MyGuiScreenDebugSbs.RegenerateWorlds(true)));
      this.AddVerticalSpacing();
      this.AddButton("ReGenerate Prefab sbsB5", (Action<MyGuiControlButton>) (x => MyGuiScreenDebugSbs.RegeneratePrefabs(true)));
    }

    public static void RegenerateWorlds(bool delete)
    {
      HashSet<string> task = new HashSet<string>();
      MyGuiScreenDebugSbs.AddWorldToTask(Path.Combine(MyFileSystem.ContentPath, "InventoryScenes"), task, delete);
      MyGuiScreenDebugSbs.AddWorldToTask(Path.Combine(MyFileSystem.ContentPath, "Scenarios"), task, delete);
      MyGuiScreenDebugSbs.AddWorldToTask(Path.Combine(MyFileSystem.ContentPath, "CustomWorlds"), task, delete);
      Parallel.ForEach<string>((IEnumerable<string>) task, (Action<string>) (file =>
      {
        ulong fileSize = 0;
        string path = file + "B5";
        MyObjectBuilder_Sector objectBuilder = (MyObjectBuilder_Sector) null;
        MyObjectBuilderSerializer.DeserializeXML<MyObjectBuilder_Sector>(file, out objectBuilder, out fileSize);
        MyObjectBuilderSerializer.SerializePB(path, false, (MyObjectBuilder_Base) objectBuilder);
        MyDebug.WriteLine("File saved - " + path, "E:\\Repo3\\Sources\\Sandbox.Game\\Game\\Screens\\DebugScreens\\Game\\MyGuiScreenDebugSbs.cs", 66);
      }));
    }

    public static void RegeneratePrefabs(bool delete)
    {
      IEnumerable<string> files = MyFileSystem.GetFiles(Path.Combine(MyFileSystem.ContentPath, "Data"), "*.sbc", MySearchOption.AllDirectories);
      int count = 0;
      Action<string> body = (Action<string>) (file =>
      {
        MyObjectBuilder_Definitions builder = MyGuiScreenDebugSbs.CheckPrefabs(file);
        if (builder == null)
          return;
        try
        {
          MyGuiScreenDebugSbs.RebuildPrefabs(builder);
          MyDebug.WriteLine("File saved - " + file + "B5", "E:\\Repo3\\Sources\\Sandbox.Game\\Game\\Screens\\DebugScreens\\Game\\MyGuiScreenDebugSbs.cs", 83);
          Interlocked.Increment(ref count);
        }
        catch (Exception ex)
        {
        }
      });
      Parallel.ForEach<string>(files, body);
    }

    private static void AddWorldToTask(string topDirectory, HashSet<string> task, bool delete)
    {
      foreach (string file in MyFileSystem.GetFiles(topDirectory, "*.sbs", MySearchOption.AllDirectories))
      {
        if (!file.EndsWith("B5"))
        {
          string path = file + "B5";
          if (MyFileSystem.FileExists(path))
          {
            if (delete)
            {
              File.Delete(path);
              task.Add(file);
            }
          }
          else
            task.Add(file);
        }
      }
    }

    private static void RebuildPrefabs(MyObjectBuilder_Definitions builder)
    {
      foreach (MyObjectBuilder_PrefabDefinition prefab1 in builder.Prefabs)
      {
        string path = prefab1.PrefabPath + "B5";
        if (MyFileSystem.FileExists(path))
          File.Delete(path);
        MyObjectBuilder_Definitions builderDefinitions = MyGuiScreenDebugSbs.LoadWithProtobuffers<MyObjectBuilder_Definitions>(prefab1.PrefabPath);
        foreach (MyObjectBuilder_PrefabDefinition prefab2 in builderDefinitions.Prefabs)
        {
          if (prefab2.CubeGrid != null)
            prefab2.CubeGrids = new MyObjectBuilder_CubeGrid[1]
            {
              prefab2.CubeGrid
            };
        }
        MyObjectBuilderSerializer.SerializePB(path, false, (MyObjectBuilder_Base) builderDefinitions);
      }
    }

    private static void CheckXmlForPrefabs(
      string file,
      ref List<MyObjectBuilder_PrefabDefinition> prefabs,
      Stream readStream)
    {
      using (XmlReader reader = XmlReader.Create(readStream))
      {
        while (reader.Read())
        {
          if (reader.IsStartElement())
          {
            if (reader.Name == "SpawnGroups")
              break;
            if (reader.Name == "Prefabs")
            {
              prefabs = new List<MyObjectBuilder_PrefabDefinition>();
              while (reader.ReadToFollowing("Prefab"))
                MyGuiScreenDebugSbs.ReadPrefabHeader(file, ref prefabs, reader);
              break;
            }
          }
        }
      }
    }

    private static void ReadPrefabHeader(
      string file,
      ref List<MyObjectBuilder_PrefabDefinition> prefabs,
      XmlReader reader)
    {
      MyObjectBuilder_PrefabDefinition prefabDefinition = new MyObjectBuilder_PrefabDefinition();
      prefabDefinition.PrefabPath = file;
      reader.ReadToFollowing("Id");
      bool flag = false;
      if (reader.AttributeCount >= 2)
      {
        for (int i = 0; i < reader.AttributeCount; ++i)
        {
          reader.MoveToAttribute(i);
          string name = reader.Name;
          if (!(name == "Type"))
          {
            if (name == "Subtype")
              prefabDefinition.Id.SubtypeId = reader.Value;
          }
          else
          {
            prefabDefinition.Id.TypeIdString = reader.Value;
            flag = true;
          }
        }
      }
      if (!flag)
      {
        while (reader.Read())
        {
          if (reader.IsStartElement())
          {
            string name = reader.Name;
            if (!(name == "TypeId"))
            {
              if (name == "SubtypeId")
              {
                reader.Read();
                prefabDefinition.Id.SubtypeId = reader.Value;
              }
            }
            else
            {
              reader.Read();
              prefabDefinition.Id.TypeIdString = reader.Value;
            }
          }
          else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "Id")
            break;
        }
      }
      prefabs.Add(prefabDefinition);
    }

    private static MyObjectBuilder_Definitions CheckPrefabs(string file)
    {
      List<MyObjectBuilder_PrefabDefinition> prefabs = (List<MyObjectBuilder_PrefabDefinition>) null;
      using (Stream stream = MyFileSystem.OpenRead(file))
      {
        if (stream != null)
        {
          using (Stream readStream = stream.UnwrapGZip())
          {
            if (readStream != null)
              MyGuiScreenDebugSbs.CheckXmlForPrefabs(file, ref prefabs, readStream);
          }
        }
      }
      MyObjectBuilder_Definitions builderDefinitions = (MyObjectBuilder_Definitions) null;
      if (prefabs != null)
      {
        builderDefinitions = new MyObjectBuilder_Definitions();
        builderDefinitions.Prefabs = prefabs.ToArray();
      }
      return builderDefinitions;
    }

    private static T LoadWithProtobuffers<T>(string path) where T : MyObjectBuilder_Base
    {
      T objectBuilder = default (T);
      MyObjectBuilderSerializer.DeserializeXML<T>(path, out objectBuilder);
      return objectBuilder;
    }
  }
}
