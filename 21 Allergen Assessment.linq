<Query Kind="Statements">
  <Reference Relative="21 input.txt">C:\Drive\Challenges\AoC 2020\21 input.txt</Reference>
</Query>

var input = @"mxmxvkd kfcds sqjhc nhms (contains dairy, fish)
trh fvjkl sbzzf mxmxvkd (contains dairy)
sqjhc fvjkl (contains soy)
sqjhc mxmxvkd sbzzf (contains fish)".Split("\r\n");

input = File.ReadAllLines("21 input.txt");

var ix = 0;

var re = new Regex(@"(.+) (contains .+)");

var lines = (
  from line in input
  let s = line.Split(" (contains ")
  let ingredients = s[0].Split()
  let allergens = s[1].TrimEnd(')').Split(", ")
  select (ix: ix++, ingredients, allergens)).ToList();

//lines.Dump();

var lookup = lines.SelectMany(x => x.allergens.Select(a => (a, x.ingredients))).ToLookup(x => x.a, x => x.ingredients);

var mapping = new Dictionary<string, string>();

for (var i = -1; i < mapping.Count; )
{
  i = mapping.Count;

  foreach (var allergen in lookup)
  {
    var ingredients = allergen.Aggregate((x, y) => x.Intersect(y).ToArray()).Except(mapping.Keys).ToArray();
    if (ingredients.Length == 1)
    {
      mapping.Add(ingredients[0], allergen.Key);
    }
  }
}

lines.SelectMany(x => x.ingredients).Where(x => !mapping.ContainsKey(x)).Count().Dump("Answer 1");

// --- Part Two ---

string.Join(',', mapping.OrderBy(x => x.Value).Select(x => x.Key)).Dump("Answer 2");