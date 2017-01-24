using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using CsvFiles;

public class Utils
{
#if UNITY_IOS
    public const string LINK_APP_STORE = "https://itunes.apple.com/us/app/block-dash-alien-rush/id1104150658?ls=1&mt=8";
#elif UNITY_ANDROID
    public const string LINK_APP_STORE = "https://play.google.com/store/apps/details?id=com.fuky.blockdash";
#else
    public const string LINK_APP_STORE = "https://play.google.com/store/apps/details?id=com.fuky.blockdash";
#endif
    public const string LINK_ANDROID_STORE = "https://play.google.com/store/apps/details?id=com.fuky.blockdash";
    public const string LINK_IOS_STORE = "https://itunes.apple.com/us/app/block-dash-alien-rush/id1104150658?ls=1&mt=8";

    public const int MAX_CUSTOMIZE = 6;

    public static string ConvertToMSM(float val)
    {
        int sec = Mathf.FloorToInt(val);
        int mic = Mathf.FloorToInt((val - sec) * 100);
        //int min = sec / 60;
        sec = sec % 60;
        return String.Format("{0:00}.{1:00}", sec, mic);
    }

    public static int ConvertToScore(float val)
    {
        int sec = Mathf.FloorToInt(val);
        int mic = Mathf.FloorToInt((val - sec) * 100);
        int min = sec / 60;
        sec = sec % 60;
        return (min * 60 * 100 + sec * 100 + mic);
    }

    public static int ColorToInt(Color32 clr)
    {
        return (clr.a << 24 | clr.r << 16 | clr.g << 8 | clr.b);
    }
    public static Color32 IntToColor(int clr)
    {
        int a = clr >> 24;
        int r = (clr & 0x00ff0000) >> 16;
        int g = (clr & 0x0000ff00) >> 8;
        int b = clr & 0x000000ff;
        return new Color32((byte)r, (byte)g, (byte)b, (byte)a);
    }
    //
    public static string ArrayIntToString(int[] A)
    {
        string s = "";
        if (A != null)
        {
            for (int i = 0; i < A.Length; i++)
            {
                s += A[i] + ",";
            }
        }
        return s;
    }
    public static int[] StringtoArrayInt(string s)
    {
        string[] array = s.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
        int[] A = new int[array.Length];
        for (int i = 0; i < array.Length; i++)
        {
            A[i] = Int32.Parse(array[i]);
        }
        return A;
    }
    //
    public static void removeAllChildren(Transform paramParent, bool paramInstant=true)
    {
        if (paramParent == null)
            return;
        for (int i = paramParent.childCount - 1; i >= 0; i--)
        {
            if (paramInstant)
            {
                GameObject.DestroyImmediate(paramParent.GetChild(i).gameObject);
            } else
            {
                paramParent.GetChild(i).gameObject.SetActive(false);
                GameObject.Destroy(paramParent.GetChild(i).gameObject);
            }
        }
    }
    public static int Random(int paramMin, int paramMax)
    {
        return UnityEngine.Random.Range(paramMin, paramMax);
    }
    public static void DebugLog(string paramLog)
    {
        Debug.Log(paramLog);
    }
    public static void DebugLogError(string paramLog)
    {
        Debug.LogError(paramLog);
    }

    public static Sprite loadResourcesSprite(string param)
    {
        return Resources.Load<Sprite>("" + param);
    }
    public static T[] CloneArray<T>(T[] paramArray)
    {
        if (paramArray == null)
            return null;
        return paramArray.Clone() as T[];
    }
    public static List<T> CloneArray<T>(List<T> paramArray)
    {
        if (paramArray == null)
            return null;
        List<T> list = new List<T>(paramArray.ToArray());
        return list;
    }

    public static IEnumerable<T> LoadCSVDataFromBytes<T>(byte[] bytes) where T : new()
    {
        if (bytes == null || bytes.Length == 0)
            return null;
        Stream stream = new MemoryStream(bytes);
        StreamReader streamReader = new StreamReader(stream);
        return CsvFile.Read<T>(streamReader);
    }

