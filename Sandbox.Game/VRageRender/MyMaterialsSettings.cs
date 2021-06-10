// Decompiled with JetBrains decompiler
// Type: VRageRender.MyMaterialsSettings
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using System.Runtime.InteropServices;
using System.Xml.Serialization;
using VRage;

namespace VRageRender
{
  [Obsolete]
  public class MyMaterialsSettings
  {
    [XmlElement(Type = typeof (MyStructXmlSerializer<MyMaterialsSettings.Struct>))]
    public MyMaterialsSettings.Struct Data = MyMaterialsSettings.Struct.Default;
    private MyMaterialsSettings.MyChangeableMaterial[] m_changeableMaterials;

    [XmlArrayItem("ChangeableMaterial")]
    public MyMaterialsSettings.MyChangeableMaterial[] ChangeableMaterials
    {
      get => this.m_changeableMaterials;
      set
      {
        if (this.m_changeableMaterials.Length != value.Length)
          this.m_changeableMaterials = new MyMaterialsSettings.MyChangeableMaterial[value.Length];
        value.CopyTo((Array) this.m_changeableMaterials, 0);
      }
    }

    public MyMaterialsSettings() => this.m_changeableMaterials = new MyMaterialsSettings.MyChangeableMaterial[0];

    public void CopyFrom(MyMaterialsSettings settings) => this.ChangeableMaterials = settings.ChangeableMaterials;

    [StructLayout(LayoutKind.Sequential, Size = 1)]
    public struct Struct
    {
      public static MyMaterialsSettings.Struct Default;
    }

    public struct MyChangeableMaterial
    {
      public string MaterialName;
    }
  }
}
