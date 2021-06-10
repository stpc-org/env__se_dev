// Decompiled with JetBrains decompiler
// Type: VRage.Game.ModAPI.ScriptingReflection
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;

namespace VRage.Game.ModAPI
{
  public class ScriptingReflection : IMyReflection
  {
    public Type BaseTypeOf(Type type) => type.BaseType;

    public Type[] GetInterfaces(Type type) => type.GetInterfaces();

    public bool IsAssignableFrom(Type baseType, Type derivedType) => baseType.IsAssignableFrom(derivedType);

    public bool IsInstanceOfType(Type type, object instance) => type.IsInstanceOfType(instance);
  }
}
