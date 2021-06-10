// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyHudSelectedObjectStatus
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game.Entity.UseObject;

namespace Sandbox.Game.Gui
{
  internal struct MyHudSelectedObjectStatus
  {
    public IMyUseObject Instance;
    public string[] SectionNames;
    public int InstanceId;
    public uint[] SubpartIndices;
    public MyHudObjectHighlightStyle Style;

    public void Reset()
    {
      this.Instance = (IMyUseObject) null;
      this.SectionNames = (string[]) null;
      this.InstanceId = -1;
      this.SubpartIndices = (uint[]) null;
      this.Style = MyHudObjectHighlightStyle.None;
    }
  }
}
