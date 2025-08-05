using System;
using UnityEngine;

public class EquipUnarmed : Equip
{
    public float attackRate = 0.8f;
    public float attackDistance = 2f;
    public int damage = 1;
    public float useStamina = 5f;

    private bool attacking = false;
    private Animator animator;
    private Camera cam;

    private void Start()
    {
        //animator = GetComponentInParent<Animator>(); // ��� �ڽ��̶�� �θ𿡼� ��������
        //cam = Camera.main;
    }

    public override void OnAttackInput()
    {
        if (attacking) return;

        attacking = true;
        Invoke(nameof(OnCanAttack), attackRate);

        PerformAttack();
    }

    void PerformAttack()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0));
        if (Physics.Raycast(ray, out RaycastHit hit, attackDistance))
        {
            if (hit.collider.TryGetComponent(out IDamagable target))
            {
                target.TakePhysicalDamage(damage);
            }
        }
    }

    void OnCanAttack()
    {
        attacking = false;
    }

}
