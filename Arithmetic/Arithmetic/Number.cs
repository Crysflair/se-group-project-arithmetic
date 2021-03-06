﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arithmetic
{
    public class Number : IEquatable<Number>
    {
        public readonly int Numerator;//分子
        public readonly int Denominator;//分母

        public Number(int numerator, int denominator)
        {
            // check
            if (denominator == 0)
                throw new Exception("Denominator cannot be zero!");

            // if zero (to prevent pass 0 into Gcd)
            if (numerator == 0)
            {
                Numerator = 0;
                Denominator = 1;
            }
            else //non-zero
            { 
                // Apply reduction automatically, also automatically move neg sign to Numerator
                int gcd = Gcd(Math.Abs(numerator), Math.Abs(denominator));
                if (numerator * denominator > 0)
                {
                    Numerator = Math.Abs(numerator / gcd);
                    Denominator = Math.Abs(denominator / gcd);
                }
                else
                {
                    Numerator = - Math.Abs(numerator / gcd);
                    Denominator = Math.Abs(denominator / gcd);
                }
            }
        }

        private static int Gcd(int a, int b)
        {
            if (a <= 0 || b <= 0)
                throw new ArgumentException("invalid Gcd input!");

            int r, t;
            if (a < b)
            {
                t = b;
                b = a;
                a = t;
            }
            while (b != 0)
            {
                r = a % b;
                a = b;
                b = r;

            }
            return a;

        }

        public Number Add(Number b)
        {
            if (this.Denominator == b.Denominator)
            {
                Number c = new Number(this.Numerator + b.Numerator, this.Denominator);
                return c;
            }
            else
            {
                int c_n = this.Numerator * b.Denominator + b.Numerator * this.Denominator;
                int c_d = this.Denominator * b.Denominator;
                // 构造函数里添加了自动约分功能, 不需要再在之前求gcd约分了
                // int gcd = Gcd(Math.Abs(c_n), Math.Abs(c_d));
                Number c = new Number(c_n, c_d);
                return c;
            }
        }

        public Number Sub(Number b)
        {
            if (this.Denominator == b.Denominator)
            {
                Number c = new Number(this.Numerator - b.Numerator, this.Denominator);
                return c;
            }
            else
            {
                int c_n = this.Numerator * b.Denominator - b.Numerator * this.Denominator;
                int c_d = this.Denominator * b.Denominator;
                Number c = new Number(c_n, c_d);
                return c;
            }
        }

        public Number Mul(Number b)
        {
            Number c = new Number(this.Numerator * b.Numerator, this.Denominator * b.Denominator);
            return c;
        }

        public Number Div(Number b)
        {
            // check b to be non-zero
            if (b.Numerator == 0)
                throw new DivideByZeroException("b is zero! cannot divide!");
            else
            {
                Number c = new Number(this.Numerator * b.Denominator, this.Denominator * b.Numerator);
                return c;
            }
        }

        public static int IntPow(int num, int power)
        {
            if (power <= 0)
            {
                throw new Exception("power should be positive!");
            }
            int product = 1;
            while(power > 0)
            {
                product *= num;
                power -= 1;
            }
            return product;
        }

        public Number Pow(Number b) 
        {
            // b must be an integar between (-3,3), otherwise not permitted.
            if (b.Denominator != 1 || Math.Abs(b.Numerator / b.Denominator) > 3)
            {
                throw new ArgumentException("invalid power!");
            }

            int power = b.Numerator; // positive, negative or zero

            // if the host number is zero, it is special
            if (this.Numerator == 0)
            {
                if (power == 0)
                    return new Number(1, 1);
                else
                    return new Number(0, 1);
            }

            // non zero           
            if (power>0)
            {
                // MysriO: 我害怕c#和c++一样浮点幂次不靠谱(可能有误差结果截断舍入少1),写了一个整数的幂次函数
                return new Number(IntPow(this.Numerator, power), IntPow(this.Denominator, power));
            }
            else if (power<0)
            {
                return new Number(IntPow(this.Denominator, -power),IntPow(this.Numerator, -power));
            }
            else // this != 0 && power == 0, return 1
            {
                return new Number(1, 1);
            }

        }

        public override string ToString()
        {
            if (Denominator != 1)
            {
                return $"({Numerator.ToString()}/{Denominator.ToString()})";
            }
            else
            {
                if (Numerator >= 0)
                    return $"{Numerator.ToString()}";
                else
                    return $"({Numerator.ToString()})";  //给负数加括号
            }
        }


        // MysriO: 下面是VS自动生成的代码
        // 用于重载各种判断相等的函数
        // 否则,单元测试时通过地址引用判断相等,而不是值的相等.
        public override bool Equals(object obj)
        {
            return Equals(obj as Number);
        }

        public bool Equals(Number other)
        {
            return other != null &&
                   Numerator == other.Numerator &&
                   Denominator == other.Denominator;
        }

        public override int GetHashCode()
        {
            var hashCode = -1534900553;
            hashCode = hashCode * -1521134295 + Numerator.GetHashCode();
            hashCode = hashCode * -1521134295 + Denominator.GetHashCode();
            return hashCode;
        }

        public static bool operator == (Number number1, Number number2)
        {
            return EqualityComparer<Number>.Default.Equals(number1, number2);
        }

        public static bool operator !=(Number number1, Number number2)
        {
            return !(number1 == number2);
        }

        // 自己添加的大于小于
        public static bool operator > (Number number1, Number number2)
        {
            Number num = number1.Sub(number2);
            if (num.Numerator * num.Denominator > 0)
                return true;
            else return false;
        }

        public static bool operator < (Number number1, Number number2)
        {
            Number num = number2.Sub(number1);
            if (num.Numerator * num.Denominator > 0)
                return true;
            else return false;
        }

    }
}
