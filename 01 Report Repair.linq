<Query Kind="Statements">
  <Reference Relative="01 input.txt">C:\Drive\Challenges\AoC 2020\01 input.txt</Reference>
</Query>

var input = new[] { 1721, 979, 366, 299, 675, 1456 };

input = File.ReadAllLines("01 input.txt").Select(int.Parse).Distinct().ToArray();

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