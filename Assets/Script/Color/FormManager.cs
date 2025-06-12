using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FormManager : MonoBehaviour
{
    [System.Serializable]
    private class FormData
    {
        public List<string> formName = new List<string>();
    }
    
    public List<Form> formList = new List<Form>();
    public Animator targetAnimator;
    public string collectedForms = "CollectedForms";
    public Form currentForm;

    [SerializeField] private FormsIconsDisplay formsIconsDisplay;
    [SerializeField] private GameObject[] formDisplaySlot;
    [SerializeField] private string formResourcesPath = "FormSO";
    [SerializeField] private Form defaultForm;
    
    private Dictionary<string, Form> _formsDict = new Dictionary<string, Form>();

    private void Awake()
    {
        LoadAllAvailableFormsFromResources();

        LoadForms();
    }

    private void Start()
    {
        if(!PlayerPrefs.HasKey(collectedForms))
        {
            Debug.Log("Not found any save");
            InitNewFormList();
        }
        
        ChangeForm(0);
    }

    public void InitNewFormList()
    {
        formList.Clear();
        AddForm(defaultForm);
        currentForm = defaultForm;
    }

    public void AddForm(Form form)
    {
        formList.Add(form);
        formsIconsDisplay.UpdateDisplayedForms();
    }

    public void ChangeForm(int formIndex)
    {
        targetAnimator.runtimeAnimatorController = formList[formIndex].animatorController;
        currentForm = formList[formIndex];
    }

    public void SaveForms()
    {
        FormData data = new FormData();
        
        data.formName = formList.Select(form => form.name).ToList();
        Debug.Log(data.formName);
        
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(collectedForms, json);
        PlayerPrefs.Save();
        Debug.Log(PlayerPrefs.GetString(collectedForms));
    }

    public void LoadForms()
    {
        formList.Clear();
        
        if (PlayerPrefs.HasKey(collectedForms))
        {
            string json = PlayerPrefs.GetString(collectedForms);
            FormData data = JsonUtility.FromJson<FormData>(json);

            if (data != null && data.formName != null)
            {
                foreach (string formName in data.formName)
                {
                    if (_formsDict.TryGetValue(formName, out Form form))
                    {
                        formList.Add(form);
                    }
                }
            }
        }
    }

    private void LoadAllAvailableFormsFromResources()
    {
        _formsDict.Clear();
        
        Form[] forms = Resources.LoadAll<Form>(formResourcesPath);
        
        if(forms.Length == 0) return;

        foreach (Form form in forms)
        {
            if(form == null) continue;

            if (!_formsDict.ContainsKey(form.name))
            {
                _formsDict.Add(form.name, form);
            }
        }
    }
    
    public void ResetForms()
    {
        PlayerPrefs.DeleteKey(collectedForms);
        PlayerPrefs.Save();
        InitNewFormList();
        Debug.Log("All collected forms data reset.");
    }

    private void OnApplicationQuit()
    {
        SaveForms();
    }
}
