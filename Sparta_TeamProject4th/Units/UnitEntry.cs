using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

[System.Serializable]
public class UnitEntry
{
    public int UnitID;
    public int UnitUpgrade;
    private int maxUpgrade = 10;
    public bool unlock; // 적은 이거 전부 트루

    private float currentHealth = 100f;
    public float CurrentHealth { get { return currentHealth; } set { currentHealth = value; } }
    private float sponCoolTime;
    private bool isCanSpon = true;
    public bool IsCanUse
    {
        get { return isCanSpon; }
        set { isCanSpon = value; }
    }
    [SerializeField]private UnitData unitData;
    public UnitData UnitData { 
        get { return unitData; }
        set { unitData = value; }
    }


    public event Action onUpgrade;
    public event Action onUnlock;
    public event Action<float> onCoolTime;

    public void Upgrade()
    {
        UnitUpgrade++;
        onUpgrade?.Invoke();
    }

    public void Unlock()
    {
        unlock = true;
        onUnlock?.Invoke();
    }
    public bool TakeDamage(float damage)
    {
        currentHealth -= damage;
        if(currentHealth <= 0) return true;
        return false;
    }
    public void Heal(float heal)
    {
        Debug.Log($"{heal}의 체력이회복했습니다.");
        currentHealth += heal;
        currentHealth = math.min(currentHealth, unitData.maxHealth);
    }
    public IEnumerator Cooldown()
    {
        isCanSpon = false;
        sponCoolTime = 0;
        while (sponCoolTime < unitData.cooldown)
        {
            sponCoolTime += Time.deltaTime;

            onCoolTime?.Invoke(sponCoolTime / unitData.cooldown);

            yield return null; // 프레임마다 반복
        }
        isCanSpon = true;
    }

    public bool IsDead()
    {
        return currentHealth <= 0;
    }

    public float GetUpgradeRatio() => Mathf.Clamp01(UnitUpgrade / maxUpgrade);
}
