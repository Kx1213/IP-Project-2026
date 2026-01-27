using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class ChopFood : MonoBehaviour
{
    [SerializeField]
    GameObject choppedFood;

    [SerializeField]
    int choppedPieces;

    [SerializeField]
    GameObject foodPieces;

    private GameObject choppedFoodSpawned;

    void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Knife"))
        {   
            for(int i = 0; i < choppedPieces; i++) //spawn indicated number of food pieces when chopped
            {
                Instantiate(foodPieces, transform.position + Vector3.forward * 0.5f, transform.rotation); //spawn food pieces at the position the food was in after cutting the food
            }

            Destroy(gameObject); //destroy the whole food item after chopping
            Debug.Log("Food Chopped");
        }  
    }
}








/*

    [SerializeField]
    bool alreadyChopped;




void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Knife"))
        {   
            Instantiate(foodPieces, transform.position + Vector3.forward * 0.5f, transform.rotation); //spawn food pieces at the position the food was in after cutting the food

            if(!alreadyChopped) //if the food hasnt been cut yet, spawn a chopped version of the food
            {
                choppedFoodSpawned = Instantiate(choppedFood, transform.position, transform.rotation);
                choppedFoodSpawned.GetComponent<ChopFood>().alreadyChopped = true; //set the chopped food's alreadyChopped to true so it doesnt keep spawning chopped versions of itself
                choppedFoodSpawned.GetComponent<ChopFood>().foodPieces = foodPieces; //assign the food pieces prefab to the chopped food's food pieces variable
            }

            Destroy(gameObject); //destroy the whole food item after chopping
            Debug.Log("Food Chopped");
        }  
    }
*/