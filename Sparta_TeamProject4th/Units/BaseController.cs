using System;
using UnityEngine;

public class BaseController : MonoBehaviour
{
    /// <summary>
    /// 골드를 사용하려 할 때 호출합니다. 골드 소모를 시도하고 골드가 충분할 경우 소모하고 true를 반환합니다.
    /// 아니면 골드를 소모하지 않은 채 false를 반환합니다.
    /// </summary>

    ///적을 쓰려뜨릴 때 획득하는 골드(EnemyUnit에 있음)
    ///유닛을 업그레이드할 때 사용할 골드 등
    ///골드 획득/ 사용시 이용할 골드 매니저입니다.
    private int gold = 0; //테스트용 돈
    private int maxGold = 9999;

    public event Action OnGoldChange;// UI에도 업데이트를 여기에 이번트 연결

    /// <summary>
    /// 증감을 전버 여기서 관리;
    /// </summary>
    public void ChangeGold(int amount)
    {
        gold += amount;
        gold = Math.Min(gold, maxGold);
        OnGoldChange?.Invoke();        
    }


    public bool SpendGold(int amount)
    {
        if (gold >= amount)
        {
            ChangeGold(amount);
            Debug.Log($"Gold를 사용했습니다. 현재 골드 : {gold}");
            return true;
        }
        Debug.Log($"돈이 없어용. 현재 골드 : {gold}");
        return false;
    }


    //외부에서 돈 얼마 있는지 확인하려면 GetGold()이거 쓰시면 됩니다.
    // if (GoldManager.Instance.GetGold() >= unitData.cost) 이런 식으로 쓰시면 됩니다.
    public int GetGold() => gold;

}
