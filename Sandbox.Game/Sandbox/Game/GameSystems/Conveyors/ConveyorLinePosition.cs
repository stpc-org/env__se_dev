// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.Conveyors.ConveyorLinePosition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using VRageMath;

namespace Sandbox.Game.GameSystems.Conveyors
{
  public struct ConveyorLinePosition : IEquatable<ConveyorLinePosition>
  {
    public Vector3I LocalGridPosition;
    public Base6Directions.Direction Direction;

    public Vector3I VectorDirection => Base6Directions.GetIntVector(this.Direction);

    public Vector3I NeighbourGridPosition => this.LocalGridPosition + Base6Directions.GetIntVector(this.Direction);

    public ConveyorLinePosition(Vector3I gridPosition, Base6Directions.Direction direction)
    {
      this.LocalGridPosition = gridPosition;
      this.Direction = direction;
    }

    public ConveyorLinePosition GetConnectingPosition() => new ConveyorLinePosition(this.LocalGridPosition + this.VectorDirection, Base6Directions.GetFlippedDirection(this.Direction));

    public ConveyorLinePosition GetFlippedPosition() => new ConveyorLinePosition(this.LocalGridPosition, Base6Directions.GetFlippedDirection(this.Direction));

    public bool Equals(ConveyorLinePosition other) => this.LocalGridPosition == other.LocalGridPosition && this.Direction == other.Direction;

    public override int GetHashCode() => (((int) this.Direction * 397 ^ this.LocalGridPosition.X) * 397 ^ this.LocalGridPosition.Y) * 397 ^ this.LocalGridPosition.Z;

    public override string ToString() => this.LocalGridPosition.ToString() + " -> " + this.Direction.ToString();
  }
}
