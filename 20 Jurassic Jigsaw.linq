<Query Kind="Statements">
  <Reference Relative="20 example.txt">C:\Drive\Challenges\AoC 2020\20 example.txt</Reference>
  <Reference Relative="20 input.txt">C:\Drive\Challenges\AoC 2020\20 input.txt</Reference>
</Query>

var input = File.ReadAllText("20 example.txt");

input = File.ReadAllText("20 input.txt");

var length = 12;

var corners = input
  .Split("\n\n", StringSplitOptions.RemoveEmptyEntries)
  .Select(x => x.Split("\n").ToArray())
  .Select(x => (key: x[0].Substring("Tile ".Length).TrimEnd(':'), lines: x[1..].ToArray(), width: x[1].Length, height: x.Length - 1))
  .Select(x => (x.key, rows: new[] { x.lines[0], x.lines[x.height - 1] }, cols: new[] { 0, x.width - 1 }.Select(y => string.Join("", Enumerable.Range(0, x.height).Select(z => x.lines[z][y]))).ToArray()))
  .Select(x => (x.key, east: x.cols[1], north: x.rows[0], west: x.cols[0], south: x.rows[1]))
  .ToArray();

string reverse(string s) => string.Join("", s.Reverse());

var edges =
  from tile in corners
  from x in new[] { tile.east, tile.north, tile.west, tile.south }
  select (tile.key, new[] { x, reverse(x) }.OrderBy(x => x).First());

var outsideBorders = edges.Where(x => !edges.Any(y => y.key != x.key && y.Item2 == x.Item2));

var candidates = outsideBorders.GroupBy(x => x.key).Where(x => x.Count() == 2).ToArray();

candidates.Aggregate(1L, (acc, x) => acc * long.Parse(x.Key)).Dump("Answer 1");

// --- Part Two ---

var cornersWithRotation =
  corners
    .SelectMany(x => new[] {
      (x.key, version: 0, east: x.east,  north: x.north, west: x.west, south: x.south), // Original
      (x.key, version: 1, east: reverse(x.south), north: x.east, west: reverse(x.north), south: x.west), // Rot 90
      (x.key, version: 2, east: reverse(x.west), north: reverse(x.south), west: reverse(x.east), south: reverse(x.north)), // Rot 180
      (x.key, version: 3, east: x.north, north: reverse(x.west), west: x.south, south: reverse(x.east)), // Rot 270
      (x.key, version: 4, east: x.west, north: reverse(x.north), west: x.east, south: reverse(x.south)), // Flip H
      (x.key, version: 5, east: reverse(x.north), north: reverse(x.east), west: reverse(x.south), south: reverse(x.west)), // Rot 90 + Flip H
      (x.key, version: 6, east: reverse(x.east), north: x.south, west: reverse(x.west), south: x.north), // Rot 180 + Flip H
      (x.key, version: 7, east: x.south, north: x.west, west: x.north, south: x.east), // Rot 270 + Flip H
    }).ToList();

var pairsH = (
  from t in cornersWithRotation
  from u in cornersWithRotation
  where t.key != u.key
  where t.east == u.west
  select ($"{t.key}-{t.version}", $"{u.key}-{u.Item2}")).ToLookup(x => x.Item1, x => x.Item2);

var pairsV = (
  from t in cornersWithRotation
  from u in cornersWithRotation
  where t.key != u.key
  where t.south == u.north
  orderby t.key
  select ($"{t.key}-{t.version}", $"{u.key}-{u.Item2}")).ToLookup(x => x.Item1, x => x.Item2);

HashSet<string> addToHashSet(HashSet<string> set, string s)
{
  var s2 = new HashSet<string>(set);
  s2.Add(s);
  return s2;
}

IEnumerable<(int row, int col, string tile)> findHV(string s, int row, int col, HashSet<string> l)
  =>
    new[] { (row, col, s) }
      .Concat(pairsH[s].Where(x => row <= length && col < length && !l.Contains(x.Split('-')[0])).SelectMany(x => findHV(x, row, col + 1, addToHashSet(l, x.Split('-')[0]))))
      .Concat(pairsV[s].Where(x => row < length && col <= length && !l.Contains(x.Split('-')[0])).SelectMany(x => findHV(x, row + 1, col, addToHashSet(l, x.Split('-')[0]))))
      .Distinct();

