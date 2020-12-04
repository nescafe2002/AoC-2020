<Query Kind="Statements">
  <Reference Relative="02 input.txt">C:\Drive\Challenges\AoC 2020\02 input.txt</Reference>
</Query>

var input = @"1-3 a: abcde
1-3 b: cdefg
2-9 c: ccccccccc";

input = File.ReadAllText("02 input.txt");

var data = (
  from m in Regex.Matches(input, @"(\d+)-(\d+) (\w): (\w+)", RegexOptions.Multiline)
  let a = int.Parse(m.Groups[1].Value) // Policy min or first position
  let b = int.Parse(m.Groups[2].Value) // Policy max or second position
  let c = (char)m.Groups[3].Value.First() // Policy character
  let pw = m.Groups[4].Value.ToCharArray() // Password
  let count = pw.Count(x => x == c) // Count of policy character in password
  select (a, b, c, pw, count)).ToList();

data.Count(x => x.a <= x.count && x.count <= x.b).Dump("Answer 1");

data.Count(x => x.pw[x.a - 1] == x.c ^ x.pw[x.b - 1] == x.c).Dump("Answer 2");
