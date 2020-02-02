using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixWhealInPlace : MonoBehaviour
{
    public bool itsOk = false;

    [SerializeField] private bool _isInverted;
    // Update is called once per frame


    private void OnTriggerEnter(Collider other)
    {
        if (itsOk)
        {
            if (other.tag == "Wheal")
            {
                if (other.GetComponent<WhealStatus>().theyAreNew)
                {
                    Simulation.SimulationInst.WheelRepair();
                    Destroy(other.gameObject);
                    transform.GetChild(0).gameObject.SetActive(true);
                    itsOk = false;
                    AudioManager.AuidoManagerInstance.PlayOneShotRatchetAudio(1);
                }
            }
        }
    }
}
