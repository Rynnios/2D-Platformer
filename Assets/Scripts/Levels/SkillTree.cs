using TMPro;
using UnityEngine;

public class SkillTree : MonoBehaviour
{
    public GameObject treeUI;
    public GameObject menuUI;
    public GameObject speedBtn1;
    public GameObject speedBtn2;
    public GameObject hpBtn1;
    public GameObject hpBtn2;
    public GameObject cooldownBtn1;
    public GameObject cooldownBtn2;

    [SerializeField] private TMP_Text skillPointsText;

    static public SkillTree S; //Singleton

    private void Awake()
    {
        S = this; //setting Singleton
    }

    private void Start()
    {
        if (Data.S.cooldownBtn1Disabled)
            cooldownBtn1.SetActive(false);
        else if (Data.S.cooldownBtn2Disabled)
            cooldownBtn2.SetActive(false);
        else if (Data.S.speedBtn1Disabled)
            speedBtn1.SetActive(false);
        else if (Data.S.speedBtn2Disabled)
            speedBtn2.SetActive(false);
        else if (Data.S.hpBtn1Disabled)
            hpBtn1.SetActive(false);
        else if (Data.S.hpBtn2Disabled)
            hpBtn2.SetActive(false);
    }

    public void Resume()
    {
        treeUI.SetActive(false);
        menuUI.SetActive(true);
    }

    public void CooldownReduce1()
    {
        if (Data.S.skillPoints >= 1)
        {
            Data.S.cooldownBtn1Disabled = true;
            cooldownBtn1.SetActive(false);
            Data.S.skillPoints -= 1;
            skillPointsText.text = ": " + Data.S.skillPoints;

            // Figure out what to do here
        }
    }

    public void CooldownReduce2()
    {
        if (Data.S.skillPoints >= 1)
        {
            Data.S.cooldownBtn2Disabled = true;
            cooldownBtn2.SetActive(false);
            Data.S.skillPoints -= 1;
            skillPointsText.text = ": " + Data.S.skillPoints;

            // Figure out what to do here
        }
    }

    public void SpeedIncrease1()
    {
        if (Data.S.skillPoints >= 1)
        {
            Data.S.speedBtn1Disabled = true;
            speedBtn1.SetActive(false);
            Data.S.skillPoints -= 1;
            skillPointsText.text = ": " + Data.S.skillPoints;

            Data.S.moveSpeed += 1f;
        }
    }

    public void SpeedIncrease2()
    {
        if (Data.S.skillPoints >= 1)
        {
            Data.S.speedBtn2Disabled = true;
            speedBtn2.SetActive(false);
            Data.S.skillPoints -= 1;
            skillPointsText.text = ": " + Data.S.skillPoints;

            Data.S.moveSpeed += 1f;
        }
    }

    public void HpIncrease1()
    {
        if (Data.S.skillPoints >= 1)
        {
            Data.S.hpBtn1Disabled = true;
            hpBtn1.SetActive(false);
            Data.S.skillPoints -= 1;
            skillPointsText.text = ": " + Data.S.skillPoints;

            PlayerController.Instance.AddHealth();
        }
    }

    public void HpIncrease2()
    {
        if (Data.S.skillPoints >= 1)
        {
            Data.S.hpBtn2Disabled = true;
            hpBtn2.SetActive(false);
            Data.S.skillPoints -= 1;
            skillPointsText.text = ": " + Data.S.skillPoints;

            PlayerController.Instance.AddHealth();
        }
    }
}