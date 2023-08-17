using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEditor;
using TMPro;

public class PersistanceObject : MonoBehaviour
{
public static PersistanceObject Instance;

    [SerializeField] private TextMeshProUGUI debugNameText;
    [SerializeField] private TextMeshProUGUI playerNameText;
    public string playerName;
    public int score;

    //-------------------------------
    private void Awake()
    {
        // only need one instance of this
        if(Instance != null){
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // this keeps it around between scenes
        DontDestroyOnLoad(gameObject);

        // load the color data 
        LoadState();
    }

    //----------------------------------------

    public void SetName(){
        name = playerNameText.text;
        debugNameText.text = name;
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        PersistanceObject.Instance.SaveState();
        
        #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
        #else
            Application.quit();
        #endif
    }

    public void SaveState()
    {
        SaveData data = new SaveData();
        data.playerName = playerName;
        data.score = score;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json",json);
    }

    public void LoadState()
    {
        string path = Application.persistentDataPath + "/savefile.json";

        if(File.Exists(path)){
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            playerName = data.playerName;
            score = data.score;
        }
    }

    //---------------------------------

    [System.Serializable]
    class SaveData
    {
        public string playerName;
        public int score;
    }
}
