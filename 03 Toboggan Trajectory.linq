<Query Kind="Statements">
  <Reference Relative="03 input.txt">C:\Drive\Challenges\AoC 2020\03 input.txt</Reference>
</Query>

var input = @"..##.......
#...#...#..
.#....#..#.
..#.#...#.#
.#...##..#.
..#.##.....
.#.#.#....#
.#........#
#.##...#...
#...##....#
.#..#...#.#".Split("\r\n");

input = File.ReadAllLines("03 input.txt");

var height = input.Length;
var width = input[0].Length;

var map = Enumerable.ToHashSet(
  from i in Enumerable.Range(0, height)
  from j in Enumerable.Range(0, width)
  where input[i][j] == '#'
  select (i, j));

int countTrees((int, int) slope) => Enumerable.Range(0, (height / slope.Item1) + 1).Count(i => map.Contains((i * slope.Item1, (i * slope.Item2) % width)));

var answer1 = countTrees((1, 3)).Dump("Answer 1");

new[] { (1, 1), /* (1, 3), */ (1, 5), (1, 7), (2, 1) }.Aggregate((long)answer1, (acc, x) => acc * countTrees(x)).Dump("Answer 2");