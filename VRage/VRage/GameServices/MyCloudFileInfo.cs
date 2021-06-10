// Decompiled with JetBrains decompiler
// Type: VRage.GameServices.MyCloudFileInfo
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

namespace VRage.GameServices
{
  public struct MyCloudFileInfo
  {
    public readonly string Name;
    public readonly string ContainerName;
    public readonly int Size;
    public readonly long Timestamp;

    public MyCloudFileInfo(string name, string containerName, int size, long timestamp)
    {
      this.Name = name;
      this.ContainerName = containerName;
      this.Size = size;
      this.Timestamp = timestamp;
    }
  }
}
