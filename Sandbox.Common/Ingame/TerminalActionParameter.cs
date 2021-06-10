// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.Ingame.TerminalActionParameter
// Assembly: Sandbox.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 59BF3DFC-03D1-4F5E-90FB-54CDF536D906
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Common.dll

using System;
using System.Globalization;
using VRage.Game;
using VRage.ObjectBuilders;

namespace Sandbox.ModAPI.Ingame
{
  public struct TerminalActionParameter
  {
    public static readonly TerminalActionParameter Empty;
    public readonly TypeCode TypeCode;
    public readonly object Value;

    private static Type ToType(TypeCode code)
    {
      switch (code)
      {
        case TypeCode.Boolean:
          return typeof (bool);
        case TypeCode.Char:
          return typeof (char);
        case TypeCode.SByte:
          return typeof (sbyte);
        case TypeCode.Byte:
          return typeof (byte);
        case TypeCode.Int16:
          return typeof (short);
        case TypeCode.UInt16:
          return typeof (ushort);
        case TypeCode.Int32:
          return typeof (int);
        case TypeCode.UInt32:
          return typeof (uint);
        case TypeCode.Int64:
          return typeof (long);
        case TypeCode.UInt64:
          return typeof (ulong);
        case TypeCode.Single:
          return typeof (float);
        case TypeCode.Double:
          return typeof (double);
        case TypeCode.Decimal:
          return typeof (Decimal);
        case TypeCode.DateTime:
          return typeof (DateTime);
        case TypeCode.String:
          return typeof (string);
        default:
          return (Type) null;
      }
    }

    public static TerminalActionParameter Deserialize(
      string serializedValue,
      TypeCode typeCode)
    {
      TerminalActionParameter.AssertTypeCodeValidity(typeCode);
      if (TerminalActionParameter.ToType(typeCode) == (Type) null)
        return TerminalActionParameter.Empty;
      object obj = Convert.ChangeType((object) serializedValue, typeCode, (IFormatProvider) CultureInfo.InvariantCulture);
      return new TerminalActionParameter(typeCode, obj);
    }

    public static TerminalActionParameter Get(object value)
    {
      if (value == null)
        return TerminalActionParameter.Empty;
      TypeCode typeCode = Type.GetTypeCode(value.GetType());
      TerminalActionParameter.AssertTypeCodeValidity(typeCode);
      return new TerminalActionParameter(typeCode, value);
    }

    private static void AssertTypeCodeValidity(TypeCode typeCode)
    {
      switch (typeCode)
      {
        case TypeCode.Empty:
        case TypeCode.Object:
        case TypeCode.DBNull:
          throw new ArgumentException("Only primitive types are allowed for action parameters", "value");
      }
    }

    private TerminalActionParameter(TypeCode typeCode, object value)
    {
      this.TypeCode = typeCode;
      this.Value = value;
    }

    public bool IsEmpty => this.TypeCode == TypeCode.Empty;

    public MyObjectBuilder_ToolbarItemActionParameter GetObjectBuilder()
    {
      MyObjectBuilder_ToolbarItemActionParameter newObject = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_ToolbarItemActionParameter>();
      newObject.TypeCode = this.TypeCode;
      newObject.Value = this.Value == null ? (string) null : Convert.ToString(this.Value, (IFormatProvider) CultureInfo.InvariantCulture);
      return newObject;
    }
  }
}
