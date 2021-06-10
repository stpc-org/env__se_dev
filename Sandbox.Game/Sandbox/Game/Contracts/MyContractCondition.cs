// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Contracts.MyContractCondition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using VRage.Game.ObjectBuilders.Components.Contracts;

namespace Sandbox.Game.Contracts
{
  public abstract class MyContractCondition
  {
    public long Id { get; private set; }

    public bool IsFinished { get; private set; }

    public long ContractId { get; private set; }

    public long StationEndId { get; private set; }

    public long FactionEndId { get; private set; }

    public long BlockEndId { get; private set; }

    public virtual void Init(MyObjectBuilder_ContractCondition builder)
    {
      this.Id = builder.Id;
      this.IsFinished = builder.IsFinished;
      this.ContractId = builder.ContractId;
      this.StationEndId = builder.StationEndId;
      this.FactionEndId = builder.FactionEndId;
      this.BlockEndId = builder.BlockEndId;
    }

    public virtual MyObjectBuilder_ContractCondition GetObjectBuilder()
    {
      MyObjectBuilder_ContractCondition objectBuilder = MyContractConditionFactory.CreateObjectBuilder(this);
      objectBuilder.Id = this.Id;
      objectBuilder.IsFinished = this.IsFinished;
      objectBuilder.ContractId = this.ContractId;
      objectBuilder.StationEndId = this.StationEndId;
      objectBuilder.FactionEndId = this.FactionEndId;
      objectBuilder.BlockEndId = this.BlockEndId;
      return objectBuilder;
    }

    public bool FinalizeCondition()
    {
      if (MySession.Static == null)
        return false;
      MySessionComponentContractSystem component = MySession.Static.GetComponent<MySessionComponentContractSystem>();
      if (component == null)
        return false;
      MyContract activeContractById = component.GetActiveContractById(this.ContractId);
      if (activeContractById == null || !activeContractById.IsOwnerOfCondition(this))
        return false;
      this.IsFinished = true;
      activeContractById.RecalculateUnfinishedCondiCount();
      return true;
    }

    public virtual string ToDebugString() => "Base condition";
  }
}
