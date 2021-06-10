// Decompiled with JetBrains decompiler
// Type: VRage.Collections.ObservableCollection`1
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace VRage.Collections
{
  public class ObservableCollection<T> : ObservableCollection<T>
  {
    public bool FireEvents = true;

    protected override void ClearItems()
    {
      NotifyCollectionChangedEventArgs e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, (IList) this);
      if (this.FireEvents)
        this.OnCollectionChanged(e);
      base.ClearItems();
    }

    public ObservableCollection<T>.Enumerator GetEnumerator() => new ObservableCollection<T>.Enumerator(this);

    public int FindIndex(Predicate<T> match)
    {
      int num = -1;
      for (int index = 0; index < this.Items.Count; ++index)
      {
        if (match(this.Items[index]))
        {
          num = index;
          break;
        }
      }
      return num;
    }

    public struct Enumerator : IEnumerator<T>, IEnumerator, IDisposable
    {
      private ObservableCollection<T> m_collection;
      private int m_index;

      public Enumerator(ObservableCollection<T> collection)
      {
        this.m_index = -1;
        this.m_collection = collection;
      }

      public T Current => this.m_collection[this.m_index];

      public void Dispose()
      {
      }

      object IEnumerator.Current => (object) this.Current;

      public bool MoveNext()
      {
        ++this.m_index;
        return this.m_index < this.m_collection.Count;
      }

      public void Reset() => this.m_index = -1;
    }
  }
}
