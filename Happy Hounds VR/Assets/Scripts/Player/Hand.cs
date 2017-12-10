using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace VRTK.Examples
{
    public class Hand : VRTK_ControllerEvents_ListenerExample
    {
        [SerializeField]
        protected PlayerStats playerStats;
        [SerializeField]
        protected AudioManager audio;

   // Use this for initialization
        void Start()
        {
            SetUpControllerEvents();
        }

        // Update is called once per frame
        void Update()
        {

        }

        void SetUpControllerEvents()
        {
            GetComponent<VRTK_ControllerEvents>().StartMenuPressed += Hand_StartMenuPressed;
        }

        private void Hand_StartMenuPressed(object sender, ControllerInteractionEventArgs e)
        {
            audio.PlayClip(AudioManager.AudioSources.Player, AudioManager.ClipNames.Whistle);
            playerStats.calledDog = true;
        }
    }
}
