// Decompiled with JetBrains decompiler
// Type: EmptyKeys.UserInterface.Generated.WorkshopBrowserView_Bindings.MyWorkshopItemModel_Rating_PropertyInfo
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using EmptyKeys.UserInterface.Data;
using EmptyKeys.UserInterface.Media.Imaging;
using Sandbox.Game.Screens.Models;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;

namespace EmptyKeys.UserInterface.Generated.WorkshopBrowserView_Bindings
{
  [GeneratedCode("Empty Keys UI Generator", "3.2.0.0")]
  public class MyWorkshopItemModel_Rating_PropertyInfo : IPropertyInfo
  {
    public Type PropertyType => typeof (List<BitmapImage>);

    public bool IsResolved => true;

    public object GetValue(object obj) => (object) ((MyWorkshopItemModel) obj).Rating;

    public object GetValue(object obj, object[] index) => (object) ((MyWorkshopItemModel) obj).Rating[(int) index[0]];

    public void SetValue(object obj, object value) => ((MyWorkshopItemModel) obj).Rating = (List<BitmapImage>) value;

    public void SetValue(object obj, object value, object[] index) => ((MyWorkshopItemModel) obj).Rating[(int) index[0]] = (BitmapImage) value;
  }
}
