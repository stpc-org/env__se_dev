// Decompiled with JetBrains decompiler
// Type: VRage.MyPositionAndOrientation
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.Serialization;
using VRageMath;

namespace VRage
{
  [ProtoContract]
  public struct MyPositionAndOrientation
  {
    [ProtoMember(1)]
    [XmlElement("Position")]
    public SerializableVector3D Position;
    [ProtoMember(4)]
    [XmlElement("Forward")]
    [NoSerialize]
    public SerializableVector3 Forward;
    [ProtoMember(7)]
    [XmlElement("Up")]
    [NoSerialize]
    public SerializableVector3 Up;
    public static readonly MyPositionAndOrientation Default = new MyPositionAndOrientation((Vector3D) Vector3.Zero, Vector3.Forward, Vector3.Up);

    [Serialize(MyPrimitiveFlags.Normalized)]
    public Quaternion Orientation
    {
      get
      {
        MatrixD matrix = this.GetMatrix();
        return Quaternion.CreateFromRotationMatrix(in matrix);
      }
      set
      {
        Matrix fromQuaternion = Matrix.CreateFromQuaternion(value);
        this.Forward = (SerializableVector3) fromQuaternion.Forward;
        this.Up = (SerializableVector3) fromQuaternion.Up;
      }
    }

    public MyPositionAndOrientation(Vector3D position, Vector3 forward, Vector3 up)
    {
      this.Position = (SerializableVector3D) position;
      this.Forward = (SerializableVector3) forward;
      this.Up = (SerializableVector3) up;
    }

    public MyPositionAndOrientation(ref MatrixD matrix)
    {
      this.Position = (SerializableVector3D) matrix.Translation;
      this.Forward = (SerializableVector3) (Vector3) matrix.Forward;
      this.Up = (SerializableVector3) (Vector3) matrix.Up;
    }

    public MyPositionAndOrientation(MatrixD matrix)
      : this(matrix.Translation, (Vector3) matrix.Forward, (Vector3) matrix.Up)
    {
    }

    public MatrixD GetMatrix() => MatrixD.CreateWorld((Vector3D) this.Position, (Vector3) this.Forward, (Vector3) this.Up);

    public override string ToString() => this.Position.ToString() + "; " + this.Forward.ToString() + "; " + this.Up.ToString();

    protected class VRage_MyPositionAndOrientation\u003C\u003EPosition\u003C\u003EAccessor : IMemberAccessor<MyPositionAndOrientation, SerializableVector3D>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyPositionAndOrientation owner, in SerializableVector3D value) => owner.Position = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyPositionAndOrientation owner, out SerializableVector3D value) => value = owner.Position;
    }

    protected class VRage_MyPositionAndOrientation\u003C\u003EForward\u003C\u003EAccessor : IMemberAccessor<MyPositionAndOrientation, SerializableVector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyPositionAndOrientation owner, in SerializableVector3 value) => owner.Forward = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyPositionAndOrientation owner, out SerializableVector3 value) => value = owner.Forward;
    }

    protected class VRage_MyPositionAndOrientation\u003C\u003EUp\u003C\u003EAccessor : IMemberAccessor<MyPositionAndOrientation, SerializableVector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyPositionAndOrientation owner, in SerializableVector3 value) => owner.Up = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyPositionAndOrientation owner, out SerializableVector3 value) => value = owner.Up;
    }

    protected class VRage_MyPositionAndOrientation\u003C\u003EOrientation\u003C\u003EAccessor : IMemberAccessor<MyPositionAndOrientation, Quaternion>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyPositionAndOrientation owner, in Quaternion value) => owner.Orientation = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyPositionAndOrientation owner, out Quaternion value) => value = owner.Orientation;
    }
  }
}
