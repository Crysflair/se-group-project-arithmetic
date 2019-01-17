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
    public class Operation : Expression, IEquatable<Operation>
    {
        public Expression left;
        private char op;
        public Expression right;
        private int depth;

        public void Setdepth(int depth) { this.depth = depth; }
        public int Getdepth() { return depth; }
        public char GetOP() { return op; }

        public void SwapBranch()
        {
            if (op != '+' && op != '*')
            {
                throw new Exception("bug.");
            }
            var tmp = right;
            right = left;
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

        // VS generated Value based Equal functions
        public override bool Equals(object obj)
        {
            return Equals(obj as Operation);
        }

        public bool Equals(Operation other)
        {
            return other != null &&
                   EqualityComparer<Expression>.Default.Equals(left, other.left) &&
                   op == other.op &&
                   EqualityComparer<Expression>.Default.Equals(right, other.right) &&
                   depth == other.depth;
        }

        public override int GetHashCode()
        {
            var hashCode = 2097590993;
            hashCode = hashCode * -1521134295 + EqualityComparer<Expression>.Default.GetHashCode(left);
            hashCode = hashCode * -1521134295 + op.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<Expression>.Default.GetHashCode(right);
            hashCode = hashCode * -1521134295 + depth.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(Operation operation1, Operation operation2)
        {
            return EqualityComparer<Operation>.Default.Equals(operation1, operation2);
        }

        public static bool operator !=(Operation operation1, Operation operation2)
        {
            return !(operation1 == operation2);
        }
    }
}
