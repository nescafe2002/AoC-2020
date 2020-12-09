<Query Kind="Statements">
  <Reference Relative="09 input.txt">C:\Drive\Challenges\AoC 2020\09 input.txt</Reference>
  <NuGetReference>morelinq</NuGetReference>
  <Namespace>MoreLinq</Namespace>
</Query>

var input = @"35
20
15
25
47
40
62
55
65
95
102
117
150
182
127
219
299
277
309
576".Split("\r\n").Select(long.Parse);

var window = 5;

input = File.ReadAllLines("09 input.txt").Select(long.Parse);

window = 25;

IEnumerable<long> cartesian(IEnumerable<long> source) => from x in source from y in source where x < y select x + y;

var answer1 =
  input
    .Window(window + 1) // n + 1 elements
    .First(x => !cartesian(Enumerable.SkipLast(x, 1)).Contains(x.Last())) // sum of n elements == current element
    .Last() // current element
    .Dump("Answer 1");

Enumerable
  .Range(0, int.MaxValue)
  .Skip(2) // take pairs of at least 2 elements
  .Select(i => input.Window(i).FirstOrDefault(x => x.Sum() == answer1)) // first pair of length i with sum(pair) == answer 1
  .Where(x => x != null) // find successful pair
  .Select(x => x.Min() + x.Max()) // min + max
  .First()
  .Dump("Answer 2");