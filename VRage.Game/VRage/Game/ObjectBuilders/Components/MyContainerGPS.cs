// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.Components.MyContainerGPS
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRage.Game.ObjectBuilders.Components
{
  [ProtoContract]
  public class MyContainerGPS
  {
    [ProtoMember(1)]
    public int TimeLeft;
    [ProtoMember(4)]
    public string GPSName;

    public MyContainerGPS()
    {
    }

    public MyContainerGPS(int time, string name)
    {
      this.TimeLeft = time;
      this.GPSName = name;
    }

    protected class VRage_Game_ObjectBuilders_Components_MyContainerGPS\u003C\u003ETimeLeft\u003C\u003EAccessor : IMemberAccessor<MyContainerGPS, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyContainerGPS owner, in int value) => owner.TimeLeft = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyContainerGPS owner, out int value) => value = owner.TimeLeft;
    }

    protected class VRage_Game_ObjectBuilders_Components_MyContainerGPS\u003C\u003EGPSName\u003C\u003EAccessor : IMemberAccessor<MyContainerGPS, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyContainerGPS owner, in string value) => owner.GPSName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyContainerGPS owner, out string value) => value = owner.GPSName;
    }

    private class VRage_Game_ObjectBuilders_Components_MyContainerGPS\u003C\u003EActor : IActivator, IActivator<MyContainerGPS>
    {
      object IActivator.CreateInstance() => (object) new MyContainerGPS();

      MyContainerGPS IActivator<MyContainerGPS>.CreateInstance() => new MyContainerGPS();
    }
  }
}
