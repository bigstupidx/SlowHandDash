using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using System;

namespace SlowHand
{
    public class Profile
    {
        string version;
        float soundVolume;
        bool enableTutorial;
        int rate;
        int customize;
        int[] customizeInfo;
        int highScore;
        int star;

        public float SoundVolume { get { return soundVolume; } set { soundVolume = value; } }
        public int HighScore { get { return highScore; } }
        public int Star { get { return star; } }
        public int Customize { get { return customize; } set { customize = value; } }
        public int[] CustomizeInfo { get { return customizeInfo; } set { customizeInfo = value; } }
        public bool EnableTutorial { get { return enableTutorial; } set { enableTutorial = value; } }
        public int Rate{ get { return rate; } set { rate = value; } }

        public void updateHighScore(int newScore)
        {
            if (newScore > highScore)
            {
                highScore = newScore;
            }
        }
        public void updateStar(int _star)
        {
            star = Mathf.Clamp(star + _star, 0, Int32.MaxValue);
        }
        public JSONClass toJSON()
        {
            JSONClass jsClass = new JSONClass();
            jsClass.Add(GameConstants.ProfileTags.Version, version);
            jsClass.Add(GameConstants.ProfileTags.SoundVolume, soundVolume.ToString());
            jsClass.Add(GameConstants.ProfileTags.HighScore, highScore.ToString());
            jsClass.Add(GameConstants.ProfileTags.Star, star.ToString());
            jsClass.Add(GameConstants.ProfileTags.Customize, customize.ToString());
            jsClass.Add(GameConstants.ProfileTags.CustomizeInfo, Utils.ArrayIntToString(customizeInfo));
            jsClass.Add(GameConstants.ProfileTags.EnableTutorial, enableTutorial.ToString());
            jsClass.Add(GameConstants.ProfileTags.Rate, rate.ToString());
            return jsClass;
        }

        public Profile(JSONNode node)
        {
            version = node[GameConstants.ProfileTags.Version].Value;
            soundVolume = node[GameConstants.ProfileTags.SoundVolume].AsFloat;
            highScore = node[GameConstants.ProfileTags.HighScore].AsInt;
            star = node[GameConstants.ProfileTags.Star].AsInt;
            customize = node[GameConstants.ProfileTags.Customize].AsInt;
            customizeInfo = Utils.StringtoArrayInt(node[GameConstants.ProfileTags.CustomizeInfo].Value);
            enableTutorial = node[GameConstants.ProfileTags.EnableTutorial].AsBool;
            rate = node[GameConstants.ProfileTags.Rate].AsInt;
        }

        public Profile()
        {
        }

        public void reset()
        {
            version = GameConstants.gameVersion;
            soundVolume = 1.0f;
            enableTutorial = true;
            rate = 0;
            customize = 0;
            highScore = 0;
            star = 0;
            customizeInfo = new int[Utils.MAX_CUSTOMIZE];
            for (int i = 0; i < customizeInfo.Length; i++)
            {
                customizeInfo[i] = 0;
            }
            customizeInfo[0] = 1;
        }
        public void unLockCustomize(int _index)
        {
            customizeInfo[_index] = 1;
        }
        public int countUnlock()
        {
            int _count = 0;
            for (int i = 0; i < customizeInfo.Length; i++)
            {
                if (customizeInfo[i] == 1)
                {
                    _count++;
                }
            }
            return _count;
        }
    }

    public class GamePreferences : MonoBehaviour
    {
        static Profile _profile;
        public static Profile initProfile()
        {
            SavedProfile tmpProfile = SaveGameManager.loadDataByUser<SavedProfile>(GameConstants.profileKey);
            if (tmpProfile == null || String.IsNullOrEmpty(tmpProfile.Profile))
            {
                _profile = new Profile();
                _profile.reset();
            }
            else
            {
                JSONNode js = JSON.Parse(tmpProfile.Profile);
                _profile = new Profile(js);
            }
            saveProfile();
            return _profile;
        }
        public static Profile profile { get { return _profile; } }

        public static void saveProfile()
        {
            string data = _profile.toJSON().ToString();
            SavedProfile tmpProfile = new SavedProfile(data);
            SaveGameManager.saveDataByUser<SavedProfile>(GameConstants.profileKey, tmpProfile);
        }      

        public static void submitScore(int _score)
        {

        }
        static void SubmitScoreCallback(bool success)
        {
        }

        class SavedProfile
        {
            public string Profile;
            public SavedProfile()
            {
            }
            public SavedProfile(string paramProfile)
            {
                Profile = paramProfile;
            }
        }
    }
}
