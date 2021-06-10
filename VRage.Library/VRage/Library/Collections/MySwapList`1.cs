// Decompiled with JetBrains decompiler
// Type: VRage.Library.Collections.MySwapList`1
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System.Collections.Generic;

namespace VRage.Library.Collections
{
  public class MySwapList<T>
  {
    private List<T> m_foreground = new List<T>();
    private List<T> m_background = new List<T>();

    public List<T> BackgroundList => this.m_background;

    public T this[int index]
    {
      get => this.m_foreground[index];
      set => this.m_foreground[index] = value;
    }

    public void Add(T element) => this.m_foreground.Add(element);

    public void Remove(T element) => this.m_background.Add(element);

    public void Clear() => this.m_foreground.Clear();

    public void Swap()
    {
      List<T> foreground = this.m_foreground;
      this.m_foreground = this.m_background;
      this.m_background = foreground;
    }

    public List<T>.Enumerator GetEnumerator() => this.m_foreground.GetEnumerator();
  }
}
