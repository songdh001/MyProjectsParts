using System;
using System.Collections.Generic;
using UnityEngine;

public class MyteamSpawner : MonoBehaviour
{
    /// <summary>
    /// 아군 캐릭터를 소환할 경우에 사용되는 스크립트입니다.
    /// 어디에서 소환하는지 어떤 유닛을 소환하는지 등이 포함되어 있습니다.
    /// </summary>

    [SerializeField] private Transform spawnPoint; // 아군 유닛이 나올 위치 (아군 기지 쪽)
    private List<Transform> UnitList = new List<Transform>();
    public void Spawn(UnitData unitData)
    {
        if (unitData == null || unitData.prefab == null)
        {
            Debug.LogWarning("유닛 데이터 또는 프리팹이 비어 있습니다.");
            return;
        }
        float randomPoint = UnityEngine.Random.Range(-0.4f, 0.5f);
        var newUnit = PoolManager.Instance.GetObject((PoolType)Enum.Parse(typeof(PoolType), unitData.unitType.ToString()));
        UnitList.Add(newUnit.transform);
        newUnit.transform.position = new (spawnPoint.position.x, spawnPoint.position.y + randomPoint);
        newUnit.transform.rotation = Quaternion.identity;
    }

    public void AllReturn()
    {
        foreach (var Unit in UnitList)
        {
            if(Unit.TryGetComponent<PoolBase>(out var poolBase))
            {
                poolBase.ReturnPool();
            }
        }
    }
}
