// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.TextSurfaceScripts.MyTSSVendingMachine
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.GameSystems.BankingAndCurrency;
using Sandbox.Game.Localization;
using Sandbox.ModAPI.Ingame;
using System;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame;
using VRage.ObjectBuilders;
using VRageMath;

namespace Sandbox.Game.GameSystems.TextSurfaceScripts
{
  [MyTextSurfaceScript("TSS_VendingMachine", "DisplayName_TSS_VendingMachine")]
  public class MyTSSVendingMachine : MyTSSCommon
  {
    private static float DEFAULT_SCREEN_SIZE = 512f;
    private static int MESSAGE_TIME_FRAMES = 18;
    private MyVendingMachine m_vendingMachine;
    private int m_messageTimer = -1;
    private MyStoreBuyItemResult m_lastStoreResult;
    private long m_tickCtr;

    public MyTSSVendingMachine(IMyTextSurface surface, IMyCubeBlock block, Vector2 size)
      : base(surface, block, size)
    {
      this.m_vendingMachine = block as MyVendingMachine;
      this.m_fontId = "White";
      if (this.m_vendingMachine == null)
        return;
      this.m_vendingMachine.OnBuyItemResult += new Action<MyStoreBuyItemResult>(this.OnBuyItemResult);
    }

    public override void Dispose()
    {
      base.Dispose();
      if (this.m_vendingMachine == null)
        return;
      this.m_vendingMachine.OnBuyItemResult -= new Action<MyStoreBuyItemResult>(this.OnBuyItemResult);
    }

    private void OnBuyItemResult(MyStoreBuyItemResult obj)
    {
      this.m_lastStoreResult = obj;
      this.m_messageTimer = 0;
    }

    public override void Run()
    {
      base.Run();
      using (MySpriteDrawFrame frame = this.m_surface.DrawFrame())
      {
        Vector2 topLeftCorner = this.m_halfSize - this.m_surface.SurfaceSize * 0.5f;
        double num1 = (double) this.m_surface.SurfaceSize.Y / (double) this.m_surface.SurfaceSize.X;
        if (this.m_vendingMachine == null)
        {
          this.AddBackground(frame, new Color?(new Color(this.m_backgroundColor, 0.66f)));
          MySprite text = MySprite.CreateText(MyTexts.GetString(MySpaceTexts.VendingMachine_Script_DataUnavailable), this.m_fontId, this.m_foregroundColor, MyTSSVendingMachine.DEFAULT_SCREEN_SIZE / this.m_size.X);
          text.Position = new Vector2?(this.m_halfSize);
          frame.Add(text);
          return;
        }
        if (this.m_vendingMachine.SelectedItemIdx < 0 || this.m_vendingMachine.SelectedItem == null)
        {
          this.DrawErrorMessage(frame, topLeftCorner);
        }
        else
        {
          MyPhysicalItemDefinition physicalItemDefinition = (MyPhysicalItemDefinition) null;
          SerializableDefinitionId? nullable1 = this.m_vendingMachine.SelectedItem.Item;
          if (nullable1.HasValue)
          {
            MyDefinitionManager definitionManager = MyDefinitionManager.Static;
            nullable1 = this.m_vendingMachine.SelectedItem.Item;
            MyDefinitionId defId = (MyDefinitionId) nullable1.Value;
            ref MyPhysicalItemDefinition local = ref physicalItemDefinition;
            if (!definitionManager.TryGetDefinition<MyPhysicalItemDefinition>(defId, out local))
            {
              this.DrawErrorMessage(frame, topLeftCorner);
              return;
            }
          }
          MySprite sprite1 = MySprite.CreateSprite("LCD_Economy_Vending_Bg", topLeftCorner + this.m_surface.SurfaceSize * 0.5f, this.m_surface.SurfaceSize);
          sprite1.Color = new Color?(this.m_foregroundColor);
          frame.Add(sprite1);
          float y = this.m_scale.Y;
          Vector2 vector2_1 = this.m_surface.MeasureStringInPixels(new StringBuilder(physicalItemDefinition.DisplayNameText), this.m_fontId, y);
          float num2 = this.m_surface.SurfaceSize.X * 0.43f;
          if ((double) vector2_1.X > (double) num2)
            y *= num2 / vector2_1.X;
          MySprite text1 = MySprite.CreateText(physicalItemDefinition.DisplayNameText, this.m_fontId, this.m_foregroundColor, y);
          text1.Position = new Vector2?(topLeftCorner + new Vector2(this.m_surface.SurfaceSize.X * 0.69f, (float) ((double) this.m_surface.SurfaceSize.Y * 0.259999990463257 - (double) vector2_1.Y * 0.5 * (double) y)));
          frame.Add(text1);
          Vector2 size = ((double) this.m_surface.SurfaceSize.X >= (double) this.m_surface.SurfaceSize.Y ? new Vector2(this.m_surface.SurfaceSize.Y) : new Vector2(this.m_surface.SurfaceSize.X)) * 0.45f;
          Vector2 position1 = topLeftCorner + new Vector2(size.X * 0.9f, this.m_surface.SurfaceSize.Y * 0.48f);
          MySprite sprite2 = MySprite.CreateSprite(physicalItemDefinition.Id.ToString(), position1, size);
          frame.Add(sprite2);
          string text2 = MyTexts.GetString(MySpaceTexts.VendingMachine_Script_ItemAmount) + (object) this.m_vendingMachine.SelectedItem.Amount;
          Color color = this.m_vendingMachine.SelectedItem.Amount <= 0 ? Color.Red : this.m_foregroundColor;
          MySprite text3 = MySprite.CreateText(text2, this.m_fontId, color, this.m_scale.Y * 0.8f);
          text3.Position = new Vector2?(topLeftCorner + new Vector2(this.m_surface.SurfaceSize.X * 0.69f, this.m_surface.SurfaceSize.Y * 0.48f));
          frame.Add(text3);
          Vector2 vector2_2 = this.m_surface.MeasureStringInPixels(new StringBuilder(text2), this.m_fontId, this.m_scale.Y * 0.8f);
          string formatedValue = MyBankingSystem.GetFormatedValue((long) this.m_vendingMachine.SelectedItem.PricePerUnit, true);
          MySprite text4 = MySprite.CreateText(MyTexts.GetString(MySpaceTexts.VendingMachine_Script_PricePerUnit) + formatedValue, this.m_fontId, this.m_foregroundColor, this.m_scale.Y * 0.8f);
          ref MySprite local1 = ref text4;
          Vector2? position2 = text3.Position;
          Vector2 vector2_3 = new Vector2(0.0f, vector2_2.Y + 5f);
          Vector2? nullable2 = position2.HasValue ? new Vector2?(position2.GetValueOrDefault() + vector2_3) : new Vector2?();
          local1.Position = nullable2;
          frame.Add(text4);
        }
        if (this.m_messageTimer >= 0)
        {
          string messageString = this.GetMessageString();
          this.DrawMessage(frame, topLeftCorner + this.m_surface.SurfaceSize * 0.5f, messageString, this.m_scale.Y);
          ++this.m_messageTimer;
          if (this.m_messageTimer > MyTSSVendingMachine.MESSAGE_TIME_FRAMES)
            this.m_messageTimer = -1;
        }
      }
      ++this.m_tickCtr;
    }

