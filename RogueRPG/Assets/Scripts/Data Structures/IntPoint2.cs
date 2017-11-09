using UnityEngine;

public struct IntPoint2
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
            throw new System.IndexOutOfRangeException("Index");
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
                    throw new System.IndexOutOfRangeException("Index");
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
        return x * y + x * x + y * y;
    }
}