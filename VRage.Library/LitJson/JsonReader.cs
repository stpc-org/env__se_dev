// Decompiled with JetBrains decompiler
// Type: LitJson.JsonReader
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace LitJson
{
  public class JsonReader
  {
    private static readonly IDictionary<int, IDictionary<int, int[]>> parse_table = JsonReader.PopulateParseTable();
    private Stack<int> automaton_stack;
    private int current_input;
    private int current_symbol;
    private bool end_of_json;
    private bool end_of_input;
    private Lexer lexer;
    private bool parser_in_string;
    private bool parser_return;
    private bool read_started;
    private TextReader reader;
    private bool reader_is_owned;
    private bool skip_non_members;
    private object token_value;
    private JsonToken token;

    public bool AllowComments
    {
      get => this.lexer.AllowComments;
      set => this.lexer.AllowComments = value;
    }

    public bool AllowSingleQuotedStrings
    {
      get => this.lexer.AllowSingleQuotedStrings;
      set => this.lexer.AllowSingleQuotedStrings = value;
    }

    public bool SkipNonMembers
    {
      get => this.skip_non_members;
      set => this.skip_non_members = value;
    }

    public bool EndOfInput => this.end_of_input;

    public bool EndOfJson => this.end_of_json;

    public JsonToken Token => this.token;

    public object Value => this.token_value;

    public JsonReader(string json_text)
      : this((TextReader) new StringReader(json_text), true)
    {
    }

    public JsonReader(TextReader reader)
      : this(reader, false)
    {
    }

    private JsonReader(TextReader reader, bool owned)
    {
      if (reader == null)
        throw new ArgumentNullException(nameof (reader));
      this.parser_in_string = false;
      this.parser_return = false;
      this.read_started = false;
      this.automaton_stack = new Stack<int>();
      this.automaton_stack.Push(65553);
      this.automaton_stack.Push(65543);
      this.lexer = new Lexer(reader);
      this.end_of_input = false;
      this.end_of_json = false;
      this.skip_non_members = true;
      this.reader = reader;
      this.reader_is_owned = owned;
    }

    private static IDictionary<int, IDictionary<int, int[]>> PopulateParseTable()
    {
      IDictionary<int, IDictionary<int, int[]>> parse_table = (IDictionary<int, IDictionary<int, int[]>>) new Dictionary<int, IDictionary<int, int[]>>();
      JsonReader.TableAddRow(parse_table, ParserToken.Array);
      JsonReader.TableAddCol(parse_table, ParserToken.Array, 91, 91, 65549);
      JsonReader.TableAddRow(parse_table, ParserToken.ArrayPrime);
      JsonReader.TableAddCol(parse_table, ParserToken.ArrayPrime, 34, 65550, 65551, 93);
      JsonReader.TableAddCol(parse_table, ParserToken.ArrayPrime, 91, 65550, 65551, 93);
      JsonReader.TableAddCol(parse_table, ParserToken.ArrayPrime, 93, 93);
      JsonReader.TableAddCol(parse_table, ParserToken.ArrayPrime, 123, 65550, 65551, 93);
      JsonReader.TableAddCol(parse_table, ParserToken.ArrayPrime, 65537, 65550, 65551, 93);
      JsonReader.TableAddCol(parse_table, ParserToken.ArrayPrime, 65538, 65550, 65551, 93);
      JsonReader.TableAddCol(parse_table, ParserToken.ArrayPrime, 65539, 65550, 65551, 93);
      JsonReader.TableAddCol(parse_table, ParserToken.ArrayPrime, 65540, 65550, 65551, 93);
      JsonReader.TableAddRow(parse_table, ParserToken.Object);
      JsonReader.TableAddCol(parse_table, ParserToken.Object, 123, 123, 65545);
      JsonReader.TableAddRow(parse_table, ParserToken.ObjectPrime);
      JsonReader.TableAddCol(parse_table, ParserToken.ObjectPrime, 34, 65546, 65547, 125);
      JsonReader.TableAddCol(parse_table, ParserToken.ObjectPrime, 125, 125);
      JsonReader.TableAddRow(parse_table, ParserToken.Pair);
      JsonReader.TableAddCol(parse_table, ParserToken.Pair, 34, 65552, 58, 65550);
      JsonReader.TableAddRow(parse_table, ParserToken.PairRest);
      JsonReader.TableAddCol(parse_table, ParserToken.PairRest, 44, 44, 65546, 65547);
      JsonReader.TableAddCol(parse_table, ParserToken.PairRest, 125, 65554);
      JsonReader.TableAddRow(parse_table, ParserToken.String);
      JsonReader.TableAddCol(parse_table, ParserToken.String, 34, 34, 65541, 34);
      JsonReader.TableAddRow(parse_table, ParserToken.Text);
      JsonReader.TableAddCol(parse_table, ParserToken.Text, 91, 65548);
      JsonReader.TableAddCol(parse_table, ParserToken.Text, 123, 65544);
      JsonReader.TableAddRow(parse_table, ParserToken.Value);
      JsonReader.TableAddCol(parse_table, ParserToken.Value, 34, 65552);
      JsonReader.TableAddCol(parse_table, ParserToken.Value, 91, 65548);
      JsonReader.TableAddCol(parse_table, ParserToken.Value, 123, 65544);
      JsonReader.TableAddCol(parse_table, ParserToken.Value, 65537, 65537);
      JsonReader.TableAddCol(parse_table, ParserToken.Value, 65538, 65538);
      JsonReader.TableAddCol(parse_table, ParserToken.Value, 65539, 65539);
      JsonReader.TableAddCol(parse_table, ParserToken.Value, 65540, 65540);
      JsonReader.TableAddRow(parse_table, ParserToken.ValueRest);
      JsonReader.TableAddCol(parse_table, ParserToken.ValueRest, 44, 44, 65550, 65551);
      JsonReader.TableAddCol(parse_table, ParserToken.ValueRest, 93, 65554);
      return parse_table;
    }

    private static void TableAddCol(
      IDictionary<int, IDictionary<int, int[]>> parse_table,
      ParserToken row,
      int col,
      params int[] symbols)
    {
      parse_table[(int) row].Add(col, symbols);
    }

    private static void TableAddRow(
      IDictionary<int, IDictionary<int, int[]>> parse_table,
      ParserToken rule)
    {
      parse_table.Add((int) rule, (IDictionary<int, int[]>) new Dictionary<int, int[]>());
    }

    private void ProcessNumber(string number)
    {
      double result1;
      if ((number.IndexOf('.') != -1 || number.IndexOf('e') != -1 || number.IndexOf('E') != -1) && double.TryParse(number, NumberStyles.Any, (IFormatProvider) CultureInfo.InvariantCulture, out result1))
      {
        this.token = JsonToken.Double;
        this.token_value = (object) result1;
      }
      else
      {
        int result2;
        if (int.TryParse(number, NumberStyles.Integer, (IFormatProvider) CultureInfo.InvariantCulture, out result2))
        {
          this.token = JsonToken.Int;
          this.token_value = (object) result2;
        }
        else
        {
          long result3;
          if (long.TryParse(number, NumberStyles.Integer, (IFormatProvider) CultureInfo.InvariantCulture, out result3))
          {
            this.token = JsonToken.Long;
            this.token_value = (object) result3;
          }
          else
          {
            ulong result4;
            if (ulong.TryParse(number, NumberStyles.Integer, (IFormatProvider) CultureInfo.InvariantCulture, out result4))
            {
              this.token = JsonToken.Long;
              this.token_value = (object) result4;
            }
            else
            {
              this.token = JsonToken.Int;
              this.token_value = (object) 0;
            }
          }
        }
      }
    }

    private void ProcessSymbol()
    {
      if (this.current_symbol == 91)
      {
        this.token = JsonToken.ArrayStart;
        this.parser_return = true;
      }
      else if (this.current_symbol == 93)
      {
        this.token = JsonToken.ArrayEnd;
        this.parser_return = true;
      }
      else if (this.current_symbol == 123)
      {
        this.token = JsonToken.ObjectStart;
        this.parser_return = true;
      }
      else if (this.current_symbol == 125)
      {
        this.token = JsonToken.ObjectEnd;
        this.parser_return = true;
      }
      else if (this.current_symbol == 34)
      {
        if (this.parser_in_string)
        {
          this.parser_in_string = false;
          this.parser_return = true;
        }
        else
        {
          if (this.token == JsonToken.None)
            this.token = JsonToken.String;
          this.parser_in_string = true;
        }
      }
      else if (this.current_symbol == 65541)
        this.token_value = (object) this.lexer.StringValue;
      else if (this.current_symbol == 65539)
      {
        this.token = JsonToken.Boolean;
        this.token_value = (object) false;
        this.parser_return = true;
      }
      else if (this.current_symbol == 65540)
      {
        this.token = JsonToken.Null;
        this.parser_return = true;
      }
      else if (this.current_symbol == 65537)
      {
        this.ProcessNumber(this.lexer.StringValue);
        this.parser_return = true;
      }
      else if (this.current_symbol == 65546)
      {
        this.token = JsonToken.PropertyName;
      }
      else
      {
        if (this.current_symbol != 65538)
          return;
        this.token = JsonToken.Boolean;
        this.token_value = (object) true;
        this.parser_return = true;
      }
    }

    private bool ReadToken()
    {
      if (this.end_of_input)
        return false;
      this.lexer.NextToken();
      if (this.lexer.EndOfInput)
      {
        this.Close();
        return false;
      }
      this.current_input = this.lexer.Token;
      return true;
    }

    public void Close()
    {
      if (this.end_of_input)
        return;
      this.end_of_input = true;
      this.end_of_json = true;
      if (this.reader_is_owned)
      {
        using (this.reader)
          ;
      }
      this.reader = (TextReader) null;
    }

    public bool Read()
    {
      if (this.end_of_input)
        return false;
      if (this.end_of_json)
      {
        this.end_of_json = false;
        this.automaton_stack.Clear();
        this.automaton_stack.Push(65553);
        this.automaton_stack.Push(65543);
      }
      this.parser_in_string = false;
      this.parser_return = false;
      this.token = JsonToken.None;
      this.token_value = (object) null;
      if (!this.read_started)
      {
        this.read_started = true;
        if (!this.ReadToken())
          return false;
      }
      while (!this.parser_return)
      {
        this.current_symbol = this.automaton_stack.Pop();
        this.ProcessSymbol();
        if (this.current_symbol == this.current_input)
        {
          if (!this.ReadToken())
          {
            if (this.automaton_stack.Peek() != 65553)
              throw new JsonException("Input doesn't evaluate to proper JSON text");
            return this.parser_return;
          }
        }
        else
        {
          int[] numArray;
          try
          {
            numArray = JsonReader.parse_table[this.current_symbol][this.current_input];
          }
          catch (KeyNotFoundException ex)
          {
            throw new JsonException((ParserToken) this.current_input, (Exception) ex);
          }
          if (numArray[0] != 65554)
          {
            for (int index = numArray.Length - 1; index >= 0; --index)
              this.automaton_stack.Push(numArray[index]);
          }
        }
      }
      if (this.automaton_stack.Peek() == 65553)
        this.end_of_json = true;
      return true;
    }
  }
}
