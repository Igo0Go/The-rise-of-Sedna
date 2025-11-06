using System;
using UnityEngine;

public class FPC_SkillSystem : MonoBehaviour
{
    [SerializeField]
    private SkillInfo moveSpeed;
    [SerializeField]
    private SkillInfo inAirMoveForce;
    [SerializeField]
    private SkillInfo jumpHeight;
    [SerializeField]
    private SkillInfo sprintMultiplier;
    [SerializeField]
    private SkillInfo sprintTime;
    [SerializeField]
    private SkillInfo recoilForceMultiplier;

    public void Awake()
    {
        SkillHolder.Instance.speed = new SkillInfo(moveSpeed);
        SkillHolder.Instance.inAirMoveForce = new SkillInfo(inAirMoveForce);
        SkillHolder.Instance.jumpHeight = new SkillInfo(jumpHeight);
        SkillHolder.Instance.sprintMultiplier = new SkillInfo(sprintMultiplier);
        SkillHolder.Instance.sprintTime = new SkillInfo(sprintTime);
        SkillHolder.Instance.recoilForceMultiplier = new SkillInfo(recoilForceMultiplier);
    }
}

public class SkillHolder
{
    public SkillInfo speed;
    public SkillInfo inAirMoveForce;
    public SkillInfo jumpHeight;
    public SkillInfo sprintMultiplier;
    public SkillInfo sprintTime;
    public SkillInfo recoilForceMultiplier;

    public event Action<int> SkillPointsChanged;
    public event Action<int> AddNewExp;

    public int SkillPoints
    {
        get
        {
            return _skillPoints;
        }
        private set
        {
            _skillPoints = value;
            SkillPointsChanged?.Invoke(_skillPoints);
        }
    }
    private int _skillPoints;

    public int CurrentExp { get; private set; } = 0;
    public int CurrentExpTargetLevel { get; private set; } = 500;

    private const int levelStep = 500;

    private SkillHolder()
    {
    }

    private static SkillHolder _instance;

    public static SkillHolder Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new SkillHolder();
            }
            return _instance;
        }
    }

    public void LevelUpSkill(SkillInfo skillInfo)
    {
        if (SkillPoints > 0)
        {
            SkillPoints--;
            skillInfo.LevelUp();
        }
    }

    public void AddExperience(int exp)
    {
        CurrentExp += exp;
        AddNewExp?.Invoke(exp);

        while(CurrentExp >= CurrentExpTargetLevel)
        {
            SkillPoints++;
            CurrentExp -= CurrentExpTargetLevel;
            CurrentExpTargetLevel += levelStep;
        }
    }
}

[Serializable]
public class SkillInfo
{
    public string name;
    public string description;
    public float currentValue;
    public float treholdValue;
    public float levelUpStep;

    public bool CanLevelUp { get; private set; } = true;

    public event Action OnLevelUp;

    public SkillInfo(SkillInfo skillInfo)
    {
        name = skillInfo.name;
        description = skillInfo.description;
        currentValue = skillInfo.currentValue;
        treholdValue = skillInfo.treholdValue;
        levelUpStep = skillInfo.levelUpStep;
    }

    public void LevelUp()
    {
        if(treholdValue > currentValue)
        {
            currentValue += levelUpStep;

            if(currentValue >= treholdValue)
            {
                CanLevelUp = false;
            }
        }
        else if (treholdValue < currentValue)
        {
            currentValue -= levelUpStep;

            if (currentValue <= treholdValue)
            {
                CanLevelUp = false;
            }
        }

        OnLevelUp?.Invoke();
    }
}