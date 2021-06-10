// Decompiled with JetBrains decompiler
// Type: VRage.Sync.Sync`2
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using VRage.Library.Collections;
using VRage.Network;
using VRage.Replication;
using VRage.Serialization;

namespace VRage.Sync
{
  public class Sync<T, TSyncDirection> : SyncBase where TSyncDirection : SyncDirection
  {
    public static readonly MySerializer<T> TypeSerializer = MyFactory.GetSerializer<T>();
    private T m_value;
    public SyncValidate<T> Validate;

    public T Value
    {
      get => this.m_value;
      set => this.SetValue(ref value, false);
    }

    private bool IsServer => MyMultiplayerMinimalBase.Instance == null || MyMultiplayerMinimalBase.Instance.IsServer;

    public bool Enabled { get; set; }

    public Sync(int id, ISerializerInfo serializeInfo)
      : base(typeof (T), id, serializeInfo)
      => this.Enabled = true;

    public override string ToString() => this.Value.ToString();

    private bool IsValid(ref T value)
    {
      SyncValidate<T> validate = this.Validate;
      return validate == null || validate(value);
    }

    private bool SetValue(ref T newValue, bool validate, bool ignoreSyncDirection = false, bool received = false)
    {
      if (!ignoreSyncDirection && MyMultiplayerMinimalBase.Instance != null && (!MyMultiplayerMinimalBase.Instance.IsServer && typeof (TSyncDirection) == typeof (SyncDirection.FromServer)))
        return false;
      if (VRage.Sync.Sync<T, TSyncDirection>.TypeSerializer.Equals(ref this.m_value, ref newValue))
        return true;
      if (validate && !this.IsValid(ref newValue))
        return false;
      this.m_value = newValue;
      this.RaiseValueChanged(this.Enabled && (!received || this.IsServer));
      return true;
    }

    public void ValidateAndSet(T newValue) => this.SetValue(ref newValue, true);

    public void SetLocalValue(T newValue)
    {
      if (MyMultiplayerMinimalBase.Instance != null && !MyMultiplayerMinimalBase.Instance.IsServer)
        this.SetValue(ref newValue, false, true, true);
      else
        this.SetValue(ref newValue, false);
    }

    public override SyncBase Clone(int newId)
    {
      VRage.Sync.Sync<T, TSyncDirection> sync = new VRage.Sync.Sync<T, TSyncDirection>(newId, (ISerializerInfo) this.SerializeInfo);
      SyncBase.CopyValueChanged((SyncBase) this, (SyncBase) sync);
      sync.Validate = this.Validate;
      sync.m_value = this.m_value;
      return (SyncBase) sync;
    }

    public override bool Serialize(BitStream stream, bool validate, bool setValueIfValid = true)
    {
      if (stream.Reading)
      {
        T newValue;
        MySerializer.CreateAndRead<T>(stream, out newValue, this.SerializeInfo);
        return setValueIfValid && this.SetValue(ref newValue, validate, true, true);
      }
      MySerializer.Write<T>(stream, ref this.m_value, this.SerializeInfo);
      return true;
    }

    public static implicit operator T(VRage.Sync.Sync<T, TSyncDirection> sync) => sync.Value;
  }
}
