using System;
using System.Collections.Generic;
using System.Text;

namespace HuffmanCompression
{
    class HeapQueue<T> where T : IPriority, IComparable
    {
        public Node Root;
        public class Node
        {
            public Node Right;
            public Node Left;
            public int nodesRight;
            public int nodesLeft;
            public int HeightLeft = 0;
            public int HeightRight = 0;
            public T data;

        }

        public void Add(T dato)
        {
            Node nuevoN = new Node();

            nuevoN.data = dato;
            if (Root == null)
            {
                Root = nuevoN;
            }
            else
            {
                AddRecursivo(Root, nuevoN);
            }

        }
        public T Peek()
        {
            if (IsEmpty())
            {
                return default;
            }
            else
            {
                return Root.data;
            }
        }
        private bool IsComplete(int Height, int nodesCount)
        {
            double Maxnodes = Math.Pow(2, Convert.ToDouble(Height)) - 1;
            if (Maxnodes == nodesCount)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void AddRecursivo(Node actual, Node nuevo)
        {

            if (actual.Left == null)
            {
                if (nuevo.data.Priority.CompareTo(actual.data.Priority) > 0 || (nuevo.data.Priority.CompareTo(actual.data.Priority) == 0 && nuevo.data.CompareTo(actual.data) >= 0))
                {
                    actual.Left = nuevo;
                }
                else
                {
                    Node aux = new Node();
                    aux.data = actual.data;
                    actual.data = nuevo.data;
                    actual.Left = aux;
                }
                actual.nodesLeft++;
            }
            else if (actual.Right == null)
            {
                if (nuevo.data.Priority.CompareTo(actual.data.Priority) > 0 || (nuevo.data.Priority.CompareTo(actual.data.Priority) == 0 && nuevo.data.CompareTo(actual.data) >= 0))
                {
                    actual.Right = nuevo;
                }
                else
                {
                    Node aux = new Node();
                    aux.data = actual.data;
                    actual.data = nuevo.data;
                    actual.Right = aux;
                }

                actual.nodesRight++;
            }
            else
            {
                if (actual.nodesLeft != actual.nodesRight && IsComplete(actual.HeightLeft, actual.nodesLeft))
                {
                    AddRecursivo(actual.Right, nuevo);
                    if (actual.Right.data.Priority.CompareTo(actual.data.Priority) < 0 || (actual.Right.data.Priority.CompareTo(actual.data.Priority) == 0 && actual.Right.data.CompareTo(actual.data) <= 0))
                    {

                        T AuxData = actual.data;

                        actual.data = actual.Right.data;

                        actual.Right.data = AuxData;
                    }
                    actual.nodesRight++;
                }
                else
                {
                    AddRecursivo(actual.Left, nuevo);
                    if (actual.Left.data.Priority.CompareTo(actual.data.Priority) < 0 || (actual.Left.data.Priority.CompareTo(actual.data.Priority) == 0 && actual.Left.data.CompareTo(actual.data) <= 0))
                    {

                        T AuxData = actual.data;
                        actual.data = actual.Left.data;
                        actual.Left.data = AuxData;
                    }
                    actual.nodesLeft++;
                }

            }
            Heights(ref actual);

        }

        private void Heights(ref Node actual)
        {
            //HeightRight
            if (actual.Right != null)
            {
                if (actual.Right.HeightLeft > actual.Right.HeightRight)
                {
                    actual.HeightRight = actual.Right.HeightLeft + 1;
                }
                else
                {
                    actual.HeightRight = actual.Right.HeightRight + 1;
                }
            }
            else
            {
                actual.HeightRight = 0;
            }


            //HeightLeft
            if (actual.Left != null)
            {
                if (actual.Left.HeightLeft > actual.Left.HeightRight)
                {
                    actual.HeightLeft = actual.Left.HeightLeft + 1;
                }
                else
                {
                    actual.HeightLeft = actual.Left.HeightRight + 1;
                }
            }
            else
            {
                actual.HeightLeft = 0;
            }


        }

        public T Remove()
        {

            if (IsEmpty())
            {
                return default;
            }
            else
            {
                T ToReturn = Root.data;
                FindLastAdded(ref Root);
                OrderCheck(Root);
                return ToReturn;
            }

        }
        private void FindLastAdded(ref Node actual)
        {
            if ((actual.nodesLeft != actual.nodesRight && !IsComplete(actual.HeightLeft, actual.nodesLeft)) || (actual.nodesLeft != actual.nodesRight && IsComplete(actual.HeightRight, actual.nodesRight)))
            {
                FindLastAdded(ref actual.Left);
                actual.nodesLeft--;
                Heights(ref actual);
            }
            else
            {
                if (actual.Right != null)
                {
                    FindLastAdded(ref actual.Right);
                    actual.nodesRight--;
                    Heights(ref actual);
                }
                else
                {
                    if (actual.Left != null)
                    {

                        Root.data = actual.Left.data;
                        actual.Left = null;
                        actual.nodesLeft--;
                        Heights(ref actual);
                    }
                    else
                    {

                        Root.data = actual.data;
                        actual = null;
                    }


                }
            }

        }

        public bool IsEmpty()
        {
            return Root == null;
        }
        private void OrderCheck(Node actual)
        {
            if (actual != null)
            {
                if (actual.Right != null && actual.Left != null)
                {
                    if (actual.Left.data.Priority.CompareTo(actual.Right.data.Priority) > 0)
                    {
                        if (actual.data.Priority.CompareTo(actual.Right.data.Priority) > 0)
                        {

                            T AuxData;

                            AuxData = actual.data;
                            actual.data = actual.Right.data;
                            actual.Right.data = AuxData;
                            OrderCheck(actual.Right);
                        }

                    }
                    else
                    {
                        if (actual.data.Priority.CompareTo(actual.Left.data.Priority) > 0)
                        {

                            T AuxData;
                            AuxData = actual.data;
                            actual.data = actual.Left.data;
                            actual.Left.data = AuxData;
                            OrderCheck(actual.Left);
                        }

                    }
                }
                else if (actual.Right != null)
                {
                    if (actual.data.Priority.CompareTo(actual.Right.data.Priority) > 0)
                    {

                        T AuxData;
                        AuxData = actual.data;
                        actual.data = actual.Right.data;
                        actual.Right.data = AuxData;

                    }
                }
                else if (actual.Left != null)
                {
                    if (actual.data.Priority.CompareTo(actual.Left.data.Priority) > 0)
                    {

                        T AuxData;
                        AuxData = actual.data;
                        actual.data = actual.Left.data;
                        actual.Left.data = AuxData;

                    }
                }
            }


        }
    }
}
