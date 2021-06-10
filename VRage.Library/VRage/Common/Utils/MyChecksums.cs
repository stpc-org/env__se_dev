// Decompiled with JetBrains decompiler
// Type: VRage.Common.Utils.MyChecksums
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Xml.Serialization;
using VRage.Serialization;

namespace VRage.Common.Utils
{
  public sealed class MyChecksums
  {
    private string m_publicKey;

    public string PublicKey
    {
      get => this.m_publicKey;
      set
      {
        this.m_publicKey = value;
        this.PublicKeyAsArray = Convert.FromBase64String(this.m_publicKey);
      }
    }

    public SerializableDictionaryHack<string, string> Items { get; set; }

    [XmlIgnore]
    public byte[] PublicKeyAsArray { get; private set; }

    public MyChecksums() => this.Items = new SerializableDictionaryHack<string, string>();
  }
}
