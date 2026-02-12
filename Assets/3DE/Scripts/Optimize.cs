using UnityEngine;

public class Optimize : MonoBehaviour
{
    [SerializeField]
    private GameObject itemsToOptimize;

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            itemsToOptimize.SetActive(false);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            itemsToOptimize.SetActive(true);
        }
    }
}
