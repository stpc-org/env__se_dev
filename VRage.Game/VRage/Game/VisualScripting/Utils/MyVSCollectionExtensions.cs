// Decompiled with JetBrains decompiler
// Type: VRage.Game.VisualScripting.Utils.MyVSCollectionExtensions
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System.Collections.Generic;

namespace VRage.Game.VisualScripting.Utils
{
  public static class MyVSCollectionExtensions
  {
    [VisualScriptingMember(false, false)]
    public static T At<T>(this List<T> list, int index) => index >= 0 && index < list.Count ? list[index] : default (T);

    [VisualScriptingMember(false, false)]
    public static int Count<T>(this List<T> list) => list.Count;

    [VisualScriptingMember(false, false)]
    public static int CountMinusOne<T>(this List<T> list) => list.Count - 1;
  }
}
