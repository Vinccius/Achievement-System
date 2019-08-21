using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementsButton : MonoBehaviour
{
    public GameObject achievementList;

    public Sprite neutral, hightlight;
    private Image Sprite;

    private void Awake()
    {
        Sprite = GetComponent<Image>();
    }

    public void Click()
    {
        if(Sprite.sprite == neutral)
        {
            Sprite.sprite = hightlight;
            achievementList.SetActive(true);
        }
        else
        {
            Sprite.sprite = neutral;
            achievementList.SetActive(false);
        }
    }
	
	
}
