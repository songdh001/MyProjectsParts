using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveWall : MonoBehaviour, IActivatable
{
    public GameObject wallImage;

    // Start is called before the first frame update
    public void Activate()
    {
        wallImage.SetActive(false); // ���� �����
    }

    // Update is called once per frame
    public void Deactivate()
    {
        wallImage.SetActive(true); // ���� �ٽ� ����
    }
}
