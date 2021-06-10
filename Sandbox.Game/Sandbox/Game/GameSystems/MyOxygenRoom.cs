// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.MyOxygenRoom
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using VRageMath;

namespace Sandbox.Game.GameSystems
{
  public class MyOxygenRoom
  {
    public int Index { get; set; }

    public bool IsAirtight { get; set; }

    public float EnvironmentOxygen { get; set; }

    public float OxygenAmount { get; set; }

    public int BlockCount { get; set; }

    public int DepressurizationTime { get; set; }

    [XmlIgnore]
    public MyOxygenRoomLink Link { get; set; }

    public bool IsDirty { get; set; }

    public HashSet<Vector3I> Blocks { get; set; }

    public Vector3I StartingPosition { get; set; }

    public MyOxygenRoom() => this.IsAirtight = true;

    public MyOxygenRoom(int index)
    {
      this.IsAirtight = true;
      this.EnvironmentOxygen = 0.0f;
      this.Index = index;
      this.OxygenAmount = 0.0f;
      this.BlockCount = 0;
      this.DepressurizationTime = 0;
    }

    public float OxygenLevel(float gridSize) => this.OxygenAmount / this.MaxOxygen(gridSize);

    public float MissingOxygen(float gridSize) => (float) Math.Max((double) this.MaxOxygen(gridSize) - (double) this.OxygenAmount, 0.0);

    public float MaxOxygen(float gridSize) => (float) this.BlockCount * gridSize * gridSize * gridSize;
  }
}
