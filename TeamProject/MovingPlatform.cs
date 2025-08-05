using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour, IActivatable
{

    public Transform startPoint;  // ���� ����
    public Transform endPoint;    // ���� ����
    public float moveSpeed = 2f;  // �����̴� �ӵ�

    private bool isTriggered = false; // ����/����ġ üũ

    private Vector3 targetPosition;  //���� �÷����� �����̷��� ��ġ


    [Header("�պ�� ����")]
    public bool repeatMoving = true; // �����Ϳ��� ���� ����

    [Header("����ġ ���� ����")]
    public bool useSwitchControl = true; // �����Ϳ��� ���� ����


    public void Activate()
    {
        isTriggered = true;
    }


    public void Deactivate()
    {
        if (!repeatMoving)
        {
            targetPosition = targetPosition == startPoint.position ? endPoint.position : startPoint.position;
        }
        isTriggered = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        targetPosition = endPoint.position; // ó������ ���� ����Ʈ�� �̵� ���߿� ��ŸƮ ����Ʈ�� �ٲ� ����
                                            // �Դٰ��� �� �� �ְ�

        if (!useSwitchControl) //���� ����ġ�� �������� �ʴ´ٸ� �ڵ����� Ʈ��� �����ϵ��� ����
            isTriggered = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isTriggered) return; //����ġ ������ Ȱ��ȭ �Ǿ�� �̵�

        // ���� �������� �̵�
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // ��ǥ ������ ���� ���������� ���� ����
        // (Ÿ�� ��ġ�� ��ŸƮ ����Ʈ�� �ٲ��ְ� �ƴϸ� ��ŸƮ ����Ʈ�� ������ ����)

        if (repeatMoving)
        {
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                targetPosition = targetPosition == startPoint.position ? endPoint.position : startPoint.position;
            }
        }

    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // �÷��̾ ���� �̵��ϰ� �ڽ����� ����
            // (�¿�� �̵��ص� �÷��̾ ����� �� �ֵ���)
            other.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // ������ �� �θ� ����
            other.transform.SetParent(null);
        }
    }

}
