// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_BoardScreen
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;

namespace VRage.Game
{
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  [ProtoContract]
  public class MyObjectBuilder_BoardScreen : MyObjectBuilder_SessionComponent
  {
    [ProtoMember(5)]
    public string Id;
    [ProtoMember(6)]
    public SerializableVector2 Coords;
    [ProtoMember(7)]
    public SerializableVector2 Size;
    [Nullable]
    [ProtoMember(10)]
    public string SortByColumn;
    [Nullable]
    [ProtoMember(20)]
    public string ShowOrderColumn;
    [ProtoMember(30)]
    public bool SortAscending;
    [XmlArray("Columns", IsNullable = true)]
    [ProtoMember(40)]
    public MyObjectBuilder_BoardScreen.BoardColumn[] Columns;
    [XmlArray("Rows", IsNullable = true)]
    [ProtoMember(50)]
    public MyObjectBuilder_BoardScreen.BoardRow[] Rows;
    [XmlArray("ColumnSort", IsNullable = true)]
    [ProtoMember(60)]
    public string[] ColumnSort;

    [ProtoContract]
    public struct BoardColumn
    {
      [ProtoMember(50)]
      public string Id;
      [ProtoMember(55)]
      public float Width;
      [ProtoMember(60)]
      public string HeaderText;
      [ProtoMember(65)]
      public MyGuiDrawAlignEnum HeaderDrawAlign;
      [ProtoMember(70)]
      public MyGuiDrawAlignEnum ColumnDrawAlign;
      [ProtoMember(80)]
      public bool Visible;

