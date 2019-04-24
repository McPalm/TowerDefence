using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData
{
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
        Unlocked.Add(Location);
        Experience += ExperienceForWin;
        if(Experience > ExperienceToNext)
        {
            Experience -= ExperienceToNext;
            PlayerLevel++;    
        }

        if (UnlockOnWin != Progression.Unlock.none)
        {
            Debug.Log("UNlock " + UnlockOnWin);
            if (Unlocks.ContainsKey(UnlockOnWin))
            {
                Unlocks[UnlockOnWin]++;
            }
            else
            {
                Unlocks[UnlockOnWin] = 1;
            }
        }
    }

    public int PlayerLevel { get; set; } = 1;
    public int Experience { get; private set; } = 0;
    public int ExperienceToNext => PlayerLevel * 1000;

}
