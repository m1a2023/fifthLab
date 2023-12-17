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
        public string token { get; set; }
        public static implicit operator string(Token _token)
        {
            return _token.token;
        }
        public void Print()
        {
            Console.WriteLine(token);
        }
    }

    class Number : Token
    {
        //here is numbers
        public double value { get; set; }
        public Number(double _value)
        {
            value = _value;
        }
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

        public static implicit operator double(Number _value)
        {
            return _value.value;
        }
        public static Number Parse(string input)
        {
            return new Number(double.Parse(input));
        }
        public static string ParseToString(Number _number)
        {
            return _number.value.ToString();
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
                if (!"1234567890".Contains(input[i]))
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
        }
        private int Priority(char operation)
        {
            if ("+-*/".Contains(operation))
            {
                return priority = operation switch
                {
                    '*' => 2,
                    '/' => 2,
                    '-' => 1,
                    '+' => 1,
                };
            }
            else
            {
                throw new ArgumentException("Incorrect operation!");
            }
        }
        public static explicit operator char(Operation _operator)
        {
            return _operator.operation;
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
        public static string ParseToString(Operation _operation)
        {
            return _operation.operation.ToString();
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
    }

    class Parenthesis : Token
    {
        // here is '(' or ')'
        public char bracket { get; set; }
        public bool isOpen;
        public Parenthesis(char _bracket)
        {
            bracket = _bracket;
        }
        public static implicit operator char(Parenthesis _bracket)
        {
            return _bracket.bracket;
        }
        private bool Transparency(char bracket)
        {
            return isOpen = bracket switch
            {
                '(' => true,
                ')' => false
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
        public static string ParseToString(Parenthesis _bracket)
        {
            return _bracket.bracket.ToString();
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
    }
}
