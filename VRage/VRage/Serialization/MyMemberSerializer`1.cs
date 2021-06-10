// Decompiled with JetBrains decompiler
// Type: VRage.Serialization.MyMemberSerializer`1
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using VRage.Library.Collections;

namespace VRage.Serialization
{
  public abstract class MyMemberSerializer<TOwner> : MyMemberSerializer
  {
    public abstract void Clone(ref TOwner original, ref TOwner clone);

    public abstract bool Equals(ref TOwner a, ref TOwner b);

    public abstract void Read(BitStream stream, ref TOwner obj, MySerializeInfo info);

    public abstract void Write(BitStream stream, ref TOwner obj, MySerializeInfo info);

    public override sealed void Clone(object original, object clone) => this.Clone((object) (TOwner) original, (object) (TOwner) clone);

    public override sealed bool Equals(object a, object b)
    {
      TOwner a1 = (TOwner) a;
      TOwner b1 = (TOwner) b;
      return this.Equals(ref a1, ref b1);
    }

    public override sealed void Read(BitStream stream, object obj, MySerializeInfo info)
    {
      TOwner owner = (TOwner) obj;
      this.Read(stream, ref owner, info);
    }

    public override sealed void Write(BitStream stream, object obj, MySerializeInfo info)
    {
      TOwner owner = (TOwner) obj;
      this.Write(stream, ref owner, info);
    }
  }
}
