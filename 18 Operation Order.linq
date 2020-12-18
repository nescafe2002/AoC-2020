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

  var re = new Regex(@"(\d+)|(\S)"); // \d+ = <decimal> | \S = <non-whitespace>

  var result = 0L;

  foreach (var line in input)
  {
    var level = 0;

    var data = (
      from m in re.Matches(line)
      let oper = m.Groups[2].Value
      select new Record
      {
        Digit = long.TryParse(m.Groups[1].Value, out var x) ? x as long? : null,
        Operator = !string.IsNullOrEmpty(oper) ? oper : null,
        Level = oper switch { "(" => level++, ")" => --level, _ => level },
      }
    ).ToList();

    // --- Part Two ---

    for (var ix = 0; true;) // <-- false; for Part One, true; for Part Two
    {
      // Group summations A + B => ( A + B )

      var next = data.Select((item, index) => (item, index)).Skip(ix).FirstOrDefault(x => x.item.Operator == "+");
      if (next == default) break;

      var (index, item) = (next.index, next.item);

      // Find nearest group ( a [op] b ) or number
      var ixLeft = data.Select((item, index) => (item, index)).Take(index).Last(x => x.item.Level == item.Level && (x.item.Operator == "(" || x.item.Digit != null)).index;
      var ixRight = data.Select((item, index) => (item, index)).Skip(index).First(x => x.item.Level == item.Level && (x.item.Operator == ")" || x.item.Digit != null)).index;

      // Add parentheses
      data.Insert(ixLeft, new Record { Operator = "(", Level = item.Level });
      data.Insert(ixRight + 2, new Record { Operator = ")", Level = item.Level });

      // Level up inner expression
      data.Skip(ixLeft + 1).Take(ixRight - ixLeft + 1).ToList().ForEach(x => x.Level++);

      ix = index + 2;
    }

    //data.Select(x => x.ToString()).Dump();

    var stack = new Stack<(long, string)>();
    var op = null as string;
    var register = 0L;

    foreach (var item in data)
    {
      var current = item.Digit ?? 0L;

      switch (item.Operator)
      {
        case "(":
          stack.Push((register, op));
          (register, op) = (0, null);
          continue;
        case ")":
          (current, (register, op)) = (register, stack.Pop());
          break;
        case not null:
          op = item.Operator;
          continue;
      }

      register = op switch
      {
        null => current,
        "+" => register + current,
        "*" => register * current,
        "-" => register - current,
        _ => throw new NotSupportedException($"Operator not supported: {op}"),
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