// Decompiled with JetBrains decompiler
// Type: EmptyKeys.UserInterface.Generated.StoreBlockView_Gamepad_Bindings.MyInventoryTargetViewModel_SelectedInventoryIndex_PropertyInfo
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using EmptyKeys.UserInterface.Data;
using Sandbox.Game.Screens.ViewModels;
using System;
using System.CodeDom.Compiler;

namespace EmptyKeys.UserInterface.Generated.StoreBlockView_Gamepad_Bindings
{
  [GeneratedCode("Empty Keys UI Generator", "3.2.0.0")]
  public class MyInventoryTargetViewModel_SelectedInventoryIndex_PropertyInfo : IPropertyInfo
  {
    public Type PropertyType => typeof (int);

    public bool IsResolved => true;

    public object GetValue(object obj) => (object) ((MyInventoryTargetViewModel) obj).SelectedInventoryIndex;

    public object GetValue(object obj, object[] index) => (object) null;

    public void SetValue(object obj, object value) => ((MyInventoryTargetViewModel) obj).SelectedInventoryIndex = (int) value;

    public void SetValue(object obj, object value, object[] index)
    {
    }
  }
}
