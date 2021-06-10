// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.MyLoadingScreenQuote
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Utils;

namespace Sandbox.Game.Screens.Helpers
{
  internal class MyLoadingScreenQuote : MyLoadingScreenText
  {
    public readonly MyStringId Author;

    public MyLoadingScreenQuote(MyStringId text, MyStringId author)
      : base(text)
      => this.Author = author;

    public static void Init()
    {
      MyStringId nullOrEmpty = MyStringId.NullOrEmpty;
      MyStringId text;
      for (int index = 0; (text = MyStringId.TryGet(string.Format("Quote{0:00}Text", (object) index))) != MyStringId.NullOrEmpty; ++index)
      {
        MyStringId author = MyStringId.TryGet(string.Format("Quote{0:00}Author", (object) index));
        MyLoadingScreenText.m_textsShared.Add((MyLoadingScreenText) new MyLoadingScreenQuote(text, author));
      }
    }
  }
}
