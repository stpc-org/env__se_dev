// Decompiled with JetBrains decompiler
// Type: VRage.Game.Voxels.StoragePin
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;

namespace VRage.Game.Voxels
{
  public struct StoragePin : IDisposable
  {
    private readonly IMyStorage m_storage;

    public bool Valid => this.m_storage != null;

    public StoragePin(IMyStorage storage) => this.m_storage = storage;

    public void Dispose()
    {
      if (this.m_storage == null)
        return;
      this.m_storage.Unpin();
    }
  }
}
