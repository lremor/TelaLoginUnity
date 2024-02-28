using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Firebase.Database;
using TMPro;

[Serializable]
public class DataToSave
{
    public int waypoint;
}
public class DataToCreate
{
    public string users;
}
public class DataSaver : MonoBehaviour
{
    public DataToSave dts;
    public DataToCreate dtc;
    DatabaseReference dbRef;
    public TMP_InputField WaypointField;
    public TMP_Text waypointText;

    private void Awake()
    {
        dbRef = FirebaseDatabase.DefaultInstance.RootReference;
        if (dbRef == null)
        {
            Debug.Log("is null");
        }
    }

    public void RemoveDataFn()
    {
        string json = JsonUtility.ToJson(dts);
        dbRef.Child("users").Child("waypoint").RemoveValueAsync();

        if (dts != null)
        {
            waypointText.text = "Users Removido!";
        }
    }

    public void AddDataFn()
    {
        dtc = new DataToCreate
        {
            users = "users"

        };

        string json = JsonUtility.ToJson(dtc);
        dbRef.Child("users").Child("waypoint").SetValueAsync(json);
        
        if (dtc != null)
        {
            // Debug.Log(dts.waypoint);
            waypointText.text = "Adicionado: " + dtc.users.ToString();
        }
    }

    public void SaveDataFn()
    {
        if (WaypointField.text == "") 
        {
            WaypointField.text = "0";
        }
        dts = new DataToSave
        {
            waypoint = int.Parse(WaypointField.text)
            
        };

        string json = JsonUtility.ToJson(dts);
        dbRef.Child("users").SetRawJsonValueAsync(json);

        if (dts != null)
        {
            // Debug.Log(dts.waypoint);
            waypointText.text = "Escrito: " + dts.waypoint.ToString();
        }
    }
 
    public void LoadDataFn()
    {
        StartCoroutine(LoadDataEnum());
    }

    IEnumerator LoadDataEnum()
    {
        var serverData = dbRef.Child("users").GetValueAsync();
        yield return new WaitUntil(predicate: () => serverData.IsCompleted);

        print("process is complete");

        DataSnapshot snapshot = serverData.Result;
        string jsonData = snapshot.GetRawJsonValue();

        if (jsonData != null)
        {
            print("server data found");

            dts = JsonUtility.FromJson<DataToSave>(jsonData);
            
            if (dts != null)
            {
                // Debug.Log(dts.waypoint);
                waypointText.text = "Lido: " + dts.waypoint.ToString();
            }
        }
        else
        {
            print("no data found");
        }

    }
}