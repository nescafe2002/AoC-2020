<Query Kind="Statements">
  <Reference Relative="19 input.txt">C:\Drive\Challenges\AoC 2020\19 input.txt</Reference>
</Query>

var input = @"0: 4 1 5
1: 2 3 | 3 2
2: 4 4 | 5 5
3: 4 5 | 5 4
4: ""a""
5: ""b""

ababbb
bababa
abbbab
aaabbb
aaaabbb".Split("\r\n");

input = File.ReadAllLines("19 input.txt");

var rules = input.TakeWhile(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Split(": ")).ToDictionary(x => int.Parse(x[0]), x => x[1]);

var lines = input.SkipWhile(x => !string.IsNullOrWhiteSpace(x)).Skip(1).ToArray();

string parse(string s) => string.Join("", s.Split().Select(x => int.TryParse(x, out var i) ? $"({parse(rules[i])})" : x.Trim('"')));

Regex regex() => new Regex($"^{parse(rules[0])}$", RegexOptions.ExplicitCapture);

int matches(Regex re) => lines.Count(x => re.IsMatch(x));

int answer() => matches(regex());

answer().Dump("Answer 1");

// --- Part Two ---

// New rules:

// 8: 42 | 42 8
rules[8] = string.Join(" | ", Enumerable.Range(1, 5).Select(x => string.Join(' ', Enumerable.Repeat("42", x))));

// 11: 42 31 | 42 11 31
rules[11] = string.Join(" | ", Enumerable.Range(1, 5).Select(x => $"{string.Join(' ', Enumerable.Repeat("42", x))} {string.Join(' ', Enumerable.Repeat("31", x))}"));

answer().Dump("Answer 2");