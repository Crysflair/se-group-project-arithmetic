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
        public string GetName() { return name; }

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
        private int depth;

        public void Setdepth(int depth) { this.depth = depth; }
        public int Getdepth() { return this.depth; }

        public void SwapBranch()
        {
            if (op != '+' && op != '*')
            {
                throw new Exception("bug.");
            }
            var tmp = right;
            right = this.left;
            left = tmp;
        }

        public Operation(Expression left, char op, Expression right, int depth)
        {
            this.left = left ?? throw new ArgumentNullException(nameof(left), "is null!");
            this.op = op;
            this.depth = depth;
            this.right = right ?? throw new ArgumentNullException(nameof(right), "is null!");
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
