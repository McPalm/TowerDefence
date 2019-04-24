using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

namespace Progression
{
    public class Challenger : MonoBehaviour
    {
        public TextMeshProUGUI ExperienceText;

        static public WaveManagement.Army Army { get; set; }

        void Start()
        {
            var save = SaveData.Current;
            if(save.Unlocked.Count == 0)
            {
                save.Unlocked.Add(Vector2Int.zero);
            }
            var camera = FindObjectOfType<CameraDrag>();
            foreach (var location in save.Unlocked)
            {
                var tile = Map.Instance.Get(location);
                tile.GetComponent<SpriteRenderer>().color = new Color(.24f, .96f, .2f);
                tile.Controlled = true;
                if (camera.MaxX < location.x) camera.MaxX = location.x;
                else if (camera.MinX > location.x) camera.MinX = location.x;
                if (camera.MaxY < location.y) camera.MaxY = location.y;
                else if (camera.MinY > location.y) camera.MinY = location.y;
            }
            ExperienceText.text = $"Level: {save.PlayerLevel}, Experience:{save.Experience}/{save.ExperienceToNext}";
        }

        // Update is called once per frame
        void Update()
        {
            if (Map.Instance)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Click(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                }
            }
        }

        void Click(Vector2 location)
        {
            var tile = Map.Instance.Get(location);
            if (tile)
            {
                if (SaveData.Current.Unlocked.Contains(tile.Position))
                    SaveData.Current.ExperienceForWin = 25 + tile.Level * 20;
                else
                    SaveData.Current.ExperienceForWin = 250 + tile.Level * 100;
                SaveData.Current.Location = tile.Position;
                // tile.GetComponent<SpriteRenderer>().color = new Color(Random.value, Random.value, Random.value);

                Army = tile.Army;
                if (false == tile.Controlled && tile is EventTile)
                    SaveData.Current.UnlockOnWin = ((EventTile)tile).Unlocks;
                else
                    SaveData.Current.UnlockOnWin = Unlock.none;
                StartStage(tile.Stage);
            }
        }

        void StartStage(int i)
        {
            var stage = Map.Instance.ScenedataFor(i);
            SceneManager.LoadScene(stage.SceneName);
#if UNITY_ANDROID
            SceneManager.LoadScene("TouchGameplay", LoadSceneMode.Additive);
#else
            SceneManager.LoadScene(1, LoadSceneMode.Additive);
#endif
        }
    }
}