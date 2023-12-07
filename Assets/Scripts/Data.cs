using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Data : MonoBehaviour
{
    static public Data S; //Singleton

    // Player fields
    public int skillPoints = 0;
    //public int level = 1;
    //public int playerLevel = 1;
    //public int enemyCount = 0;
    //public float attackDamage = 0.4f;
    public int cooldownReducer = 0;
    public float moveSpeed = 7f;
    public float maxHealth = 3f;
    public float jumpPower = 15f;
    public float knockbackStrengthX = 8f; // Horizontal knockback strength
    public float knockbackStrengthY = 10f; // Vertical knockback strength
    public float invincibilityDuration = 2f; // Duration of invincibility in seconds
    public bool doubleJumpEnabled = false;
    public bool trueInvincibilityEnabled = false;
    public string lastLevelPlayed;

    public enum SkillState { Locked, Available, Purchased }

    // Skill tree fields
    public SkillState speed1State { get; set; } = SkillState.Available;
    public SkillState speed2State { get; set; } = SkillState.Locked;
    public SkillState jump1State { get; set; } = SkillState.Available;
    public SkillState jump2State { get; set; } = SkillState.Locked;
    public SkillState health1State { get; set; } = SkillState.Available;
    public SkillState health2State { get; set; } = SkillState.Locked;
    public SkillState resistance1State { get; set; } = SkillState.Available;
    public SkillState resistance2State { get; set; } = SkillState.Locked;
    public SkillState doubleJumpState { get; set; } = SkillState.Locked;
    public SkillState trueInvincibilityState { get; set; } = SkillState.Locked;

    // Level times
    public float level1Time = 0;
    public float level2Time = 0;
    public float bossTime = 0;

    // Pineapple collection tracking
    public Dictionary<string, bool> collectedPineapples = new Dictionary<string, bool>();

    private void Awake()
    {
        if (S == null)
        {
            S = this;
            DontDestroyOnLoad(S);
            InitializePineapples();
            SceneManager.sceneLoaded += OnSceneLoaded; // Subscribe to the sceneLoaded event
        }
        else if (S != this)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Unsubscribe when the object is destroyed
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdatePineappleVisuals(); // Update visuals whenever a new scene is loaded
    }

    // Initialize pineapples' collection state
    void InitializePineapples()
    {
        // 3 pineapples in each level except the boss level
        string[] levels = { "HubWorld", "Level1", "Level2" };
        int pineapplesPerLevel = 3;

        foreach (var level in levels)
        {
            for (int i = 1; i <= pineapplesPerLevel; i++)
            {
                string key = level + "_Pineapple" + i;
                if (!collectedPineapples.ContainsKey(key))
                {
                    collectedPineapples.Add(key, false);
                }
            }
        }

        // Special case for Boss Level cus it has only 1 pineapple
        collectedPineapples["BossLevel_Pineapple1"] = false;
    }

    void UpdatePineappleVisuals()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        int pineapplesInScene = (sceneName == "BossLevel") ? 1 : 3; // 1 pineapple in Boss Level, 3 in others

        for (int i = 1; i <= pineapplesInScene; i++)
        {
            string pineappleName = "Pineapple" + i;
            GameObject pineapple = GameObject.Find(pineappleName);

            if (pineapple != null)
            {
                string pineappleKey = sceneName + "_" + pineappleName;
                if (Data.S.collectedPineapples.ContainsKey(pineappleKey) && Data.S.collectedPineapples[pineappleKey])
                {
                    SpriteRenderer sr = pineapple.GetComponent<SpriteRenderer>();
                    if (sr != null)
                    {
                        Color color = sr.color;
                        color.a = 0.5f; // Set alpha to 50%
                        sr.color = color;
                    }
                }
            }
        }
    }
}