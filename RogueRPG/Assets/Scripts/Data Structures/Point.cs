
public struct Point<T>
{
    T[] values;

    public T this[int i]
    {
        get { return values[i]; }
        set { values[i] = value; }
    }

    public T x { get { return values[0]; } }
    public T y { get { return values[1]; } }
    public T z { get { return values[2]; } }
    public T w { get { return values[3]; } }

    public int Dimensions { get { return values.Length; } }

    public Point(params T[] values)
    {
        this.values = new T[values.Length];
        for (int i = 0; i < Dimensions; ++i)
        {
            this.values[i] = values[i];
        }
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;
        
        Point<T> other = (Point<T>)obj;

        if (Dimensions != other.Dimensions) return false;

        for (int i = 0; i < Dimensions; ++i)
        {
            if (!this[i].Equals(other[i]))
                return false;
        }

        return true;
    }

    public override int GetHashCode()
    {
        int hashCode = 1;
        for (int i = 0; i < Dimensions; ++i)
        {
            if (i % 2 == 0)
                hashCode += this[i].GetHashCode();
            else
                hashCode *= this[i].GetHashCode();
        }
        return hashCode;
    }
}