using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Again, its a cloud. Go look at better code please
public class Cloud : MonoBehaviour
{
    public float speed;
    public float time;

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.right * speed * Time.deltaTime;
        time -= Time.deltaTime;
        if (time <= 0)
        {
            Destroy(gameObject);
        }
    }
}
