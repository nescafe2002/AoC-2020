<Query Kind="Statements" />

var (card, door) = (10212254, 12577395);

//(card, door) = (5764801, 17807724);

int loopSize(int n, int value = 1) => Enumerable.Range(1, int.MaxValue).Select(i => (value = (value * 7) % 20201227, i)).First(x => x.Item1 == n).i;

long privateKey(long subject, int n) => Enumerable.Range(1, n).Aggregate(1L, (acc, x) => (acc * subject) % 20201227);

privateKey(door, loopSize(card)).Dump("Answer 1");