using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;


public class DavMaster : MonoBehaviour
{

    public static Texture2D RotateTexture2D(Texture2D originalTexture, bool clockwise)
    {
        Color32[] original = originalTexture.GetPixels32();
        Color32[] rotated = new Color32[original.Length];
        int w = originalTexture.width;
        int h = originalTexture.height;

        int iRotated, iOriginal;

        for (int j = 0; j < h; ++j)
        {
            for (int i = 0; i < w; ++i)
            {
                iRotated = (i + 1) * h - j - 1;
                iOriginal = clockwise ? original.Length - 1 - (j * w + i) : j * w + i;
                rotated[iRotated] = original[iOriginal];
            }
        }

        Texture2D rotatedTexture = new Texture2D(h, w);
        rotatedTexture.SetPixels32(rotated);
        rotatedTexture.Apply();
        return rotatedTexture;
    }

    public static Texture2D FlipTexture2D(Texture2D originalTexture)
    {
        Texture2D flipped = new Texture2D(originalTexture.width, originalTexture.height);
        int xN = originalTexture.width;
        int yN = originalTexture.height;


        for (int i = 0; i < xN; i++)
        {
            for (int j = 0; j < yN; j++)
            {
                flipped.SetPixel(xN - i - 1, j, originalTexture.GetPixel(i, j));
            }
        }
        flipped.Apply();

        return flipped;
    }

    public static string SpriteToString(Sprite sprite)
    {
        return Texture2DToString(sprite.texture);
    }

    public static string Texture2DToString(Texture2D texture)
    {
        Texture2D tex = new Texture2D(texture.width, texture.height);
        tex = texture;
        byte[] bArray = tex.EncodeToPNG();
        string code = Convert.ToBase64String(bArray);
        return code;
    }

    public static Sprite StringToSprite(string codee)
    {
        return  Texture2DToSprite(StringToTexture2D(codee));
    }

    public static Texture2D StringToTexture2D(string code)
    {
        byte[] bArray = Convert.FromBase64String(code);
        return ByteArrayToTexture2D(bArray);
    }

    static Texture2D ByteArrayToTexture2D(byte[] theArray)
    {
        Texture2D tex = new Texture2D(800, 600, TextureFormat.RGBA32, false);
        tex.LoadImage(theArray);
        tex.Apply();
        return tex;
    }

    public static Texture2D StringToTexture2D(string code, Vector2 textureSize)
    {
        byte[] bArray = Convert.FromBase64String(code);
        return ByteArrayToTexture2D(bArray, textureSize);
    }

    static Texture2D ByteArrayToTexture2D(byte[] theArray, Vector2 size)
    {
        Texture2D tex = new Texture2D((int)size.x, (int)size.y, TextureFormat.RGBA32, false);
        tex.LoadImage(theArray);
        tex.Apply();
        return tex;
    }

    public static string[] LongStringToShortStrings(string longString, int shortStringLength)
    {
        int parts = longString.Length / shortStringLength;
        int rem = 0;
        if (longString.Length % shortStringLength != 0)
        {
            rem = longString.Length % shortStringLength;
            parts++;
        }
        string[] shortStrings = new string[parts];
        if (parts > 0)
        {
            for (int i = 1; i <= parts; i++)
            {
                if (i != parts)
                {
                    shortStrings[i - 1] = longString.Substring((i - 1) * shortStringLength, shortStringLength);
                }
                else
                {
                    shortStrings[i - 1] = longString.Substring((i - 1) * shortStringLength, rem);
                }
            }
        }
        else
        {
            shortStrings[0] = longString.Substring(0, rem);
        }
        return shortStrings;
    }

    public static string ConcatenateStrings(string[] shortStrings)
    {
        int parts = shortStrings.Length;
        string realCode = "";
        for (int i = 1; i <= parts; i++)
        {
            realCode += shortStrings[i - 1];
        }
        return realCode;
    }

