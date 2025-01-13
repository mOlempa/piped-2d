using UnityEngine;

/**
 * A class handling the updating of the connection nets of Bits stored in Game Manager
 */
public class UpdateBitConnection : MonoBehaviour
{

    /**
     * A public void method handling the updating of the connection nets of Bits stored in Game Manager.
     * @param optional int ignoredBitCell which is the number of the cell where a bit is deleted before updating the connection net.
     */
    public void updateBitConnections(int ignoredBitCell = 0)
    {
        // On any Bit connection updates caused by a Bit, rewrite all the connection chains
        GameManager.Instance.bitConnectionChains.Clear();
        foreach (GameObject gear in GameManager.Instance.allGears)
        {
            gear.GetComponent<GearBlock>().detectAndConnectNeighbourBits(true, ignoredBitCell);
        }
    }
}
