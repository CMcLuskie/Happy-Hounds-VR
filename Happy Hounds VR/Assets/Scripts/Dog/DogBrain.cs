using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogBrain : Dog {

    public float wanderTimer;
    public Animator animator;
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
            

        wanderTimer += Time.deltaTime;
        if ((wanderTimer >= 5 || wanderTimer <= 6f) && isLerping == false)
        {
            Wandering();
        }
	}

    void Wandering()
    {
        wanderTimer = 0;
        Lerping(gridScript.GetRandomNode());      
        animator.SetFloat("Move", 0.6f);
    }

    void DogCall(Vector3 playerPos)
    {
        //converts playerpos to node# 
        Node node = new Node();
        node.coord = playerPos;
        Lerping(node);
    }
}
