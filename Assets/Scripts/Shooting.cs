using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public GameObject shootingItem;
    public Transform shootingPoint;
    public bool canShoot;

    public void Shoot(){
        if(!canShoot)
            return;

        GameObject si = Instantiate(shootingItem, shootingPoint);
        si.transform.parent = null;
    }


    public void CanShoot(){
        canShoot = true;
    }

    public void CanNoShoot(){
        canShoot = false;
    }
}
