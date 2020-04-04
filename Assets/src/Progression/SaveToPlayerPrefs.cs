using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Progression;

public class SaveToPlayerPrefs : ISaveStorage
{
    public int Level
    {
        get => PlayerPrefs.GetInt("level", 0);
        set => PlayerPrefs.SetInt("level", value);
    }
    public int Experience
    {
        get => PlayerPrefs.GetInt("experience", 0);
        set => PlayerPrefs.SetInt("experience", value);
    }
    

    public IEnumerable<Vector2Int> GetOwnedSpaces()
    {
        var line = PlayerPrefs.GetString("owned");
        var collection = line.Split(':').Select(s =>
        {
            var v = s.Split(',');
            return new Vector2Int(int.Parse(v[0]), int.Parse(v[1]));
        });
        return collection;
    }

    public int GetUnlockLevel(Unlock unlock, int EmptyValue)
    {
        return PlayerPrefs.GetInt(unlock.ToString(), EmptyValue);
    }

    public void SetUnlockLevel(Unlock unlock, int level)
    {
        PlayerPrefs.SetInt(unlock.ToString(), level);
    }

    public void StoreOwnedSpaces(IEnumerable<Vector2Int> owned)
    {
        var line = string.Join(":", owned.Select(v2 => $"{v2.x},{v2.x}"));
        PlayerPrefs.SetString("owned", line);
    }
}
