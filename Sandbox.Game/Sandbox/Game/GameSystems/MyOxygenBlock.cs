// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.MyOxygenBlock
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Xml.Serialization;
using VRageMath;

namespace Sandbox.Game.GameSystems
{
  public class MyOxygenBlock
  {
    [XmlIgnore]
    public MyOxygenRoomLink RoomLink { get; set; }

    public float PreviousOxygenAmount { get; set; }

    public int OxygenChangeTime { get; set; }

    public MyOxygenRoom Room => this.RoomLink == null ? (MyOxygenRoom) null : this.RoomLink.Room;

    public MyOxygenBlock()
    {
    }

    public MyOxygenBlock(MyOxygenRoomLink roomPointer) => this.RoomLink = roomPointer;

    internal float OxygenAmount()
    {
      if (this.Room == null)
        return 0.0f;
      float num = this.Room.IsAirtight ? this.Room.OxygenAmount / (float) this.Room.BlockCount : this.Room.EnvironmentOxygen;
      float amount = (float) (MySandboxGame.TotalGamePlayTimeInMilliseconds - this.OxygenChangeTime) / 1500f;
      if ((double) amount > 1.0)
        amount = 1f;
      return MathHelper.Lerp(this.PreviousOxygenAmount, num, amount);
    }

    public float OxygenLevel(float gridSize) => this.OxygenAmount() / (gridSize * gridSize * gridSize);

    public override string ToString() => "MyOxygenBlock - Oxygen: " + (object) this.OxygenAmount() + "/" + (object) this.PreviousOxygenAmount;
  }
}
