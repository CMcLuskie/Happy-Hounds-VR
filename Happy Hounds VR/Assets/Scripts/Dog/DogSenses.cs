using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogSenses : MonoBehaviour {

    public DogBrain dogBrainScript;

    private void OnTriggerEnter(Collider other)
    {
        if ((other.tag == "Toy") && (dogBrainScript.previousBehaviour != DogBrain.DogBehaviours.FollowToy))
            dogBrainScript.ChangeBehaviour(DogBrain.DogBehaviours.FollowToy);
    }

    private void OnTriggerStay(Collider other)
    {
        if ((other.tag == "Toy") && (dogBrainScript.previousBehaviour != DogBrain.DogBehaviours.FollowToy))
            dogBrainScript.ChangeBehaviour(DogBrain.DogBehaviours.FollowToy);

    }
    private void OnTriggerExit(Collider other)
    {
        if ((other.tag == "Toy") && (dogBrainScript.previousBehaviour != DogBrain.DogBehaviours.FollowToy))
            dogBrainScript.ChangeBehaviour(DogBrain.DogBehaviours.Sitting);
        
    }
}
