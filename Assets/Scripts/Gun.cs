using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    private Animator animator;
    // Start is called before the first frame update
    private Transform Shoot_P;
    public Bullets bullets;
    void Start()
    {
        Shoot_P = transform.Find("Shoot_P");
        animator = GetComponent<Animator>();
        EventCenter.GetInstance().AddEventListener(EventName.ROLE_SHOOT, Shoot);
    }

    void Shoot()
    {
        animator.SetTrigger("Shoot");
        SetBullets();
    }

    void SetBullets()
    {
        Bullets bullet =  Instantiate<Bullets>(bullets, Shoot_P.position, Quaternion.identity);
        bullet.SetSpeed(transform.right);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
