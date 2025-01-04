
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

public class TestUIScript : MonoBehaviour
{
    public List<TestScript> scripts = new List<TestScript>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            scripts.AddRange(GetComponentsInParent<TestScript>());
        }
     
    }
}
