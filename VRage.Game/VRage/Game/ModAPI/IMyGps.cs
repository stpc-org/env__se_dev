// Decompiled with JetBrains decompiler
// Type: VRage.Game.ModAPI.IMyGps
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using VRageMath;

namespace VRage.Game.ModAPI
{
  public interface IMyGps
  {
    int Hash { get; }

    void UpdateHash();

    string Name { get; set; }

    string Description { get; set; }

    Vector3D Coords { get; set; }

    Color GPSColor { get; set; }

    bool ShowOnHud { get; set; }

    TimeSpan? DiscardAt { get; set; }

    string ToString();

    string ContainerRemainingTime { get; set; }
  }
}
