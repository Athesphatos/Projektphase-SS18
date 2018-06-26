
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{

    public float delay = 5f;


    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, delay);
    }
}