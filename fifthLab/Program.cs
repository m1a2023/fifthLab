namespace fifthLab;

class Program
{
    static void Main()
    {
        string inputConsole = Console.ReadLine();
        List<Token> TokenList = RPN.TokenListMake(inputConsole);
        List<Token> TokenListRPN = RPN.RewriteToRPN(TokenList);
        Number result = RPN.CalculateRPN(TokenListRPN);
        Console.WriteLine(result.value);
    }
}