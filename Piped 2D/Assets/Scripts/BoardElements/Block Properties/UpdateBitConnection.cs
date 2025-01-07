using UnityEngine;

public class UpdateBitConnection : MonoBehaviour
{


    // Find and connect with any neighbouring Gears
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
