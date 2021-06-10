// Decompiled with JetBrains decompiler
// Type: VRage.Generics.StateMachine.MyCondition`1
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System.Collections.Generic;
using System.Text;
using VRage.Library.Utils;
using VRage.Utils;

namespace VRage.Generics.StateMachine
{
  public class MyCondition<T> : IMyCondition where T : struct
  {
    private readonly IMyVariableStorage<T> m_storage;
    private readonly MyCondition<T>.MyOperation m_operation;
    private readonly MyStringId m_leftSideStorage;
    private readonly MyStringId m_rightSideStorage;
    private readonly T m_leftSideValue;
    private readonly T m_rightSideValue;

    public MyCondition(
      IMyVariableStorage<T> storage,
      MyCondition<T>.MyOperation operation,
      string leftSideStorage,
      string rightSideStorage)
    {
      this.m_storage = storage;
      this.m_operation = operation;
      this.m_leftSideStorage = MyStringId.GetOrCompute(leftSideStorage);
      this.m_rightSideStorage = MyStringId.GetOrCompute(rightSideStorage);
    }

    public MyCondition(
      IMyVariableStorage<T> storage,
      MyCondition<T>.MyOperation operation,
      string leftSideStorage,
      T rightSideValue)
    {
      this.m_storage = storage;
      this.m_operation = operation;
      this.m_leftSideStorage = MyStringId.GetOrCompute(leftSideStorage);
      this.m_rightSideStorage = MyStringId.NullOrEmpty;
      this.m_rightSideValue = rightSideValue;
    }

    public MyCondition(
      IMyVariableStorage<T> storage,
      MyCondition<T>.MyOperation operation,
      T leftSideValue,
      string rightSideStorage)
    {
      this.m_storage = storage;
      this.m_operation = operation;
      this.m_leftSideStorage = MyStringId.NullOrEmpty;
      this.m_rightSideStorage = MyStringId.GetOrCompute(rightSideStorage);
      this.m_leftSideValue = leftSideValue;
    }

    public MyCondition(
      IMyVariableStorage<T> storage,
      MyCondition<T>.MyOperation operation,
      T leftSideValue,
      T rightSideValue)
    {
      this.m_storage = storage;
      this.m_operation = operation;
      this.m_leftSideStorage = MyStringId.NullOrEmpty;
      this.m_rightSideStorage = MyStringId.NullOrEmpty;
      this.m_leftSideValue = leftSideValue;
      this.m_rightSideValue = rightSideValue;
    }

    public bool Evaluate()
    {
      T leftSideValue;
      if (this.m_leftSideStorage != MyStringId.NullOrEmpty)
      {
        if (!this.m_storage.GetValue(this.m_leftSideStorage, out leftSideValue))
          return false;
      }
      else
        leftSideValue = this.m_leftSideValue;
      T rightSideValue;
      if (this.m_rightSideStorage != MyStringId.NullOrEmpty)
      {
        if (!this.m_storage.GetValue(this.m_rightSideStorage, out rightSideValue))
          return false;
      }
      else
        rightSideValue = this.m_rightSideValue;
      int num = Comparer<T>.Default.Compare(leftSideValue, rightSideValue);
      switch (this.m_operation)
      {
        case MyCondition<T>.MyOperation.AlwaysFalse:
          return false;
        case MyCondition<T>.MyOperation.AlwaysTrue:
          return true;
        case MyCondition<T>.MyOperation.NotEqual:
          return (uint) num > 0U;
        case MyCondition<T>.MyOperation.Less:
          return num < 0;
        case MyCondition<T>.MyOperation.LessOrEqual:
          return num <= 0;
        case MyCondition<T>.MyOperation.Equal:
          return num == 0;
        case MyCondition<T>.MyOperation.GreaterOrEqual:
          return num >= 0;
        case MyCondition<T>.MyOperation.Greater:
          return num > 0;
        default:
          return false;
      }
    }

    public override string ToString()
    {
      if (this.m_operation == MyCondition<T>.MyOperation.AlwaysTrue)
        return "true";
      if (this.m_operation == MyCondition<T>.MyOperation.AlwaysFalse)
        return "false";
      StringBuilder stringBuilder = new StringBuilder(128);
      if (this.m_leftSideStorage != MyStringId.NullOrEmpty)
        stringBuilder.Append(this.m_leftSideStorage.ToString());
      else
        stringBuilder.Append((object) this.m_leftSideValue);
      stringBuilder.Append(" ");
      switch (this.m_operation)
      {
        case MyCondition<T>.MyOperation.NotEqual:
          stringBuilder.Append("!=");
          break;
        case MyCondition<T>.MyOperation.Less:
          stringBuilder.Append("<");
          break;
        case MyCondition<T>.MyOperation.LessOrEqual:
          stringBuilder.Append("<=");
          break;
        case MyCondition<T>.MyOperation.Equal:
          stringBuilder.Append("==");
          break;
        case MyCondition<T>.MyOperation.GreaterOrEqual:
          stringBuilder.Append(">=");
          break;
        case MyCondition<T>.MyOperation.Greater:
          stringBuilder.Append(">");
          break;
        default:
          stringBuilder.Append("???");
          break;
      }
      stringBuilder.Append(" ");
      if (this.m_rightSideStorage != MyStringId.NullOrEmpty)
        stringBuilder.Append(this.m_rightSideStorage.ToString());
      else
        stringBuilder.Append((object) this.m_rightSideValue);
      return stringBuilder.ToString();
    }

    public enum MyOperation
    {
      AlwaysFalse,
      AlwaysTrue,
      NotEqual,
      Less,
      LessOrEqual,
      Equal,
      GreaterOrEqual,
      Greater,
    }
  }
}
