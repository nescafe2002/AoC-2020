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

    var data = new LinkedList<Record>(
      from m in re.Matches(line)
      let oper = m.Groups[2].Value
      select new Record
      {
        Digit = long.TryParse(m.Groups[1].Value, out var x) ? x as long? : null,
        Operator = !string.IsNullOrEmpty(oper) ? oper : null,
        Level = oper switch { "(" => level++, ")" => --level, _ => level },
      }
    );

    // --- Part Two ---

    for (var node = data.First; node != null; node = node.Next)
    {
      if (node.Value.Operator != "+")
      {
        continue;
      }

      // Group summations A + B => ( A + B )

      for (var prev = node.Previous; prev != null; prev = prev.Previous) // Left
      {
        prev.Value.Level++;

        if (prev.Value.Level - 1 == node.Value.Level && (prev.Value.Operator == "(" || prev.Value.Digit != null))
        {
          data.AddBefore(prev, new Record { Operator = "(", Level = node.Value.Level });
          break;
        }
      }

      for (var next = node.Next; next != null; next = next.Next) // Right
      {
        next.Value.Level++;

        if (next.Value.Level - 1 == node.Value.Level && (next.Value.Operator == ")" || next.Value.Digit != null))
        {
          data.AddAfter(next, new Record { Operator = ")", Level = node.Value.Level });
          break;
        }
      }

      node.Value.Level++;
    }

    //data.Select(x => x.ToString()).Dump();
    //string.Join("", data.Select(x => x.Digit?.ToString() ?? x.Operator)).Dump();

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
  public long? Digit { get; set; }
  public string Operator { get; set; }
  public int Level { get; set; }

  public override string ToString() => string.Join(" ", Enumerable.Repeat("  ", Level).Append(Operator ?? Digit.ToString()).ToArray());
}