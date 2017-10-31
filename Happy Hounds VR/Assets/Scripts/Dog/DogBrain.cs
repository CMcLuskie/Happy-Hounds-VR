using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogBrain : Dog {

    public float wanderTimer;
    
    float move;
    
    bool wandering;

    private void OnEnable()
    {
        LeftControl.DogCall += DogCall;
        Controls.HeadScratch += HeadScratch;
        Controls.BodyScratch += BodyScratch;
    }

    

    private void OnDisable()
    {
        LeftControl.DogCall -= DogCall;
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
            animator.SetFloat("Move", 0.5f);
        }
	}

    void Wandering()
    {
        // Lerping(gridScript.GetRandomNode());
       // TempWander(gridScript.GetRandomNode());
        animator.SetFloat("Move", .6f);
    }

    void DogCall(Vector3 playerPos)
    {
        isWandering = false;
        attentionGiven = true;
        Node node = gridScript.coordToNode(playerPos);
        // Lerping(node);
        TempWander(playerPos);
        animator.SetFloat("Move", 6f);

    }

    private void HeadScratch()
    {
      
    }
    private void BodyScratch()
    {
        throw new NotImplementedException();
    }
}
