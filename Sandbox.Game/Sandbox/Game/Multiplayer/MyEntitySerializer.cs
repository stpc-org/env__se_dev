// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Multiplayer.MyEntitySerializer
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using VRage;
using VRage.Game.Entity;
using VRage.Serialization;

namespace Sandbox.Game.Multiplayer
{
  internal class MyEntitySerializer : ISerializer<MyEntity>
  {
    public static readonly MyEntitySerializer Default = new MyEntitySerializer();

    void ISerializer<MyEntity>.Serialize(
      ByteStream destination,
      ref MyEntity data)
    {
      long entityId = data.EntityId;
      BlitSerializer<long>.Default.Serialize(destination, ref entityId);
    }

    void ISerializer<MyEntity>.Deserialize(ByteStream source, out MyEntity data)
    {
      long data1;
      BlitSerializer<long>.Default.Deserialize(source, out data1);
      MyEntities.TryGetEntityById(data1, out data);
    }
  }
}
