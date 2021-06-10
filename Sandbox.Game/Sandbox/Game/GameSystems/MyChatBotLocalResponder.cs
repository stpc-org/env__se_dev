// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.MyChatBotLocalResponder
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Collections;

namespace Sandbox.Game.GameSystems
{
  public class MyChatBotLocalResponder : IMyChatBotResponder
  {
    private List<MyChatBotLocalResponder.ScoredResponse> m_responseScores = new List<MyChatBotLocalResponder.ScoredResponse>(300);

    public ChatBotResponseDelegate OnResponse { get; set; }

    public void LoadData()
    {
      ListReader<MyChatBotResponseDefinition> responseDefinitions = MyDefinitionManager.Static.GetChatBotResponseDefinitions();
      for (int index = 0; index < responseDefinitions.Count; ++index)
      {
        MyChatBotLocalResponder.ScoredResponse scoredResponse = new MyChatBotLocalResponder.ScoredResponse()
        {
          Definition = responseDefinitions[index]
        };
        scoredResponse.ResponseSubscores = new int[scoredResponse.Definition.Questions.Length];
        this.m_responseScores.Add(scoredResponse);
      }
      this.ResetScore();
    }

    private void ResetScore()
    {
      for (int index1 = 0; index1 < this.m_responseScores.Count; ++index1)
      {
        for (int index2 = 0; index2 < this.m_responseScores[index1].ResponseSubscores.Length; ++index2)
          this.m_responseScores[index1].ResponseSubscores[index2] = 0;
      }
    }

    public void SendMessage(
      string originalQuestion,
      string preprocessedQuestion,
      string potentialResponseId,
      Action<string> responseAction)
    {
      ResponseType responseType = ResponseType.Unavailable;
      string response = this.GetResponse(originalQuestion, out responseType);
      ChatBotResponseDelegate onResponse = this.OnResponse;
      if (onResponse == null)
        return;
      onResponse(originalQuestion, response, responseType, responseAction);
    }

    public void PerformDebugTest(PreprocessDelegate preprocess)
    {
    }

    private string GetResponse(string question, out ResponseType responseType)
    {
      this.ResetScore();
      this.ScoreQuestion(question);
      this.m_responseScores.Sort();
      if (this.m_responseScores[0].Score == 0)
      {
        responseType = ResponseType.Misunderstanding;
        return MyChatBot.GetMisunderstandingTextId();
      }
      responseType = ResponseType.ChatBot;
      return this.m_responseScores[0].Definition.Response;
    }

    private void ScoreQuestion(string question)
    {
      string[] strArray = question.Split(new char[4]
      {
        ' ',
        ',',
        '?',
        '!'
      }, StringSplitOptions.RemoveEmptyEntries);
      for (int index1 = 0; index1 < this.m_responseScores.Count; ++index1)
      {
        foreach (string word in strArray)
        {
          if (word.Length >= 3)
          {
            for (int index2 = 0; index2 < this.m_responseScores[index1].Definition.Questions.Length; ++index2)
              this.m_responseScores[index1].ResponseSubscores[index2] += this.ScoreQuestionWord(word, this.m_responseScores[index1].Definition.Questions[index2]);
          }
        }
      }
    }

    private int ScoreQuestionWord(string word, string question) => question.IndexOf(word, StringComparison.InvariantCultureIgnoreCase) == -1 ? 0 : word.Length;

    private struct ChatbotQuestion
    {
      public string OriginalQuestion;
      public string PreprocessedQuestion;
      public string PotentialResponseId;
      public Action<string> ResponseAction;
    }

    private class ScoredResponse : IEquatable<MyChatBotLocalResponder.ScoredResponse>, IComparable<MyChatBotLocalResponder.ScoredResponse>
    {
      public int[] ResponseSubscores;
      public MyChatBotResponseDefinition Definition;

      public int Score => ((IEnumerable<int>) this.ResponseSubscores).Sum();

      public override string ToString() => this.Definition.Id.SubtypeName + " (" + (object) this.Score + ")";

      public override bool Equals(object obj) => obj != null && obj is MyChatBotLocalResponder.ScoredResponse other && this.Equals(other);

      public int CompareTo(
        MyChatBotLocalResponder.ScoredResponse compareScoredResponse)
      {
        return compareScoredResponse == null ? 1 : compareScoredResponse.Score.CompareTo(this.Score);
      }

      public override int GetHashCode() => this.Score;

      public bool Equals(MyChatBotLocalResponder.ScoredResponse other) => other != null && this.Score.Equals(other.Score);
    }
  }
}
