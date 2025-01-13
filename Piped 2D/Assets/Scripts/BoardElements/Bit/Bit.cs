
using UnityEngine;

/**
 * A class controlling the Bit behaviour.
 */
public class Bit : MonoBehaviour
{
    /**
    * A private GameObject field for referencing the ramp of the Bit.
    * Serialized in order to access it from the Unity editor.
    */
    [SerializeField]
    GameObject bitRamp;

    /**
    * A private UpdateBitConnection object field for referencing the script that updated the connection for all the Bits.
    */
    UpdateBitConnection connection;

    /**
     * A public bool field that stores the state of the Bit.
     */
    public bool isOn;

    /**
    * A private void method called when the script instance is being loaded. It sets necessary variables.
    */
    private void Start()
    {
        connection = gameObject.GetComponent<UpdateBitConnection>();
        isOn = false;
        connection.updateBitConnections();
    }

    /**
    * A public void method that resets the ramp of the Bit.
    */
    public void resetBit()
    {
        if (bitRamp != null)
        {
            bitRamp.GetComponent<Ramp>().resetRamp();
            isOn = false;
        }
    }

    /**
    * A public void method that sets the ramp of the Bit.
    */
    public void setBit()
    {
        if (bitRamp != null)
        {
            bitRamp.GetComponent<Ramp>().setRamp();
            isOn = true;
        }
    }



}
