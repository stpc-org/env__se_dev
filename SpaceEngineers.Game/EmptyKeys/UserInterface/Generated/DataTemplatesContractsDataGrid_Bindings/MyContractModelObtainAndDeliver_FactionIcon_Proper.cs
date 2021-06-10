// Decompiled with JetBrains decompiler
// Type: EmptyKeys.UserInterface.Generated.DataTemplatesContractsDataGrid_Bindings.MyContractModelObtainAndDeliver_FactionIcon_PropertyInfo
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using EmptyKeys.UserInterface.Data;
using EmptyKeys.UserInterface.Media.Imaging;
using Sandbox.Game.Screens.Models;
using System;
using System.CodeDom.Compiler;

namespace EmptyKeys.UserInterface.Generated.DataTemplatesContractsDataGrid_Bindings
{
  [GeneratedCode("Empty Keys UI Generator", "3.2.0.0")]
  public class MyContractModelObtainAndDeliver_FactionIcon_PropertyInfo : IPropertyInfo
  {
    public Type PropertyType => typeof (BitmapImage);

    public bool IsResolved => true;

    public object GetValue(object obj) => (object) ((MyContractModel) obj).FactionIcon;

    public object GetValue(object obj, object[] index) => (object) null;

    public void SetValue(object obj, object value) => ((MyContractModel) obj).FactionIcon = (BitmapImage) value;

    public void SetValue(object obj, object value, object[] index)
    {
    }
  }
}
