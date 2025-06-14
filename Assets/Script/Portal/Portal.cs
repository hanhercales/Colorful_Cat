using UnityEngine;

public class Portal : MonoBehaviour
{ 
    public string sceneName;
    
    [SerializeField] private FormManager formManager;
    
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
}
