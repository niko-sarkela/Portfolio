using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tachometer : MonoBehaviour
{
    public float rpm;
    public Transform needle;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        needle.rotation = Quaternion.Euler(0, 0, -(rpm / 100 * 2.5f - 32));
    }
}
