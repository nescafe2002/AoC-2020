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
}

// --- Part Two ---

map.Where(x => x.Value).ToList().ForEach(x => map[x.Key] = false);

// Dictionary of keys => visible neighbors

var visibles = map.Keys.ToDictionary(
  x => x,
  x => new[] { (0, 1), (0, -1), (1, 0), (-1, 0), (1, 1), (1, -1), (-1, 1), (-1, -1) } // Vectors (dRow, dCol)
    .Select(v =>
      Enumerable.Range(1, 50) // Needs tuning
        .Select(i => (x.row + i * v.Item1, x.col + i * v.Item2)) // Key + i * Vector
        .Where(x => x.Item1 >= 0 && x.Item1 < height)
        .Where(x => x.Item2 >= 0 && x.Item2 < width)
        .Where(x => map.ContainsKey(x)) // Has seat
        .Take(1)) // First visible seat
    .Where(x => x.Any())
    .SelectMany(x => x));

while (true)
{
  var newStates = (
    from key in map.Keys
    let neighbors = visibles.TryGetValue(key, out var value) ? value.Count(x => occupied(x)) : 0 // Count visible occupied seats
    where map.TryGetValue(key, out var value) && (value ? neighbors >= 5 : neighbors == 0)
    select key).ToList();

  if (!newStates.Any())
  {
    map.Count(x => x.Value).Dump("Answer 2");
    break;
  }

  foreach (var item in newStates)
  {
    map[item] ^= true;
  }
}