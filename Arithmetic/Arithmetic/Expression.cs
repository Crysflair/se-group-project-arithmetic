using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arithmetic
{
    // abstruct class. 
    // Subclassed: VariableReference & Operation
    public abstract class Expression
    {
        public abstract Number Evaluate(Dictionary<string, object> vars);
    }

    public class VariableReference : Expression
    {
        private string name;
        public VariableReference(string name)
        {
            this.name = name;
        }
        public override Number Evaluate(Dictionary<string, object> vars)
        {
            object value = vars[name];
            if (value == null)
            {
                throw new Exception("Unknown variable name: " + name);
            }
            return (Number)value;
        }
    }
    public class Operation : Expression
    {
        private Expression left;
        private char op;
        private Expression right;

        public Operation(Expression left, char op, Expression right)
        {
            this.left = left;
            this.op = op;
            this.right = right;
        }
        public override Number Evaluate(Dictionary<string, object> vars)
        {
            Number x = left.Evaluate(vars);
            Number y = right.Evaluate(vars);
            switch (op)
            {
                case '+': return x.Add(y);
                case '-': return x.Sub(y);
                case '*': return x.Mul(y);
                case '/': return x.Div(y);  // TODO: divide zero?
                case '^': return x.Pow(y);  
            }
            throw new Exception("Unknown operator");
        }
    }
}
