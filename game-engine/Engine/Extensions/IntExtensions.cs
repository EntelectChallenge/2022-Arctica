namespace Engine.Extensions;

public static class IntExtensions
{
    public static int NeverLessThan(this int quantity, int lowerBound)
    {
        return quantity < lowerBound ? lowerBound : quantity;
    }
    
    public static int NeverMoreThan(this int quantity, int upperBound)
    {
        return quantity > upperBound ? upperBound : quantity;
    }
}