var validMaps = pairsH.Concat(pairsV).Select(x => x.Key).AsParallel().Where(x => x.StartsWith(candidates.First().Key)).Select(x => findHV(x, 0, 0, new[] { x }.ToHashSet()).ToList()).Where(x => x.Count == corners.Length).ToList();

var size = 8;

IEnumerable<(int row, int col)> rotateTile(IEnumerable<(int row, int col)> pixels) => pixels.Select(x => (size - 1 - x.col, x.row));
IEnumerable<(int row, int col)> flipTileH(IEnumerable<(int row, int col)> pixels) => pixels.Select(x => (x.row, size - 1 - x.col));

var tilesWithRotation =
  input
    .Split("\n\n", StringSplitOptions.RemoveEmptyEntries)
    .Select(x => x.Split("\n").ToArray())
    .Select(x => (key: x[0].Substring("Tile ".Length).TrimEnd(':'), lines: x[1..].ToArray(), width: x[1].Length, height: x.Length - 1))
    .Select(x => (x.key, pixels: (from row in Enumerable.Range(1, x.height - 2) from col in Enumerable.Range(1, x.width - 2) where x.lines[row][col] == '#' select (row - 1, col - 1)).ToList()))
    .SelectMany(x => new[] {
      (key: $"{x.key}-0", pixels: x.pixels), // Original
      (key: $"{x.key}-1", pixels: rotateTile(x.pixels)), // Rot 90
      (key: $"{x.key}-2", pixels: rotateTile(rotateTile(x.pixels))), // Rot 180
      (key: $"{x.key}-3", pixels: rotateTile(rotateTile(rotateTile(x.pixels)))), // Rot 270
      (key: $"{x.key}-4", pixels: flipTileH(x.pixels)), // Flip H
      (key: $"{x.key}-5", pixels: flipTileH(rotateTile(x.pixels))), // Rot 90 + Flip H
      (key: $"{x.key}-6", pixels: flipTileH(rotateTile(rotateTile(x.pixels)))), // Rot 180 + Flip H
      (key: $"{x.key}-7", pixels: flipTileH(rotateTile(rotateTile(rotateTile(x.pixels))))), // Rot 270 + Flip H
    }).ToDictionary(x => x.key, x => x.pixels.ToArray());


IEnumerable<(int row, int col)> rotateMap(IEnumerable<(int row, int col)> pixels) => pixels.Select(x => (size * length - 1 - x.col, x.row));
IEnumerable<(int row, int col)> flipMapH(IEnumerable<(int row, int col)> pixels) => pixels.Select(x => (x.row, size * length - 1 - x.col));

//Enumerable.Range(0, 8).Select(row => string.Join("", Enumerable.Range(0, 8).Select(col => tiles3["2311-6"].Contains((row, col)) ? '#' : '.'))).Dump();

var maps =
  validMaps
    .Select(x => x.SelectMany(y => tilesWithRotation[y.tile].Select(z => (row: y.row * size + z.row, col: y.col * size + z.col))))
    .SelectMany(x => new[]{
      x,
      rotateMap(x),
      rotateMap(rotateMap(x)),
      rotateMap(rotateMap(rotateMap(x))),
      flipMapH(x),
      flipMapH(rotateMap(x)),
      flipMapH(rotateMap(rotateMap(x))),
      flipMapH(rotateMap(rotateMap(rotateMap(x)))),
    }.Select(x => x.ToList()))
    .Distinct()
    .ToList();

var monster = @"                  #
#    ##    ##    ###
 #  #  #  #  #  #".Split("\r\n").SelectMany((x, row) => x.Select((y, col) => (y, col)).Where(z => z.y == '#').Select(z => (row: row - 1, z.col))).ToList();

maps.Select(x => (x.Count(), x.Count(y => monster.All(z => x.Contains((y.row + z.row, y.col + z.col)))))).Where(x => x.Item2 > 0).Select(x => x.Item1 - 15 * x.Item2).Distinct().Single().Dump("Answer 2");