    public static IEnumerable<T> LoadCSVDataFromFile<T>(string resourcePath) where T : new()
    {
        TextAsset txtData = (TextAsset)Resources.Load(resourcePath);
        if (txtData == null)
            return null;

        Stream stream = new MemoryStream(txtData.bytes);
        StreamReader streamReader = new StreamReader(stream);
        return CsvFile.Read<T>(streamReader);
    }

    public static GameObject Spawn(GameObject paramPrefab, Transform paramParent = null)
    {
        GameObject newObject = GameObject.Instantiate(paramPrefab) as GameObject;
        newObject.transform.SetParent(paramParent);
        newObject.transform.localPosition = paramPrefab.transform.localPosition;
        newObject.transform.localScale = paramPrefab.transform.localScale;
        newObject.SetActive(true);
        return newObject;
    }
    public static string removeExtension(string paramPath)
    {
        int index = paramPath.LastIndexOf('.');
        if (index < 0)
            return paramPath;
        return paramPath.Substring(0, index);
    }
    public static void setActive(GameObject paramObject, bool paramValue)
    {
        if (paramObject != null)
            paramObject.SetActive(paramValue);
    }
    public static string getFileName(string paramFilePath)
    {
        string fileName = paramFilePath;
        int index = paramFilePath.LastIndexOfAny(new char[] { '/', '\\' });
        if (index >= 0)
        {
            fileName = paramFilePath.Substring(index + 1);
        }
        return fileName;
    }

    public static string getLocalUIPath(string paramPictureURL)
    {
        string pictureName = getFileName(paramPictureURL);

        return "UI/" + removeExtension(pictureName);
    }

    public static string jsonToBase64(string paramJson)
    {
        try
        {
            SimpleJSON.JSONNode node = SimpleJSON.JSON.Parse(paramJson);
            return node.SaveToBase64();
        } catch (Exception ex)
        {
            DebugLogError(ex.Message);
            return paramJson;
        }
    }

    public static string base64ToJson(string paramBase64)
    {
        try
        {
            SimpleJSON.JSONNode node = SimpleJSON.JSONNode.LoadFromBase64(paramBase64);
            return node.ToString();
        } catch (Exception ex)
        {
            DebugLogError(ex.Message);
            return paramBase64;
        }
    }

    public static int getUnixTimeNow()
    {
        TimeSpan timeSpan = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
        return (int)timeSpan.TotalSeconds;
    }

    public static bool isURL(string paramURL)
    {
        if (string.IsNullOrEmpty(paramURL))
            return false;
        return (paramURL.IndexOf("http") >= 0);
    }

    public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
        dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
        return dtDateTime;
    }

    public static string LoadJsonFromFile(string filePath)
    {
        try
        {
            string json;
            using (StreamReader r = new StreamReader(filePath))
            {
                json = r.ReadToEnd();
            }
            return json;
        } catch (Exception error)
        {
            Debug.Log("Read Json File Error: " + error);
            return null;
        }
    }

    public static  string Md5Sum(string strToEncrypt)
    {
        System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
        byte[] bytes = ue.GetBytes(strToEncrypt);
        
        // encrypt bytes
        System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] hashBytes = md5.ComputeHash(bytes);
        
        // Convert the encrypted bytes back to a string (base 16)
        string hashString = "";
        
        for (int i = 0; i < hashBytes.Length; i++)
        {
            hashString += System.Convert.ToString(hashBytes [i], 16).PadLeft(2, '0');
        }
        
        return hashString.PadLeft(32, '0');
    }
    public static int RandomArray(int[] array)
    {
        int rand = Random(0, 100);
        int count = 0;
        for (int i = 0; i < array.Length; i++)
        {
            count += array[i];
            if (rand <= count)
            {
                return i;
            }
        }
        return array.Length - 1;
    }
}

public class EnumParser
{
    public static T Parse<T>(string value)
    {
        return (T)Enum.Parse(typeof(T), value, true);
    }
}
