using UnityEngine;
using UnityEditor;

namespace SlowHand
{
    public class SplitItMenu : MonoBehaviour
    {
        [MenuItem("Ping/Delete Saved files")]
        public static void ClearPlayerPrefs()
        {
            PlayerPrefs.DeleteKey(GameConstants.profileKey);
        }
    }
}
