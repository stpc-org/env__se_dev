// Decompiled with JetBrains decompiler
// Type: VRage.Collections.MySwapQueue
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

namespace VRage.Collections
{
  public class MySwapQueue
  {
    public static MySwapQueue<T> Create<T>() where T : class, new() => new MySwapQueue<T>(new T(), new T(), new T());
  }
}
