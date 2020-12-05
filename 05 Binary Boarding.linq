<Query Kind="Statements">
  <Reference Relative="05 input.txt">C:\Drive\Challenges\AoC 2020\05 input.txt</Reference>
</Query>

int str2dec(string s, char c)
  => Enumerable.Range(0, s.Length).Where(i => s[i] == c).Sum(i => (int)Math.Pow(2, s.Length - i - 1));

int calculateId(string s) => str2dec(s.Substring(0, 7), 'B') * 8 + str2dec(s.Substring(7, 3), 'R');

//parse("BFFFBBFRRR").Dump();
//parse("FFFBBBFRRR").Dump();
//parse("BBFFBBFRLL").Dump();

var seats = File.ReadAllLines("05 input.txt").Select(x => calculateId(x)).ToHashSet();

var min = seats.Min(); // First used seat
var max = seats.Max().Dump("Answer 1"); // Last used seat

var missing = Enumerable.Range(min, max - min + 1).Single(x => !seats.Contains(x)).Dump("Answer 2"); // Unused seat