    public string GetMessageString()
    {
      string str;
      switch (this.m_lastStoreResult.Result)
      {
        case MyStoreBuyItemResults.Success:
          str = MyTexts.GetString(MySpaceTexts.VendingMachine_Script_MessageBuy);
          break;
        case MyStoreBuyItemResults.NotEnoughMoney:
          str = MyTexts.GetString(MySpaceTexts.VendingMachine_Script_NoMoney);
          break;
        case MyStoreBuyItemResults.ItemsTimeout:
          str = MyTexts.GetString(MySpaceTexts.VendingMachine_Script_ItemsTimeout);
          break;
        case MyStoreBuyItemResults.NotEnoughInventorySpace:
          str = MyTexts.GetString(MySpaceTexts.VendingMachine_Script_NotEnoughSpace);
          break;
        case MyStoreBuyItemResults.NotEnoughAmount:
          str = MyTexts.GetString(MySpaceTexts.VendingMachine_Script_OutOfStock);
          break;
        default:
          str = MyTexts.GetString(MySpaceTexts.VendingMachine_Script_MessageError);
          break;
      }
      return str;
    }

    private void DrawErrorMessage(MySpriteDrawFrame frame, Vector2 topLeftCorner)
    {
      Vector2 position1 = topLeftCorner + new Vector2(this.m_surface.SurfaceSize.X * 0.5f, this.m_surface.SurfaceSize.Y * 0.32f);
      this.DrawMessage(frame, position1, MyTexts.GetString(MySpaceTexts.VendingMachine_Script_ConnectingToServer), this.m_scale.Y * 0.9f, false);
      Vector2 position2 = position1 + new Vector2(0.0f, this.m_surface.SurfaceSize.Y * 0.09f);
      this.DrawMessage(frame, position2, MyTexts.GetString(MySpaceTexts.VendingMachine_Script_ContactAdmin), this.m_scale.Y * 0.4f, false);
      Vector2 vector2_1 = position1 + new Vector2(0.0f, this.m_surface.SurfaceSize.Y * 0.3f);
      Vector2 vector2_2 = new Vector2(this.m_surface.SurfaceSize.X * 0.1f);
      MySprite sprite1 = new MySprite(data: "Screen_LoadingBar", position: new Vector2?(vector2_1), size: new Vector2?(vector2_2), rotation: ((float) -this.m_tickCtr * 0.12f));
      frame.Add(sprite1);
      MySprite sprite2 = new MySprite(data: "Screen_LoadingBar", position: new Vector2?(vector2_1), size: new Vector2?(vector2_2 * 0.66f), rotation: ((float) this.m_tickCtr * 0.12f));
      frame.Add(sprite2);
      MySprite sprite3 = new MySprite(data: "Screen_LoadingBar", position: new Vector2?(vector2_1), size: new Vector2?(vector2_2 * 0.41f), rotation: ((float) -this.m_tickCtr * 0.12f));
      frame.Add(sprite3);
    }

    private void DrawMessage(
      MySpriteDrawFrame frame,
      Vector2 position,
      string messageString,
      float fontSize,
      bool drawBg = true)
    {
      Vector2 position1 = position;
      Vector2 vector2 = this.m_surface.MeasureStringInPixels(new StringBuilder(messageString), this.m_fontId, fontSize * 1.5f);
      if (drawBg)
      {
        MySprite sprite = MySprite.CreateSprite("SquareSimple", position1, vector2 * 1.05f);
        sprite.Color = new Color?(Color.Black);
        frame.Add(sprite);
      }
      MySprite text = MySprite.CreateText(messageString, this.m_fontId, this.m_foregroundColor, fontSize * 1.5f);
      text.Position = new Vector2?(position1 - new Vector2(0.0f, vector2.Y * 0.5f));
      frame.Add(text);
    }

    public override ScriptUpdate NeedsUpdate => ScriptUpdate.Update10;
  }
}
