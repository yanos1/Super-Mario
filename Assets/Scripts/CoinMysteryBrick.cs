using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinMysteryBrick : MysteryBrick, BaseBrick
{
    public Coin consumable;
    public void Hit(GameObject obj)
    {
        Coin coin = Instantiate(consumable, this.gameObject.transform.position, Quaternion.identity);
        Animator animator = coin.GetComponent<Animator>();
        animator.SetTrigger("hit");
        GameManager.game.AddCoins(1);
        GameManager.game.AddScore(200);
        Deactivate();
    }


}
