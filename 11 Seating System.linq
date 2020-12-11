<Query Kind="Statements">
  <Reference Relative="11 input.txt">C:\Drive\Challenges\AoC 2020\11 input.txt</Reference>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

var input = @"L.LL.LL.LL
LLLLLLL.LL
L.L.L..L..
LLLL.LL.LL
L.LL.LL.LL
L.LLLLL.LL
..L.L.....
LLLLLLLLLL
L.LLLLLL.L
L.LLLLL.LL".Split("\r\n");

input = File.ReadAllLines("11 input.txt");

var height = input.Length;
var width = input[0].Length;

var map = Enumerable.ToDictionary(
  from row in Enumerable.Range(0, height)
  from col in Enumerable.Range(0, width)
  where input[row][col] == 'L' // empty seat
  select (row, col),
  x => x,
  _ => false);

bool occupied((int row, int col) key) => map.TryGetValue(key, out var value) ? value : false;

var dc = new DumpContainer(Enumerable.Range(0, height).Select(row =>string.Join("", Enumerable.Range(0, width).Select(col => occupied((row, col)) ? "#" : " ")))); // .Dump();

while (true)
{
  var newStates = new List<((int row, int col), bool value)>();

  for (var row = 0; row < height; row++)
  {
    var neighbors = 0;

    for (var col = -1; col < width; col++)
    {
      neighbors += Enumerable.Range(row - 1, 3).Count(x => occupied((x, col + 1))); // One col to right
      neighbors -= Enumerable.Range(row - 1, 3).Count(x => occupied((x, col - 2))); // Two cols to left

      var key = (row, col);

      if (map.TryGetValue(key, out var value) && (value ? neighbors > 4 : neighbors == 0))
      {
        newStates.Add((key, !value));
      }
    }
  }
  if (!newStates.Any())
  {
    map.Count(x => x.Value).Dump("Answer 1");
    break;
  }
  foreach (var item in newStates)
  {
    map[item.Item1] = item.Item2;
  }
  dc.Refresh();
  //await Task.Delay(500);
}

// --- Part Two ---

foreach (var item in map.Keys.ToList())
{
  map[item] = false;
}

IEnumerable<(int row, int col)> visible((int row, int col) key) =>
  new[] { (0, 1), (0, -1), (1, 0), (-1, 0), (1, 1), (1, -1), (-1, 1), (-1, -1) }
    .Select(v =>
      Enumerable.Range(1, 50) // Needs tuning
        .Select(i => (key.row + i * v.Item1, key.col + i * v.Item2))
        .Where(x => x.Item1 >= 0 && x.Item1 < height)
        .Where(x => x.Item2 >= 0 && x.Item2 < width)
        .Where(x => map.ContainsKey(x))
        .Take(1))
    .Where(x => x.Any())
    .SelectMany(x => x);

var visibles = map.Keys.ToDictionary(x => x, x => visible(x));

while (true)
{
  var newStates = new List<((int row, int col), bool value)>();

  for (var row = 0; row < height; row++)
  {
    for (var col = 0; col < width; col++)
    {
      var key = (row, col);

      var neighbors = visibles.TryGetValue(key, out var value) ? value.Count(x => map.TryGetValue(x, out var value) && value) : 0;
      
      if (map.TryGetValue(key, out var value2) && (value2 ? neighbors >= 5 : neighbors == 0))
      {
        newStates.Add((key, !value2));
      }
    }
  }
  if (!newStates.Any())
  {
    map.Count(x => x.Value).Dump("Answer 2");
    break;
  }
  foreach (var item in newStates)
  {
    map[item.Item1] = item.Item2;
  }
  dc.Refresh();
  //await Task.Delay(500);
}