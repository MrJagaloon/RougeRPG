using System.Collections.Generic;
using System.Collections;
using System;

namespace TileMapLib.BaseMaps
{
    public class BaseMapZone<T> : IEnumerable
    {
        List<BaseMapZoneNode<T>> nodes;

        public int Count { get { return nodes.Count; } }
        public BaseMapZoneNode<T> this[int i]
        {
            get { return nodes[i]; }
        }

        public BaseMapZone()
        {
            nodes = new List<BaseMapZoneNode<T>>();
        }

        /* Add a node with the given position and state to nodes, maintaining nodes order. 
         * 
         * Returns: The index of the inserted node
         * Throws: Exception if the node already exists.
         */
        public int AddNode(BaseMapZoneNode<T> node)
        {
            int index = -(nodes.BinarySearch(node)) - 1;

            if (index < 0)
                throw new Exception("Node already exists.");
            
            nodes.Insert(index, node);

            return index;
        }
        public int AddNode(IntPoint2 position, T state) 
        { 
            return AddNode(new BaseMapZoneNode<T>(position, state)); 
        }
        public int AddNode(int x, int y, T state) 
        {
            return AddNode(new IntPoint2(x, y), state);
        }

        public int GetNodeIndex(BaseMapZoneNode<T> node)
        {
            return nodes.BinarySearch(node);
        }
        public int GetNodeIndex(IntPoint2 position)
        {
            return GetNodeIndex(new BaseMapZoneNode<T>(position, default(T)));
        }
        public int GetNodeIndex(int x, int y)
        {
            return GetNodeIndex(new IntPoint2(x, y));
        }

        public void RemoveNode(int index)
        {
            nodes.RemoveAt(index);
        }

        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable)nodes).GetEnumerator();
        }
    }

    public class BaseMapZoneNode<T> : IComparable, IComparable<BaseMapZoneNode<T>>
    {
        public IntPoint2 position;
        public T state;

        public BaseMapZoneNode(IntPoint2 position, T state)
        {
            this.position = position;
            this.state = state;
        }
        public BaseMapZoneNode(int x, int y, T state) : this(new IntPoint2(x, y), state) {}

        public bool Equals(BaseMapZoneNode<T> other)
        {
            return position.Equals(other);
        }

        public override bool Equals(object obj)
        {
            if (obj != null && !(obj is BaseMapZoneNode<T>))
                return false;
        
            return Equals(obj as BaseMapZoneNode<T>);
        }

        public int CompareTo(BaseMapZoneNode<T> other)
        {
            throw new NotImplementedException();
        }

        public int CompareTo(object obj)
        {
            if (obj != null && !(obj is BaseMapZoneNode<T>))
                throw new ArgumentException(
                    string.Format("Object must be of type BaseMapZoneNode<{0}>.", typeof(T)));
            
            return ((IComparable)position).CompareTo(obj);
        }

        public override int GetHashCode()
        {
            return position.GetHashCode();
        }
    }
}
