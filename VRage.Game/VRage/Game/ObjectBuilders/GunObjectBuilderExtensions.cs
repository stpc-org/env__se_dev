// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.GunObjectBuilderExtensions
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

namespace VRage.Game.ObjectBuilders
{
  public static class GunObjectBuilderExtensions
  {
    public static void InitializeDeviceBase<T>(
      this IMyObjectBuilder_GunObject<T> gunObjectBuilder,
      MyObjectBuilder_DeviceBase newBuilder)
      where T : MyObjectBuilder_DeviceBase
    {
      if (newBuilder.TypeId != typeof (T))
        return;
      gunObjectBuilder.DeviceBase = newBuilder;
    }

    public static T GetDevice<T>(
      this IMyObjectBuilder_GunObject<T> gunObjectBuilder)
      where T : MyObjectBuilder_DeviceBase
    {
      return gunObjectBuilder.DeviceBase as T;
    }
  }
}
