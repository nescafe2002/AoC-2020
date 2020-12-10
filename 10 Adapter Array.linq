<Query Kind="Statements">
  <Reference Relative="10 input.txt">C:\Drive\Challenges\AoC 2020\10 input.txt</Reference>
  <NuGetReference>morelinq</NuGetReference>
  <Namespace>MoreLinq</Namespace>
</Query>

var input = @"28
33
18
42
31
14
46
20
48
47
24
23
49
45
19
38
39
11
1
32
25
35
8
17
7
9
4
2
34
10
3".Split("\r\n").Select(int.Parse);

input = File.ReadAllLines("10 input.txt").Select(int.Parse);

var diffs =
  new[] { 0 } // Outlet joltage
    .Concat(input) // Adapter array
    .OrderBy(x => x) // Adapter order
    .Window(2) // Take pairs
    .Select(x => x.Last() - x.First()) // Calc diff
    .Concat(new[] { 3 }) // Device joltage is 3 higher
    .GroupBy(x => x)
    .ToDictionary(x => x.Key, x => x.Count());

(diffs[1] * diffs[3]).Dump("Answer 1");

var sections =
  new[] { 0 } // Outlet joltage is 0
    .Concat(input) // Adapter array
    .OrderBy(x => x) // Adapter order
    .Window(2) // Take pairs
    .Select(x => x.Last() - x.First()) // Calc diff (actually, just 1 or 3)
    .Split(3) // Split into sections of diff 1, e.g. 32, 35, 36, 37, 38, 39, 42 => 4
    .Select(x => x.Count()); // Section length

var permutations = new Dictionary<int, long>() { { 0, 1 }, { 1, 1 }, { 2, 2 }, { 3, 4 }, { 4, 7 } };

sections.Aggregate(1L, (acc, x) => acc * permutations[x]).Dump("Answer 2");

// Permutations based on section length:

// 1
// 0, 3, 4, 7

// 2
// 0, 3, 4, 5, 8
// 0, 3, 5, 8

// 3
// 0, 3, 4, 5, 6, 9
// 0, 3, 4, 6, 9
// 0, 3, 5, 6, 9
// 0, 3, 6, 9

// 4
// 0, 3, 4, 5, 6, 7, 10
// 0, 3, 4, 5, 7, 10
// 0, 3, 4, 6, 7, 10
// 0, 3, 4, 7, 10
// 0, 3, 5, 6, 7, 10
// 0, 3, 5, 7, 10
// 0, 3, 6, 7, 10