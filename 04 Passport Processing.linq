<Query Kind="Statements" />

var input = @"ecl:gry pid:860033327 eyr:2020 hcl:#fffffd
byr:1937 iyr:2017 cid:147 hgt:183cm

iyr:2013 ecl:amb cid:350 eyr:2023 pid:028048884
hcl:#cfa07d byr:1929

hcl:#ae17e1 iyr:2013
eyr:2024
ecl:brn pid:760753108 byr:1931
hgt:179cm

hcl:#cfa07d eyr:2025 pid:166559648
iyr:2011 ecl:brn hgt:59in";

input = File.ReadAllText(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "04 input.txt"));

var passports = (
  from s in input.Split(new[] { "\r\n\r\n", "\n\n" }, StringSplitOptions.RemoveEmptyEntries)
  let l = s.Split(new[] { " ", "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries)
  select l.Select(x => x.Split(':', 2)).ToDictionary(x => x[0], x => x[1])).ToList();

var requiredFields = new[] { "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid" /*, "cid" */ };

passports.Count(x => requiredFields.All(y => x.ContainsKey(y))).Dump("Answer 1");

bool isValidProperty(string key, string value)
{
  switch (key)
  {
    case "byr" when int.TryParse(value, out var byr) && 1920 <= byr && byr <= 2002:
    case "iyr" when int.TryParse(value, out var iyr) && 2010 <= iyr && iyr <= 2020:
    case "eyr" when int.TryParse(value, out var eyr) && 2020 <= eyr && eyr <= 2030:
    case "hgt" when value.EndsWith("cm") && int.TryParse(value.Substring(0, value.Length - 2), out var cm) && 150 <= cm && cm <= 193:
    case "hgt" when value.EndsWith("in") && int.TryParse(value.Substring(0, value.Length - 2), out var inch) && 59 <= inch && inch <= 76:
    case "hcl" when Regex.IsMatch(value, @"^#[0-9a-f]{6}$"):
    case "ecl" when new[] { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" }.Contains(value):
    case "pid" when value.Length == 9 && int.TryParse(value, out _):
    case "cid":
      return true;
  }
  return false;
}

passports.Count(x => requiredFields.All(y => x.ContainsKey(y)) && x.All(y => isValidProperty(y.Key, y.Value))).Dump("Answer 2");