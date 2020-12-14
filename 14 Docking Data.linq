<Query Kind="Statements">
  <Reference Relative="14 input.txt">C:\Drive\Challenges\AoC 2020\14 input.txt</Reference>
</Query>

var input = @"mask = XXXXXXXXXXXXXXXXXXXXXXXXXXXXX1XXXX0X
mem[8] = 11
mem[7] = 101
mem[8] = 0".Split("\r\n").ToArray();

input = File.ReadAllLines("14 input.txt");

var mask = "";
var orMask = 0L;
var andMask = 0L;
var memory = new Dictionary<long, long>();
var re = new Regex(@"mem\[(\d+)\] = (\d+)");

IEnumerable<long> decompose(string s, char c) => Enumerable.Range(0, s.Length).Where(x => s[x] == c).Select(x => 1L << (s.Length - x - 1));

foreach (var line in input)
{
  if (line.StartsWith("mask = "))
  {
    mask = line.Substring("mask = ".Length);
    orMask = decompose(mask, '1').Sum();
    andMask = decompose(mask, '0').Sum();
  }
  else if (re.Match(line) is var m)
  {
    memory[long.Parse(m.Groups[1].Value)] = long.Parse(m.Groups[2].Value) & andMask | orMask;
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
    orMask = decompose(mask, '1').Sum();
  }
  else if (re.Match(line) is var m)
  {
    // If the bitmask bit is 1, the corresponding memory address bit is overwritten with 1.

    var address = long.Parse(m.Groups[1].Value) | orMask;

    var list = new List<long>() { address };

    // If the bitmask bit is X, the corresponding memory address bit is floating.

    foreach (var bit in decompose(mask, 'X'))
    {
      list = list.SelectMany(x => new[] { x | bit, x & (bit ^ long.MaxValue) }).ToList();
    }

    foreach (var item in list)
    {
      memory[item] = long.Parse(m.Groups[2].Value);
    }
  }
}

memory.Sum(x => x.Value).Dump("Answer 2");