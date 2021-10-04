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

    public float heatAttackCost = 5;

    public float steamGlideCostPerSecond = 5;


    //State of gauges
    private float currentHeat;
    private float currentSteam;
    private float currentHeatToSteamMultiplierPerSecond;
    private bool ending = false;

    public void End()
    {
        ending = true;
    }

    void BuildUI()
    {
        heatBar.SetValue(currentHeat);
        steamBar.SetValue(currentSteam);
    }

    public void Start()
    {
        heatBar = heatBarObject.GetComponent<BarScript>();
        steamBar = steamBarObject.GetComponent<BarScript>();
        currentHeat = startingHeat;
        currentSteam = startingSteam;
        currentHeatToSteamMultiplierPerSecond = heatToSteamMultiplierPerSecond;
        BuildUI();
    }

    // Update is called once per frame
    void Update()
    {
        BuildUI();
    }

    private void FixedUpdate()
    {
        if(ending)
        {
            return;
        }

        currentHeat += Time.fixedDeltaTime * heatRaisePerSecond;
        currentSteam += Time.fixedDeltaTime * currentHeat * currentHeatToSteamMultiplierPerSecond;

        ClampHeat();
        ClampSteam();

        if (currentHeat == maxHeat) //Heat multiplier starts increasing at max heat level
        {
            currentHeatToSteamMultiplierPerSecond += heatToSteamCriticalMultiplier * Time.fixedDeltaTime;
        }
        else
        {
            currentHeatToSteamMultiplierPerSecond = heatToSteamMultiplierPerSecond;
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
        return true;
    }

    public bool CanGlide()
    {
        return currentSteam > minimumGlideSteam;
    }

    public void OnAttack()
    {
        currentHeat += heatAttackCost;
        ClampHeat();
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

    public void HeatUp(float heat)
    {
        currentHeat += heat;
        ClampHeat();
    }
}
