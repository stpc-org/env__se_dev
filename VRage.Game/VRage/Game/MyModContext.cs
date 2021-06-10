// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyModContext
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.IO;
using System.Xml.Serialization;
using VRage.FileSystem;
using VRage.Game.ModAPI;

namespace VRage.Game
{
  public class MyModContext : IEquatable<MyModContext>, IMyModContext
  {
    private static MyModContext m_baseContext;
    private static MyModContext m_unknownContext;
    public string CurrentFile;

    public static MyModContext BaseGame
    {
      get
      {
        if (MyModContext.m_baseContext == null)
          MyModContext.InitBaseModContext();
        return MyModContext.m_baseContext;
      }
    }

    public static MyModContext UnknownContext
    {
      get
      {
        if (MyModContext.m_unknownContext == null)
          MyModContext.InitUnknownModContext();
        return MyModContext.m_unknownContext;
      }
    }

    [XmlIgnore]
    public string ModName { get; private set; }

    [XmlIgnore]
    public string ModId { get; private set; }

    [XmlIgnore]
    public string ModServiceName { get; private set; }

    [XmlIgnore]
    public string ModPath { get; private set; }

    [XmlIgnore]
    public string ModPathData { get; private set; }

    public void Init(MyObjectBuilder_Checkpoint.ModItem modItem)
    {
      this.ModName = modItem.FriendlyName;
      this.ModId = modItem.Name;
      this.ModServiceName = modItem.PublishedServiceName;
      this.ModPath = modItem.GetPath();
      this.ModPathData = Path.Combine(this.ModPath, "Data");
    }

    public void Init(MyModContext context)
    {
      this.ModName = context.ModName;
      this.ModPath = context.ModPath;
      this.ModId = context.ModId;
      this.ModServiceName = context.ModServiceName;
      this.ModPathData = context.ModPathData;
      this.CurrentFile = context.CurrentFile;
    }

    public void Init(string modName, string fileName, string modPath = null)
    {
      this.ModName = modName;
      this.ModPath = modPath;
      this.ModPathData = modPath != null ? Path.Combine(modPath, "Data") : (string) null;
      this.CurrentFile = fileName;
    }

    public bool IsBaseGame => MyModContext.m_baseContext != null && this.ModName == MyModContext.m_baseContext.ModName && this.ModPath == MyModContext.m_baseContext.ModPath && this.ModPathData == MyModContext.m_baseContext.ModPathData;

    private static void InitBaseModContext()
    {
      MyModContext.m_baseContext = new MyModContext();
      MyModContext.m_baseContext.ModName = (string) null;
      MyModContext.m_baseContext.ModPath = MyFileSystem.ContentPath;
      MyModContext.m_baseContext.ModPathData = Path.Combine(MyModContext.m_baseContext.ModPath, "Data");
    }

    private static void InitUnknownModContext()
    {
      MyModContext.m_unknownContext = new MyModContext();
      MyModContext.m_unknownContext.ModName = "Unknown MOD";
      MyModContext.m_unknownContext.ModPath = (string) null;
      MyModContext.m_unknownContext.ModPathData = (string) null;
    }

    public bool Equals(MyModContext other) => this.ModName == other.ModName && this.ModPath == other.ModPath;

    public override int GetHashCode() => this.ModPath.GetHashCode() ^ (this.ModName != null ? this.ModName.GetHashCode() : 0);
  }
}
