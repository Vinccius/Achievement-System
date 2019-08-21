using UnityEngine.EventSystems;
using UnityEngine;

public class AchievementClickable : MonoBehaviour
{
    public string AchievementName;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject(-1))
        {
            AchievementsManager.Instance.EarnAchievement(AchievementName);
        }
    }
}
