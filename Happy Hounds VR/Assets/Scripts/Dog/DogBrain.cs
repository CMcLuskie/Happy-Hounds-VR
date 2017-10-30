using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogBrain : Dog {

    public float wanderTimer;
    
    float move;
    
    bool wandering;

    private void OnEnable()
    {
        Controls.DogCall += DogCall;
    }

    private void OnDisable()
    {
        Controls.DogCall -= DogCall;
    }

    // Use this for initialization
    void Start ()
    {
        GetGridObject();
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
            Wandering();

        if (!isLerping)
        {
            Wandering();
        }
	}

    void Wandering()
    {
        // Lerping(gridScript.GetRandomNode());
        TempWander(gridScript.GetRandomNode());
        animator.SetFloat("Move", .6f);
    }

    void DogCall(Vector3 playerPos)
    {
        Node node = gridScript.coordToNode(playerPos);
        // Lerping(node);
        TempWander(gridScript.coordToNode(playerPos));
        animator.SetFloat("Move", 6f);

    }
}
