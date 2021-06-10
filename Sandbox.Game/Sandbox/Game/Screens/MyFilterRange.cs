// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.MyFilterRange
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRageMath;

namespace Sandbox.Game.Screens
{
  public class MyFilterRange : IMyFilterOption
  {
    public MyFilterRange()
    {
      this.Value = new SerializableRange();
      this.Active = false;
    }

    public MyFilterRange(SerializableRange value, bool active = false)
    {
      this.Value = value;
      this.Active = active;
    }

    public void Configure(string value)
    {
      if (string.IsNullOrEmpty(value))
      {
        this.Value = new SerializableRange();
      }
      else
      {
        string[] strArray = value.Split(':');
        this.Value = new SerializableRange()
        {
          Min = float.Parse(strArray[0]),
          Max = float.Parse(strArray[1])
        };
        this.Active = bool.Parse(strArray[2]);
      }
    }

    public SerializableRange Value { get; set; }

    public bool Active { get; set; }

    public bool IsMatch(float value) => !this.Active || this.Value.ValueBetween(value);

    public string SerializedValue => this.Value.Min.ToString() + ":" + (object) this.Value.Max + ":" + this.Active.ToString();
  }
}
