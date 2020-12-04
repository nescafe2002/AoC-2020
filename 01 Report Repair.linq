<Query Kind="Statements" />

var input = new[] { 1721, 979, 366, 299, 675, 1456 };

input = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "01 input.txt")).Select(int.Parse).Distinct().ToArray();

var list = input.Select(x => (sum: x, product: (long)x)).ToList();

for (var i = 1; i <= 2; i++)
{
  list = (
    from x in list
    from y in input
    let s = x.sum + y
    where s <= 2020
    select (sum: s, product: x.product * y)
  ).Distinct().ToList();

  list.First(x => x.sum == 2020).product.Dump($"Answer {i}");
}