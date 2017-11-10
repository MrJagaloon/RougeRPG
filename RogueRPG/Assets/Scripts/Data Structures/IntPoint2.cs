using UnityEngine;
using System;

public struct IntPoint2 : IComparable, IComparable<IntPoint2>
{
    public int x;
    public int y;

    public int this[int i]
    {
        get 
        {
            if (i == 0)
                return x;
            if (i == 1)
                return y;
            throw new IndexOutOfRangeException("Index");
        }
        set 
        {
            switch (i)
            {
                case 0:
                    x = value;
                    break;
                case 1:
                    y = value;
                    break;
                default:
                    throw new IndexOutOfRangeException("Index");
            }
        }
    }

    public IntPoint2(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public Vector2 ToVector2()
    {
        return new Vector2(x, y);
    }

    public override string ToString()
    {
        return string.Format("(x={0}, y={1}]", x, y);
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;
        
        IntPoint2 other = (IntPoint2)obj;

        return x == other.x && y == other.y;
    }

    public override int GetHashCode()
    {
        int hash = 17;
        // Suitable nullity checks etc, of course :)
        hash = hash * 23 + x.GetHashCode();
        hash = hash * 23 + y.GetHashCode();
        return hash;
    }

    public int CompareTo(IntPoint2 other)
    {
        if (x != other.x)
            return x - other.x;
        if (y != other.y)
            return y - other.y;
        return 0;
    }

    public int CompareTo(object obj)
    {
        if (obj != null && !(obj is IntPoint2))
            throw new ArgumentException("Object must be of type IntPoint2.");

        return CompareTo((IntPoint2)obj);
    }
}