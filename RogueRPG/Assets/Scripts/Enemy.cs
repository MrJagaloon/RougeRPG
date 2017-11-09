//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Enemy : MovingObject
//{
//    public int playerDamage = 10;

//    Animator animator;
//    Transform target;
//    bool skipMove;

//    bool lastMovedHorizontally;


//	protected override void Start ()
//    {
//        GameManager.instance.AddEnemyToList(this);
//        animator = GetComponent<Animator>();
//        target = GameObject.FindGameObjectWithTag("Player").transform;
//        base.Start();
//	}

//    protected override void AttemptMove<T>(int xDir, int yDir)
//    {
//        base.AttemptMove<T>(xDir, yDir);
             
//        skipMove = true;
//    }

//    public void MoveEnemy()
//    {
//        if (skipMove)
//        {
//            skipMove = false;
//            return;
//        }

//        int xDir = 0;
//        int yDir = 0;

//        bool diffX = Mathf.Abs(target.position.x - transform.position.x) > float.Epsilon;
//        bool diffY = Mathf.Abs(target.position.y - transform.position.y) > float.Epsilon;

//        if (diffX && diffY)
//        {
//            if (lastMovedHorizontally)
//                yDir = (target.position.y > transform.position.y) ? 1 : -1;
//            else
//                xDir = (target.position.x > transform.position.x) ? 1 : -1;

//            lastMovedHorizontally = !lastMovedHorizontally;
//        }
//        else if (diffX)
//        {
//            xDir = (target.position.x > transform.position.x) ? 1 : -1;
//            lastMovedHorizontally = true;
//        }
//        else if (diffY)
//        {
//            yDir = (target.position.y > transform.position.y) ? 1 : -1;
//            lastMovedHorizontally = false;
//        }

//        AttemptMove<Player>(xDir, yDir);
//    }

//    protected override void OnCantMove<T>(T component)
//    {
//        Player hitPlayer = component as Player;

//        animator.SetTrigger("enemyAttack");

//        hitPlayer.LoseHealth(playerDamage);
//    }
//}
