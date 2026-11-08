using TMPro;
using UnityEngine;

public class ClothingUI : MonoBehaviour
{   
    [SerializeField] private TMP_Text clothesCounterText;
    [SerializeField] private GameObject shiftCompletedPanel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void UpdateCounter (int current, int target)
    {
        clothesCounterText.text = "CLOTHES CLEANED: " + current + "/" + target;
        
    }

    public void ShowShiftCompleted()
    {
        shiftCompletedPanel.SetActive(true);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
