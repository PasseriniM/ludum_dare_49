using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gauges : MonoBehaviour
{
    [Header("UI")]
    [Space]

    public GameObject heatBarObject;

    public GameObject steamBarObject;

    BarScript heatBar;

    BarScript steamBar;


    [Header("Gauges Settings")]
    [Space]

    public float startingHeat = 30;

    public float startingSteam = 10;

    public float maxHeat = 100f;

    public float maxSteam = 100f;

    public float criticalHeat = 90f;

    public float criticalSteam = 90f;

    public float heatRaisePerSecond = 2;

    public float heatToSteamMultiplierPerSecond = 1;

    public float heatToSteamCriticalMultiplier = 1;

    [Header("Action Costs")]
    [Space]

    public float minimumMovementSteam = 10;

    public float minimumGlideSteam = 5;

    public float steamJumpCost = 20;

    public float steamAttackCost = 5;

    public float steamGlideCostPerSecond = 5;


    //State of gauges
    private float currentHeat;
    private float currentSteam;

    void BuildUI()
    {
        heatBar.SetValue(currentHeat);
        steamBar.SetValue(currentSteam);
    }

    private void Awake()
    {
        heatBar = heatBarObject.GetComponent<BarScript>();
        steamBar = steamBarObject.GetComponent<BarScript>();
        currentHeat = startingHeat;
        currentSteam = startingSteam;
        BuildUI();
    }

    // Update is called once per frame
    void Update()
    {
        BuildUI();
    }

    private void FixedUpdate()
    {
        currentHeat += Time.fixedDeltaTime * heatRaisePerSecond;
        currentSteam += Time.fixedDeltaTime * currentHeat * heatToSteamMultiplierPerSecond;

        ClampHeat();
        ClampSteam();

        if (currentHeat == maxHeat) //Heat multiplier starts increasing at max heat level
        {
            heatToSteamMultiplierPerSecond += heatToSteamCriticalMultiplier * Time.fixedDeltaTime;
        }
    }

    private void ClampSteam()
    {
        currentSteam = System.Math.Max(0f, currentSteam);
        currentSteam = System.Math.Min(maxSteam, currentSteam);
    }

    private void ClampHeat()
    {
        currentHeat = System.Math.Max(0f, currentHeat);
        currentHeat = System.Math.Min(maxHeat, currentHeat);
    }

    public bool ShouldDie()
    {
        return currentSteam == maxSteam;
    }

    public bool ShouldSlowDown()
    {
       return currentSteam < minimumMovementSteam;
    }

    public bool CanJump()
    {
        return currentSteam > steamJumpCost;
    }

    public bool CanAttack()
    {
        return currentSteam > steamAttackCost;
    }

    public bool CanGlide()
    {
        return currentSteam > minimumGlideSteam;
    }

    public void OnAttack()
    {
        currentSteam -= steamAttackCost;
        ClampSteam();
    }

    public void OnJump()
    {
        currentSteam -= steamJumpCost;
        ClampSteam();
    }

    public void OnGlide(float deltaTime)
    {
        currentSteam -= steamGlideCostPerSecond * deltaTime;
        ClampSteam();
    }

    public void OnDamageTaken(float steamDamage)
    {
        currentHeat += steamDamage;
        ClampHeat();
    }

    public void Cooldown(float cooling)
    {
        currentHeat -= cooling;
        ClampHeat();
    }
}
