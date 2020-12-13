<Query Kind="Statements">
  <Reference Relative="13 input.txt">C:\Drive\Challenges\AoC 2020\13 input.txt</Reference>
</Query>

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

input[1]
  .Split(',')
  .Select(x => x == "x" ? 1 : int.Parse(x))
  .Select((x, i) => (factor: x, position: i))
  .Where(x => x.factor != 1)
  .Aggregate(
    (index: 0m, factor: 1m),
    (acc, item) =>
      Enumerable
        .Range(1, 100000)
        .Select(x => acc.index + acc.factor * x) // for (var x = acc.index; ; x += acc.factor)
        .Where(x => (x + item.position) % item.factor == 0) // divisible by current position
        .Take(2) // take 2 to determine new factor
        .GroupBy(x => 1) // aggregate
        .Select(x => (index: x.Min(), factor: x.Max() - x.Min())) // determine starting index + factor for next round (or final answer)
        .First())
  .index
  .Dump("Answer 2");