      protected class VRage_Game_MyObjectBuilder_BoardScreen\u003C\u003EBoardColumn\u003C\u003EId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_BoardScreen.BoardColumn, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyObjectBuilder_BoardScreen.BoardColumn owner, in string value) => owner.Id = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyObjectBuilder_BoardScreen.BoardColumn owner, out string value) => value = owner.Id;
      }

      protected class VRage_Game_MyObjectBuilder_BoardScreen\u003C\u003EBoardColumn\u003C\u003EWidth\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_BoardScreen.BoardColumn, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyObjectBuilder_BoardScreen.BoardColumn owner, in float value) => owner.Width = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyObjectBuilder_BoardScreen.BoardColumn owner, out float value) => value = owner.Width;
      }

      protected class VRage_Game_MyObjectBuilder_BoardScreen\u003C\u003EBoardColumn\u003C\u003EHeaderText\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_BoardScreen.BoardColumn, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyObjectBuilder_BoardScreen.BoardColumn owner, in string value) => owner.HeaderText = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyObjectBuilder_BoardScreen.BoardColumn owner, out string value) => value = owner.HeaderText;
      }

      protected class VRage_Game_MyObjectBuilder_BoardScreen\u003C\u003EBoardColumn\u003C\u003EHeaderDrawAlign\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_BoardScreen.BoardColumn, MyGuiDrawAlignEnum>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_BoardScreen.BoardColumn owner,
          in MyGuiDrawAlignEnum value)
        {
          owner.HeaderDrawAlign = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_BoardScreen.BoardColumn owner,
          out MyGuiDrawAlignEnum value)
        {
          value = owner.HeaderDrawAlign;
        }
      }

      protected class VRage_Game_MyObjectBuilder_BoardScreen\u003C\u003EBoardColumn\u003C\u003EColumnDrawAlign\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_BoardScreen.BoardColumn, MyGuiDrawAlignEnum>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_BoardScreen.BoardColumn owner,
          in MyGuiDrawAlignEnum value)
        {
          owner.ColumnDrawAlign = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_BoardScreen.BoardColumn owner,
          out MyGuiDrawAlignEnum value)
        {
          value = owner.ColumnDrawAlign;
        }
      }

      protected class VRage_Game_MyObjectBuilder_BoardScreen\u003C\u003EBoardColumn\u003C\u003EVisible\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_BoardScreen.BoardColumn, bool>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyObjectBuilder_BoardScreen.BoardColumn owner, in bool value) => owner.Visible = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyObjectBuilder_BoardScreen.BoardColumn owner, out bool value) => value = owner.Visible;
      }

      private class VRage_Game_MyObjectBuilder_BoardScreen\u003C\u003EBoardColumn\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_BoardScreen.BoardColumn>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_BoardScreen.BoardColumn();

        MyObjectBuilder_BoardScreen.BoardColumn IActivator<MyObjectBuilder_BoardScreen.BoardColumn>.CreateInstance() => new MyObjectBuilder_BoardScreen.BoardColumn();
      }
    }

    [ProtoContract]
    public struct BoardRow
    {
      [ProtoMember(150)]
      public string Id;
      [ProtoMember(160)]
      public int Ranking;
      [ProtoMember(170)]
      public SerializableDictionary<string, string> Cells;

      protected class VRage_Game_MyObjectBuilder_BoardScreen\u003C\u003EBoardRow\u003C\u003EId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_BoardScreen.BoardRow, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyObjectBuilder_BoardScreen.BoardRow owner, in string value) => owner.Id = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyObjectBuilder_BoardScreen.BoardRow owner, out string value) => value = owner.Id;
      }

      protected class VRage_Game_MyObjectBuilder_BoardScreen\u003C\u003EBoardRow\u003C\u003ERanking\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_BoardScreen.BoardRow, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyObjectBuilder_BoardScreen.BoardRow owner, in int value) => owner.Ranking = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyObjectBuilder_BoardScreen.BoardRow owner, out int value) => value = owner.Ranking;
      }

      protected class VRage_Game_MyObjectBuilder_BoardScreen\u003C\u003EBoardRow\u003C\u003ECells\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_BoardScreen.BoardRow, SerializableDictionary<string, string>>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_BoardScreen.BoardRow owner,
          in SerializableDictionary<string, string> value)
        {
          owner.Cells = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_BoardScreen.BoardRow owner,
          out SerializableDictionary<string, string> value)
        {
          value = owner.Cells;
        }
      }

      private class VRage_Game_MyObjectBuilder_BoardScreen\u003C\u003EBoardRow\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_BoardScreen.BoardRow>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_BoardScreen.BoardRow();

        MyObjectBuilder_BoardScreen.BoardRow IActivator<MyObjectBuilder_BoardScreen.BoardRow>.CreateInstance() => new MyObjectBuilder_BoardScreen.BoardRow();
      }
    }

    protected class VRage_Game_MyObjectBuilder_BoardScreen\u003C\u003EId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_BoardScreen, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_BoardScreen owner, in string value) => owner.Id = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_BoardScreen owner, out string value) => value = owner.Id;
    }

    protected class VRage_Game_MyObjectBuilder_BoardScreen\u003C\u003ECoords\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_BoardScreen, SerializableVector2>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_BoardScreen owner, in SerializableVector2 value) => owner.Coords = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_BoardScreen owner, out SerializableVector2 value) => value = owner.Coords;
    }

    protected class VRage_Game_MyObjectBuilder_BoardScreen\u003C\u003ESize\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_BoardScreen, SerializableVector2>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_BoardScreen owner, in SerializableVector2 value) => owner.Size = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_BoardScreen owner, out SerializableVector2 value) => value = owner.Size;
    }

    protected class VRage_Game_MyObjectBuilder_BoardScreen\u003C\u003ESortByColumn\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_BoardScreen, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_BoardScreen owner, in string value) => owner.SortByColumn = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_BoardScreen owner, out string value) => value = owner.SortByColumn;
    }

    protected class VRage_Game_MyObjectBuilder_BoardScreen\u003C\u003EShowOrderColumn\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_BoardScreen, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_BoardScreen owner, in string value) => owner.ShowOrderColumn = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_BoardScreen owner, out string value) => value = owner.ShowOrderColumn;
    }

    protected class VRage_Game_MyObjectBuilder_BoardScreen\u003C\u003ESortAscending\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_BoardScreen, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_BoardScreen owner, in bool value) => owner.SortAscending = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_BoardScreen owner, out bool value) => value = owner.SortAscending;
    }

    protected class VRage_Game_MyObjectBuilder_BoardScreen\u003C\u003EColumns\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_BoardScreen, MyObjectBuilder_BoardScreen.BoardColumn[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_BoardScreen owner,
        in MyObjectBuilder_BoardScreen.BoardColumn[] value)
      {
        owner.Columns = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_BoardScreen owner,
        out MyObjectBuilder_BoardScreen.BoardColumn[] value)
      {
        value = owner.Columns;
      }
    }

    protected class VRage_Game_MyObjectBuilder_BoardScreen\u003C\u003ERows\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_BoardScreen, MyObjectBuilder_BoardScreen.BoardRow[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_BoardScreen owner,
        in MyObjectBuilder_BoardScreen.BoardRow[] value)
      {
        owner.Rows = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_BoardScreen owner,
        out MyObjectBuilder_BoardScreen.BoardRow[] value)
      {
        value = owner.Rows;
      }
    }

    protected class VRage_Game_MyObjectBuilder_BoardScreen\u003C\u003EColumnSort\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_BoardScreen, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_BoardScreen owner, in string[] value) => owner.ColumnSort = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_BoardScreen owner, out string[] value) => value = owner.ColumnSort;
    }

    protected class VRage_Game_MyObjectBuilder_BoardScreen\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_BoardScreen, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_BoardScreen owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_BoardScreen owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_BoardScreen\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_BoardScreen, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_BoardScreen owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_BoardScreen owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_BoardScreen\u003C\u003EDefinition\u003C\u003EAccessor : MyObjectBuilder_SessionComponent.VRage_Game_MyObjectBuilder_SessionComponent\u003C\u003EDefinition\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_BoardScreen, SerializableDefinitionId?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_BoardScreen owner,
        in SerializableDefinitionId? value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_SessionComponent&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_BoardScreen owner,
        out SerializableDefinitionId? value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_SessionComponent&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_BoardScreen\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_BoardScreen, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_BoardScreen owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_BoardScreen owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_BoardScreen\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_BoardScreen, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_BoardScreen owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_BoardScreen owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_BoardScreen\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_BoardScreen>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_BoardScreen();

      MyObjectBuilder_BoardScreen IActivator<MyObjectBuilder_BoardScreen>.CreateInstance() => new MyObjectBuilder_BoardScreen();
    }
  }
}
