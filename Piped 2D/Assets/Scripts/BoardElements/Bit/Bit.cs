using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine;

public class Bit : MonoBehaviour
{

    [SerializeField]
    GameObject bitRamp;

    UpdateBitConnection connection;

    public bool isOn;

    private void Start()
    {
        connection = gameObject.GetComponent<UpdateBitConnection>();
        isOn = false;
        connection.updateBitConnections();
    }


    public void resetBit()
    {
        if (bitRamp != null)
        {
            bitRamp.GetComponent<Ramp>().resetRamp();
            isOn = false;
        }
    }

    public void setBit()
    {
        if (bitRamp != null)
        {
            bitRamp.GetComponent<Ramp>().setRamp();
            isOn = true;
        }
    }



}