    public static void CopyRectTransform(RectTransform from, RectTransform to)
    {
        to.anchorMax = from.anchorMax;
        to.anchorMin = from.anchorMin;
        to.pivot = from.pivot;
        to.anchoredPosition = from.anchoredPosition;
        to.sizeDelta = from.sizeDelta;
    }

    public static Vector2 SameRatioCustomPercentage(Vector2 originalSize, float requiredSizePercentage)
    {
        Vector2 temp;
        temp = new Vector2((requiredSizePercentage / 100) * originalSize.x, (requiredSizePercentage / 100) * originalSize.y);
        return temp;
    }

    public static Vector2 SameRatioCustomParameter(Vector2 originalSize, float requiredSize, bool isProvidedWidth)
    {
        Vector2 temp = Vector2.zero;
        if (isProvidedWidth)
        {
            float percentage = requiredSize / originalSize.x * 100;
            temp = new Vector2(requiredSize, percentage / 100 * originalSize.y);
        }
        else
        {
            float percentage = requiredSize / originalSize.y * 100;
            temp = new Vector2(percentage / 100 * originalSize.x, requiredSize);
        }
        return temp;
    }

    public static IEnumerator PopupWithInfo(string info, TextMeshProUGUI infoObject, GameObject popupObject, float popupTime)
    {
        infoObject.text = info;
        popupObject.SetActive(true);
        yield return new WaitForSecondsRealtime(popupTime);
        popupObject.SetActive(false);
        
    }

    public static IEnumerator PopupWithInfo(string info, Text infoObject, GameObject popupObject, float popupTime)
    {
        infoObject.text = info;
        popupObject.SetActive(true);
        yield return new WaitForSecondsRealtime(popupTime);
        popupObject.SetActive(false);
    }

    public static IEnumerator Popup(GameObject popupObject, float popupTime)
    {
        popupObject.SetActive(true);
        yield return new WaitForSecondsRealtime(popupTime);
        popupObject.SetActive(false);
    }

    public static IEnumerator EnableDisableWithDelay(GameObject target, float time, bool state)
    {
        yield return new WaitForSecondsRealtime(time);
        target.SetActive(state);
    }
    
    public static string GetTodayDate(char formatSymbol)
    {
        string month, day, year;
        DateTime datetime = DateTime.Now;
        month = datetime.Month.ToString();
        day = datetime.Day.ToString();
        year = datetime.Year.ToString();
        if (month.Length == 1)
        {
            month = "0" + month;
        }
        if (day.Length == 1)
        {
            day = "0" + day;
        }
        string date = year + formatSymbol + month + formatSymbol + day;
        return date;
    }

    public static string GetCurrentTime(char formatSymbol)
    {
        string hour, minutes, seconds;
        DateTime datetime = DateTime.Now;
        hour = datetime.Hour.ToString();
        minutes = datetime.Minute.ToString();
        seconds = datetime.Second.ToString();
        if (hour.Length == 1) hour = "0" + hour;
        if (minutes.Length == 1) minutes = "0" + minutes;
        if (seconds.Length == 1) seconds = "0" + seconds;
        string time = hour + formatSymbol + minutes + formatSymbol + seconds;
        return time;
    }

    public static int GetCurrentTimeInSeconds(string currentTime, char formatSymbol)
    {
        string[] currentTimeArray = currentTime.Split(formatSymbol);
        int timeInSeconds;
        timeInSeconds = (int.Parse(currentTimeArray[0]) * 3600) + (int.Parse(currentTimeArray[1]) * 60) + int.Parse(currentTimeArray[2]);
        return timeInSeconds;
    }

    Hashtable daysInMonth = new Hashtable()
    {
         {"1","31"},  {"2","28"}, {"3","31"}, {"4","30"}, {"5","31"}, {"6","30"},
          {"7","31"}, {"8","31"}, {"9","30"}, {"10","31"}, {"11","30"}, {"12","31"},
    };

