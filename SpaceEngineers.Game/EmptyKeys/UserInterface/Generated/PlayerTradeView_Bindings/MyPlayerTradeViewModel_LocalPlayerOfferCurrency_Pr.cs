// Decompiled with JetBrains decompiler
// Type: EmptyKeys.UserInterface.Generated.PlayerTradeView_Bindings.MyPlayerTradeViewModel_LocalPlayerOfferCurrency_PropertyInfo
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using EmptyKeys.UserInterface.Data;
using Sandbox.Game.Screens.ViewModels;
using System;
using System.CodeDom.Compiler;

namespace EmptyKeys.UserInterface.Generated.PlayerTradeView_Bindings
{
  [GeneratedCode("Empty Keys UI Generator", "3.2.0.0")]
  public class MyPlayerTradeViewModel_LocalPlayerOfferCurrency_PropertyInfo : IPropertyInfo
  {
    public Type PropertyType => typeof (float);

    public bool IsResolved => true;

    public object GetValue(object obj) => (object) ((MyPlayerTradeViewModel) obj).LocalPlayerOfferCurrency;

    public object GetValue(object obj, object[] index) => (object) null;

    public void SetValue(object obj, object value) => ((MyPlayerTradeViewModel) obj).LocalPlayerOfferCurrency = (float) value;

    public void SetValue(object obj, object value, object[] index)
    {
    }
  }
}
