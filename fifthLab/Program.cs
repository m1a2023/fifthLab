namespace fifthLab;

class Program
{
    static void Main()
    {
        //input values
        string inputConsole = Console.ReadLine();
        //make tokens list
        List<Token> TokenList = RPN.TokenListMake(inputConsole);
        //rewrites tokens to RP notation
        List<Token> TokenListRPN = RPN.RewriteToRPN(TokenList);
        //calculate 
        Number result = RPN.CalculateRPN(TokenListRPN);
        //output result
        Console.WriteLine(result);
    }
}