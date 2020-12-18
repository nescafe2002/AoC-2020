<Query Kind="Program">
  <Reference Relative="18 input.txt">C:\Drive\Challenges\AoC 2020\18 input.txt</Reference>
</Query>

void Main()
{
  var input = @"1 + 2 * 3 + 4 * 5 + 6
  1 + (2 * 3) + (4 * (5 + 6))
  2 * 3 + (4 * 5)
  5 + (8 * 3 + 9 + 3 * 4 * 3)
  5 * 9 * (7 * 3 * 3 + 9 * 3 + (8 + 6 * 4))
  ((2 + 4 * 9) * (6 + 9 * 8 + 6) + 6) + 2 + 4 * 2".Split("\r\n");

//input = "1 + (2 + 3) + 6 + 7 + 8".Split("\r\n");

  input = File.ReadAllLines("18 input.txt");

  var re = new Regex(@"(\d+)|([+*-])|(\(|\))|\w");

  var result = 0L;

  foreach (var line in input)
  {
    var level = 0;
    var handled = new HashSet<int>();
    var lineNumber = 0;

    var data = (
      from m in re.Matches("(" + line + ")")
      let par = m.Groups[3].Value
      select new Record
      {
        Line = lineNumber++,
        Digit = long.TryParse(m.Groups[1].Value, out var x) ? x as long? : null,
        Operator = m.Groups[2].Value,
        Parenthesis = par,
        Level = par == "(" ? level++ : par == ")" ? --level : level,
      }
    ).ToList();

    //data.Dump(string.Join(" ", data.Select(x => x.ToString())));

    while (true)
    {
      var next = data.FirstOrDefault(x => x.Operator == "+" && !handled.Contains(x.Line));
      if (next == default) break;

      var index = data.IndexOf(next).Dump();

      var lvl = next.Level;

      var l = data.Take(index).Reverse().First(x => x.Level == next.Level && (x.Parenthesis == "(" || x.Digit != null));
      var r = data.Skip(index).First(x => x.Level == next.Level && (x.Parenthesis == ")" || x.Digit != null));

      data.SkipWhile(x => x != l).TakeWhile(x => x != r).ToList().ForEach(x => x.Level++);

      r.Level++;

      data.Insert(data.IndexOf(l), new Record { Line = lineNumber++, Parenthesis = "(", Level = lvl });
      data.Insert(data.IndexOf(r) + 1, new Record { Line = lineNumber++, Parenthesis = ")", Level = lvl  });

      handled.Add(next.Line);
    }

    //data.Dump(string.Join(" ", data.Select(x => x.ToString())));

    var stack = new Stack<(long, string)>();
    var op = null as string;
    var left = 0L;

    foreach (var item in data)
    {
      var digit = item.Digit ?? 0L;

      if (item.Parenthesis == "(")
      {
        stack.Push((left, op));
        (left, op) = (0, null);
        continue;
      }
      else if (item.Parenthesis == ")")
      {
        digit = left;
        (left, op) = stack.Pop();
      }
      else if (!string.IsNullOrEmpty(item.Operator))
      {
        op = item.Operator;
        continue;
      }

      if (op != null)
      {
        var right = op switch
        {
          "+" => left + digit,
          "*" => left * digit,
          "-" => left - digit,
          _ => throw new NotSupportedException(op),
        };
        $"{left} {op} {digit} = {right}".Dump();
        left = right;
      }
      else
      {
        left = digit;
      }
    }

    result += left;
  }

  result.Dump(" Answer 1");
}

// You can define other methods, fields, classes and namespaces here

class Record
{
  public int Line { get; set; }
  public long? Digit { get; set; }
  public string Operator { get; set; }
  public string Parenthesis { get; set; }
  public int Level { get; set; }
  public bool Handled { get; set; }

  public override string ToString() => Digit?.ToString() + (Operator ?? "") + Parenthesis ?? "";
}