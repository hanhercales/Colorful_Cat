using System;
using UnityEngine;

public class Portal : MonoBehaviour
{ 
    public string sceneName;
    public string formName;
    
    [SerializeField] private FormManager formManager;

    private void Start()
    {
        if(formManager.formList.Count == 8)
            this.gameObject.SetActive(true);
        
        foreach (Form form in formManager.formList)
        {
            if(form.originalName == formName)
                this.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (string.IsNullOrEmpty(sceneName))
            {
                Debug.LogError("Scene name is empty");
                return;
            }
            Debug.Log("Portal activated.");
            formManager.SaveForms();
            GameManager.Instance.LoadScene(sceneName);
        }
    }

    public void ChangeScene()
    {
        Debug.Log(sceneName);
        GameManager.Instance.LoadScene(sceneName);
    }
}
