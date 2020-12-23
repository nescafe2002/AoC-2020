<Query Kind="Statements" />

var input = new[] { 3, 8, 9, 1, 2, 5, 4, 6, 7 };

input = new[] { 6, 4, 3, 7, 1, 9, 2, 5, 8 };

var list = new LinkedList<int>(input);

IEnumerable<LinkedListNode<int>> right(LinkedListNode<int> node, int i) => Enumerable.Range(0, i).Select(x => node = node.Next ?? node.List.First);

var dic = right(list.First, list.Count).ToDictionary(x => x.Value);

for (var i = 10; i <= 1000000; i++)
{
  dic[i] = list.AddLast(i); // disable for Part One
}

var current = list.First;
var max = list.Max();

for (var i = 0; i < 10000000; i++) // i < 100 for Part One
{
  var next = right(current, 3).ToList();

  next.ForEach(x => list.Remove(x));

  var nr = current.Value - 1;

  while (true)
  {
    if (nr < 1)
    {
      nr = max;
    }
    else if (next.Any(x => x.Value == nr))
    {
      nr = nr - 1;
    }
    else
    {
      break;
    }
  }

  var item = dic[nr];

  next.ForEach(x => list.AddAfter(item, item = x));

  current = current.Next ?? list.First;
}

string.Join("", right(list.Find(1), 8).Select(x => x.Value)).Dump("Answer 1");

right(list.Find(1), 2).Aggregate(1L, (acc, x) => acc * x.Value).Dump("Answer 2");