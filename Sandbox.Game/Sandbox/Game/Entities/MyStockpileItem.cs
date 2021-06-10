// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyStockpileItem
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Game;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;

namespace Sandbox.Game.Entities
{
  [ProtoContract]
  public struct MyStockpileItem
  {
    [ProtoMember(1)]
    public int Amount;
    [ProtoMember(4)]
    [Serialize(MyObjectFlags.DefaultZero | MyObjectFlags.Dynamic, DynamicSerializerType = typeof (MyObjectBuilderDynamicSerializer))]
    public MyObjectBuilder_PhysicalObject Content;

    public override string ToString() => string.Format("{0}x {1}", (object) this.Amount, (object) this.Content.SubtypeName);

    protected class Sandbox_Game_Entities_MyStockpileItem\u003C\u003EAmount\u003C\u003EAccessor : IMemberAccessor<MyStockpileItem, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyStockpileItem owner, in int value) => owner.Amount = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyStockpileItem owner, out int value) => value = owner.Amount;
    }

    protected class Sandbox_Game_Entities_MyStockpileItem\u003C\u003EContent\u003C\u003EAccessor : IMemberAccessor<MyStockpileItem, MyObjectBuilder_PhysicalObject>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyStockpileItem owner, in MyObjectBuilder_PhysicalObject value) => owner.Content = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyStockpileItem owner, out MyObjectBuilder_PhysicalObject value) => value = owner.Content;
    }

    private class Sandbox_Game_Entities_MyStockpileItem\u003C\u003EActor : IActivator, IActivator<MyStockpileItem>
    {
      object IActivator.CreateInstance() => (object) new MyStockpileItem();

      MyStockpileItem IActivator<MyStockpileItem>.CreateInstance() => new MyStockpileItem();
    }
  }
}
