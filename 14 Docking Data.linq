<Query Kind="Statements">
  <Reference Relative="14 input.txt">C:\Drive\Challenges\AoC 2020\14 input.txt</Reference>
</Query>

var input = @"mask = XXXXXXXXXXXXXXXXXXXXXXXXXXXXX1XXXX0X
mem[8] = 11
mem[7] = 101
mem[8] = 0".Split("\r\n").ToArray();

input = File.ReadAllLines("14 input.txt");

var mask = "";
var memory = new Dictionary<long, long>();
var re = new Regex(@"mem\[(\d+)\] = (\d+)");

foreach (var line in input)
{
  if (line.StartsWith("mask = "))
  {
    mask = line.Substring("mask = ".Length);
  }
  else
  {
    var m = re.Match(line);

    memory[long.Parse(m.Groups[1].Value)] =
      Convert.ToInt64(
        new string(
          Convert.ToString(long.Parse(m.Groups[2].Value), 2)
            .PadLeft(36, '0')
            .Zip(mask)
            .Select(x => x.Second switch { 'X' => x.First, _ => x.Second })
            .ToArray()), 2);
  }
}

memory.Sum(x => x.Value).Dump("Answer 1");

// --- Part Two ---

memory.Clear();

//input = @"mask = 000000000000000000000000000000X1001X
//mem[42] = 100
//mask = 00000000000000000000000000000000X0XX
//mem[26] = 1".Split("\r\n");

foreach (var line in input)
{
  if (line.StartsWith("mask = "))
  {
    mask = line.Substring("mask = ".Length);
  }
  else
  {
    var m = re.Match(line);

    var address =
      Convert.ToString(long.Parse(m.Groups[1].Value), 2)
        .PadLeft(36, '0')
        .Zip(mask)
        .Select(x => x.Second switch { '0' => x.First, _ => x.Second })
        .ToArray();

    var list = new List<char[]>() { address };

    foreach (var ix in Enumerable.Range(0, 36).Where(x => address[x] == 'X'))
    {
      list = list.SelectMany(item => new[] { '1', '0' }.Select(c => item.Select((x, i) => i == ix ? c : x).ToArray())).ToList();
    }

    foreach (var item in list)
    {
      memory[Convert.ToInt64(new string(item), 2)] = long.Parse(m.Groups[2].Value);
    }
  }
}

memory.Sum(x => x.Value).Dump("Answer 2");