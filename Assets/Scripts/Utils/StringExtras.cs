using System;
using UnityEngine;

namespace Utils
{
    public static class StringExtras
    {
        private static readonly string[] ScoreNames = { "", "k", "M", "B", "T", "aa", "ab", "ac", "ad", "ae", "af", "ag", "ah", "ai", "aj", "ak", "al", "am", "an", "ao", "ap", "aq", "ar", "as", "at", "au", "av", "aw", "ax", "ay", "az", "ba", "bb", "bc", "bd", "be", "bf", "bg", "bh", "bi", "bj", "bk", "bl", "bm", "bn", "bo", "bp", "bq", "br", "bs", "bt", "bu", "bv", "bw", "bx", "by", "bz", };

        public const int FormatBound = 900;

        public static string FormatAmount(float amount)
        {
            int i;
            for (i = 0; i < ScoreNames.Length; i++)
            {
                if (amount < FormatBound) break;
                amount = Mathf.Floor(amount / 100f) / 10f;
            }

            if (Math.Abs(amount - Math.Floor(amount)) < 1)
            {
                return amount + ScoreNames[i];
            }
            return amount.ToString("F1") + ScoreNames[i];
        }
    }
}