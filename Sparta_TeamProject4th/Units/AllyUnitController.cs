using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyUnitController : MonoBehaviour
{
    /// <summary>
    /// 아군 유닛이 소환될 수 있는 조건(코스트, 쿨타임)을 확인합니다.
    /// </summary>

    private MyteamSpawner spawner;
    [SerializeField] Dictionary<UnitType, UnitEntry> allyUnitEntrys;


    private void Start()
    {
        spawner = transform.GetComponent<MyteamSpawner>();
        allyUnitEntrys = UnitManager.Instance.GetAllyUnitEntrys();
    }

    void Update()//우선 임시로 직접 키값을 받는 거로 해놨습니다.
    {
        // 1번 키 → 첫 번째 유닛 소환
        if (Input.GetKeyDown(KeyCode.Alpha1))
            TrySummon(UnitType.Kale);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            TrySummon(UnitType.Lyra);

        if (Input.GetKeyDown(KeyCode.Alpha3))
            TrySummon(UnitType.Zephyr);
    }


    public void TrySummon(UnitType unitType)
    {
        if (!allyUnitEntrys[unitType].unlock)
        {
            Debug.Log("아직 동료가 아님");
            return;
        }
        if (!allyUnitEntrys[unitType].IsCanUse)
        {
            Debug.Log(" 소환 쿨타임 도는중");
            return;
        }
        if (!MyPlayerManager.Instance.Status.UseFood(allyUnitEntrys[unitType].UnitData.cost))
        {
            Debug.Log("고기 부족");
            return;
        }

        spawner.Spawn(allyUnitEntrys[unitType].UnitData);
        StartCoroutine(allyUnitEntrys[unitType].Cooldown());
        SoundManager.Instance.PlaySound(SoundType.SFX, "SFX_UnitSpawnClick", false);
    }
}
