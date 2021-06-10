// Decompiled with JetBrains decompiler
// Type: VRage.GameServices.ModMetadataFile
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;

namespace VRage.GameServices
{
  [XmlRoot("ModMetadata")]
  [Serializable]
  public class ModMetadataFile
  {
    [XmlElement("ModVersion")]
    public string ModVersion;
    [XmlElement("MinGameVersion")]
    public string MinGameVersion;
    [XmlElement("MaxGameVersion")]
    public string MaxGameVersion;

    protected class VRage_GameServices_ModMetadataFile\u003C\u003EModVersion\u003C\u003EAccessor : IMemberAccessor<ModMetadataFile, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref ModMetadataFile owner, in string value) => owner.ModVersion = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref ModMetadataFile owner, out string value) => value = owner.ModVersion;
    }

    protected class VRage_GameServices_ModMetadataFile\u003C\u003EMinGameVersion\u003C\u003EAccessor : IMemberAccessor<ModMetadataFile, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref ModMetadataFile owner, in string value) => owner.MinGameVersion = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref ModMetadataFile owner, out string value) => value = owner.MinGameVersion;
    }

    protected class VRage_GameServices_ModMetadataFile\u003C\u003EMaxGameVersion\u003C\u003EAccessor : IMemberAccessor<ModMetadataFile, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref ModMetadataFile owner, in string value) => owner.MaxGameVersion = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref ModMetadataFile owner, out string value) => value = owner.MaxGameVersion;
    }
  }
}
