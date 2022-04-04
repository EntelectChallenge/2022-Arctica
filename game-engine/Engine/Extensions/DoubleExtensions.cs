namespace Engine.Extensions;

public static class DoubleExtensions
{
    public static double NeverLessThan(this double quantity, double lowerBound)
    {
        return quantity < lowerBound ? lowerBound : quantity;
    }
    
    public static double NeverMoreThan(this double quantity, double upperBound)
    {
        return quantity > upperBound ? upperBound : quantity;
    }
}
