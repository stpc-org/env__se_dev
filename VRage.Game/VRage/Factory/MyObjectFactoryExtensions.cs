// Decompiled with JetBrains decompiler
// Type: VRage.Factory.MyObjectFactoryExtensions
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using VRage.Core;
using VRage.Game.Common;
using VRage.ObjectBuilders;

namespace VRage.Factory
{
  public static class MyObjectFactoryExtensions
  {
    public static TCreated CreateAndDeserialize<TAttribute, TCreated>(
      this MyObjectFactory<TAttribute, TCreated> self,
      MyObjectBuilder_Base builder)
      where TAttribute : MyFactoryTagAttribute
      where TCreated : class, IMyObject
    {
      TCreated instance = self.CreateInstance(builder.TypeId);
      instance.Deserialize(builder);
      return instance;
    }
  }
}
