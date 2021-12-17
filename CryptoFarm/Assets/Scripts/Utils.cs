using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils
{
    private static string[] Suffixes = new[] {"K", "M", "B", "t", "q", "Q", "s", "S", "o", "n", "d", "U", "D", "T", "Qt", "Qd", "Sd", "St", "O", "N", "v", "c"};
    private static string[] SuffixesFull = new[] { "Thousand", "Million", "Billion", "Trillion", "Quadrillion", "Quintillion", "Sextillion", "Septillion",
        "Octillion", "Nonillion", "Decillion", "Undecillion", "Duodecillion", "Tredecillion", "Quattuordecillion", "Quindecillion", "Sexdecillion", 
        "Septendecillion", "Octodecillion", "Novemdecillion", "Vigintillion", "Unvigintillion" };

    public static string MoneyToString(double money, bool fullString = false)
    {
        var digitCount = Math.Floor(Math.Log10(Math.Abs(money)));// digit count - 1
        if (money == 0)
            return "0";
        /*if (Math.Abs(money) < 1)
        {
            
            var floatingPointCount = Math.Abs(digitCount);//+1;
            var format = "N" + floatingPointCount;
            return money.ToString(format);
        }*/
        if (Math.Abs(money) < 100)
            return money.ToString("0.##");
        if (Math.Abs(money) < 10000)
            return money.ToString("N0");

        
        
        int suffixIndex = (int)(Math.Floor(digitCount / 3));//index of suffix from suffix array
        var tempMoney = (money / Math.Pow(10, (suffixIndex * 3)));
        
        var suffix = !fullString ? Suffixes[suffixIndex - 1] : SuffixesFull[suffixIndex - 1];

        return tempMoney.ToString("N2") + " " + suffix;//N2 0.##
    }
}
