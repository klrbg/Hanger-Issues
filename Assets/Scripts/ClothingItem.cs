using UnityEngine;

public class ClothingItem : MonoBehaviour
{   
    private bool wasCounted = false;

    public bool IsCounted()
    {
        return wasCounted;
    }

    public void MarkAsCounted()
    {
        wasCounted = true;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
