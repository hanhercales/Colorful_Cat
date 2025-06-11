using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ColorManager : MonoBehaviour
{
    public TilemapRenderer tilemapRenderer;
    private ColorCube colorCube;

    private void Start()
    {
        colorCube = GameObject.FindGameObjectWithTag("ColorCube").GetComponent<ColorCube>();
    }

    private void Update()
    {
        if(colorCube != null)
            tilemapRenderer.material.SetFloat("_GrayscaleAmount", 1f);
        else
            tilemapRenderer.material.SetFloat("_GrayscaleAmount", 0f);
    }
}
