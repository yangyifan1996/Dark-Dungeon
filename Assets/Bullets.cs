using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour
{
    public float speed;
    public float time;
    private Rigidbody2D rig;
    // Start is called before the first frame update
    void Awake()
    {
        rig = GetComponent<Rigidbody2D>();

    }

    public void SetSpeed(Vector3 direction)
    {
        float angle = Random.Range(-5f, 5f);
        Vector3 m_direction = Quaternion.AngleAxis(angle, Vector3.forward) * direction;
        transform.right = m_direction;
        rig.velocity = m_direction * speed;
        Destroy(gameObject, time);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Destroy(gameObject);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
