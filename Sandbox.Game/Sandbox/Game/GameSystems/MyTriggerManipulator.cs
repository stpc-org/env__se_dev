// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.MyTriggerManipulator
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Components;
using Sandbox.Game.SessionComponents;
using System;
using System.Collections.Generic;
using VRageMath;

namespace Sandbox.Game.GameSystems
{
  public class MyTriggerManipulator
  {
    private Vector3D m_currentPosition;
    private readonly List<MyTriggerComponent> m_currentQuery = new List<MyTriggerComponent>();
    private readonly Predicate<MyTriggerComponent> m_triggerEvaluationpPredicate;
    private MyTriggerComponent m_selectedTrigger;

    public Vector3D CurrentPosition
    {
      get => this.m_currentPosition;
      set
      {
        if (value == this.m_currentPosition)
          return;
        Vector3D currentPosition = this.m_currentPosition;
        this.m_currentPosition = value;
        this.OnPositionChanged(currentPosition, this.m_currentPosition);
      }
    }

    public List<MyTriggerComponent> CurrentQuery => this.m_currentQuery;

    public MyTriggerComponent SelectedTrigger
    {
      get => this.m_selectedTrigger;
      set
      {
        if (this.m_selectedTrigger == value)
          return;
        if (this.m_selectedTrigger != null)
          this.m_selectedTrigger.CustomDebugColor = new Color?(Color.Red);
        this.m_selectedTrigger = value;
        if (this.m_selectedTrigger == null)
          return;
        this.m_selectedTrigger.CustomDebugColor = new Color?(Color.Yellow);
      }
    }

    public MyTriggerManipulator(
      Predicate<MyTriggerComponent> triggerEvaluationPredicate = null)
    {
      this.m_triggerEvaluationpPredicate = triggerEvaluationPredicate;
    }

    protected virtual void OnPositionChanged(Vector3D oldPosition, Vector3D newPosition)
    {
      List<MyTriggerComponent> intersectingTriggers = MySessionComponentTriggerSystem.Static.GetIntersectingTriggers(newPosition);
      this.m_currentQuery.Clear();
      foreach (MyTriggerComponent triggerComponent in intersectingTriggers)
      {
        if (this.m_triggerEvaluationpPredicate != null)
        {
          if (this.m_triggerEvaluationpPredicate(triggerComponent))
            this.m_currentQuery.Add(triggerComponent);
        }
        else
          this.m_currentQuery.Add(triggerComponent);
      }
    }

    public void SelectClosest(Vector3D position)
    {
      double num1 = double.MaxValue;
      if (this.SelectedTrigger != null)
        this.SelectedTrigger.CustomDebugColor = new Color?(Color.Red);
      foreach (MyTriggerComponent triggerComponent in this.m_currentQuery)
      {
        double num2 = (triggerComponent.Center - position).LengthSquared();
        if (num2 < num1)
        {
          num1 = num2;
          this.SelectedTrigger = triggerComponent;
        }
      }
      if (Math.Abs(num1 - double.MaxValue) < double.Epsilon)
        this.SelectedTrigger = (MyTriggerComponent) null;
      if (this.SelectedTrigger == null)
        return;
      this.SelectedTrigger.CustomDebugColor = new Color?(Color.Yellow);
    }
  }
}
