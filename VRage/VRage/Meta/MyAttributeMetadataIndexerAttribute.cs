// Decompiled with JetBrains decompiler
// Type: VRage.Meta.MyAttributeMetadataIndexerAttribute
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;

namespace VRage.Meta
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true, Inherited = false)]
  public class MyAttributeMetadataIndexerAttribute : MyAttributeMetadataIndexerAttributeBase
  {
    private readonly Type m_attrType;
    private readonly Type m_target;

    public override Type AttributeType => this.m_attrType;

    public override Type TargetType => this.m_target;

    public MyAttributeMetadataIndexerAttribute(Type attrType)
    {
      this.m_attrType = attrType;
      this.m_target = (Type) null;
    }

    public MyAttributeMetadataIndexerAttribute(Type attrType, Type indexerType)
    {
      this.m_attrType = attrType;
      this.m_target = indexerType;
    }
  }
}
