<Query Kind="Statements">
  <Reference Relative="24 input.txt">C:\Drive\Challenges\AoC 2020\24 input.txt</Reference>
</Query>

var input = @"sesenwnenenewseeswwswswwnenewsewsw
neeenesenwnwwswnenewnwwsewnenwseswesw
seswneswswsenwwnwse
nwnwneseeswswnenewneswwnewseswneseene
swweswneswnenwsewnwneneseenw
eesenwseswswnenwswnwnwsewwnwsene
sewnenenenesenwsewnenwwwse
wenwwweseeeweswwwnwwe
wsweesenenewnwwnwsenewsenwwsesesenwne
neeswseenwwswnwswswnw
nenwswwsewswnenenewsenwsenwnesesenew
enewnwewneswsewnwswenweswnenwsenwsw
sweneswneswneneenwnewenewwneswswnese
swwesenesewenwneswnwwneseswwne
enesenwswwswneneswsenwnewswseenwsese
wnwnesenesenenwwnenwsewesewsesesew
nenewswnwewswnenesenwnesewesw
eneswnwswnwsenenwnwnwwseeswneewsenese
neswnwewnwnwseenwseesewsenwsweewe
wseweeenwnesenwwwswnew".Split("\r\n");

input = File.ReadAllLines("24 input.txt");

var re = new Regex(@"e|se|sw|w|nw|ne");

var set = new HashSet<(int, int)>();

(int, int) neighbor((int, int) x, string direction) => direction switch
{
  "e" => (x.Item1, x.Item2 + 2),
  "se" => (x.Item1 + 1, x.Item2 + 1),
  "sw" => (x.Item1 + 1, x.Item2 - 1),
  "w" => (x.Item1, x.Item2 - 2),
  "nw" => (x.Item1 - 1, x.Item2 - 1),
  "ne" => (x.Item1 - 1, x.Item2 + 1),
};

foreach (var item in input.Select(line => re.Matches(line).Aggregate((0, 0), (acc, x) => neighbor(acc, x.Value))))
{
  if (!set.Add(item))
  {
    set.Remove(item);
  }
}

set.Count().Dump("Answer 1");

// --- Part Two ---

IEnumerable<(int, int)> neighbors((int, int) coord) => new[] { "e", "se", "sw", "w", "nw", "ne" }.Select(x => neighbor(coord, x));

for (var day = 0; day < 100; day++)
{
  var adds = new HashSet<(int, int)>();
  var removes = new HashSet<(int, int)>();

  foreach (var tile in set.SelectMany(x => neighbors(x).Append(x)).Distinct().ToList())
  {
    var count = neighbors(tile).Count(x => set.Contains(x));

    if (set.Contains(tile))
    {
      if (count == 0 || count > 2)
      {
        removes.Add(tile);
      }
    }
    else if (count == 2)
    {
      adds.Add(tile);
    }

  }
  foreach (var item in removes)
  {
    set.Remove(item);
  }
  foreach (var item in adds)
  {
    set.Add(item);
  }
}

set.Count().Dump("Answer 2");