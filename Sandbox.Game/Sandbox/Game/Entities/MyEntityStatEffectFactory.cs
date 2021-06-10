// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyEntityStatEffectFactory
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using System.Reflection;
using VRage.Game.ObjectBuilders;
using VRage.ObjectBuilders;

namespace Sandbox.Game.Entities
{
  internal static class MyEntityStatEffectFactory
  {
    private static MyObjectFactory<MyEntityStatEffectTypeAttribute, MyEntityStatRegenEffect> m_objectFactory = new MyObjectFactory<MyEntityStatEffectTypeAttribute, MyEntityStatRegenEffect>();

    static MyEntityStatEffectFactory() => MyEntityStatEffectFactory.m_objectFactory.RegisterFromAssembly(Assembly.GetAssembly(typeof (MyEntityStatRegenEffect)));

    public static MyEntityStatRegenEffect CreateInstance(
      MyObjectBuilder_EntityStatRegenEffect builder)
    {
      return MyEntityStatEffectFactory.m_objectFactory.CreateInstance(builder.TypeId);
    }

    public static MyObjectBuilder_EntityStatRegenEffect CreateObjectBuilder(
      MyEntityStatRegenEffect effect)
    {
      return MyEntityStatEffectFactory.m_objectFactory.CreateObjectBuilder<MyObjectBuilder_EntityStatRegenEffect>(effect);
    }

    public static Type GetProducedType(MyObjectBuilderType objectBuilderType) => MyEntityStatEffectFactory.m_objectFactory.GetProducedType(objectBuilderType);
  }
}
