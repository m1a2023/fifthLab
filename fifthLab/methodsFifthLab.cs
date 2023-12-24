using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fifthLab
{
    class RPN
    {
        public static List<Token> TokenListMake(string input)
        {
            List<Token> tokens = new List<Token>(); string numberString = null;
            for (int i = 0; i < input.Length; i++)
            {
                if (char.IsDigit(input[i]))
                {
                    numberString += input[i];
                }
                else if (input[i] != ' ')
                {
                    tokens.Add(Number.Parse(numberString));
                    if (Operation.TryParse(input[i].ToString(), out Operation outputOperation))
                    {
                        tokens.Add(outputOperation);
                    }
                    else if (Parenthesis.TryParse(input[i].ToString(), out Parenthesis outputBracket))
                    {
                        tokens.Add(outputBracket);
                    }
                    numberString = null;
                }
            }
            if (!string.IsNullOrEmpty(numberString))
            {
                if (Number.TryParse(numberString, out Number outputNumber))
                {
                    tokens.Add(outputNumber);
                }
            }
            return tokens;
        }

        public static List<Token> RewriteToRPN(List<Token> inputTokens)
        {
            Stack<Token> stack = new Stack<Token>();
            List<Token> output = new List<Token>();

            foreach (Token token in inputTokens)
            {
                if (Number.TryParse(token.ToString(), out Number outputNumber))
                {
                    output.Add(outputNumber);
                }

                else if (Operation.TryParse(token.ToString(), out Operation outputOperator))
                {
                    while (stack.Count > 0 && outputOperator.priority <= Operation.Parse(stack.Peek().ToString()).priority)
                    {
                        output.Add(stack.Pop());
                    }
                    stack.Push(token);
                }

                else if (Parenthesis.TryParse(token.ToString(), out Parenthesis bracketOpen))
                {
                    if (bracketOpen.isOpen)
                    {
                        stack.Push(bracketOpen);
                    }
                }

                else if (Parenthesis.TryParse(token.ToString(), out Parenthesis bracketClose))
                {
                    if (!bracketClose.isOpen)
                    {
                        while (stack.Count > 0 && Parenthesis.Parse(stack.Peek().ToString()).isOpen)
                        {
                            output.Add(stack.Pop());
                        }
                        stack.Pop();
                    }
                }
            }
            while (stack.Count > 0)
            {
                output.Add(stack.Pop());
            }

            return output;
        }

        public static Number CalculateRPN(List<Token> inputTokens)
        {
            Stack<Number> stack = new Stack<Number>();

            foreach (Token token in inputTokens)
            {
                if (Number.TryParse(token.ToString(), out Number number))
                {
                    stack.Push(number);
                }
                else if (token is Operation)
                {
                    Number firstOperand = stack.Pop();
                    Number secondOperand = stack.Pop();
                    Number result = Perform(Operation.Parse(token.ToString()), firstOperand, secondOperand);

                    stack.Push(result);
                }
            }
            return stack.Pop();
        }
        public static Number Perform(Operation Operator, Number firstNumber, Number secondNumber)
        {
            return Operator.operation switch
            {
                '+' => firstNumber + secondNumber,
                '-' => firstNumber - secondNumber,
                '*' => firstNumber * secondNumber,
                '/' => firstNumber / secondNumber
            };
        }
    }
}