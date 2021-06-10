// Decompiled with JetBrains decompiler
// Type: VRage.ObjectBuilders.MyRuntimeObjectBuilderIdComparer
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System.Collections.Generic;

namespace VRage.ObjectBuilders
{
  public class MyRuntimeObjectBuilderIdComparer : IComparer<MyRuntimeObjectBuilderId>, IEqualityComparer<MyRuntimeObjectBuilderId>
  {
    public int Compare(MyRuntimeObjectBuilderId x, MyRuntimeObjectBuilderId y) => (int) x.Value - (int) y.Value;

    public bool Equals(MyRuntimeObjectBuilderId x, MyRuntimeObjectBuilderId y) => (int) x.Value == (int) y.Value;

    public int GetHashCode(MyRuntimeObjectBuilderId obj) => obj.Value.GetHashCode();
  }
}
