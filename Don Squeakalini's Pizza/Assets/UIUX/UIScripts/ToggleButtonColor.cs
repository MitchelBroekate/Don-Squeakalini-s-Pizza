using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonColour : MonoBehaviour
{
    private Button button;         
    private Color originalColor;   
    private bool isToggled = false; 

    public Color toggledColor = Color.green; 

   public void Start()
    {
        button = GetComponent<Button>(); 
        if (button != null)
        {
            originalColor = button.image.color; 
            button.onClick.AddListener(ToggleColor);
        }
    }

   public void ToggleColor()
    {
        if (button != null)
        {
            isToggled = !isToggled; 
            button.image.color = isToggled ? toggledColor : originalColor; 
        }
    }
}
