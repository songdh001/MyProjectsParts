using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveWall : MonoBehaviour, IActivatable
{
    public GameObject wallImage;

    // Start is called before the first frame update
    public void Activate()
    {
        wallImage.SetActive(false); // 벽이 사라짐
    }

    // Update is called once per frame
    public void Deactivate()
    {
        wallImage.SetActive(true); // 벽이 다시 생김
    }
}
