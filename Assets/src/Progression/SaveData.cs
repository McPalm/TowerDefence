using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData
{
    public SaveData()
    {
        Storage = new SaveToPlayerPrefs();
        Experience = Storage.Experience;
        PlayerLevel = Storage.Level;
        if (PlayerLevel + Experience > 1)
        {
            Unlocked = new HashSet<Vector2Int>(Storage.GetOwnedSpaces());
            foreach (Progression.Unlock unlock in System.Enum.GetValues(typeof(Progression.Unlock)))
            {
                if (unlock == Progression.Unlock.none)
                    continue;
                Unlocks[unlock] = Storage.GetUnlockLevel(unlock, unlock == Progression.Unlock.Bounce ? 1 : 0);
            }
        }
        else
            Unlocked = new HashSet<Vector2Int>();
    }


    ISaveStorage Storage { get; }
    static SaveData _current;
    static public SaveData Current => _current ?? (_current = new SaveData());

    public HashSet<Vector2Int> Unlocked = new HashSet<Vector2Int>();

    public Vector2Int Location { get; set; }
    public int ExperienceForWin { get; set; }
    public Progression.Unlock UnlockOnWin { get; set; } = Progression.Unlock.none;

    public int LevelOf(Progression.Unlock unlock) => Unlocks.ContainsKey(unlock) ? Unlocks[unlock] : 0;

    public Dictionary<Progression.Unlock, int> Unlocks = new Dictionary<Progression.Unlock, int>();

    public void Win()
    {
        var newArea = ! Unlocked.Contains(Location);
        Unlocked.Add(Location);
        Experience += ExperienceForWin;
        if(Experience > ExperienceToNext)
        {
            Experience -= ExperienceToNext;
            PlayerLevel++;
            Storage.Level = PlayerLevel;
        }

        if (UnlockOnWin != Progression.Unlock.none)
        {
            Debug.Log("UNlock " + UnlockOnWin);
            if (Unlocks.ContainsKey(UnlockOnWin))
            {
                Unlocks[UnlockOnWin]++;
                Storage.SetUnlockLevel(UnlockOnWin, Unlocks[UnlockOnWin]);
            }
            else
            {
                Unlocks[UnlockOnWin] = 1;
            }
        }

        if (newArea)
            Storage.StoreOwnedSpaces(Unlocked);
        Storage.Experience = Experience;
    }

    public int PlayerLevel { get; set; } = 1;
    public int Experience { get; private set; } = 0;
    public int ExperienceToNext => PlayerLevel * 1000;
}
