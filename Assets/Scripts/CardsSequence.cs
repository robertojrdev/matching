using System.Collections.Generic;

[System.Serializable]
public struct CardsSequence
{
    public List<int> units;
    public List<int> order;

    public CardsSequence(IList<int> units, IList<int> order)
    {
        this.units = new List<int>(units);
        this.order = new List<int>(order);
    }
}