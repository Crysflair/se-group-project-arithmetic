using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arithmetic
{
    public class TreeGenerator
    {
        // Description:
        // 1. Right subtree is deeper than left subtree, as long as one of them is Operation.
        // 2. If they are both VariableReference, register the "smaller-bigger" pair to SBpairs.
        // 3. If a '^' operation is created, force it to have an int power.

        //   variable: for the class
        static Random rnd = new Random();
        static char[] NodeTypes = { '+', '-', '*', '/', '^', '~' };
        static string[] VariableNames = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p" };

        // private attribute: for an instance
        private int VariableCur = 0;
        private List<Tuple<string, string>> SBpairs;
        private List<string> IntNodes;
        
        public Expression Generate(ref int MaxNode)
        {
            // restore variables
            VariableCur = 0;
            SBpairs.Clear();

            // start running
            Expression root = null;
            GenerateNode(ref root, ref MaxNode);
            return root; 
        }

        private static int GetNodeDepth(ref Expression exp)
        {
            if (exp is VariableReference)
                return 0;
            else if (exp is Operation)
                return ((Operation)exp).Getdepth();
            else throw new Exception("Bug.");
        }

        private string GetNextVariableName()
        {
            try
            {
                return VariableNames[VariableCur++];
            }
            catch (Exception e)
            {
                Console.WriteLine("Out of range of 'VariableNames' table! (Other bugs possible)");
                Console.WriteLine(e.ToString());
                throw e;
            }
        }

        private void GenerateIntVariable(ref Expression right)
        {
            right = new VariableReference(GetNextVariableName());
            IntNodes.Add(((VariableReference)right).GetName());
        }
        private void GenerateVariable(ref Expression leaf)
        {
            leaf = new VariableReference(GetNextVariableName());
        }

        private void GenerateNode(ref Expression parent, ref int NodeRemain)
        {
            char nodetype = NodeTypes[rnd.Next(NodeTypes.Length)];

            if (nodetype == '~' || NodeRemain == 0)
            {
                GenerateVariable(ref parent);
                return;
            }

            // else, nodetype is an two eye operator
            // power is special for its right node must be an integar.
            if (nodetype == '^')
            {
                NodeRemain -= 1;
                Expression left = null; GenerateNode(ref left, ref NodeRemain);
                Expression right = null; GenerateIntVariable(ref right);
                int depth = Math.Max(GetNodeDepth(ref left), GetNodeDepth(ref right)) + 1;
                parent = new Operation(left, '^', right, depth);
            }
            else // +, -, *, /
            {
                NodeRemain -= 1;
                Expression left = null; GenerateNode(ref left, ref NodeRemain);
                Expression right = null; GenerateNode(ref right, ref NodeRemain);
                int depth = Math.Max(GetNodeDepth(ref left), GetNodeDepth(ref right)) + 1;

                if (nodetype == '+' || nodetype == '*')
                {

                    if (GetNodeDepth(ref left) < GetNodeDepth(ref right)) //desired result
                    {
                        parent = new Operation(left, nodetype, right, depth);
                    }
                    else if (GetNodeDepth(ref left) > GetNodeDepth(ref right)) //reversed result
                    {
                        parent = new Operation(right, nodetype, left, depth);
                    }
                    else // equal
                    {
                        if (left is VariableReference && right is VariableReference)
                        {
                            SBpairs.Add(new Tuple<string, string>(
                                ((VariableReference)left).GetName(),
                                ((VariableReference)right).GetName()
                                ));
                        }
                        else
                        {
                            if (nodetype == '+')
                                parent = new Operation(left, '-', right, depth);
                            else //nodetype == '*'
                                parent = new Operation(left, '/', right, depth);
                        }
                    }
                }
                else  // -, /
                {
                    parent = new Operation(left, nodetype, right, depth);
                }

            }
        }
    }
}
