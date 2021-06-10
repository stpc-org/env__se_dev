// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.MyFilterBool
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using VRage.Game;

namespace Sandbox.Game.Screens
{
  public class MyFilterBool : IMyFilterOption
  {
    public MyFilterBool(bool? value = null) => this.Value = value;

    public void Configure(string value)
    {
      if (!(value == "0"))
      {
        if (!(value == "1"))
        {
          if (!(value == "2"))
            throw new InvalidBranchException();
          this.Value = new bool?();
        }
        else
          this.Value = new bool?(true);
      }
      else
        this.Value = new bool?(false);
    }

    public bool? Value { get; set; }

    public CheckStateEnum CheckValue
    {
      get
      {
        bool? nullable = this.Value;
        if (!nullable.HasValue)
          return CheckStateEnum.Indeterminate;
        bool valueOrDefault = nullable.GetValueOrDefault();
        if (!valueOrDefault)
          return CheckStateEnum.Unchecked;
        if (valueOrDefault)
          return CheckStateEnum.Checked;
        throw new InvalidBranchException();
      }
      set
      {
        switch (value)
        {
          case CheckStateEnum.Checked:
            this.Value = new bool?(true);
            break;
          case CheckStateEnum.Unchecked:
            this.Value = new bool?(false);
            break;
          case CheckStateEnum.Indeterminate:
            this.Value = new bool?();
            break;
          default:
            throw new ArgumentOutOfRangeException(nameof (value), (object) value, (string) null);
        }
      }
    }

    public bool IsMatch(object value)
    {
      if (!this.Value.HasValue)
        return true;
      bool? nullable1 = this.Value;
      bool? nullable2 = (bool?) value;
      return nullable1.GetValueOrDefault() == nullable2.GetValueOrDefault() & nullable1.HasValue == nullable2.HasValue;
    }

    public string SerializedValue
    {
      get
      {
        bool? nullable = this.Value;
        if (!nullable.HasValue)
          return "2";
        bool valueOrDefault = nullable.GetValueOrDefault();
        if (!valueOrDefault)
          return "0";
        if (valueOrDefault)
          return "1";
        throw new InvalidBranchException();
      }
    }
  }
}
