using System;

public class PathStep<T>
{
    public T data { get; private set; }
    public float gCost;
    public float hCost;
    public float fCost { get { return gCost + hCost; } }
    public PathStep<T> parent;

    public PathStep(T data, float gScore = 0, float hScore = 0, PathStep<T> parent = null)
    {
        this.data = data;
        this.gCost = gScore;
        this.hCost = hScore;
        this.parent = parent;
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType()) 
            return false;

        PathStep<T> other = (PathStep<T>)obj;
        
        return (data.Equals(other.data));
    }

    public override int GetHashCode()
    {
        return data.GetHashCode();
    }
}
