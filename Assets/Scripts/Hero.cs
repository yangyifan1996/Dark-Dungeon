using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    #region 变量定义
    //当前动画控制器
    private Animator animator;
    //刚体组件
    private Rigidbody2D rig;

    private Transform weapon;

    private float weaponScale;

    private float scale = 1.2f;
    //速度
    public float speed = 5;


    private float sprintTimer = 0;
    //冲刺方向
    private Vector2 sprintDirection;

    private bool isSprint = false;
    private GhostEffect ghost;
    #endregion

    void Start()
    {
        animator = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
        ghost = GetComponent<GhostEffect>();
        weapon = transform.Find("weapon").transform;
        weaponScale = weapon.localScale.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (isSprint)
        {
            sprintTimer += Time.deltaTime;
            if (sprintTimer >= 0.1)
            {
                StopSprint();
                return;
            }
            rig.AddForce(sprintDirection * 250);
            return;
        }
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        weapon.right = (mousePosition - new Vector2(transform.position.x, transform.position.y)).normalized;
        if (weapon.right.x > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            weapon.localScale = new Vector3(weaponScale, weaponScale, 1);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
            weapon.localScale = new Vector3(weaponScale, -weaponScale, 1);
        }
        if (Input.GetMouseButtonDown(1))
        {
            StartSprint();
        }
        if (Input.GetMouseButtonDown(2))
        {
            print("2");

        }
        if (Input.GetMouseButtonDown(0))
        {

            EventCenter.GetInstance().EventTrigger(EventName.ROLE_SHOOT);

        }
    }

    private void FixedUpdate()
    {
        if (isSprint)
        {
            rig.velocity = new Vector2(0, rig.velocity.y);
            rig.velocity = new Vector2(0, rig.velocity.x);
            return;
        }
        float horizontaMove = Input.GetAxisRaw("Horizontal");
        float verticalMove = Input.GetAxisRaw("Vertical");
        if (horizontaMove != 0 && verticalMove != 0)
        {
            horizontaMove *= 0.7f;
            verticalMove *= 0.7f;
        }
        if (horizontaMove != 0 || verticalMove != 0)
        {
            animator.SetBool("isRun", true);
        }
        else
        {
            animator.SetBool("isRun", false);
        }
        rig.velocity = new Vector2(verticalMove * speed, rig.velocity.y);
        rig.velocity = new Vector2(horizontaMove * speed, rig.velocity.x);
    }

    void StartSprint()
    {
        isSprint = true;
        ghost.StartEffect();
        sprintDirection = weapon.right.normalized;
        sprintTimer = 0;
    }

    void StopSprint()
    {
        ghost.StopEffect();
        isSprint = false;
    }
}
