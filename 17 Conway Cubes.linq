<Query Kind="Statements">
  <Reference Relative="17 input.txt">C:\Drive\Challenges\AoC 2020\17 input.txt</Reference>
</Query>

var input = @".#.
..#
###".Split("\r\n");

input = File.ReadAllLines("17 input.txt");

var height = input.Length;
var width = input[0].Length;

var map = Enumerable.ToHashSet(
  from row in Enumerable.Range(0, height)
  from col in Enumerable.Range(0, width)
  where input[row][col] == '#'
  select (row, col, z: 0, w: 0));

IEnumerable<(int row, int col, int z, int w)> block((int row, int col, int z, int w) key) =>
    from row in Enumerable.Range(0, 3).Select(x => key.row - 1 + x)
    from col in Enumerable.Range(0, 3).Select(x => key.col - 1 + x)
    from z in Enumerable.Range(0, 3).Select(x => key.z - 1 + x)
    //let w = 0 // <-- Answer 1
    from w in Enumerable.Range(0, 3).Select(x => key.w - 1 + x) // <-- Answer 2
    let value = (row, col, z, w)
    where value != key
    select value;

//var dc = new DumpContainer(Enumerable.Range(-6, height + 12).Select(row => string.Join("", Enumerable.Range(-6, width + 12).Select(col => map.Contains((row, col, 0, 0)) ? "#" : ".")))).Dump();
//var dc = new DumpContainer(map.OrderBy(x => x.Item3)).Dump();

for (var i = 0; i < 6; i++)
{
  var newStates = new List<((int row, int col, int z, int w), bool value)>();

  foreach (var item in map.SelectMany(x => block(x).Append(x)).Distinct())
  {
    switch (map.Contains(item))
    {
      case true when !new[] { 2, 3 }.Contains(block(item).Count(x => map.Contains(x))):
        newStates.Add((item, false));
        break;
      case false when new[] { 3 }.Contains(block(item).Count(x => map.Contains(x))):
        newStates.Add((item, true));
        break;
    }
  }

  foreach (var item in newStates)
  {
    if (item.value)
    {
      map.Add(item.Item1);
    }
    else
    {
      map.Remove(item.Item1);
    }
  }
}

map.Count.Dump("Answer");