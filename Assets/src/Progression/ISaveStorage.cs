using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveStorage
{
    int Level { get; set; }
    int Experience { get; set; }

    void SetUnlockLevel(Progression.Unlock unlock, int level);

    int GetUnlockLevel(Progression.Unlock unlock, int EmptyValue);

    IEnumerable<Vector2Int> GetOwnedSpaces();
    void StoreOwnedSpaces(IEnumerable<Vector2Int> owned);
}
