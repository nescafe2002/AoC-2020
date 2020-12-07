<Query Kind="Statements">
  <Reference Relative="07 input.txt">C:\Drive\Challenges\AoC 2020\07 input.txt</Reference>
</Query>

var input = @"light red bags contain 1 bright white bag, 2 muted yellow bags.
dark orange bags contain 3 bright white bags, 4 muted yellow bags.
bright white bags contain 1 shiny gold bag.
muted yellow bags contain 2 shiny gold bags, 9 faded blue bags.
shiny gold bags contain 1 dark olive bag, 2 vibrant plum bags.
dark olive bags contain 3 faded blue bags, 4 dotted black bags.
vibrant plum bags contain 5 faded blue bags, 6 dotted black bags.
faded blue bags contain no other bags.
dotted black bags contain no other bags.";

input = @"shiny gold bags contain 2 dark red bags.
dark red bags contain 2 dark orange bags.
dark orange bags contain 2 dark yellow bags.
dark yellow bags contain 2 dark green bags.
dark green bags contain 2 dark blue bags.
dark blue bags contain 2 dark violet bags.
dark violet bags contain no other bags.";

input = File.ReadAllText("07 input.txt");

var rules =
  Regex
    .Matches(input, @"([\w ]+) bags contain (?:(?:(\d+) ([\w ]+)|no other) bags?(?:, )?)+\.")
    .ToDictionary(
      match => match.Groups[1].Value, // ([\w ]+)
      match =>
        Enumerable
          .Range(0, match.Groups[2].Captures.Count) // (\d+)
          .Select(i => (
            count: int.Parse(match.Groups[2].Captures[i].Value), // (\d+)
            color: match.Groups[3].Captures[i].Value))); // ([\w ]+)|no other)

// How many bag colors can eventually contain at least one shiny gold bag?

var reversedLookup = rules.SelectMany(x => x.Value.Select(y => (x.Key, y.color))).ToLookup(x => x.color, x => x.Key);
var queue = new Queue<string>(new[] { "shiny gold" });
var set = new HashSet<string>();

while (queue.TryDequeue(out var bag))
{
  foreach (var item in reversedLookup[bag]) // find the bags which can hold the current bag
  {
    if (set.Add(item)) // keep track of already added bags
    {
      queue.Enqueue(item);
    }
  }
}

set.Count.Dump("Answer 1");

// How many individual bags are required inside your single shiny gold bag?

var bags = new List<(int count, string color)>() { (1, "shiny gold") };

for (int i = 0; i < bags.Count; i++) // bags.Count is re-evaluated every cycle
{
  var bag = bags[i];
  foreach (var item in rules[bag.color]) // find the bags in the current bag
  {
    bags.Add((item.count * bag.count, item.color));
  }
}

bags.Skip(1).Sum(x => x.count).Dump("Answer 2");