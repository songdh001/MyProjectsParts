using System.Collections;
using UnityEngine;

public class EnemySpawner : MyteamSpawner
{
    /// <summary>
    ///몬스터들이 소환되는 것을 관리하는 스크립트입니다.
    ///어떤 몬스터들이 얼마나 많이, 얼마나 자주 소환되는 지를 다루고 있습니다.
    /// </summary>

    [SerializeField] private UnitData[] currentSpawnPool;//'이 스테이지에선 이런 적들이 나와요' 하는 리스트입니다
    private float[] cooldownTimers; //소환 가능한 얘들 쿨타임 체크하는 용도

    [SerializeField]private BaseController baseController;
    [SerializeField] private float checkInterval = 0.1f; // 반복 주기 코루틴 쓰려고 넣어놨습니다.

    public Transform backGround;
    public Transform ememyHouse;
    private GameObject backGroundObj;
    private GameObject ememyHouseObj;
    public WaveData waveData;
    public void SetStage(int stageIndex)
    {
        waveData = TableManager.Instance.GetTable<WaveTable>().GetDataByID(stageIndex);
        currentSpawnPool = waveData.data.ToArray();
        cooldownTimers = new float[currentSpawnPool.Length];
        backGroundObj = Instantiate(DataManager.Instance.BackGroundDic[waveData.InGameBackgroundType].BackGround, backGround);
        ememyHouseObj = Instantiate(DataManager.Instance.HouseDic[waveData.HouseType].EnemyHouse, ememyHouse);
        SoundManager.Instance.PlaySound(SoundType.BGM, "BGM_BATTLE_1", true);
        StartCoroutine(AutoSpawnLoop());
    }


    private void Start()
    {
        StartCoroutine(AddGold());
        SetStage(StageManager.Instance.currentPlayingStage);
    }

    private void OnDestroy()
    {
        Destroy(backGroundObj); 
        Destroy(ememyHouseObj);
    }

    private IEnumerator AutoSpawnLoop()
    {
        while (true)
        {
            // 모든 유닛 쿨타임 감소
            for (int i = 0; i < cooldownTimers.Length; i++)
            {
                if (cooldownTimers[i] > 0)
                    cooldownTimers[i] -= checkInterval;
            }

            // 랜덤한 유닛 인덱스 선택
            int tryIndex = Random.Range(0, currentSpawnPool.Length);
            UnitData chosenUnit = currentSpawnPool[tryIndex];
            // 쿨타임 중이 아니고, 골드도 충분하다면 소환
            if (cooldownTimers[tryIndex] <= 0f &&
                baseController.GetGold() >= chosenUnit.cost)
            {
                Spawn(chosenUnit);
                baseController.SpendGold(chosenUnit.cost);
                cooldownTimers[tryIndex] = chosenUnit.cooldown;
            }
            yield return new WaitForSeconds(checkInterval);
        }
    }
    private IEnumerator AddGold()
    {
        while (true)
        {
            baseController.ChangeGold(10);
            yield return new WaitForSeconds(5f);
        }
        
    }
}
