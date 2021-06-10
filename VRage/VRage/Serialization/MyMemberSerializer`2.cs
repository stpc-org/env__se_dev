// Decompiled with JetBrains decompiler
// Type: VRage.Serialization.MyMemberSerializer`2
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Reflection;
using VRage.Library.Collections;
using VRage.Network;

namespace VRage.Serialization
{
  public sealed class MyMemberSerializer<TOwner, TMember> : MyMemberSerializer<TOwner>
  {
    private Getter<TOwner, TMember> m_getter;
    private Setter<TOwner, TMember> m_setter;
    private MySerializer<TMember> m_serializer;
    private MemberInfo m_memberInfo;

    public override sealed void Init(MemberInfo memberInfo, MySerializeInfo info)
    {
      if (this.m_serializer != null)
        throw new InvalidOperationException("Already initialized");
      IMemberAccessor memberAccessor1;
      if ((memberAccessor1 = CodegenUtils.GetMemberAccessor(typeof (TOwner), memberInfo)) != null)
      {
        IMemberAccessor<TOwner, TMember> memberAccessor2 = (IMemberAccessor<TOwner, TMember>) memberAccessor1;
        this.m_getter = new Getter<TOwner, TMember>(memberAccessor2.Get);
        this.m_setter = new Setter<TOwner, TMember>(memberAccessor2.Set);
      }
      else
      {
        this.m_getter = memberInfo.CreateGetterRef<TOwner, TMember>();
        this.m_setter = memberInfo.CreateSetterRef<TOwner, TMember>();
      }
      this.m_serializer = MyFactory.GetSerializer<TMember>();
      this.m_info = info;
      this.m_memberInfo = memberInfo;
    }

    public override string ToString() => string.Format("{2} {0}.{1}", (object) this.m_memberInfo.DeclaringType.Name, (object) this.m_memberInfo.Name, (object) this.m_memberInfo.GetMemberType().Name);

    public override void Clone(ref TOwner original, ref TOwner clone)
    {
      TMember member;
      this.m_getter(ref original, out member);
      this.m_serializer.Clone(ref member);
      this.m_setter(ref clone, in member);
    }

    public override bool Equals(ref TOwner a, ref TOwner b)
    {
      TMember a1;
      this.m_getter(ref a, out a1);
      TMember b1;
      this.m_getter(ref b, out b1);
      return this.m_serializer.Equals(ref a1, ref b1);
    }

    public override sealed void Read(BitStream stream, ref TOwner obj, MySerializeInfo info)
    {
      TMember result;
      if (!MySerializationHelpers.CreateAndRead<TMember>(stream, out result, this.m_serializer, info ?? this.m_info))
        return;
      this.m_setter(ref obj, in result);
    }

    public override sealed void Write(BitStream stream, ref TOwner obj, MySerializeInfo info)
    {
      try
      {
        TMember member;
        this.m_getter(ref obj, out member);
        MySerializationHelpers.Write<TMember>(stream, ref member, this.m_serializer, info ?? this.m_info);
      }
      catch (MySerializeException ex)
      {
        string message;
        switch (ex.Error)
        {
          case MySerializeErrorEnum.NullNotAllowed:
            message = string.Format("Error serializing {0}.{1}, member contains null, but it's not allowed, consider adding attribute [Serialize(MyObjectFlags.Nullable)]", (object) this.m_memberInfo.DeclaringType.Name, (object) this.m_memberInfo.Name);
            break;
          case MySerializeErrorEnum.DynamicNotAllowed:
            message = string.Format("Error serializing {0}.{1}, member contains inherited type, but it's not allowed, consider adding attribute [Serialize(MyObjectFlags.Dynamic)]", (object) this.m_memberInfo.DeclaringType.Name, (object) this.m_memberInfo.Name);
            break;
          default:
            message = "Unknown serialization error";
            break;
        }
        throw new InvalidOperationException(message, (Exception) ex);
      }
    }
  }
}
