namespace fifthLab;

class Program
{
    static void Main()
    {
        Number num1 = new Number(5);
        Number num2 = new Number(9);
        Number result = num1 + num2;
        Console.WriteLine(result.Value);
    }
}