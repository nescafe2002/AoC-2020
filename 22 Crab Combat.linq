<Query Kind="Statements">
  <Reference Relative="22 input.txt">C:\Drive\Challenges\AoC 2020\22 input.txt</Reference>
</Query>

var input = @"Player 1:
9
2
6
3
1

Player 2:
5
8
4
7
10";

input = File.ReadAllText("22 input.txt");

var players = input
  .Split(new[] { "\r\n\r\n", "\n\n" }, StringSplitOptions.RemoveEmptyEntries)
  .Select(x => x.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries))
  .Select(x => new Queue<int>(x[1..].Select(int.Parse).ToArray()));

int game(Queue<int>[] state, bool recurse)
{
  var previousStates = state.Select(x => new HashSet<string>()).ToArray();

  while (true)
  {
    var cards = state.Select(x => x.Dequeue()).ToList(); // Players draw cards.

    var winner =
      recurse && Enumerable.Range(0, state.Length).All(x => cards[x] <= state[x].Count)
        ? game(Enumerable.Range(0, state.Length).Select(x => new Queue<int>(state[x].Take(cards[x]))).ToArray(), true) // New game.
        : cards.IndexOf(cards.Max()); // Max card wins.

    foreach (var x in new[] { winner, 1 - winner }) // Put cards on deck, winning card first.
    {
      state[winner].Enqueue(cards[x]);
    }

    if (state.Any(x => x.Count == 0)) // No cards left.
    {
      return Enumerable.Range(0, state.Length).First(x => state[x].Count > 0); // Player with cards wins.
    }

    if (Enumerable.Range(0, state.Length).Any(x => !previousStates[x].Add(string.Join(',', state[x]))))
    {
      return 0; // Recursion. Player 1 wins.
    }
  }
}

var state1 = players.ToArray();

game(state1, false);

state1.Sum(x => x.Reverse().Select((x, i) => x * (i + 1)).Sum()).Dump("Answer 1");

// --- Part Two ---

var state2 = players.ToArray();

game(state2, true);

state2.Sum(x => x.Reverse().Select((x, i) => x * (i + 1)).Sum()).Dump("Answer 2");