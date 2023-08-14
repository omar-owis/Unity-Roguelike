public enum StatModType
{
    Flat = 100,
    PercentAdd = 200,
    PercentMult = 300
}
public class StatModifier
{
    public readonly float Value;
    public readonly StatModType ModType;
    public readonly int Order;
    public readonly object Source;

    public StatModifier(float value, StatModType modType, int order, object source)
    {
        Value = value;
        ModType = modType;
        Order = order;
        Source = source;
    }

    public StatModifier(float value, StatModType modType) : this(value, modType, (int)modType, null) { }

    public StatModifier(float value, StatModType modType, int order) : this(value, modType, order, null) { }

    public StatModifier(float value, StatModType modType, object source) : this(value, modType, (int)modType, source) { }

}
