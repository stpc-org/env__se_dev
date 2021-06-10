// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiScreenBoard
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Network;
using VRage.Serialization;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
  public class MyGuiScreenBoard : MyGuiScreenBase
  {
    private MyGuiControlTable m_boardTable;
    private readonly StringBuilder m_textCache = new StringBuilder();
    private Dictionary<string, MyGuiScreenBoard.MyColumn> m_columns = new Dictionary<string, MyGuiScreenBoard.MyColumn>();
    private List<string> m_indexToColumnIdMap = new List<string>();
    private Dictionary<string, MyGuiScreenBoard.MyRow> m_rows = new Dictionary<string, MyGuiScreenBoard.MyRow>();
    private string m_sortByColumn;
    private bool m_sortAscending = true;
    private string m_showOrderColumn;
    private Vector2 m_normalizedSize;
    private Vector2 m_normalizedCoord;

    public MyGuiScreenBoard(Vector2 normalizedCoord, Vector2 localOffset, Vector2 size)
      : base(new Vector2?(normalizedCoord + localOffset), size: new Vector2?(size), backgroundTransition: MySandboxGame.Config.UIBkOpacity, guiTransition: MySandboxGame.Config.UIOpacity)
    {
      this.m_normalizedSize = size;
      this.m_normalizedCoord = normalizedCoord;
      this.RecreateControls(true);
    }

    public override string GetFriendlyName() => nameof (MyGuiScreenBoard);

    protected override void OnClosed() => base.OnClosed();

    private int TextComparison(MyGuiControlTable.Cell a, MyGuiControlTable.Cell b)
    {
      if (this.m_sortByColumn == null)
      {
        MyGuiScreenBoard.MyRow userData1 = (MyGuiScreenBoard.MyRow) a.Row.UserData;
        MyGuiScreenBoard.MyRow userData2 = (MyGuiScreenBoard.MyRow) b.Row.UserData;
        return !this.m_sortAscending ? userData2.Ranking.CompareTo(userData1.Ranking) : userData1.Ranking.CompareTo(userData2.Ranking);
      }
      return !this.m_sortAscending ? b.Text.CompareToIgnoreCase(a.Text) : a.Text.CompareToIgnoreCase(b.Text);
    }

    private void Clear()
    {
      this.m_columns.Clear();
      this.m_indexToColumnIdMap.Clear();
      this.m_rows.Clear();
      this.m_sortByColumn = (string) null;
      this.m_sortAscending = true;
      this.m_showOrderColumn = (string) null;
    }

    public void Init(MyObjectBuilder_BoardScreen ob)
    {
      this.Clear();
      this.m_sortByColumn = ob.SortByColumn;
      this.m_showOrderColumn = ob.ShowOrderColumn;
      this.m_sortAscending = ob.SortAscending;
      foreach (MyObjectBuilder_BoardScreen.BoardColumn column in ob.Columns)
      {
        this.AddColumn(column.Id, column.Width, column.HeaderText, column.HeaderDrawAlign, column.ColumnDrawAlign);
        if (!column.Visible)
          this.SetColumnVisibility(column.Id, false);
      }
      this.m_indexToColumnIdMap = ob.ColumnSort != null ? ((IEnumerable<string>) ob.ColumnSort).ToList<string>() : new List<string>();
      foreach (MyObjectBuilder_BoardScreen.BoardRow row in ob.Rows)
      {
        this.AddRow(row.Id);
        this.SetRowRanking(row.Id, row.Ranking);
        foreach (KeyValuePair<string, string> keyValuePair in row.Cells.Dictionary)
          this.SetCell(row.Id, keyValuePair.Key, keyValuePair.Value);
      }
      this.Sort();
    }

    public MyObjectBuilder_BoardScreen GetBoardObjectBuilder(string id)
    {
      MyObjectBuilder_BoardScreen builderBoardScreen = new MyObjectBuilder_BoardScreen();
      builderBoardScreen.Id = id;
      builderBoardScreen.SortByColumn = this.m_sortByColumn;
      builderBoardScreen.ShowOrderColumn = this.m_showOrderColumn;
      builderBoardScreen.SortAscending = this.m_sortAscending;
      builderBoardScreen.Coords = (SerializableVector2) this.m_normalizedCoord;
      builderBoardScreen.Size = (SerializableVector2) this.m_normalizedSize;
      builderBoardScreen.ColumnSort = this.m_indexToColumnIdMap.ToArray();
      builderBoardScreen.Columns = this.m_columns.Select<KeyValuePair<string, MyGuiScreenBoard.MyColumn>, MyObjectBuilder_BoardScreen.BoardColumn>((Func<KeyValuePair<string, MyGuiScreenBoard.MyColumn>, MyObjectBuilder_BoardScreen.BoardColumn>) (x => new MyObjectBuilder_BoardScreen.BoardColumn()
      {
        Id = x.Key,
        Width = x.Value.Width,
        ColumnDrawAlign = x.Value.ColumnDrawAlign,
        HeaderDrawAlign = x.Value.HeaderDrawAlign,
        HeaderText = x.Value.HeaderText,
        Visible = x.Value.Visible
      })).ToArray<MyObjectBuilder_BoardScreen.BoardColumn>();
      List<MyObjectBuilder_BoardScreen.BoardRow> boardRowList = new List<MyObjectBuilder_BoardScreen.BoardRow>();
      foreach (KeyValuePair<string, MyGuiScreenBoard.MyRow> row in this.m_rows)
      {
        MyObjectBuilder_BoardScreen.BoardRow boardRow = new MyObjectBuilder_BoardScreen.BoardRow()
        {
          Id = row.Key,
          Ranking = row.Value.Ranking
        };
        boardRow.Cells = new SerializableDictionary<string, string>();
        foreach (KeyValuePair<string, string> cell in row.Value.Cells)
          boardRow.Cells.Dictionary.Add(cell.Key, cell.Value);
        boardRowList.Add(boardRow);
      }
      builderBoardScreen.Rows = boardRowList.ToArray();
      return builderBoardScreen;
    }

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.CloseButtonEnabled = false;
      this.CanHaveFocus = false;
      this.CanBeHidden = false;
      this.CanHideOthers = false;
      this.m_boardTable = new MyGuiControlTable(false);
      this.m_boardTable.Position = new Vector2(0.0f, 0.0f);
      this.m_boardTable.Size = this.m_size.Value;
      this.m_boardTable.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      this.m_boardTable.VisibleRowsCount = 1;
      this.m_boardTable.ColumnLinesVisible = true;
      this.m_boardTable.RowLinesVisible = true;
      this.m_boardTable.BackgroundTexture = MyGuiConstants.TEXTURE_HIGHLIGHT_DARK;
      this.m_boardTable.ColorMask = new Vector4(1f, 1f, 1f, 0.5f);
      this.Controls.Add((MyGuiControlBase) this.m_boardTable);
    }

    public void AddColumn(
      string columnId,
      float width,
      string headerText,
      MyGuiDrawAlignEnum headerDrawAlign,
      MyGuiDrawAlignEnum columnDrawAlign)
    {
      MyGuiScreenBoard.MyColumn column = new MyGuiScreenBoard.MyColumn()
      {
        Width = width,
        HeaderText = headerText,
        HeaderDrawAlign = headerDrawAlign,
        ColumnDrawAlign = columnDrawAlign,
        Visible = true
      };
      this.AddColumn(columnId, column);
    }

    public void AddColumn(string columnId, MyGuiScreenBoard.MyColumn column)
    {
      this.m_columns[columnId] = column;
      if (!this.m_indexToColumnIdMap.Contains(columnId))
        this.m_indexToColumnIdMap.Add(columnId);
      this.UpdateColumns();
      this.UpdateRows();
    }

    public void RemoveColumn(string columnId)
    {
      this.m_indexToColumnIdMap.RemoveAt(this.m_indexToColumnIdMap.FindIndex((Predicate<string>) (x => x == columnId)));
      this.m_columns.Remove(columnId);
      if (columnId == this.m_showOrderColumn)
        this.m_showOrderColumn = (string) null;
      if (columnId == this.m_sortByColumn)
        this.m_sortByColumn = (string) null;
      this.UpdateColumns();
      this.UpdateRows();
      this.Sort();
    }

    public void AddRow(string rowId)
    {
      this.m_rows[rowId] = new MyGuiScreenBoard.MyRow();
      this.UpdateRows();
      this.Sort();
    }

    public void RemoveRow(string rowId)
    {
      this.m_rows.Remove(rowId);
      this.UpdateRows();
      this.Sort();
    }

    private void UpdateRows()
    {
      this.m_boardTable.Clear();
      foreach (KeyValuePair<string, MyGuiScreenBoard.MyRow> row1 in this.m_rows)
      {
        MyGuiControlTable.Row row2 = new MyGuiControlTable.Row((object) row1.Value);
        for (int index = 0; index < this.m_indexToColumnIdMap.Count; ++index)
        {
          string indexToColumnId = this.m_indexToColumnIdMap[index];
          string str = row1.Value.Cells.ContainsKey(indexToColumnId) ? row1.Value.Cells[indexToColumnId] : "";
          row2.AddCell(new MyGuiControlTable.Cell(new StringBuilder().Append(str), toolTip: ""));
        }
        this.m_boardTable.Add(row2);
      }
      this.m_boardTable.VisibleRowsCount = this.m_rows.Count;
    }

    public void SetCell(string rowId, string columnId, string text)
    {
      MyGuiScreenBoard.MyRow myRow;
      if (this.m_rows.TryGetValue(rowId, out myRow))
        myRow.Cells[columnId] = text;
      this.UpdateRows();
      this.Sort();
    }

    public void SetRowRanking(string rowId, int ranking)
    {
      MyGuiScreenBoard.MyRow myRow;
      if (this.m_rows.TryGetValue(rowId, out myRow))
        myRow.Ranking = ranking;
      this.Sort();
    }

    public void SortByColumn(string columnId, bool ascending)
    {
      this.m_sortByColumn = columnId;
      this.m_sortAscending = ascending;
      this.Sort();
    }

    public void SortByRanking(bool ascending)
    {
      this.m_sortByColumn = (string) null;
      this.m_sortAscending = ascending;
      this.Sort();
    }

    public void ShowOrderInColumn(string columnId)
    {
      this.m_showOrderColumn = columnId;
      this.UpdateRows();
      this.Sort();
    }

    public void SetColumnVisibility(string columnId, bool visible)
    {
      MyGuiScreenBoard.MyColumn myColumn1;
      if (this.m_columns.TryGetValue(columnId, out myColumn1))
      {
        MyGuiScreenBoard.MyColumn myColumn2 = myColumn1;
        myColumn2.Visible = visible;
        this.m_columns[columnId] = myColumn2;
      }
      this.UpdateColumns();
    }

    private void Sort()
    {
      if (this.m_sortByColumn != null)
        this.m_boardTable.SortByColumn(this.m_indexToColumnIdMap.FindIndex((Predicate<string>) (x => x == this.m_sortByColumn)), new MyGuiControlTable.SortStateEnum?(this.m_sortAscending ? MyGuiControlTable.SortStateEnum.Ascending : MyGuiControlTable.SortStateEnum.Descending));
      else if (this.m_columns.Count > 0)
        this.m_boardTable.SortByColumn(0, new MyGuiControlTable.SortStateEnum?(this.m_sortAscending ? MyGuiControlTable.SortStateEnum.Ascending : MyGuiControlTable.SortStateEnum.Descending));
      if (this.m_showOrderColumn == null)
        return;
      int index1 = this.m_indexToColumnIdMap.FindIndex((Predicate<string>) (x => x == this.m_showOrderColumn));
      for (int index2 = 0; index2 < this.m_boardTable.RowsCount; ++index2)
        this.m_boardTable.GetRow(index2).GetCell(index1).Text.Clear().Append(index2 + 1);
    }

    private void UpdateColumns()
    {
      this.m_boardTable.ColumnsCount = this.m_columns.Count;
      this.m_boardTable.SetCustomColumnWidths(this.m_columns.Values.Select<MyGuiScreenBoard.MyColumn, float>((Func<MyGuiScreenBoard.MyColumn, float>) (x => x.Width)).ToArray<float>());
      for (int index = 0; index < this.m_indexToColumnIdMap.Count; ++index)
      {
        string indexToColumnId = this.m_indexToColumnIdMap[index];
        this.m_boardTable.SetHeaderColumnAlign(index, this.m_columns[indexToColumnId].HeaderDrawAlign);
        this.m_boardTable.SetColumnAlign(index, this.m_columns[indexToColumnId].ColumnDrawAlign);
        this.m_boardTable.SetColumnName(index, new StringBuilder().Append(this.m_columns[indexToColumnId].HeaderText));
        this.m_boardTable.SetColumnComparison(index, new Comparison<MyGuiControlTable.Cell>(this.TextComparison));
        this.m_boardTable.SetColumnVisibility(index, this.m_columns[indexToColumnId].Visible);
      }
    }

    [Serializable]
    public struct MyColumn
    {
      public string HeaderText;
      public MyGuiDrawAlignEnum HeaderDrawAlign;
      public MyGuiDrawAlignEnum ColumnDrawAlign;
      public float Width;
      public bool Visible;

      protected class Sandbox_Game_Gui_MyGuiScreenBoard\u003C\u003EMyColumn\u003C\u003EHeaderText\u003C\u003EAccessor : IMemberAccessor<MyGuiScreenBoard.MyColumn, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyGuiScreenBoard.MyColumn owner, in string value) => owner.HeaderText = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyGuiScreenBoard.MyColumn owner, out string value) => value = owner.HeaderText;
      }

      protected class Sandbox_Game_Gui_MyGuiScreenBoard\u003C\u003EMyColumn\u003C\u003EHeaderDrawAlign\u003C\u003EAccessor : IMemberAccessor<MyGuiScreenBoard.MyColumn, MyGuiDrawAlignEnum>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyGuiScreenBoard.MyColumn owner, in MyGuiDrawAlignEnum value) => owner.HeaderDrawAlign = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyGuiScreenBoard.MyColumn owner, out MyGuiDrawAlignEnum value) => value = owner.HeaderDrawAlign;
      }

      protected class Sandbox_Game_Gui_MyGuiScreenBoard\u003C\u003EMyColumn\u003C\u003EColumnDrawAlign\u003C\u003EAccessor : IMemberAccessor<MyGuiScreenBoard.MyColumn, MyGuiDrawAlignEnum>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyGuiScreenBoard.MyColumn owner, in MyGuiDrawAlignEnum value) => owner.ColumnDrawAlign = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyGuiScreenBoard.MyColumn owner, out MyGuiDrawAlignEnum value) => value = owner.ColumnDrawAlign;
      }

      protected class Sandbox_Game_Gui_MyGuiScreenBoard\u003C\u003EMyColumn\u003C\u003EWidth\u003C\u003EAccessor : IMemberAccessor<MyGuiScreenBoard.MyColumn, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyGuiScreenBoard.MyColumn owner, in float value) => owner.Width = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyGuiScreenBoard.MyColumn owner, out float value) => value = owner.Width;
      }

      protected class Sandbox_Game_Gui_MyGuiScreenBoard\u003C\u003EMyColumn\u003C\u003EVisible\u003C\u003EAccessor : IMemberAccessor<MyGuiScreenBoard.MyColumn, bool>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyGuiScreenBoard.MyColumn owner, in bool value) => owner.Visible = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyGuiScreenBoard.MyColumn owner, out bool value) => value = owner.Visible;
      }
    }

    private class MyRow
    {
      public Dictionary<string, string> Cells;
      public int Ranking;

      public MyRow() => this.Cells = new Dictionary<string, string>();
    }
  }
}
