namespace RoundRobinGroupLibrary.Extensions
{
    public static class Int32Extensions
    {
        public static bool IsOdd(this int value) => value % 2 != 0;
        public static bool IsEven(this int value) => value % 2 == 0;
    }
}