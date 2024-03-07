using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PowerupMysteryBrick : MysteryBrick, BaseBrick
{
    public MushroomPowerup mushroom;
    public FlowerPowerup flower;

    public void Hit(GameObject obj)
    {
        SoundManager.sounds.PlayPowerupAppear();

        if (GameManager.game.GetLevel() == 0) {
             Instantiate(mushroom, this.gameObject.transform.position, Quaternion.identity);
            }
        else if (GameManager.game.GetLevel()== 1)
        {
           Instantiate(flower, this.gameObject.transform.position, Quaternion.identity);
        }
        Deactivate();
    }
}
