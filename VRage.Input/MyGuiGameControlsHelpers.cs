// Decompiled with JetBrains decompiler
// Type: VRage.Input.MyGuiGameControlsHelpers
// Assembly: VRage.Input, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 02474C5E-7189-409A-98E6-D5E3CA7AB23A
// Assembly location: D:\Files\library_development\lib_se\VRage.Input.dll

using System.Collections.Generic;
using VRage.Utils;

namespace VRage.Input
{
  public static class MyGuiGameControlsHelpers
  {
    private static readonly Dictionary<MyStringId, MyDescriptor> m_gameControlHelpers = new Dictionary<MyStringId, MyDescriptor>((IEqualityComparer<MyStringId>) MyStringId.Comparer);

    public static MyDescriptor GetGameControlHelper(MyStringId controlHelper)
    {
      MyDescriptor myDescriptor;
      return MyGuiGameControlsHelpers.m_gameControlHelpers.TryGetValue(controlHelper, out myDescriptor) ? myDescriptor : (MyDescriptor) null;
    }

    public static void Add(MyStringId control, MyDescriptor descriptor) => MyGuiGameControlsHelpers.m_gameControlHelpers.Add(control, descriptor);

    public static void Reset() => MyGuiGameControlsHelpers.m_gameControlHelpers.Clear();
  }
}
