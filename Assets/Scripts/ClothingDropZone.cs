
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ClothingDropZone : MonoBehaviour
{
    [SerializeField] private ClothingUI clothingUI;
    private int cleanedClothes = 0;
    private int targetClothes = 3;
    private bool shiftCompleted = false;
    
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PickupItem"))
        {
            ClothingItem item = other.GetComponent<ClothingItem>();

            if (item != null 
                && !item.IsCounted())
            {
                cleanedClothes += 1;
                item.MarkAsCounted();
                clothingUI.UpdateCounter(cleanedClothes, targetClothes);
                Debug.Log("Score: " + cleanedClothes);
                 
                 if (cleanedClothes >= targetClothes
                    && !shiftCompleted)
                {
                    shiftCompleted = true;
                    Debug.Log("Shift Completed!");
                    clothingUI.ShowShiftCompleted();
                }
            } 
        }
    }

    private void Start()
    {
        clothingUI.UpdateCounter(cleanedClothes, targetClothes);
    }

    private void Update()
    {
        if (shiftCompleted
            && Keyboard.current.rKey.wasPressedThisFrame)
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.buildIndex);
        }
    }
}
