using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.Tilemaps;

public class HiddenWall : MonoBehaviour
{
    Tilemap Tilemap;
    Color currentColor;
    public LayerMask firePlayerLayer; // 퉁사후르레이어
    public LayerMask waterPlayerLayer;// 타사후르레이어

    private void Awake()
    {
        Tilemap = GetComponent<Tilemap>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if((firePlayerLayer.value & (1 << collision.gameObject.layer)) != 0 || (waterPlayerLayer.value & (1 << collision.gameObject.layer)) != 0)
        {
            Tilemap.color = new Color(1,1,1,0.2f);
        }
    }
}
