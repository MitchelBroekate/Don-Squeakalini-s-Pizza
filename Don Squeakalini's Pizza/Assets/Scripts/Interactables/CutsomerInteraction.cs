using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerInteraction : MonoBehaviour
{
    //customer talks (pop-up)
    Transform canvas;
    List<GameObject> foodIcons = new();

    CanvasGroup canvasAlpha;

    void Start()
    {
        canvas = transform.GetChild(0).transform;

        for(int i = 0; i < canvas.childCount; i++)
        {
            foodIcons.Add(canvas.GetChild(i).gameObject);
        }
    }

    public IEnumerator ImageFade()
    {
        int imageCount = 0;

        foreach(GameObject foodImage in foodIcons)
        {
            bool alphaIncrease = true;

            SpriteRenderer currentSprite = foodIcons[imageCount].GetComponent<SpriteRenderer>();
            canvasAlpha = foodIcons[imageCount].GetComponent<CanvasGroup>();

            currentSprite.enabled = true;

            while(alphaIncrease)
            {
                //canvasAlpha.alpha += 0.3 * Time.deltaTime;
            }

            yield return new WaitForSeconds(0.9f);

            alphaIncrease = false;

            imageCount++;
        }
        

    }

    //icon fade-in
}
