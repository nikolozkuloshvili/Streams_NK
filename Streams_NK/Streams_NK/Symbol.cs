namespace Streams_NK;

public class Symbol : IComparable<Symbol>
{
    public int Count { get; private set; }
    public char Name { get; }
    public Symbol(char name, int count)
    {
        Name = name;
        Count = count;
    }

    public int CompareTo(Symbol? other) => other!.Count.CompareTo(Count);
}
