using System;
using UnityEngine;

namespace Board
{
    [RequireComponent(typeof(Transform))]
    public class Tile : MonoBehaviour
    {
        public Cell cell { get; set; }

        public virtual void MoveToCell()
        {
            transform.position = cell.transform.position;
        }

        public virtual void Collision(ICollider c)
        {
            
        }
    }
}
