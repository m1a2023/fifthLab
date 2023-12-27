using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace fifthLab
{
    class Token
    {
        //Number, operation and parenthesis
        
    }

    class Number : Token
    {
        //here is numbers
        public double value { get; set; }
        public Number(double _value)
        {
            value = _value;
        }

        //new operators for Number
        public static Number operator +(Number x, Number y)
        {
            return new Number(x.value + y.value);
        }
        public static Number operator -(Number x, Number y)
        {
            return new Number(x.value - y.value);
        }
        public static Number operator *(Number x, Number y)
        {
            return new Number(x.value * y.value);
        }
        public static Number operator /(Number x, Number y)
        {
            if (y.value != 0)
            {
                return new Number(x.value / y.value);
            }
            else
            {
                throw new ArgumentException("Division by zero!");
            }
        }

        //simpler output
        public static implicit operator double(Number _value)
        {
            return _value.value;
        }

        //parsing - useful!
        public static Number Parse(string input)
        {
            return new Number(double.Parse(input));
        }
        public static bool TryParse([NotNullWhen(true)] string? input, out Number output)
        {
            if (input == null)
            {
                output = new Number(0);
                return false;
            }
            for (int i = 0; i < input.Length; i++)
            {
                if (!char.IsDigit(input[i]))
                {
                    output = new Number(0);
                    return false;
                }
            }
            output = new Number(double.Parse(input));
            return true;
        }
    }

    class Operation : Token
    {
        //here is +, -, *, /
        public char operation { get; set; }
        public int priority;
        public Operation(char _operation)
        {
            operation = _operation;
            if (IsOperator(_operation))
            {
                priority = GetPriority(_operation);
            }
        }

        //parsing and other methods for Operation
        private static int GetPriority(char operation)
        {
            if ("+-*/".Contains(operation))
            {
                return operation switch
                {
                    '*' => 2,
                    '/' => 2,
                    '-' => 1,
                    '+' => 1,
                    _ => throw new ArgumentException("Incorrect operation!")
                };
            }
            else
            {
                throw new ArgumentException("Incorrect operation!");
            }
        }
        public static Operation Parse(string input)
        {
            if (TryParse(input, out Operation output))
            {
                return output;
            }
            else
            {
                throw new ArgumentException("Parse is impossible");
            }
        }
        public static bool TryParse([NotNullWhen(true)] string? input, out Operation output)
        {
            output = new Operation('\0');
            if (input == null)
            {
                return false;
            }
            if (input.Length != 1)
            {
                return false;
            }
            if (!"+-*/".Contains(input[0]))
            {
                return false;
            }
            output = new Operation(input[0]);
            return true;
        }
        public static bool IsOperator(char symbol)
        {
            if ("+-*/".Contains(symbol))
            {
                return true;
            }
            return false;
        }
    }

    class Parenthesis : Token
    {
        // here is '(' or ')'
        public char bracket { get; set; }
        public bool isOpen;
        public Parenthesis(char _bracket)
        {
            bracket = _bracket;
            if (IsBracket(_bracket))
            {
                isOpen = Transparency(_bracket);
            }
        }

        //parsing and other methods for Parenthesis
        private bool Transparency(char _bracket)
        {
            return isOpen = _bracket switch
            {
                '(' => true,
                ')' => false,
                _ => throw new ArgumentException("Incorrect bracket!")
            };
        }
        public static Parenthesis Parse(string input)
        {
            if (char.TryParse(input, out char output))
            {
                if ("()".Contains(output))
                {
                    return new Parenthesis(output);
                }
                else
                {
                    throw new ArgumentException("Parse is impossible");
                }
            }
            else
            {
                throw new ArgumentException("Parse is impossible");
            }
        }
        public static bool TryParse([NotNullWhen(true)] string? input, out  Parenthesis output)
        {
            output = new Parenthesis('\0');
            if (input == null)
            {
                return false;
            }
            if (input.Length != 1)
            {
                return false;
            }
            if (!"()".Contains(input[0]))
            {
                return false;
            }
            output = new Parenthesis(input[0]);
            return true;
        }
        public static bool IsBracket(char _bracket)
        {
            if ("()".Contains(_bracket))
            {
                return true;
            }
            return false;
        }
    }
}