using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using RandomGenerationFramework;

namespace SlowHand
{
    [System.Serializable]
    public class MultiDimensionalValue
    {
        public int[] values;
    }
    public class ProportionLevel
    {
        PrioritisedListRandomGenerator<int> pg2;
        public ProportionLevel(MultiDimensionalValue arr)
        {

            Dictionary<int, int> dict = new Dictionary<int, int>();
            for (int i = 0; i < arr.values.Length; i++)
            {
                dict.Add(i, arr.values[i]);
            }
            pg2 = new PrioritisedListRandomGenerator<int>(dict);
        }
        public ProportionLevel(int[] arr)
        {

            Dictionary<int, int> dict = new Dictionary<int, int>();
            for (int i = 0; i < arr.Length; i++)
            {
                dict.Add(i, arr[i]);
            }
            pg2 = new PrioritisedListRandomGenerator<int>(dict);
        }
        public int GenValue()
        {
            return pg2.GetRandom();
        }
    }
    public class SpikeGenerator : MonoBehaviour
    {
        int[] getNextSpikePercents(int currentSpike, int currentLevel)
        {
            return GameConstants.spike_SpawnPercentMatrix[currentSpike];
        }
        public int GenSpike(int level, int lastSpikeType)
        {
            int[] percents = getNextSpikePercents(lastSpikeType, level);
            return Utils.RandomArray(percents);
        }

        public float getYDistance(int currentSpikeType, int nextSpikeType, int currentLevel)
        {
            float yy = GameConstants.spike_yDistanceMatrix[0][0];
            if (currentSpikeType < (int)ESpikeType.TotalCount
               && nextSpikeType < (int)ESpikeType.TotalCount)
            {
                yy = GameConstants.spike_yDistanceMatrix[currentSpikeType][nextSpikeType];
            }
            float bonus = GameConstants.Instance.getSpikeDistancePerLever(currentLevel);
            return (yy + bonus);
        }
    }
}