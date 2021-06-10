// Decompiled with JetBrains decompiler
// Type: VRage.Factory.MyFactorableAttribute
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using VRage.Meta;
using VRage.Utils;

namespace VRage.Factory
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = false)]
  public class MyFactorableAttribute : MyAttributeMetadataIndexerAttributeBase
  {
    private readonly Type m_factoryType;
    private readonly Type m_attributeType;

    public MyFactorableAttribute(Type factoryType)
    {
      this.m_factoryType = factoryType;
      Type genericType = typeof (MyObjectFactory<,>);
      if (!this.m_factoryType.IsInstanceOfGenericType(genericType))
      {
        MyLog.Default.Error("Type {0} is not an object factory");
      }
      else
      {
        while (!factoryType.IsGenericType || factoryType.GetGenericTypeDefinition() != genericType)
          factoryType = factoryType.BaseType;
        this.m_attributeType = factoryType.GenericTypeArguments[0];
      }
    }

    public override Type AttributeType => this.m_attributeType;

    public override Type TargetType => this.m_factoryType;
  }
}
