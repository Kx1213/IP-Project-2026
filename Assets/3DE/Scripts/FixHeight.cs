using UnityEngine;

public class HeightCalibrator : MonoBehaviour
{
    public Transform xrCamera; //Get the transfor of the main camera
    public float targetHeight = 2f; //Set player height

    void Start() //fix player's height at the start of the game
    {
        float currentHeight = xrCamera.localPosition.y; //get the camera height

        float difference = targetHeight - currentHeight; //find how different the heights are to adjust

        transform.position += new Vector3(0, difference, 0); //fix the player's height
    }
}
