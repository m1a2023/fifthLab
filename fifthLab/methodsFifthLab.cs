using System;
using System.Collections.Generic;

namespace fifthLab
{
    class RPN
    {
        public static List<Token> TokenListMake(string input)
        {
            List<Token> tokens = new List<Token>();
            string numberString = null;
            for (int i = 0; i < input.Length; i++)
            {
                if (char.IsDigit(input[i]))
                {
                    numberString += input[i];
                }
                else if (input[i] != ' ')
                {
                    if (Number.TryParse(numberString, out Number outputNumber))
                    {
                        tokens.Add(outputNumber);
                    }
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

        public static List<Token> RewriteToRPN(List<Token> tokens)
        {
            List<Token> output = new List<Token>();
            Stack<Token> stack = new Stack<Token>();
            foreach (Token token in tokens)
            {
                if (stack.Count == 0 && !(token is Number))
                {
                    stack.Push(token);
                    continue;
                }
                if (token is Operation)
                {
                    if (stack.Peek() is Parenthesis)
                    {
                        stack.Push(token);
                        continue;
                    }
                    if (((Operation)token).priority > ((Operation)stack.Peek()).priority)
                    {
                        stack.Push(token);
                    }
                    else if (((Operation)token).priority <= ((Operation)stack.Peek()).priority)
                    {
                        while (stack.Count > 0 && !(token is Parenthesis))
                        {
                            output.Add(stack.Pop());
                        }
                        stack.Push(token);
                    }
                }
                else if (token is Parenthesis)
                {
                    if (!((Parenthesis)token).isOpen)
                    {
                        while (!(stack.Peek() is Parenthesis))
                        {
                            output.Add(stack.Pop());
                        }
                        stack.Pop();
                    }
                    else
                    {
                        stack.Push(token);
                    }
                }
                else if (token is Number)
                {
                    output.Add(token);
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
                if (token is Number)
                {
                    stack.Push((Number)token);
                }
                else if (token is Operation)
                {
                    Number b = stack.Pop();
                    Number a = stack.Pop();
                    Number result = Perform((Operation)token, a, b);
                    stack.Push(result);
                }
            }
            return stack.Pop();
        }
        public static Number Perform(Operation inputOperation, Number inputA, Number inputB)
        {
            return inputOperation.operation switch
            {
                '+' => inputA + inputB,
                '-' => inputA - inputB,
                '*' => inputA * inputB,
                '/' => inputA / inputB,
                _ => throw new ArgumentException("Invalid operation")
            };
        }
    }
}