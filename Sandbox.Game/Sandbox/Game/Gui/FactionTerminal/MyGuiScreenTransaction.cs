// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.FactionTerminal.MyGuiScreenTransaction
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.GameSystems.BankingAndCurrency;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using VRage;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui.FactionTerminal
{
  public class MyGuiScreenTransaction : MyGuiScreenBase
  {
    private MyTransactionType m_transactionType;
    private long m_currentBalance;
    private long m_finalBalance;
    private long m_fromIdentifier;
    private long m_toIdentifier;
    private bool m_invalidInput;
    private MyGuiControlLabel m_labelFinalBalanceValue;
    private MyGuiControlButton m_btnOk;
    private MyGuiControlTextbox m_textboxAmount;

    public MyGuiScreenTransaction(
      MyTransactionType transactionType,
      long fromIdentifier,
      long toIdentifier)
      : base()
    {
      this.m_transactionType = transactionType;
      this.BackgroundColor = new Vector4?(MyGuiConstants.SCREEN_BACKGROUND_COLOR);
      this.CanHideOthers = false;
      MyAccountInfo account;
      MyBankingSystem.Static.TryGetAccountInfo(fromIdentifier, out account);
      this.m_currentBalance = account.Balance;
      this.m_finalBalance = account.Balance;
      this.m_fromIdentifier = fromIdentifier;
      this.m_toIdentifier = toIdentifier;
      this.RecreateControls(true);
    }

    public override string GetFriendlyName() => "Transaction Form";

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.Size = new Vector2?(new Vector2(0.5f, 0.712f));
      Vector2 vector2_1 = new Vector2((float) (-(double) this.m_size.Value.X * 0.5), (float) (-(double) this.m_size.Value.Y * 0.5));
      Vector2 vector2_2 = new Vector2((float) (-(double) this.m_size.Value.X * 0.5), this.m_size.Value.Y * 0.5f);
      Vector2 vector2_3 = new Vector2(this.m_size.Value.X * 0.5f, this.m_size.Value.Y * 0.5f);
      Vector2 vector2_4 = new Vector2(this.m_size.Value.X * 0.5f, (float) (-(double) this.m_size.Value.Y * 0.5));
      this.CloseButtonEnabled = true;
      this.CloseButtonStyle = MyGuiControlButtonStyleEnum.Close;
      if (this.m_transactionType == MyTransactionType.Withdraw)
        this.AddCaption(MySpaceTexts.FactionTerminal_Withdraw_Currency, captionOffset: new Vector2?(new Vector2(0.0f, -0.01f)));
      else if (this.m_transactionType == MyTransactionType.Deposit)
        this.AddCaption(MySpaceTexts.FactionTerminal_Deposit_Currency, captionOffset: new Vector2?(new Vector2(0.0f, -0.01f)));
      float x = 0.05f;
      float y = 0.02f;
      Vector2 vector2_5 = new Vector2(x, 0.062f);
      Vector2 start1 = vector2_1 + vector2_5;
      MyGuiControlSeparatorList controlSeparatorList1 = new MyGuiControlSeparatorList();
      controlSeparatorList1.AddHorizontal(start1, this.m_size.Value.X - 2f * x);
      this.Controls.Add((MyGuiControlBase) controlSeparatorList1);
      Vector2 vector2_6 = new Vector2(0.13f, 0.04f);
      MyGuiControlSeparatorList controlSeparatorList2 = new MyGuiControlSeparatorList();
      controlSeparatorList2.AddHorizontal(vector2_2 + new Vector2(x, (float) (-(double) vector2_6.Y - 2.0 * (double) y)), this.m_size.Value.X - 2f * x);
      this.Controls.Add((MyGuiControlBase) controlSeparatorList2);
      float num1 = -1f / 500f;
      MyGuiControlButton guiControlButton = new MyGuiControlButton();
      guiControlButton.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM;
      guiControlButton.Position = vector2_3 + new Vector2(-x - num1, -y);
      guiControlButton.VisualStyle = MyGuiControlButtonStyleEnum.Rectangular;
      guiControlButton.Size = vector2_6;
      guiControlButton.TextEnum = MySpaceTexts.Transaction_Form_Ok_Btn;
      this.m_btnOk = guiControlButton;
      this.m_btnOk.SetTooltip(MyTexts.GetString(MySpaceTexts.Transaction_Form_Ok_Btn_TTIP));
      this.m_btnOk.ButtonClicked += new Action<MyGuiControlButton>(this.OnButtonOkPressed);
      this.Controls.Add((MyGuiControlBase) this.m_btnOk);
      float num2 = -1f / 500f;
      float num3 = 0.0018f;
      Vector2 vector2_7 = new Vector2(0.0f, 1f / 500f);
      string[] icons = MyBankingSystem.BankingSystemDefinition.Icons;
      string texture = (icons != null ? ((uint) icons.Length > 0U ? 1 : 0) : 0) != 0 ? MyBankingSystem.BankingSystemDefinition.Icons[0] : string.Empty;
      Vector2 fromNormalizedSize = MyGuiManager.GetScreenSizeFromNormalizedSize(new Vector2(1f));
      Vector2 vector2_8 = new Vector2(0.018f, fromNormalizedSize.X / fromNormalizedSize.Y * 0.018f);
      MyGuiControlLabel myGuiControlLabel1 = new MyGuiControlLabel();
      myGuiControlLabel1.Position = start1 + new Vector2(num1 + num3, 2.2f * y);
      myGuiControlLabel1.TextEnum = this.m_transactionType == MyTransactionType.Withdraw ? MySpaceTexts.Transaction_Form_FactionBalance : MySpaceTexts.Transaction_Form_PersonalBalance;
      myGuiControlLabel1.IsAutoEllipsisEnabled = false;
      MyGuiControlLabel myGuiControlLabel2 = myGuiControlLabel1;
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel2);
      MyGuiControlImage myGuiControlImage1 = new MyGuiControlImage();
      myGuiControlImage1.Position = new Vector2(vector2_3.X - x - num1 + num2, myGuiControlLabel2.PositionY) + vector2_7;
      myGuiControlImage1.Size = vector2_8;
      myGuiControlImage1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
      MyGuiControlImage myGuiControlImage2 = myGuiControlImage1;
      myGuiControlImage2.SetTexture(texture);
      this.Controls.Add((MyGuiControlBase) myGuiControlImage2);
      MyGuiControlLabel myGuiControlLabel3 = new MyGuiControlLabel();
      myGuiControlLabel3.Position = new Vector2(myGuiControlImage2.PositionX - myGuiControlImage2.Size.X * 1.2f, myGuiControlLabel2.PositionY);
      myGuiControlLabel3.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
      myGuiControlLabel3.Text = MyBankingSystem.GetFormatedValue(this.m_currentBalance);
      myGuiControlLabel3.IsAutoEllipsisEnabled = false;
      myGuiControlLabel3.ColorMask = (Vector4) MyTerminalFactionController.COLOR_CUSTOM_GREY;
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel3);
      MyGuiControlLabel myGuiControlLabel4 = new MyGuiControlLabel();
      myGuiControlLabel4.Position = myGuiControlLabel2.Position + new Vector2(0.0f, 2f * y);
      myGuiControlLabel4.TextEnum = MySpaceTexts.Transaction_Form_Amount;
      myGuiControlLabel4.IsAutoEllipsisEnabled = false;
      MyGuiControlLabel myGuiControlLabel5 = myGuiControlLabel4;
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel5);
      MyGuiControlImage myGuiControlImage3 = new MyGuiControlImage();
      myGuiControlImage3.Position = new Vector2(vector2_3.X - x - num1 + num2, myGuiControlLabel5.PositionY) + vector2_7;
      myGuiControlImage3.Size = vector2_8;
      myGuiControlImage3.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
      MyGuiControlImage myGuiControlImage4 = myGuiControlImage3;
      myGuiControlImage4.SetTexture(texture);
      this.Controls.Add((MyGuiControlBase) myGuiControlImage4);
      MyGuiControlTextbox guiControlTextbox = new MyGuiControlTextbox(textColor: new Vector4?((Vector4) MyTerminalFactionController.COLOR_CUSTOM_GREY), type: MyGuiControlTextboxType.DigitsOnly);
      guiControlTextbox.Position = new Vector2(myGuiControlImage4.PositionX - myGuiControlImage4.Size.X * 1.2f, myGuiControlLabel5.PositionY);
      guiControlTextbox.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
      guiControlTextbox.MaxLength = 19;
      guiControlTextbox.TextAlignment = TextAlingmentMode.Right;
      this.m_textboxAmount = guiControlTextbox;
      this.m_textboxAmount.Size = new Vector2(0.2f, this.m_textboxAmount.Size.Y);
      this.m_textboxAmount.ColorMask = new Vector4(this.m_textboxAmount.ColorMask.X, this.m_textboxAmount.ColorMask.Y, this.m_textboxAmount.ColorMask.Z, 0.5f);
      this.m_textboxAmount.TextChanged += new Action<MyGuiControlTextbox>(this.OnAmountChanged);
      this.m_textboxAmount.HighlightType = MyGuiControlHighlightType.NEVER;
      this.Controls.Add((MyGuiControlBase) this.m_textboxAmount);
      MyGuiControlLabel myGuiControlLabel6 = new MyGuiControlLabel();
      myGuiControlLabel6.Position = myGuiControlLabel5.Position + new Vector2(0.0f, 2f * y);
      myGuiControlLabel6.TextEnum = MySpaceTexts.Transaction_Form_FinalBalance;
      myGuiControlLabel6.IsAutoEllipsisEnabled = false;
      MyGuiControlLabel myGuiControlLabel7 = myGuiControlLabel6;
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel7);
      MyGuiControlImage myGuiControlImage5 = new MyGuiControlImage();
      myGuiControlImage5.Position = new Vector2(vector2_3.X - x - num1 + num2, myGuiControlLabel7.PositionY) + vector2_7;
      myGuiControlImage5.Size = vector2_8;
      myGuiControlImage5.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
      MyGuiControlImage myGuiControlImage6 = myGuiControlImage5;
      myGuiControlImage6.SetTexture(texture);
      this.Controls.Add((MyGuiControlBase) myGuiControlImage6);
      MyGuiControlLabel myGuiControlLabel8 = new MyGuiControlLabel();
      myGuiControlLabel8.Position = new Vector2(myGuiControlImage6.PositionX - myGuiControlImage6.Size.X * 1.2f, myGuiControlLabel7.PositionY);
      myGuiControlLabel8.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
      myGuiControlLabel8.Text = MyBankingSystem.GetFormatedValue(this.m_finalBalance > 0L ? this.m_finalBalance : 0L);
      myGuiControlLabel8.IsAutoEllipsisEnabled = false;
      myGuiControlLabel8.ColorMask = (Vector4) MyTerminalFactionController.COLOR_CUSTOM_GREY;
      this.m_labelFinalBalanceValue = myGuiControlLabel8;
      this.Controls.Add((MyGuiControlBase) this.m_labelFinalBalanceValue);
      Vector2 start2 = new Vector2(start1.X, myGuiControlImage6.PositionY + 3f * y);
      float num4 = this.m_size.Value.X - 2f * x;
      controlSeparatorList1.AddHorizontal(start2, num4);
      MyGuiControlLabel myGuiControlLabel9 = new MyGuiControlLabel();
      myGuiControlLabel9.Position = start2 + new Vector2(num4 * 0.5f, -y);
      myGuiControlLabel9.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
      myGuiControlLabel9.Text = MyTexts.GetString(MySpaceTexts.Transaction_Form_ActivityLogLabel);
      myGuiControlLabel9.Font = "ScreenCaption";
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel9);
      MyGuiControlTable myGuiControlTable = new MyGuiControlTable();
      myGuiControlTable.Position = start2 + new Vector2(0.0f, y);
      myGuiControlTable.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlTable.Size = new Vector2(num4, 0.35f);
      myGuiControlTable.VisualStyle = MyGuiControlTableStyleEnum.Default;
      MyGuiControlTable listHistory = myGuiControlTable;
      listHistory.ColumnsCount = 3;
      listHistory.SetCustomColumnWidths(new float[3]
      {
        0.38f,
        0.27f,
        0.3f
      });
      listHistory.VisibleRowsCount = 9;
      listHistory.ColumnLinesVisible = true;
      listHistory.RowLinesVisible = true;
      listHistory.SetColumnName(0, MyTexts.Get(MySpaceTexts.Transaction_Form_Log_DateHeader));
      listHistory.SetColumnName(1, MyTexts.Get(MySpaceTexts.Transaction_Form_Log_NameHeader));
      listHistory.SetColumnName(2, MyTexts.Get(MySpaceTexts.Transaction_Form_Log_AmountHeader));
      listHistory.SetColumnAlign(2, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER);
      listHistory.SetHeaderColumnMargin(0, new Thickness(0.01f));
      listHistory.SetHeaderColumnMargin(1, new Thickness(0.01f));
      listHistory.SetHeaderColumnMargin(2, new Thickness(0.01f));
      this.UpdateActivityLog(listHistory);
      this.Controls.Add((MyGuiControlBase) listHistory);
      this.m_invalidInput = true;
      this.UpdateControls();
      Vector2 minSizeGui = MyGuiControlButton.GetVisualStyle(MyGuiControlButtonStyleEnum.Default).NormalTexture.MinSizeGui;
      MyGuiControlLabel myGuiControlLabel10 = new MyGuiControlLabel(new Vector2?(new Vector2(myGuiControlLabel2.PositionX, this.m_btnOk.Position.Y - minSizeGui.Y / 2f)));
      myGuiControlLabel10.Name = MyGuiScreenBase.GAMEPAD_HELP_LABEL_NAME;
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel10);
      this.GamepadHelpTextId = new MyStringId?(MySpaceTexts.Transaction_Help_Screen);
      this.FocusedControl = (MyGuiControlBase) this.m_textboxAmount;
    }

    public override void HandleUnhandledInput(bool receivedFocusInThisUpdate)
    {
      base.HandleUnhandledInput(receivedFocusInThisUpdate);
      if (!MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.BUTTON_X))
        return;
      this.OnButtonOkPressed((MyGuiControlButton) null);
    }

    public override bool Update(bool hasFocus)
    {
      int num = base.Update(hasFocus) ? 1 : 0;
      if (!hasFocus)
        return num != 0;
      this.m_btnOk.Visible = !MyInput.Static.IsJoystickLastUsed;
      return num != 0;
    }

    private void UpdateActivityLog(MyGuiControlTable listHistory)
    {
      MyAccountInfo account;
      if (this.m_transactionType == MyTransactionType.Deposit)
        MyBankingSystem.Static.TryGetAccountInfo(this.m_toIdentifier, out account);
      else
        MyBankingSystem.Static.TryGetAccountInfo(this.m_fromIdentifier, out account);
      for (int index = account.Log.Length - 1; index >= 0; --index)
      {
        MyAccountLogEntry myAccountLogEntry = account.Log[index];
        MyIdentity identity = MySession.Static.Players.TryGetIdentity(myAccountLogEntry.ChangeIdentifier);
        string accountOwnerName;
        if (identity != null)
        {
          accountOwnerName = identity.DisplayName;
        }
        else
        {
          IMyFaction factionById = MySession.Static.Factions.TryGetFactionById(myAccountLogEntry.ChangeIdentifier);
          accountOwnerName = factionById == null ? myAccountLogEntry.ChangeIdentifier.ToString() : factionById.Name;
        }
        listHistory.Add(this.CreateRow(myAccountLogEntry.DateTime, accountOwnerName, myAccountLogEntry.Amount));
      }
    }

    private MyGuiControlTable.Row CreateRow(
      DateTime dateTime,
      string accountOwnerName,
      long amount)
    {
      MyGuiControlTable.Row row = new MyGuiControlTable.Row();
      row.AddCell(new MyGuiControlTable.Cell(dateTime.ToLocalTime().ToString("dd.MM.yyyy HH:mm")));
      row.AddCell(new MyGuiControlTable.Cell(accountOwnerName));
      row.AddCell(new MyGuiControlTable.Cell(MyBankingSystem.GetFormatedValue(amount))
      {
        TextColor = new Color?(amount > 0L ? Color.Green : Color.Red)
      });
      return row;
    }

    private void OnButtonOkPressed(MyGuiControlButton obj)
    {
      long result;
      if (this.m_invalidInput || !long.TryParse(this.m_textboxAmount.Text, out result))
        return;
      MyBankingSystem.RequestTransfer(this.m_fromIdentifier, this.m_toIdentifier, result);
      this.CloseScreen();
    }

    private void OnAmountChanged(MyGuiControlTextbox obj)
    {
      long result;
      this.m_finalBalance = !long.TryParse(obj.Text, out result) || result <= 0L ? this.m_currentBalance : this.m_currentBalance - result;
      this.m_invalidInput = !this.IsFinalBalanceValid() || result <= 0L;
      this.UpdateControls();
    }

    private bool IsFinalBalanceValid() => this.m_finalBalance >= 0L && this.m_finalBalance != this.m_currentBalance;

    private void UpdateControls()
    {
      if (this.m_invalidInput)
      {
        this.m_labelFinalBalanceValue.ColorMask = (Vector4) MyTerminalFactionController.COLOR_CUSTOM_RED;
        this.m_btnOk.Enabled = false;
      }
      else
      {
        this.m_labelFinalBalanceValue.ColorMask = (Vector4) MyTerminalFactionController.COLOR_CUSTOM_GREY;
        this.m_btnOk.Enabled = true;
      }
      this.m_labelFinalBalanceValue.Text = MyBankingSystem.GetFormatedValue(this.m_finalBalance > 0L ? this.m_finalBalance : 0L);
    }
  }
}
