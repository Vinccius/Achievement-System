using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementsManager : MonoBehaviour
{
    [Header("Achievements settings")]
    [Space(10)]
    [Tooltip("Feedback achievement in Achievement Menu (In PlayMode press I)")]
    public GameObject AchievementPrefab;

    [Header("Sprite index list")]
    [Space(10)]
    [Tooltip("Contais this sprite index")]
    public Sprite[] Sprites;
      
    
    [Header("Settings")]
    [Space(10)]
    [Tooltip("Where the object will be created")]
    public ScrollRect scrollRect;

    [Tooltip("Menu of Achievements")]
    public GameObject AchivmentMenu;

    [Tooltip("Feedback in Screen")]
    public GameObject visualAchievement;

    [Tooltip("Contains the achievement")]
    public Dictionary<string, Achievement> achievements = new Dictionary<string, Achievement>();

    [Tooltip("Sprite indicating that the achievement is unlocked")]
    public Sprite unlockedSprite;

    [Tooltip("Points that the player will receive upon completing the achievement")]
    public Text textPoints;

    private int FadeTime = 2;

    private AchievementsButton activeButton;
    private static AchievementsManager instance;
    private Animator anim;


    public static AchievementsManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<AchievementsManager>();
            }
            return AchievementsManager.instance;
        }


    }



    // Use this for initialization
    void Start ()
    {
        PlayerPrefs.DeleteAll();
        anim = visualAchievement.GetComponent<Animator>();

        activeButton = GameObject.Find("GeneralBotton").GetComponent<AchievementsButton>();
        AchievementCreate("General", "Press W", 5, "Press W to unlock this achievement", 0);
        AchievementCreate("General", "Press A", 5, "Press S to unlock this achievement", 1);
        AchievementCreate("General", "Press S", 5, "Press S to unlock this achievement", 2);
        AchievementCreate("General", "Press D", 5, "Press D to unlock this achievement", 3);
        AchievementCreate("General", "Press All Keys", 10, "Press All keys", 4, new string[] { "Press W", "Press A", "Press A", "Press D" });

        AchievementCreate("Other", "Click in Axe", 50, "Click in Axe for unlock", 5);

        foreach (GameObject achievementList in GameObject.FindGameObjectsWithTag("AchievementList"))
        {
            achievementList.SetActive(false);
        }

        activeButton.Click();
        AchivmentMenu.SetActive(false);
	}
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            AchivmentMenu.SetActive(!AchivmentMenu.activeSelf);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            EarnAchievement("Press W");
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            EarnAchievement("Press A");
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            EarnAchievement("Press S");
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            EarnAchievement("Press D");
        }
    }

    public void EarnAchievement(string title)
    {
        if (achievements[title].EarnAchievement())
        {
            GameObject achievement = Instantiate(visualAchievement);
            SetAchievementInfo("EarnCanvas", achievement, title, new Vector3(9, 5, 0));
            textPoints.text = "Points: " + PlayerPrefs.GetInt("Points");
            StartCoroutine(AchivementEffect(achievement));
        }
    }

    public IEnumerator HideAchievement(GameObject achivment)
    {
        yield return new WaitForSeconds(3);
        Destroy(achivment);
    }


    public void AchievementCreate(string parent, string title, int points, string description,int spriteIndex, string[] Dependencies = null)
    {
        GameObject Achievment = Instantiate(AchievementPrefab);
        Achievement newAchievement = new Achievement(name, description, points, spriteIndex, Achievment);

        achievements.Add(title, newAchievement);
        SetAchievementInfo(parent, Achievment, title, new Vector3(1, 1, 1));

        if(Dependencies != null)
        {
            foreach(string AchievementTitle in Dependencies)
            {
                Achievement dependency = achievements[AchievementTitle];
                dependency.child = title;
                newAchievement.AddDependencies(dependency);
            }
        }
    }

    public void SetAchievementInfo(string parent, GameObject Achievment, string title, Vector3 Scale)
    {
        Achievment.transform.SetParent(GameObject.Find(parent).transform);
        Achievment.transform.localScale = Scale;
        Achievment.transform.GetChild(0).GetComponent<Text>().text = title;
        Achievment.transform.GetChild(1).GetComponent<Text>().text = achievements[title].Description;
        Achievment.transform.GetChild(2).GetComponent<Text>().text = achievements[title].Points.ToString();
        Achievment.transform.GetChild(3).GetComponent<Image>().sprite = Sprites[achievements[title].SpriteIndex];
    }

    public void ChangeCategory(GameObject Button)
    {
        AchievementsButton achievementsButton = Button.GetComponent<AchievementsButton>();
        scrollRect.content = achievementsButton.achievementList.GetComponent<RectTransform>();
        achievementsButton.Click();
        activeButton.Click();
        activeButton = achievementsButton;
    }

    private IEnumerator AchivementEffect(GameObject Achievement)
    {       
        if (Achievement.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("ShowCardAchievement"))
        {
            yield return new WaitForSeconds(Achievement.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + Achievement.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime);
            Achievement.GetComponent<Animator>().SetTrigger("ShowCard");
            yield return new WaitForSeconds(5f);
            Destroy(Achievement);
        }

    }        

    private IEnumerator FadeAchievementEffect(GameObject Achievement)
    {
        CanvasGroup canvasGroup = Achievement.GetComponent<CanvasGroup>();

        float Rate = 1.0f / FadeTime;

        int StartAlpha = 0;
        int EndAlpha = 1;


        for(int i = 0; i < 2; i++)
        {
            float Progress = 0.0f;

            while (Progress < 1.0f)
            {
                canvasGroup.alpha = Mathf.Lerp(StartAlpha, EndAlpha, Progress);

                Progress += Rate * Time.deltaTime;

                yield return null;
            }
            yield return new WaitForSeconds(2);
            StartAlpha = 1;
            EndAlpha = 0;
        }

        Destroy(Achievement);
    }
}
