using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Data;

[System.Serializable]
public struct Skill
{
    public GameObject button;
    public string skillIdentifier; // Skill identifier
    public string description; // Skill description
}
public class SkillTree : MonoBehaviour
{
    public GameObject treeUI;
    public GameObject menuUI;
    public Skill[] skills; // Array to hold skills
    [SerializeField] private TMP_Text skillPointsText;
    [SerializeField] private TMP_Text skillDescriptionText;

    static public SkillTree S; // Singleton

    private void Awake()
    {
        S = this;
    }

    private void Start()
    {
        UpdateSkillButtons();
    }

    public void Resume()
    {
        treeUI.SetActive(false);
        menuUI.SetActive(true);
    }

    private void UpdateSkillButtons()
    {
        foreach (var skill in skills)
        {
            SkillState state = GetSkillState(skill.skillIdentifier);
            UpdateButtonAppearance(skill.button, state);
        }
    }

    private SkillState GetSkillState(string skillIdentifier)
    {
        return (SkillState)typeof(Data).GetProperty(skillIdentifier).GetValue(Data.S);
    }

    private void UpdateButtonAppearance(GameObject button, SkillState state)
    {
        var buttonImage = button.GetComponent<Image>();
        switch (state)
        {
            case SkillState.Locked:
                buttonImage.color = Color.grey;
                break;
            case SkillState.Available:
                buttonImage.color = Color.white;
                break;
            case SkillState.Purchased:
                buttonImage.color = Color.yellow; // Gold tint
                break;
        }
    }

    // Button hover
    public void OnSkillHover(int skillIndex)
    {
        if (skillIndex >= 0 && skillIndex < skills.Length)
        {
            skillDescriptionText.text = skills[skillIndex].description;
        }
    }

    public void OnSkillHoverExit()
    {
        skillDescriptionText.text = "";
    }


    // Button press
    public void ActivateSkill(int index)
    {
        if (index >= 0 && index < skills.Length && Data.S.skillPoints >= 1)
        {
            Skill skill = skills[index];
            SkillState state = GetSkillState(skill.skillIdentifier);

            if (state == SkillState.Available)
            {
                // Update Data
                typeof(Data).GetProperty(skill.skillIdentifier).SetValue(Data.S, SkillState.Purchased);

                // Deduct skill point and update UI
                Data.S.skillPoints -= 1;
                skillPointsText.text = ": " + Data.S.skillPoints;

                // Execute skill effect
                ExecuteSkillEffect(skill.skillIdentifier);

                // Update skill states and buttons
                UpdateSkillStates();
                UpdateSkillButtons();
            }
        }
    }

    private void UpdateSkillStates()
    {
        // Logic to update the state of each skill based on prerequisites
        if (Data.S.speed1State == SkillState.Purchased && Data.S.speed2State == SkillState.Locked)
        {
            Data.S.speed2State = SkillState.Available;
        }
        if (Data.S.jump1State == SkillState.Purchased && Data.S.jump2State == SkillState.Locked)
        {
            Data.S.jump2State = SkillState.Available;
        }
        if (Data.S.resistance1State == SkillState.Purchased && Data.S.resistance2State == SkillState.Locked)
        {
            Data.S.resistance2State = SkillState.Available;
        }
        if (Data.S.health1State == SkillState.Purchased && Data.S.health2State == SkillState.Locked)
        {
            Data.S.health2State = SkillState.Available;
        }
        if (Data.S.speed2State == SkillState.Purchased && Data.S.jump2State == SkillState.Purchased && Data.S.resistance2State == SkillState.Purchased && Data.S.health2State == SkillState.Purchased && Data.S.doubleJumpState == SkillState.Locked)
        {
            Data.S.doubleJumpState = SkillState.Available;
        }
        if (Data.S.doubleJumpState == SkillState.Purchased && Data.S.trueInvincibilityState == SkillState.Locked)
        {
            Data.S.trueInvincibilityState = SkillState.Available;
        }
    }

    private void ExecuteSkillEffect(string skillIdentifier)
    {
        switch (skillIdentifier)
        {
            case "speed1State":
                SpeedEffect();
                break;
            case "speed2State":
                SpeedEffect();
                break;
            case "health1State":
                HealthEffect();
                break;
            case "health2State":
                HealthEffect();
                break;
            case "jump1State":
                JumpEffect();
                break;
            case "jump2State":
                JumpEffect();
                break;
            case "resistance1State":
                DecreaseKnockbackEffect();
                break;
            case "resistance2State":
                BuffInvincibilityFramesEffect();
                break;
            case "doubleJumpState":
                DoubleJumpEffect();
                break;
            case "trueInvincibilityState":
                TrueInvincibilityEffect();
                break;
        }
    }

    // Skill effects
    private void SpeedEffect()
    {
        Data.S.moveSpeed += 1f;
    }

    private void HealthEffect()
    {
        PlayerController.Instance.AddHealth();
    }

    private void JumpEffect()
    {
        Data.S.jumpPower += 1f;
    }

    private void DecreaseKnockbackEffect()
    {
        Data.S.knockbackStrengthX -= 3f;
        Data.S.knockbackStrengthY -= 1f;
    }
    private void BuffInvincibilityFramesEffect()
    {
        Data.S.invincibilityDuration += 2f;
    }

    private void DoubleJumpEffect()
    {
        Data.S.doubleJumpEnabled = true;
    }

    private void TrueInvincibilityEffect()
    {
        Data.S.moveSpeed += 1f;
        Data.S.jumpPower += 1f;
        for (int i = 0; i < 3; i++)
        {
            PlayerController.Instance.AddHealth();
        }
        Data.S.trueInvincibilityEnabled = true;
    }
}