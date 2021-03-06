﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arithmetic
{
    public class TreeGenerator
    {
        // Description:
        // For '+' and '*':
        //      1. Right subtree is **deeper** than left subtree, as long as one of them is Operation.
        //      2. If they are both VariableReference, register the "smaller-bigger" pair to SBpairs.
        // For '^'
        //      1. If a '^' operation is created, force it to have an int power.

        
        private static Random rnd = new Random();
        private static readonly char[] NodeTypes_supported = { '+', '-', '*', '/', '^', '~' }; 
        private static readonly string[] VariableNames = { "a", "b", "c", "d", "e",
                                          "f", "g", "h", "i", "j",
                                          "k", "l", "m", "n", "o",
                                          "p", "q", "r", "s", "t",
                                          "u", "v", "w", "x", "y", "z" };

        // Constructor
        private int VariableCur;
        private List<Tuple<string, string>> SBpairs;
        private List<string> IntNodes;
        private bool is_Generated;
        private char[] NodeTypes;
        public TreeGenerator(char[] nodetypes = null)
        {
            is_Generated = false;
            VariableCur = 0;
            SBpairs = new List<Tuple<string, string>>();
            IntNodes = new List<string>();
            if (nodetypes == null)
            {
                NodeTypes = NodeTypes_supported;
            }
            else // check and set symbol set.
            {
                foreach(char sym in nodetypes)
                {
                    if (Array.IndexOf(NodeTypes_supported, sym) == -1)
                        throw new ArgumentException("Input symbol set not supported!");
                }
                NodeTypes = nodetypes;
            }
        }

        // make sure: Only generate a tree once 
        public Expression Generate(int MaxNodeCeiling)
        {
            if (is_Generated)
                throw new Exception("this tree is already generated!");

            // int MaxNode = 1 + rnd.Next(MaxNodeCeiling);
            int MaxNode = MaxNodeCeiling;
            Expression root = null;
            GenerateNode(ref root, ref MaxNode);
            is_Generated = true;
            return root; 
        }

        // facility
        private static int GetNodeDepth(ref Expression exp)
        {
            if (exp is VariableReference)
                return 0;
            else if (exp is Operation)
                return ((Operation)exp).Getdepth();
            else if (exp == null)
                throw new NullReferenceException("in GetNodeDepth: exp is Null");
            else
                throw new Exception("in GetNodeDepth: unknown Bug.");
        }

        // facility
        public string GetNextVariableName()
        {
            try
            {
                return VariableNames[VariableCur++];
            }
            catch (Exception e)
            {
                Console.WriteLine("Out of range of 'VariableNames' table! (Other bugs possible)");
                Console.WriteLine(e.ToString());
                throw;
            }
        }

        // facility: assign a VariableReference to leaf Node. Register int if need.
        private void GenerateIntVariable(ref Expression leaf)
        {
            leaf = new VariableReference(GetNextVariableName());
            IntNodes.Add(((VariableReference)leaf).GetName());
        }
        private void GenerateVariable(ref Expression leaf)
        {
            leaf = new VariableReference(GetNextVariableName());
        }

        // CORE CODE
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
                            parent = new Operation(left, nodetype, right, depth);
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

        public List<Tuple<string, string>> get_SBpairs()
        {
            return SBpairs;
        }
        public List<string> get_IntNodes()
        {
            return IntNodes;
        }
    }
}
