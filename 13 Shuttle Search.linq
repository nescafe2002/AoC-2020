<Query Kind="Program">
  <Reference Relative="..\..\..\Challenges\AoC 2020\13 input.txt">C:\Drive\Challenges\AoC 2020\13 input.txt</Reference>
</Query>

void Main()
{
  var input = @"939
  7,13,x,x,59,x,31,19".Split("\r\n");

  input = File.ReadAllLines("13 input.txt");

  var time = int.Parse(input[0]);
  var intervals = input[1].Split(',').Where(x => x != "x").Select(int.Parse);

  var data =
    from t in Enumerable.Range(time, 100)
    from i in intervals
    where t % i == 0
    select (t - time) * i;

  data.First().Dump("Answer 1");

  // --- Part Two ---

  var list = input[1].Split(',').Select(x => x == "x" ? 1L : decimal.Parse(x)).Select((x, i) => (factor: x, position: (decimal)i)).Where(x => x.factor != 1).OrderByDescending(x => x.factor).ToArray();

  // First run:
  //MySequence(list[0].factor - list[0].position, list.Aggregate((decimal)1, (x, y) => x * y.factor), list[0].factor)

  MySequence(64658750, list.Aggregate((decimal)1, (x, y) => x * y.factor), 168482981)
    .Where(a => Decimal.Remainder(a + list[0].position, list[0].factor) == 0)
    //.Take(4).Dump() // Note startpos + factor and apply in MySequence()
    .Where(a => Decimal.Remainder(a + list[1].position, list[1].factor) == 0)
    //.Take(4).Dump() // Note startpos + factor and apply in MySequence()
    .Where(a => Decimal.Remainder(a + list[2].position, list[2].factor) == 0)
    //.Take(4).Dump() // Note startpos + factor and apply in MySequence()
    .Where(a => Decimal.Remainder(a + list[3].position, list[3].factor) == 0)
    .FirstOrDefault(a => list[4..].All(x => Decimal.Remainder(a + x.position, x.factor) == 0))
    .Dump("Answer 2");
}

public IEnumerable<decimal> MySequence(decimal start, decimal end, decimal factor)
{
  for (var i = start; i < end; i += factor)
  {
    yield return i;
  }
}