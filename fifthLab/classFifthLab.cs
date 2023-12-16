using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
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
        
        public void Print()
        {
            Console.WriteLine(token.ToString());
        }
    }

    class Number : Token
    {
        //here is numbers
        public double Value { get; set; }
        public Number(double value)
        {
            Value = value;
        }
        public static Number operator +(Number x, Number y)
        {
            return new Number(x.Value + y.Value);
        }
        public static Number operator -(Number x, Number y)
        {
            return new Number(x.Value - y.Value);
        }
        public static Number operator *(Number x, Number y)
        {
            return new Number(x.Value * y.Value);
        }
        public static Number operator /(Number x, Number y)
        {
            if (y.Value != 0)
            {
                return new Number(x.Value / y.Value);
            }
            else
            {
                throw new ArgumentException("Division by zero!");
            }
        }
        
    }

    class Operation : Token
    {
        //here is +, -, *, /
        public char operation { get; set; }
        public int priority;

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
    }

    class Parenthesis : Token
    {
        // here is '(' or ')'
        public char bracket { get; set; }
        public bool isOpen;
        private bool Transparency(char bracket)
        {
            return isOpen = bracket switch
            {
                '(' => true,
                ')' => false
            };
        }
    }
}