    public int CheckDayOfYear(int date, int month, int year)
    {
        bool isLeapYear = false;
        int day = 0;
        if (year % 4 == 0)
        {
            isLeapYear = true;
        }
        for (int i = 1; i < month; i++)
        {
            day += int.Parse(daysInMonth[i.ToString()].ToString());
            if (isLeapYear && i == 2)
            {
                day += 1;
            }
        }
        day += date;
        return day;
    }

    public static bool IsLessThanTwentyFourHours(string requiredDate, string currentDate, string currentTimee, string startTimee)
    {
        string[] reqDate = new string[3];
        string[] curDate = new string[3];
        string[] startTime = startTimee.Split(':');
        string[] curTime = currentTimee.Split(':');
        reqDate = requiredDate.Split('-');
        curDate = currentDate.Split('-');
        if (int.Parse(reqDate[0]) < int.Parse(curDate[0]))
        {
            return false;
        }
        else if (int.Parse(reqDate[0]) == int.Parse(curDate[0]))
        {
            if (int.Parse(reqDate[1]) < int.Parse(curDate[1]))
            {
                return false;
            }
            else
            {
                if (int.Parse(reqDate[2]) < int.Parse(curDate[2]))
                {
                    return false;
                }
            }
        }

        if (int.Parse(reqDate[1]) - int.Parse(curDate[1]) <= 1 && int.Parse(reqDate[1]) - int.Parse(curDate[1]) >= 0)
        {
            if (int.Parse(reqDate[1]) == int.Parse(curDate[1]))
            {
                int startOne, curOne;
                if (startTime.Length == 2) startOne = (int.Parse(startTime[0]) * 3600) + (int.Parse(startTime[1]) * 60);
                else startOne = (int.Parse(startTime[0]) * 3600) + (int.Parse(startTime[1]) * 60) + int.Parse(startTime[2]);

                if (curTime.Length == 2) curOne = (int.Parse(curTime[0]) * 3600) + (int.Parse(curTime[1]) * 60);
                else curOne = (int.Parse(curTime[0]) * 3600) + (int.Parse(curTime[1]) * 60) + int.Parse(curTime[2]);

                if (startOne < curOne)
                {
                    return false;
                }

                int a = startOne - curOne;
                return true;
            }
            else
            {
                int startOne, curOne;
                if (startTime.Length == 2) startOne = (int.Parse(startTime[0]) * 3600) + (int.Parse(startTime[1]) * 60);
                else startOne = (int.Parse(startTime[0]) * 3600) + (int.Parse(startTime[1]) * 60) + int.Parse(startTime[2]);

                if (curTime.Length == 2) curOne = (int.Parse(curTime[0]) * 3600) + (int.Parse(curTime[1]) * 60);
                else curOne = (int.Parse(curTime[0]) * 3600) + (int.Parse(curTime[1]) * 60) + int.Parse(curTime[2]);

                if ((86400 - curOne) + startOne <= 86400)
                {
                    int a = (86400 - curOne) + startOne;
                    return true;
                }
                else
                {
                    int a = (86400 - curOne) + startOne;
                    return false;
                }
            }
        }
        else
        {
            return false;
        }
    }

    public static int[] SecondsToTime(int secondss)
    {
        int hr, mnts, scnds;
        hr = secondss / 3600;
        mnts = (secondss % 3600) / 60;
        scnds = (secondss % 3600) % 60;
        int[] temp = new int[3];
        temp[0] = hr;
        temp[1] = mnts;
        temp[2] = scnds;
        return temp;
    }

    public static string TwentyFourToTweleveFormat(string twentyFourFormat)
    {
        string[] time = twentyFourFormat.Split(':');
        string ampm;
        int hour = int.Parse(time[0]);
        if (hour > 11) ampm = "pm";
        else ampm = "am";
        hour = hour % 12;
        if (hour == 0) hour = 12;
        return hour.ToString() + ":" + time[1] + ampm;
    }

    public static Sprite Texture2DToSprite(Texture2D tex)
    {
        return Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
    }
}
