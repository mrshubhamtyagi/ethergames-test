using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class MainController : MonoBehaviour
{
    [SerializeField] private Vector3 floor1Position;

    [Header("UI")]
    public TMPro.TextMeshProUGUI testName;
    public TMPro.TextMeshProUGUI testDesciption;
    public TMPro.TextMeshProUGUI rowNumber;


    private string bundleUrl = "https://drive.google.com/uc?export=download&id=1zskzoBm-Lea_BIjkiCLoGtyKHDtTAXFG";
    //private string jsonUrl = "https://drive.google.com/uc?export=download&id=1mAycj5eJmd2g1QNswScO5pZm1wmC_Lp_";
    private string jsonUrl = "C:/Users/Tyanz/Desktop/MachineTest.json";

    private Ball ball;
    private ModelClass data = new ModelClass();
    [SerializeField] private Vector3 startingPosition;

    public static MainController Instance;
    private void Awake()
    {
        Instance = this;
        StartCoroutine(GetJson());
    }

    void Start()
    {
        StartCoroutine(GetAssetBundle());
    }


    private IEnumerator GetJson()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(jsonUrl))
        {
            //print(1);
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                data = DeserializeJson(www.downloadHandler.text);
                UpdateUI();
            }
        }

    }


    private IEnumerator GetAssetBundle()
    {
        using (UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(bundleUrl))
        {
            //print(2);
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                InstantiateBall(DownloadHandlerAssetBundle.GetContent(www));
            }
        }
    }



    private void InstantiateBall(AssetBundle _bundle)
    {
        print(_bundle.name);
        GameObject _prefab = (GameObject)Instantiate(_bundle.LoadAsset(_bundle.name), startingPosition, Quaternion.identity);
        ball = _prefab.GetComponent<Ball>();
        _bundle.Unload(false);
    }


    public void UpdateUI()
    {
        if (data != null)
        {
            testName.text = data.TestingAssetBundle[0].Name;
            testDesciption.text = data.TestingAssetBundle[0].Description;
            rowNumber.text = data.TestingAssetBundle[0].RowNumber.ToString();


            int _row = data.TestingAssetBundle[0].RowNumber > 0 ? data.TestingAssetBundle[0].RowNumber - 1 : 1;
            print("ROw " + _row);
            float _x = _row / 2 == 0 ? 1 : -1;
            float _y = floor1Position.y - (2f * _row);
            startingPosition = new Vector3(2 * _x, _y, 0);
            print(startingPosition);
        }
    }

    public void UpdateRowNumber(int _rn)
    {
        if (data != null)
        {
            if (data.TestingAssetBundle.Count > 0)
            {
                data.TestingAssetBundle[0].RowNumber = _rn;
                rowNumber.text = _rn.ToString();
            }
        }
    }


    public void Save()
    {
        print(0);
        SaveJson(data);
    }

    private void SaveJson(ModelClass _data)
    {
        print(Application.persistentDataPath + Path.PathSeparator + "test.json");
        File.WriteAllText(Application.persistentDataPath + Path.PathSeparator + "test.json", Newtonsoft.Json.JsonConvert.SerializeObject(_data));
    }


    private ModelClass DeserializeJson(string _json)
    {
        return Newtonsoft.Json.JsonConvert.DeserializeObject<ModelClass>(_json);
    }

}


[System.Serializable]
public class ModelClass
{
    public List<Data> TestingAssetBundle = new List<Data>();

    public class Data
    {
        public string Name;
        public string Description;
        public int RowNumber;
    }
}





