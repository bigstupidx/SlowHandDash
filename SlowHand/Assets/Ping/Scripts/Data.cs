using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Data : MonoBehaviour {

    static Data _instance;
    public static Data Instance
    {
        get
        {
            return _instance;
        }
    }
    public Dictionary<int, DataCustomize> dataCustomize;

	void Start () {
        DontDestroyOnLoad(this);
        _instance = this;
        loadCustomizeData();
    }
    void loadCustomizeData()
    {
        string filepath = ResourceDataTags.customize;

        IEnumerable<DataCustomize> dataListFromCSV = Utils.LoadCSVDataFromFile<DataCustomize>(filepath);
        dataCustomize = new Dictionary<int, DataCustomize>();

        foreach (DataCustomize datafromCSV in dataListFromCSV)
        {
            dataCustomize.Add(datafromCSV.id, datafromCSV);
        }
    }
    public DataCustomize GetCustomize(int _id)
    {
        if (dataCustomize != null && dataCustomize.Count > _id)
        {
            return dataCustomize[_id];
        }
        else
        {
            return null;
        }
    }
}
