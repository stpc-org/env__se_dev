// Decompiled with JetBrains decompiler
// Type: VRage.Game.ModAPI.IMyFaction
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using VRage.Collections;
using VRage.Utils;
using VRageMath;

namespace VRage.Game.ModAPI
{
  public interface IMyFaction
  {
    long FactionId { get; }

    string Tag { get; }

    string Name { get; }

    string Description { get; }

    string PrivateInfo { get; }

    int Score { get; set; }

    float ObjectivePercentageCompleted { get; set; }

    MyStringId? FactionIcon { get; }

    bool AutoAcceptMember { get; }

    bool AutoAcceptPeace { get; }

    bool AcceptHumans { get; }

    long FounderId { get; }

    Vector3 CustomColor { get; }

    Vector3 IconColor { get; }

    bool IsFounder(long playerId);

    bool IsLeader(long playerId);

    bool IsMember(long playerId);

    bool IsNeutral(long playerId);

    bool IsEnemy(long playerId);

    bool IsFriendly(long playerId);

    bool IsEveryoneNpc();

    DictionaryReader<long, MyFactionMember> Members { get; }

    DictionaryReader<long, MyFactionMember> JoinRequests { get; }

    bool TryGetBalanceInfo(out long balance);

    string GetBalanceShortString();

    void RequestChangeBalance(long amount);
  }
}
