﻿// Decompiled with JetBrains decompiler
// Type: EmptyKeys.UserInterface.Generated.DataTemplatesContractsDataGrid_Bindings.MyContractModelEscort_FactionIconBackgroundColor_PropertyInfo
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using EmptyKeys.UserInterface.Data;
using EmptyKeys.UserInterface.Media;
using Sandbox.Game.Screens.Models;
using System;
using System.CodeDom.Compiler;

namespace EmptyKeys.UserInterface.Generated.DataTemplatesContractsDataGrid_Bindings
{
  [GeneratedCode("Empty Keys UI Generator", "3.2.0.0")]
  public class MyContractModelEscort_FactionIconBackgroundColor_PropertyInfo : IPropertyInfo
  {
    public Type PropertyType => typeof (ColorW);

    public bool IsResolved => true;

    public object GetValue(object obj) => (object) ((MyContractModel) obj).FactionIconBackgroundColor;

    public object GetValue(object obj, object[] index) => (object) null;

    public void SetValue(object obj, object value) => ((MyContractModel) obj).FactionIconBackgroundColor = (ColorW) value;

    public void SetValue(object obj, object value, object[] index)
    {
    }
  }
}
