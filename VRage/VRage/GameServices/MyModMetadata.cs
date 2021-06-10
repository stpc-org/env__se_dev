// Decompiled with JetBrains decompiler
// Type: VRage.GameServices.MyModMetadata
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace VRage.GameServices
{
  public class MyModMetadata
  {
    public Version ModVersion;
    public Version MinGameVersion;
    public Version MaxGameVersion;

    public override string ToString() => string.Format("ModVersion: {0}, MinGameVersion: {1}, MaxGameVersion: {2}", this.ModVersion != (Version) null ? (object) this.ModVersion.ToString() : (object) "N/A", this.MinGameVersion != (Version) null ? (object) this.MinGameVersion.ToString() : (object) "N/A", this.MaxGameVersion != (Version) null ? (object) this.MaxGameVersion.ToString() : (object) "N/A");

    public static implicit operator MyModMetadata(ModMetadataFile file)
    {
      if (file == null)
        return (MyModMetadata) null;
      MyModMetadata myModMetadata = new MyModMetadata();
      Version.TryParse(file.ModVersion, out myModMetadata.ModVersion);
      if (file.MinGameVersion != null)
      {
        string[] strArray = file.MinGameVersion.Split('.');
        file.MinGameVersion = string.Join(".", ((IEnumerable<string>) strArray).Take<string>(3));
        Version.TryParse(file.MinGameVersion, out myModMetadata.MinGameVersion);
      }
      else
        myModMetadata.MinGameVersion = (Version) null;
      if (file.MaxGameVersion != null)
      {
        string[] strArray = file.MaxGameVersion.Split('.');
        file.MaxGameVersion = string.Join(".", ((IEnumerable<string>) strArray).Take<string>(3));
        Version.TryParse(file.MaxGameVersion, out myModMetadata.MaxGameVersion);
      }
      else
        myModMetadata.MaxGameVersion = (Version) null;
      return myModMetadata;
    }

    public static implicit operator ModMetadataFile(MyModMetadata metadata)
    {
      if (metadata == null)
        return (ModMetadataFile) null;
      ModMetadataFile modMetadataFile = new ModMetadataFile();
      if (metadata.ModVersion != (Version) null)
        modMetadataFile.ModVersion = metadata.ModVersion.ToString();
      if (metadata.MinGameVersion != (Version) null)
        modMetadataFile.MinGameVersion = metadata.MinGameVersion.ToString();
      if (metadata.MaxGameVersion != (Version) null)
        modMetadataFile.MaxGameVersion = metadata.MaxGameVersion.ToString();
      return modMetadataFile;
    }
  }
}
