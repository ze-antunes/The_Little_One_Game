using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedPlant : MonoBehaviour
{
    public GameObject bullet;
    public Transform bulletPos;
    public Animator animator;
    public float shootingTimer = 3;
    float timer;

    void Update(){
        timer += Time.deltaTime;

        if (timer > shootingTimer)
        {
            timer = 0;
            StartCoroutine(Attack(.5f));
        }
    }

    public IEnumerator Attack(float duration){
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(duration);
        Instantiate(bullet, bulletPos.position, Quaternion.identity);
    }
}
