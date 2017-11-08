using System;
namespace Board
{
    public interface ICollider
    {
        void OnCollision(Tile t);
    }
}
