// Decompiled with JetBrains decompiler
// Type: VRage.Network.TypeId
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

namespace VRage.Network
{
  public struct TypeId
  {
    internal uint Value;

    public TypeId(uint value) => this.Value = value;

    public static implicit operator uint(TypeId tp) => tp.Value;

    public override string ToString() => this.Value.ToString();
  }
}
