using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class FoodBowl : MonoBehaviour
{

    private bool isMixed = false; //lets food be mixed only when spoon is touched the bowl
    private MeshRenderer foodMeshRenderer;

    [SerializeField]
    GameObject mixedFood;

    private Material bowlFoodMaterial;

    [SerializeField]
    Material correctFinalMaterial;

    [SerializeField]
    Material wrongFoodMaterial;
    
    [SerializeField]
    GameObject foodInBowl; //allows a prefab to be spawned - change material to the one the player added

    private int correctFood = 0; //correct food added

    [SerializeField]
    int foodNeeded = 2; //correct food needed to pass

    private bool clearBowl = true; //check if bowl is empty

    [SerializeField]
    GameObject utensilMixed; //hold the ustensil used to mix food
    

    private GameObject newFoodInBowl; //holds the instanciated food in bowl object

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("rightFood"))
        {
            foodMeshRenderer = other.gameObject.GetComponent<MeshRenderer>();
            newFoodInBowl = Instantiate(foodInBowl, transform.position + Vector3.up * 0.5f + Vector3.forward * 0.5f, transform.rotation);
            newFoodInBowl.GetComponent<MeshRenderer>().material = foodMeshRenderer.material;
            newFoodInBowl.GetComponent<XRGrabInteractable>().enabled = false; //disable grabbing of food so players are not able to pick it up once inside
            correctFood += 1; //increase correct food count
            
            Destroy(other.gameObject); //destroy added food item after adding to bowl
        } 
        if(other.CompareTag("wrongFood")) //Does not need to get the material as it will always be wrong food material
        {
            newFoodInBowl = Instantiate(foodInBowl, transform.position + Vector3.up * 0.5f + Vector3.forward * 0.5f, transform.rotation);
            newFoodInBowl.GetComponent<MeshRenderer>().material = wrongFoodMaterial;
            newFoodInBowl.GetComponent<XRGrabInteractable>().enabled = false; //disable grabbing of food so players are not able to pick it up once inside
            correctFood -= 1000; //decrease correct food count significantly to avoid passing


            Destroy(other.gameObject); //destroy added food item after adding to bowl
        } 
        if(other.CompareTag("mix")) 
        {
            isMixed = true; //only able to mix once
            mixingFood();
        } 
    }
    
    void mixingFood()
    {
            mixedFood.SetActive(true); //show mixed food

            if (correctFood > foodNeeded || correctFood < foodNeeded)
            {
                mixedFood.GetComponent<MeshRenderer>().material = wrongFoodMaterial;
            }
            else if (correctFood == foodNeeded)
            {
                mixedFood.GetComponent<MeshRenderer>().material = correctFinalMaterial;
            }
            else
            {
                mixedFood.SetActive(false); //hide mixed food
            }

        clearBowl = false; //emptys the bowl after mixing
    }

    void OnTriggerStay(Collider other)
    {
        if(clearBowl == false && other.CompareTag("rightFood") || other.CompareTag("wrongFood"))
        {
            Destroy(other.gameObject); //destroy all food in bowl when mixed
        }
        
        clearBowl = true; //bowl has been cleared
    }
}
