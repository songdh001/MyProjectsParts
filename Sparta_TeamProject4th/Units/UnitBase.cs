using Unity.Mathematics;
using UnityEngine;

public class UnitBase : MonoBehaviour
{
    /// <summary>
    /// 아군 적군 유닛의 공통된 동작을 다루기 위한 스크립트입니다.
    /// 데미지를 받거나 죽는 메서드가 포함되어 있습니다.
    /// </summary>
    public UnitType type;
    public UnitEntry entry;
    private PoolBase poolBase;
    public bool isCanGetGold;
    private void OnEnable()
    {
        if(StageManager.Instance.currentPlayingStage==0)
        {
            var data = TableManager.Instance.GetTable<WaveTable>().GetDataByID(0).data.Find(x => x.unitType == type);
            if (data != null)
                entry.UnitData = new UnitData(data);
        }
        if(entry.UnitData == null|| entry.UnitData.unitType == UnitType.None)
            entry = UnitManager.Instance.GetUnitEntry(type);
        entry.CurrentHealth = entry.UnitData.maxHealth;

        if(entry.UnitData.unitType != UnitType.EnemyBase) poolBase = this.transform.parent.GetComponent<PoolBase>();

        isCanGetGold = true;
    }


    public void TakeDamage(float damage)
    {
        //체력이 감소될 때 남은 체력을 계산하기 위한 함수입니다.
        //만약 체력이 0 이하가 될 경우 사망하게 됩니다.        

        if (entry.TakeDamage(damage))
        {
            Debug.Log($"{entry.UnitData.unitName}의 체력이 0이 되어 사망했습니다.");
            if (entry.UnitData.unitType == UnitType.EnemyBase)
            {
                GameManager.Instance.OnBaseDestroyed(entry.UnitData.teamType);
            }
            if (isCanGetGold && entry.UnitData.teamType == UnitTeamType.Enemy)
            {
                isCanGetGold = false;
                GoldManager.Instance.AddGold(entry.UnitData.Reward);
            }
            Die();            
        }
    }
    protected virtual void Die()
    {
        if (entry.UnitData.unitType != UnitType.EnemyBase) poolBase.ReturnPool();
    }

    public void Heal(float healPoint)
    {
        entry.Heal(healPoint);
    }

}
