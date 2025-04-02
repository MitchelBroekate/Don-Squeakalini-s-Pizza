using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedButton : MonoBehaviour
{
  [SerializeField] Button myButton;
    void ResetButtonVisuals()
    {
        myButton.OnDeselect(null);  
        myButton.targetGraphic.CrossFadeColor(myButton.colors.normalColor, 0f, true, true);
    }

}
