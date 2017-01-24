using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SlowHand
{
    [ExecuteInEditMode]
    public class GameConstants : MonoBehaviour
    {
        public const string gameVersion = "1.0.0";
        public const string profileKey = "blockdash_profile";
        static GameConstants _instance;
        public static GameConstants Instance { get { return _instance; } }
        public Color32[] spikeColors;
        public Color32[] wallColors;
        public Color32[] bgColors;
        public Color32[] psColors;
        public Sprite[] blockBlue;
        public Sprite[] blockRed;
        public Sprite[] blockBlack;
        public int themeIndex;
        public int gemRescue = 20;

        public void InitRandomTheme()
        {
            themeIndex = Utils.Random(0, spikeColors.Length - 1);
            UpdateTheme();
        }
        public void InitNextTheme()
        {
            themeIndex++;
            if (themeIndex >= spikeColors.Length)
            {
                themeIndex = 0;
            }
            UpdateTheme();
        }
        public Color32 GetThemeColor(ThemeElementType te, int index)
        {
            switch (te)
            {
                case ThemeElementType.Spike:
                    return spikeColors[index];
                case ThemeElementType.Wall:
                    return wallColors[index];
                case ThemeElementType.BG:
                    return bgColors[index];
                case ThemeElementType.PS:
                    return psColors[index];
            }
            return Color.white;
        }
        public Color32[] GetThemeCheckPoint()
        {
            Color32[] _color = { bgColors[themeIndex], bgColors[themeIndex + 1] };
            return _color;
        }
        public Color32 GetThemeColor(ThemeElementType te)
        {
            switch (te)
            {
                case ThemeElementType.Spike:
                    return spikeColors[themeIndex];
                case ThemeElementType.Wall:
                    return wallColors[themeIndex];
                case ThemeElementType.BG:
                    return bgColors[themeIndex];
                case ThemeElementType.PS:
                    return psColors[themeIndex];
            }
            return Color.white;
        }
        
        public void UpdateTheme()
        {
            ThemeElement[] tes = FindObjectsOfType(typeof(ThemeElement)) as ThemeElement[];
            for (int i = 0; i < tes.Length; i++)
            {
                tes[i].UpdateTheme();
            }
        }        
        public Sprite getBlockBlue(int id)
        {
            return blockBlue[id];
        }
        public Sprite getBlockRed(int id)
        {
            return blockRed[id];
        }
        public Sprite getBlockBlack(int id)
        {
            return blockBlack[id];
        }
        void Awake()
        {
            _instance = this;
        }

        void OnEnable()
        {
            _instance = this;
        }

        public static class GOTags
        {
            public const string PSBackground = "PSBackground";
            public const string Spike = "Spike";
            public const string Wall = "Wall";
            public const string Coin = "Coin";
        }

        public static class ProfileTags
        {
            public const string Version = "Version";
            public const string MusicVolume = "MusicVolume";
            public const string SoundVolume = "SoundVolume";
            public const string HighScore = "HighScore";
            public const string Star = "Star";
            public const string Customize = "Customize";
            public const string CustomizeInfo = "CustomizeInfo";
            public const string EnableTutorial = "EnableTutorial";
            public const string Rate = "Rate";
        }
        // Balance game
        public int[] spikeNumberPerLevel;
        public float[] spike_yDistancePerLevel;
        public Vector3[] cammeraVelPerLevel;
        public int getSpikeNumperPerLever(int lv)
        {
            if (lv < spikeNumberPerLevel.Length)
                return spikeNumberPerLevel[lv];
            else
                return spikeNumberPerLevel[spikeNumberPerLevel.Length - 1];
        }
        public float getSpikeDistancePerLever(int lv)
        {
            if (lv < spike_yDistancePerLevel.Length)
                return spike_yDistancePerLevel[lv];
            else
                return spike_yDistancePerLevel[spike_yDistancePerLevel.Length - 1];
        }
        public Vector3 getCammeraVelPerLevel(int lv)
        {
            if (lv < cammeraVelPerLevel.Length)
                return cammeraVelPerLevel[lv];
            else
                return cammeraVelPerLevel[cammeraVelPerLevel.Length - 1];
        }
        //
        #region Spikes
        public static float checkPoint_yDistance = 15;
        public static float[][] spike_yDistanceMatrix = new float[][]
        {
								        /*L*//*R*//*Big*//*Small*//*LR*/
		    new float[] /*L*/	    { 8,     8,     8.5f,   7.5f,   7.5f },
            new float[] /*R*/	    { 8f,    8f,    8.5f,   7.5f,   7.5f},
            new float[] /*Big*/	    { 8.5f,  8.5f,  9f,     8f,     8f, },
            new float[] /*Small*/	{ 7.5f,  7.5f,  8f,     7f,     7f, },
            new float[] /*LR*/	    { 7.5f,  7.5f,  8f,     7f,     7f, },
        };

        public static int[][] spike_SpawnPercentMatrix = new int[][]
	    {
                                    /*L*//*R*//*Big*//*Small*//*LR*/
		    new int[] /*L*/	        { 4, 24, 24, 24, 24},	
		    new int[] /*R*/	        { 24, 4, 24, 24, 24},
            new int[] /*Big*/	    { 24, 24, 4, 24, 24},
            new int[] /*Small*/     { 24, 24, 24, 4, 24},
            new int[] /*LR*/	    { 24, 24, 24, 24, 4},
        };

        #endregion
    }
}

