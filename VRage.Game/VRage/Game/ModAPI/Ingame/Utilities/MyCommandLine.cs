// Decompiled with JetBrains decompiler
// Type: VRage.Game.ModAPI.Ingame.Utilities.MyCommandLine
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRage.Game.ModAPI.Ingame.Utilities
{
  public class MyCommandLine
  {
    private readonly List<StringSegment> m_items = new List<StringSegment>();
    private readonly Dictionary<StringSegment, int> m_switchIndexes = new Dictionary<StringSegment, int>((IEqualityComparer<StringSegment>) StringSegmentIgnoreCaseComparer.DEFAULT);
    private readonly List<int> m_argumentIndexes = new List<int>();

    public MyCommandLine()
    {
      this.Items = new MyCommandLine.ItemCollection(this.m_items);
      this.Switches = new MyCommandLine.SwitchCollection(this.m_switchIndexes);
    }

    public MyCommandLine.ItemCollection Items { get; private set; }

    public MyCommandLine.SwitchCollection Switches { get; private set; }

    public bool TryParse(string argument)
    {
      this.Clear();
      if (string.IsNullOrEmpty(argument))
        return false;
      TextPtr ptr = new TextPtr(argument);
      while (true)
      {
        do
        {
          ptr = ptr.SkipWhitespace();
          if (ptr.IsOutOfBounds())
            goto label_6;
        }
        while (this.TryParseSwitch(ref ptr));
        this.ParseParameter(ref ptr);
      }
label_6:
      return this.Items.Count > 0;
    }

    public int ArgumentCount => this.m_argumentIndexes.Count;

    public string Argument(int index) => index < 0 || index >= this.m_argumentIndexes.Count ? (string) null : this.Items[this.m_argumentIndexes[index]];

    public bool Switch(string name) => this.m_switchIndexes.ContainsKey(new StringSegment(name));

    public string Switch(string name, int relativeArgument)
    {
      int num;
      if (!this.m_switchIndexes.TryGetValue(name.Length <= 0 || name[0] != '-' ? new StringSegment(name) : new StringSegment(name, 1, name.Length - 1), out num))
        return (string) null;
      relativeArgument += 1 + num;
      for (int index = relativeArgument; index > num; --index)
      {
        if (!this.m_argumentIndexes.Contains(index))
          return (string) null;
      }
      return this.m_items[relativeArgument].ToString();
    }

    public void Clear()
    {
      this.m_items.Clear();
      this.m_switchIndexes.Clear();
      this.m_argumentIndexes.Clear();
    }

    private bool TryParseSwitch(ref TextPtr ptr)
    {
      if (ptr.Char != '-')
        return false;
      StringSegment quoted = this.ParseQuoted(ref ptr);
      int count = this.Items.Count;
      this.m_items.Add(quoted);
      this.m_switchIndexes[new StringSegment(quoted.Text, quoted.Start + 1, quoted.Length - 1)] = count;
      return true;
    }

    private void ParseParameter(ref TextPtr ptr)
    {
      StringSegment quoted = this.ParseQuoted(ref ptr);
      int count = this.Items.Count;
      this.m_items.Add(quoted);
      this.m_argumentIndexes.Add(count);
    }

    private StringSegment ParseQuoted(ref TextPtr ptr)
    {
      TextPtr textPtr1 = ptr;
      bool flag = textPtr1.Char == '"';
      if (flag)
        ++textPtr1;
      TextPtr textPtr2 = textPtr1;
      while (!textPtr2.IsOutOfBounds())
      {
        if (textPtr2.Char == '"')
          flag = !flag;
        if (!flag && char.IsWhiteSpace(textPtr2.Char))
        {
          ptr = textPtr2;
          TextPtr textPtr3 = textPtr2 - 1;
          if (textPtr3.Char == '"')
            textPtr2 = textPtr3;
          return new StringSegment(textPtr1.Content, textPtr1.Index, textPtr2.Index - textPtr1.Index);
        }
        ++textPtr2;
      }
      textPtr2 = new TextPtr(ptr.Content, ptr.Content.Length);
      ptr = textPtr2;
      TextPtr textPtr4 = textPtr2 - 1;
      if (textPtr4.Char == '"')
        textPtr2 = textPtr4;
      return new StringSegment(textPtr1.Content, textPtr1.Index, textPtr2.Index - textPtr1.Index);
    }

    public class ItemCollection : IReadOnlyList<string>, IEnumerable<string>, IEnumerable, IReadOnlyCollection<string>
    {
      private readonly List<StringSegment> m_items;

      internal ItemCollection(List<StringSegment> items) => this.m_items = items;

      public MyCommandLine.Enumerator GetEnumerator() => new MyCommandLine.Enumerator(this.m_items.GetEnumerator());

      IEnumerator<string> IEnumerable<string>.GetEnumerator() => (IEnumerator<string>) new MyCommandLine.Enumerator(this.m_items.GetEnumerator());

      IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) new MyCommandLine.Enumerator(this.m_items.GetEnumerator());

      public int Count => this.m_items.Count;

      public string this[int index] => index < 0 || index >= this.m_items.Count ? (string) null : this.m_items[index].ToString();
    }

    public class SwitchCollection : IReadOnlyCollection<string>, IEnumerable<string>, IEnumerable
    {
      private readonly Dictionary<StringSegment, int> m_switches;

      internal SwitchCollection(Dictionary<StringSegment, int> switches) => this.m_switches = switches;

      public MyCommandLine.SwitchEnumerator GetEnumerator() => new MyCommandLine.SwitchEnumerator(this.m_switches.Keys.GetEnumerator());

      IEnumerator<string> IEnumerable<string>.GetEnumerator() => (IEnumerator<string>) new MyCommandLine.SwitchEnumerator(this.m_switches.Keys.GetEnumerator());

      IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) new MyCommandLine.SwitchEnumerator(this.m_switches.Keys.GetEnumerator());

      public int Count => this.m_switches.Count;
    }

    [Serializable]
    public struct SwitchEnumerator : IEnumerator<string>, IEnumerator, IDisposable
    {
      private Dictionary<StringSegment, int>.KeyCollection.Enumerator m_enumerator;

      internal SwitchEnumerator(
        Dictionary<StringSegment, int>.KeyCollection.Enumerator enumerator)
      {
        this.m_enumerator = enumerator;
      }

      public void Dispose() => this.m_enumerator.Dispose();

      public bool MoveNext() => this.m_enumerator.MoveNext();

      public string Current => this.m_enumerator.Current.ToString();

      object IEnumerator.Current => (object) this.Current;

      void IEnumerator.Reset() => ((IEnumerator) this.m_enumerator).Reset();

      protected class VRage_Game_ModAPI_Ingame_Utilities_MyCommandLine\u003C\u003ESwitchEnumerator\u003C\u003Em_enumerator\u003C\u003EAccessor : IMemberAccessor<MyCommandLine.SwitchEnumerator, Dictionary<StringSegment, int>.KeyCollection.Enumerator>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyCommandLine.SwitchEnumerator owner,
          in Dictionary<StringSegment, int>.KeyCollection.Enumerator value)
        {
          owner.m_enumerator = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyCommandLine.SwitchEnumerator owner,
          out Dictionary<StringSegment, int>.KeyCollection.Enumerator value)
        {
          value = owner.m_enumerator;
        }
      }
    }

    [Serializable]
    public struct Enumerator : IEnumerator<string>, IEnumerator, IDisposable
    {
      private List<StringSegment>.Enumerator m_enumerator;

      internal Enumerator(List<StringSegment>.Enumerator enumerator) => this.m_enumerator = enumerator;

      public void Dispose() => this.m_enumerator.Dispose();

      public bool MoveNext() => this.m_enumerator.MoveNext();

      public string Current => this.m_enumerator.Current.ToString();

      object IEnumerator.Current => (object) this.Current;

      void IEnumerator.Reset() => ((IEnumerator) this.m_enumerator).Reset();

      protected class VRage_Game_ModAPI_Ingame_Utilities_MyCommandLine\u003C\u003EEnumerator\u003C\u003Em_enumerator\u003C\u003EAccessor : IMemberAccessor<MyCommandLine.Enumerator, List<StringSegment>.Enumerator>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyCommandLine.Enumerator owner,
          in List<StringSegment>.Enumerator value)
        {
          owner.m_enumerator = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyCommandLine.Enumerator owner,
          out List<StringSegment>.Enumerator value)
        {
          value = owner.m_enumerator;
        }
      }
    }
  }
}
