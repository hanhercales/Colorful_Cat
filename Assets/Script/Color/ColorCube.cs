using UnityEngine;
using UnityEngine.Tilemaps;

public class ColorCube : MonoBehaviour
{
    public Form form;
    public FormManager formManager;
    public TilemapRenderer tilemap;

    private void Start()
    {
        foreach (Form _form in formManager.formList)
        {
            if (_form.name == form.name)
            {
                ColorRestore();
                break;
            }
            else
            {
                tilemap.material.SetFloat("_GrayscaleAmount", 1f);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D player)
    {
        if (player.CompareTag("Player"))
        {
            formManager.AddForm(form);
            ColorRestore();
        }
    }

    private void ColorRestore()
    {
        tilemap.material.SetFloat("_GrayscaleAmount", 0f);
        Destroy(this.gameObject);
    }
}