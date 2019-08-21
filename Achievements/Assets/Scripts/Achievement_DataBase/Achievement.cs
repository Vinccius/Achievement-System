using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Achievement
{
    private string name; 
    private string description;
    private bool unlocked; 
    private int points;
    private int spriteIndex;
    private GameObject achievementRef;

    private List<Achievement> Dependencies = new List<Achievement>();

    private string Child;

    public Achievement(string name, string description, int points, int SpriteIndex, GameObject achievementRef)
    {
        this.name = name;
        this.description = description;
        this.Unlocked = false;
        this.Points = points;
        this.SpriteIndex = SpriteIndex;
        this.achievementRef = achievementRef;
        LoadAchievement();
    }

    public void AddDependencies(Achievement Dependency)
    {
        Dependencies.Add(Dependency);
    }
    
    public string child
    {
        get
        {
            return Child;
        }
        set
        {
            Child = value;
        }
    }

    public string Description
    {
        get
        {
            return description;
        }

        set
        {
            description = value;
        }
    }

    public bool Unlocked
    {
        get
        {
            return unlocked;
        }

        set
        {
            unlocked = value;
        }
    }

    public int Points
    {
        get
        {
            return points;
        }

        set
        {
            points = value;
        }
    }

    public int SpriteIndex
    {
        get
        {
            return spriteIndex;
        }

        set
        {
            spriteIndex = value;
        }
    } 

    public bool EarnAchievement()
    {
        if (!Unlocked && !Dependencies.Exists(x => x.unlocked == false))
        {         
            achievementRef.GetComponent<Image>().sprite = AchievementsManager.Instance.unlockedSprite;
            SavaAchievement(true);

            if(child != null)
            {
                AchievementsManager.Instance.EarnAchievement(child);
            }
            return true;
        }
        return false;
    }

    public void SavaAchievement(bool value)
    {
        unlocked = value;
        int tmpPoints = PlayerPrefs.GetInt("Points");
        PlayerPrefs.SetInt("Points", tmpPoints += points);
        PlayerPrefs.SetInt(name, value ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void LoadAchievement()
    {
        unlocked = PlayerPrefs.GetInt(name) == 1 ? true : false;
        if (unlocked)
        {
            AchievementsManager.Instance.textPoints.text = "Points: " + PlayerPrefs.GetInt("Points");
            achievementRef.GetComponent<Image>().sprite = AchievementsManager.Instance.unlockedSprite;
        }

    }
}
