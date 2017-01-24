using UnityEngine;
using System.Collections;

public class IOSDisable : MonoBehaviour {

    public GameObject[] _object;
	void Start () {
#if UNITY_IOS
        for (int i = 0; i < _object.Length; i++)
        {
            _object[i].SetActive(false);
        }
#endif
    }
}
