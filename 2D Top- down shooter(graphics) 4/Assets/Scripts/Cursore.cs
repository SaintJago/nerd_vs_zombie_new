﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursore : MonoBehaviour
{
    private void Start()
    {
        Cursor.visible = false;
    }

    private void Update()
    {
        Vector2 cPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = cPos;
        Cursor.visible = false;
    }
}
