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

  input = File.ReadAllLines("18 input.txt");

  var re = new Regex(@"(\d+)|([+*-]|\(|\))|\w");

  var result = 0L;

  foreach (var line in input)
  {
    var level = 0;
    var handled = new HashSet<int>();
    var lineNumber = 0;

    var data = (
      from m in re.Matches(line)
      let oper = m.Groups[2].Value
      select new Record
      {
        Line = lineNumber++,
        Digit = long.TryParse(m.Groups[1].Value, out var x) ? x as long? : null,
        Operator = !string.IsNullOrEmpty(oper) ? oper : null,
        Level = oper == "(" ? level++ : oper == ")" ? --level : level,
      }
    ).ToList();

    // Group summations A + B => ( A + B )

    while (true) // <-- false for Part One, true for Part Two
    {
      var next = data.Select((item, index) => (item, index)).FirstOrDefault(x => x.item.Operator == "+" && !handled.Contains(x.item.Line));
      if (next == default) break;

      var index = next.index;
      var item = next.item;
      var lvl = item.Level;

      // Find nearest group ( a [op] b ) or number
      var l = data.Select((item, index) => (item, index)).Where(x => x.index <= index).Reverse().First(x => x.item.Level == item.Level && (x.item.Operator == "(" || x.item.Digit != null)).index;
      var r = data.Select((item, index) => (item, index)).Where(x => x.index > index).First(x => x.item.Level == item.Level && (x.item.Operator == ")" || x.item.Digit != null)).index;

      // Level up
      data.Skip(l).Take(r - l + 1).ToList().ForEach(x => x.Level++);

      // Add parentheses
      data.Insert(l, new Record { Line = lineNumber++, Operator = "(", Level = lvl });
      data.Insert(r + 2, new Record { Line = lineNumber++, Operator = ")", Level = lvl });

      handled.Add(item.Line);
    }

    //data.Select(x => x.ToString()).Dump();

    var stack = new Stack<(long, string)>();
    var op = null as string;
    var register = 0L;

    foreach (var item in data)
    {
      var current = item.Digit ?? 0L;

      if (item.Operator == "(")
      {
        stack.Push((register, op));
        (register, op) = (0, null);
        continue;
      }
      else if (item.Operator == ")")
      {
        (current, (register, op)) = (register, stack.Pop());
      }
      else if (!string.IsNullOrEmpty(item.Operator))
      {
        op = item.Operator;
        continue;
      }

      register = op switch
      {
        null => current,
        "+" => register + current,
        "*" => register * current,
        "-" => register - current,
        _ => throw new NotSupportedException(op),
      };
    }

    result += register;
  }

  result.Dump("Answer 1 / 2");
}

// You can define other methods, fields, classes and namespaces here

class Record
{
  public int Line { get; set; }
  public long? Digit { get; set; }
  public string Operator { get; set; }
  public int Level { get; set; }

  public override string ToString() => string.Join(" ", Enumerable.Repeat("  ", Level).Append(Operator ?? Digit.ToString()).ToArray());
}