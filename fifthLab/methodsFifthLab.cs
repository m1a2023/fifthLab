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
            List<Token> token = new List<Token>(); string numberLine = null;

            foreach (char pieceInput in input)
            {
                if (char.IsDigit(pieceInput))
                {
                    numberLine += pieceInput;
                }
                else
                {
                    if (pieceInput !=  ' ')
                    {
                        token.Add(Number.Parse(numberLine));
                        if (Operation.TryParse(pieceInput.ToString(), out Operation outputOperator))
                        {
                            token.Add(outputOperator);
                        }
                        else if (Parenthesis.TryParse(pieceInput.ToString(), out Parenthesis outputBracket))
                        {
                            token.Add(outputBracket);
                        }
                        numberLine = null; 
                    }
                }
            }

            if (!string.IsNullOrEmpty(numberLine))
            {
                if (Number.TryParse(numberLine.ToString(), out Number outputNumber))
                {
                    token.Add(outputNumber);
                }
                else if (Operation.TryParse(numberLine.ToString(), out Operation outputOperator))
                {
                    token.Add(outputOperator);
                }
                else if (Parenthesis.TryParse(numberLine.ToString(), out Parenthesis outputBracket))
                {
                    token.Add(outputBracket);
                }
            }
             
            return token;
        }

        public static List<Token> RewriteToRPN(List<Token> inputTokens)
        {
            Stack<Token> stack = new Stack<Token>();
            List<Token> output = new List<Token>();

            foreach (Token token in inputTokens)
            {
                if (token is Number)
                {
                    output.Add(token);
                }

                else if (token is Operation)
                {
                    while (stack.Count > 0 && Operation.Parse(token).priority <= Operation.Parse(stack.Peek()).priority)
                    {
                        output.Add(stack.Pop());
                    }
                    stack.Push(Operation.Parse(token));
                }

                else if (token is Parenthesis && Parenthesis.Parse(token).isOpen)
                {
                    stack.Push(Parenthesis.Parse(token));
                }

                else if (token is Parenthesis && !Parenthesis.Parse(token).isOpen)
                {
                    while (stack.Count > 0 && Parenthesis.Parse(stack.Peek()).isOpen)
                    {
                        output.Add(stack.Pop());
                    }
                    stack.Pop();
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
                if (Number.TryParse(token, out Number number))
                {
                    stack.Push(number);
                }
                else if (token is Operation)
                {
                    Number firstOperand = stack.Pop();
                    Number secondOperand = stack.Pop();
                    Number result = Perform(Operation.Parse(token), firstOperand, secondOperand);

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
