<Query Kind="Statements">
  <Reference Relative="12 input.txt">C:\Drive\Challenges\AoC 2020\12 input.txt</Reference>
</Query>

var input = @"F10
N3
F7
R90
F11".Split("\r\n");

input = File.ReadAllLines("12 input.txt");

const int EAST = 0;
const int SOUTH = 1;
const int WEST = 2;
const int NORTH = 3;

var orientation = EAST;

int distance(int x, int y) => Math.Abs(x) + Math.Abs(y);

var movements =
  input
    .Select(x => (action: x[0], value: int.Parse(x[1..])))
    .ToLookup(
      x => x.action switch
      {
        'N' => NORTH,
        'S' => SOUTH,
        'E' => EAST,
        'W' => WEST,
        'L' => orientation = (orientation + 3 * x.value / 90) % 4,
        'R' => orientation = (orientation + x.value / 90) % 4,
        _ => orientation,
      },
      x => x.action switch { 'L' => 0, 'R' => 0, _ => x.value });

distance(movements[EAST].Sum() - movements[WEST].Sum(), movements[SOUTH].Sum() - movements[NORTH].Sum()).Dump("Answer 1");

// --- Part Two ---

var wp = (x: 10, y: 1); // The waypoint starts 10 units east and 1 unit north relative to the ship.
var ship = (x: 0, y: 0); // The waypoint is relative to the ship; that is, if the ship moves, the waypoint moves with it.

foreach (var item in input)
{
  var action = item[0];
  var value = int.Parse(item[1..]);

  switch (action)
  {
    case 'N':
      wp.y += value;
      break;
    case 'S':
      wp.y -= value;
      break;
    case 'E':
      wp.x += value;
      break;
    case 'W':
      wp.x -= value;
      break;
    case 'L':
      for (var i = 0; i < value; i += 90)
      {
        wp = (-wp.y, wp.x);
      }
      break;
    case 'R':
      for (var i = 0; i < value; i += 90)
      {
        wp = (wp.y, -wp.x);
      }
      break;
    case 'F':
      ship = (ship.x + wp.x * value, ship.y + wp.y * value);
      break;
  }
}

distance(ship.x, ship.y).Dump("Answer 2");