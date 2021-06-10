// Decompiled with JetBrains decompiler
// Type: VRage.Game.Entity.MyComponentChange
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

namespace VRage.Game.Entity
{
  public struct MyComponentChange
  {
    private const int OPERATION_REMOVAL = 0;
    private const int OPERATION_ADDITION = 1;
    private const int OPERATION_CHANGE = 2;
    private byte m_operation;
    private MyDefinitionId m_toRemove;
    private MyDefinitionId m_toAdd;
    public int Amount;

    public bool IsRemoval() => this.m_operation == (byte) 0;

    public bool IsAddition() => this.m_operation == (byte) 1;

    public bool IsChange() => this.m_operation == (byte) 2;

    public MyDefinitionId ToRemove
    {
      get => this.m_toRemove;
      set => this.m_toRemove = value;
    }

    public MyDefinitionId ToAdd
    {
      get => this.m_toAdd;
      set => this.m_toAdd = value;
    }

    public static MyComponentChange CreateRemoval(
      MyDefinitionId toRemove,
      int amount)
    {
      return new MyComponentChange()
      {
        ToRemove = toRemove,
        Amount = amount,
        m_operation = 0
      };
    }

    public static MyComponentChange CreateAddition(
      MyDefinitionId toAdd,
      int amount)
    {
      return new MyComponentChange()
      {
        ToAdd = toAdd,
        Amount = amount,
        m_operation = 1
      };
    }

    public static MyComponentChange CreateChange(
      MyDefinitionId toRemove,
      MyDefinitionId toAdd,
      int amount)
    {
      return new MyComponentChange()
      {
        ToRemove = toRemove,
        ToAdd = toAdd,
        Amount = amount,
        m_operation = 2
      };
    }
  }
}
