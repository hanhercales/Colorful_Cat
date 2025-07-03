using UnityEngine;
using UnityEngine.Tilemaps;

public class ColorCube : MonoBehaviour
{
    public Form form;
    public FormManager formManager;
    public TilemapRenderer[] tilemaps;
    public GameObject enemies;

    private void Start()
    {
        enemies.SetActive(false);
        
        foreach (Form _form in formManager.formList)
        {
            if (_form.name == form.name)
            {
                ColorRestore();
                Destroy(this.gameObject);
                break;
            }
            else
            {
                DisColor();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D player)
    {
        if (player.CompareTag("Player"))
        {
            formManager.AddForm(form);
            enemies.SetActive(true);
            ColorRestore();
        }
    }

    private void ColorRestore()
    {
        foreach (TilemapRenderer tilemap in tilemaps)
        {
            tilemap.material.SetFloat("_GrayscaleAmount", 0f);
        }
    }

    public void DisColor()
    {
        foreach (TilemapRenderer tilemap in tilemaps)
        {
            tilemap.material.SetFloat("_GrayscaleAmount", 1f);
        }
    }
}