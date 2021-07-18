using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class reloadAmmo : Pickup
{
    private void Start() {
        Debug.Log("start");

    }
    
    public override void DoOnPickup(Collider collision)
    {
        Debug.Log("collided");
        if (collision.tag == "Player")
        {
            List<Gun> gunsList=collision.gameObject.GetComponentInChildren<Shooter>().guns;
            
            //.roundsLoaded+=20;
            AmmoTracker.Reload(gunsList[1]);
            //gunsList[1].magazineSize+=10;
            //GameManager.UpdateUIElements();
            //GameObject.Find("AmmoText").GetComponent<Text>().text=gunsList[1].roundsLoaded.ToString();
            
        }
        base.DoOnPickup(collision);
    }

}
