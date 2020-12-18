<Query Kind="Statements">
  <Reference Relative="16 input.txt">C:\Drive\Challenges\AoC 2020\16 input.txt</Reference>
</Query>

var input = @"class: 1-3 or 5-7
row: 6-11 or 33-44
seat: 13-40 or 45-50

your ticket:
7,1,14

nearby tickets:
7,3,47
40,4,50
55,2,20
38,6,12".Split("\r\n\r\n").Select(x => x.Split("\r\n")).ToArray();

input = @"class: 0-1 or 4-19
row: 0-5 or 8-19
seat: 0-13 or 16-19

your ticket:
11,12,13

nearby tickets:
3,9,18
15,1,5
5,14,9".Split("\r\n\r\n").Select(x => x.Split("\r\n")).ToArray();

input = File.ReadAllText("16 input.txt").Split("\n\n").Select(x => x.Split("\n", StringSplitOptions.RemoveEmptyEntries)).ToArray();

var fields = input[0].Select(x => x.Split(": ")).Select(x => (name: x[0], allowed: x[1].Split(" or ").Select(y => y.Split('-').Select(int.Parse).ToArray()).Select(y => (min: y[0], max: y[1])))).ToArray();
var your = input[1].Skip(1).Select(x => x.Split(',').Select(int.Parse).ToArray()).ToArray();
var nearby = input[2].Skip(1).Select(x => x.Split(',').Select(int.Parse).ToArray()).ToArray();

nearby.SelectMany(x => x).Where(x => !fields.Any(field => field.allowed.Any(a => a.min <= x && x <= a.max))).Sum().Dump("Answer 1");

// --- Part Two ---

// Discard invalid tickets

var valid = your.Concat(nearby).Where(x => x.All(y => fields.Any(z => z.allowed.Any(a => a.min <= y && y <= a.max)))).ToArray();

// Enumerate all possible field / index combinations

var data = (
  from index in Enumerable.Range(0, your[0].Length)
  from field in fields
  where valid.All(x => field.allowed.Any(a => a.min <= x[index] && x[index] <= a.max))
  select (index, field.name)).ToArray();

// Store known field name => index combinations

var known = new Dictionary<string, int>();

while (known.Count < fields.Length)
{
  var next = data.Where(x => !known.ContainsKey(x.name)).GroupBy(x => x.index).Single(x => x.Count() == 1).First();
  known.Add(next.name, next.index);
}

// look for the six fields on your ticket that start with the word departure
// What do you get if you multiply those six values together?

known.Where(x => x.Key.StartsWith("departure ")).Aggregate(1L, (acc, x) => acc * your[0][x.Value]).Dump("Answer 2");