// Decompiled with JetBrains decompiler
// Type: EmptyKeys.UserInterface.Generated.WorkshopBrowserView_Bindings.MyWorkshopBrowserViewModel_WorkshopItems_PropertyInfo
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using EmptyKeys.UserInterface.Data;
using Sandbox.Game.Screens.Models;
using Sandbox.Game.Screens.ViewModels;
using System;
using System.CodeDom.Compiler;
using System.Collections.ObjectModel;

namespace EmptyKeys.UserInterface.Generated.WorkshopBrowserView_Bindings
{
  [GeneratedCode("Empty Keys UI Generator", "3.2.0.0")]
  public class MyWorkshopBrowserViewModel_WorkshopItems_PropertyInfo : IPropertyInfo
  {
    public Type PropertyType => typeof (ObservableCollection<MyWorkshopItemModel>);

    public bool IsResolved => true;

    public object GetValue(object obj) => (object) ((MyWorkshopBrowserViewModel) obj).WorkshopItems;

    public object GetValue(object obj, object[] index) => (object) ((MyWorkshopBrowserViewModel) obj).WorkshopItems[(int) index[0]];

    public void SetValue(object obj, object value) => ((MyWorkshopBrowserViewModel) obj).WorkshopItems = (ObservableCollection<MyWorkshopItemModel>) value;

    public void SetValue(object obj, object value, object[] index) => ((MyWorkshopBrowserViewModel) obj).WorkshopItems[(int) index[0]] = (MyWorkshopItemModel) value;
  }
}
