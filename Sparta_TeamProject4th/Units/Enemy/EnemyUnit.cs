using UnityEngine;

public class EnemyUnit : UnitBase
{
    ///UnitBase를 기반으로 적군 유닛과 관련된 동작을 다루는 스크립트입니다.
    ///사망시 골드 획득 / 아이템 획득(필요시) 등이 포함되어 있습니다.

    public int goldReward = 10; //획득하는 골드
    public GameObject[] dropItems; // 버프 아이템 프리팹들 - 아직 안 만듦

    protected override void Die()//죽을 경우 골드 획득/ 아이템 드랍을 실행
    {
        DropGold();
        TryDropItem();
        base.Die();
    }

    private void DropGold()
    {
        GoldManager.Instance.AddGold(goldReward);
    }

    private void TryDropItem()
    {
        if (dropItems.Length == 0) return;

        float dropChance = 0.3f; // 30% 확률로 드랍
        if (Random.value < dropChance)
        {
            int index = Random.Range(0, dropItems.Length);
            Instantiate(dropItems[index], transform.position, Quaternion.identity);
        }
    }
}
