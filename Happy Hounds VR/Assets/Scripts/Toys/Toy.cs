using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toy : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (transform.position.y < 0)
        {
            ResetYPosition();
        }
		
	}

    public void ResetYPosition()
    {
        Vector3 fix = new Vector3(transform.position.x, 0, transform.position.z);
        transform.position = fix;
        
    }
}
