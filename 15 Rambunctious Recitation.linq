<Query Kind="Statements" />

var start = new[] { 6, 3, 15, 13, 1, 0 };
var round = 1;

foreach (var rounds in new[] { 2020, 30000000 })
{
  var memory = new Dictionary<int, int>();
  var next = -1;

  for (var i = 0; i < rounds - 1; i++)
  {
    var num = i < start.Length ? start[i] : next;

    next = memory.TryGetValue(num, out var last) ? i - last : 0;

    memory[num] = i;
  }

  next.Dump($"Answer {round++}");
}