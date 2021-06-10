// Decompiled with JetBrains decompiler
// Type: VRage.Sync.SyncBase
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using VRage.Library.Collections;
using VRage.Network;
using VRage.Serialization;

namespace VRage.Sync
{
  public abstract class SyncBase : IBitSerializable, ISyncType
  {
    public readonly int Id;
    public readonly Type ValueType;
    public readonly MySerializeInfo SerializeInfo;
    public string DebugName;
    public bool ShouldValidate = true;

    public event Action<SyncBase> ValueChanged;

    public event Action<SyncBase> ValueChangedNotify;

    public SyncBase(Type valueType, int id, ISerializerInfo serializeInfo)
    {
      this.ValueType = valueType;
      this.Id = id;
      this.SerializeInfo = (MySerializeInfo) serializeInfo;
    }

    protected void RaiseValueChanged(bool notify)
    {
      Action<SyncBase> valueChanged = this.ValueChanged;
      if (valueChanged != null)
        valueChanged(this);
      if (!notify)
        return;
      Action<SyncBase> valueChangedNotify = this.ValueChangedNotify;
      if (valueChangedNotify == null)
        return;
      valueChangedNotify(this);
    }

    public abstract SyncBase Clone(int newId);

    public abstract bool Serialize(BitStream stream, bool validate, bool setValueIfValid = true);

    protected static void CopyValueChanged(SyncBase from, SyncBase to)
    {
      to.ValueChanged = from.ValueChanged;
      to.ValueChangedNotify = from.ValueChangedNotify;
    }

    public static implicit operator BitReaderWriter(SyncBase sync) => new BitReaderWriter((IBitSerializable) sync);

    public void SetDebugName(string debugName) => this.DebugName = debugName;
  }
}
