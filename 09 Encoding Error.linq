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

IEnumerable<long> cartesian(IEnumerable<long> source) => source.Cartesian(source, (x, y) => x + y);

var answer1 = input.Window(window + 1).First(x => !cartesian(MoreEnumerable.SkipLast(x, 1)).Contains(x.Last())).Last().Dump("Answer 1");

Enumerable.Range(0, int.MaxValue).Skip(2).Select(i => input.Window(i).Where(x => x.Sum() == answer1)).First(x => x.Any()).Select(x => x.Min() + x.Max()).First().Dump("Answer 2");