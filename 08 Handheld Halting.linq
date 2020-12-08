<Query Kind="Statements">
  <Reference Relative="08 input.txt">C:\Drive\Challenges\AoC 2020\08 input.txt</Reference>
</Query>

var input = @"nop +0
acc +1
jmp +4
acc +3
jmp -3
acc -99
acc +1
jmp -4
acc +6".Split("\r\n");

input = File.ReadAllLines("08 input.txt");

var program = input.Select(x => x.Split(' ')).Select((x, i) => (address: i, op: x[0], arg: int.Parse(x[1]))).ToArray();

(int acc, bool finished) run(int address = -1, Dictionary<string, string> translator = null)
{
  var acc = 0;
  var visited = new HashSet<int>();

  for (var ip = 0; ip < program.Length;)
  {
    if (!visited.Add(ip))
    {
      return (acc, false);
    }

    var line = program[ip];

    //$"{line.address:D3} | {line.op} | {line.arg}".Dump();

    switch (line.address == address ? translator[line.op] : line.op)
    {
      case "acc":
        acc += line.arg;
        break;
      case "jmp":
        ip += line.arg;
        continue;
    }
    ip += 1;
  }

  return (acc, true);
}

run().acc.Dump("Answer 1");

// --- Part Two ---

var translator = new Dictionary<string, string>() { { "nop", "jmp" }, { "jmp", "nop" } };

for (var i = 0; i < program.Length; i++)
{
  if (translator.ContainsKey(program[i].op))
  {
    var result = run(i, translator);

    if (result.finished)
    {
      result.acc.Dump("Answer 2");
      return;
    }
  }
}