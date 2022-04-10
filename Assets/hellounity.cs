using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hellounity : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        //Debug.Log("awake");
    }
    void Start()
    {
        //Debug.Log("start");
       
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(transform.position.y+" "+Time.time);
        //transform.position =
        //    new Vector3(transform.position.x, transform.position.y+1.0f*Time.deltaTime, transform.position.z);
    }
    
    private void FixedUpdate()
    {
        //Debug.Log("fixedupdate" + Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("trigger");
    }
}
