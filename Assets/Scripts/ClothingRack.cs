using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ClothingRack : MonoBehaviour
{   
    [SerializeField] private Transform[] hangSlots;
    [SerializeField] private float hangDistance = 1.5f;
    [SerializeField] private ClothingUI clothingUI;


    private int nextSlotIndex = 0;
    private bool shiftCompleted = false;
    public bool HasFreeSlot()
    {
        return nextSlotIndex < hangSlots.Length;
       
    }

    public bool TryHang(Rigidbody item)
    {
        if (item == null
            || !HasFreeSlot()
            || !IsItemNearNextSlot(item))
        {
            return false;
        }

        ClothingItem itemData = item.GetComponent<ClothingItem>();

        if (itemData == null
            || itemData.IsCounted())
        {
            return false;
        }
       
         Transform slot = hangSlots[nextSlotIndex];
         item.linearVelocity = Vector3.zero;
         item.angularVelocity = Vector3.zero;
         item.useGravity = false;
         item.isKinematic = true;
         item.transform.position = slot.position;
         item.transform.rotation = slot.rotation;
         nextSlotIndex++;
         itemData.MarkAsCounted();
         clothingUI.UpdateCounter(nextSlotIndex, hangSlots.Length);
         
         if (nextSlotIndex >= hangSlots.Length && !shiftCompleted)
            {
                shiftCompleted = true;
                clothingUI.ShowShiftCompleted();
            }
         return true;
    }
    
    public bool IsItemNearNextSlot(Rigidbody item)
    {
        if (item == null
            || !HasFreeSlot())
        {
            return false;
        }

        Transform slot = hangSlots[nextSlotIndex];
        float distance = Vector3.Distance(
            item.position,
            slot.position
        );

        return distance <= hangDistance;
    }

    

    private void Start()
    {
        clothingUI.UpdateCounter(nextSlotIndex, hangSlots.Length);
    }

    private void Update()
    {
        if (shiftCompleted
            && Keyboard.current != null
            && Keyboard.current.rKey.wasPressedThisFrame)
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.buildIndex);
        }
    }

}
