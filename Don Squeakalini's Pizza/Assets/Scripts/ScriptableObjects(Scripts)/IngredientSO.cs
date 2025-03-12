using UnityEngine;

[CreateAssetMenu(fileName = "IngredientSO", menuName = "ScriptableObjects/IngredientSO", order = 1)]
public class IngredientSO : ScriptableObject
{
    public string ingredientName;
    public Sprite ingredientSprite;
    public GameObject ingredientObject;
}