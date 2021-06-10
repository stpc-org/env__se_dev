// Decompiled with JetBrains decompiler
// Type: VRage.Serialization.MyMemberSerializer
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System.Reflection;
using VRage.Library.Collections;

namespace VRage.Serialization
{
  public abstract class MyMemberSerializer
  {
    protected MySerializeInfo m_info;

    public MySerializeInfo Info => this.m_info;

    public abstract void Init(MemberInfo memberInfo, MySerializeInfo info);

    public abstract void Clone(object original, object clone);

    public abstract bool Equals(object a, object b);

    public abstract void Read(BitStream stream, object obj, MySerializeInfo info);

    public abstract void Write(BitStream stream, object obj, MySerializeInfo info);
  }
}
