<Query Kind="Statements">
  <Reference Relative="06 input.txt">C:\Drive\Challenges\AoC 2020\06 input.txt</Reference>
</Query>

var input = File.ReadAllText("06 input.txt");

var groups =
  input
    .Split(new[] { "\r\n\r\n", "\n\n" }, StringSplitOptions.RemoveEmptyEntries) // Split groups
    .Select(x => x.Split(new[] { "\r\n", "\n"}, StringSplitOptions.RemoveEmptyEntries).ToList()) // Split persons
    .ToList();

groups.Sum(x => string.Join("", x).Distinct().Count()).Dump("Answer 1");

groups.Sum(x => string.Join("", x).Distinct().Count(y => x.All(z => z.Contains(y)))).Dump("Answer 2");