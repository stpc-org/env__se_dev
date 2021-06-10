// Decompiled with JetBrains decompiler
// Type: VRage.Generics.IMyVariableStorage`1
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System.Collections;
using System.Collections.Generic;
using VRage.Utils;

namespace VRage.Generics
{
  public interface IMyVariableStorage<T> : IEnumerable<KeyValuePair<MyStringId, T>>, IEnumerable
  {
    void SetValue(MyStringId key, T newValue);

    bool GetValue(MyStringId key, out T value);
  }
}
