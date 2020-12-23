<Query Kind="Statements" />

var input = new[] { 3, 8, 9, 1, 2, 5, 4, 6, 7 };

input = new[] { 6, 4, 3, 7, 1, 9, 2, 5, 8 };

static LinkedListNode<int> next(LinkedListNode<int> node) => node.Next ?? node.List.First;

static IEnumerable<LinkedListNode<int>> findNext(LinkedListNode<int> node, int count) => Enumerable.Range(0, count).Select(x => node = node.Next ?? node.List.First);

LinkedListNode<int> compute(IEnumerable<int> input, bool partTwo)
{
  var list = new LinkedList<int>(input.Concat(partTwo ? Enumerable.Range(10, 1000000 - 9) : Enumerable.Empty<int>()));

  var dic = findNext(list.First, list.Count).ToDictionary(x => x.Value);

  var max = partTwo ? list.Last.Value : list.Max();

  var currentNode = list.First;

  for (var i = partTwo ? 10000000 : 100; i > 0; i--)
  {
    var pick1 = next(currentNode);
    var pick2 = next(pick1);
    var pick3 = next(pick2);

    list.Remove(pick1);
    list.Remove(pick2);
    list.Remove(pick3);

    var destNr = currentNode.Value - 1;

    while (true)
    {
      if (destNr < 1)
      {
        destNr = max;
      }
      else if (destNr == pick1.Value || destNr == pick2.Value || destNr == pick3.Value)
      {
        destNr = destNr - 1;
      }
      else
      {
        break;
      }
    }

    var item = dic[destNr];

    list.AddAfter(item, item = pick1);
    list.AddAfter(item, item = pick2);
    list.AddAfter(item, item = pick3);

    currentNode = next(currentNode);
  }

  return dic[1];
}

string.Join("", findNext(compute(input, false), 8).Select(x => x.Value)).Dump("Answer 1");

findNext(compute(input, true), 2).Aggregate(1L, (acc, x) => acc * x.Value).Dump("Answer 2");