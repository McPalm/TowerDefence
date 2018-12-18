using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ArmyEditor : EditorWindow
{


    [MenuItem("Tower Defence/Army Editor")]
    public static void ShowWindow()
    {
        GetWindow(typeof(ArmyEditor));
    }

    void OnGUI()
    {
        var o = Selection.activeObject;
        if (o is WaveManagement.Army)
            DisplayArmy(o as WaveManagement.Army);
        else
            GUILayout.Label("Select an Army to Edit");
    }

    Vector2 scrollPosition;

    void DisplayArmy(WaveManagement.Army army)
    {
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        GUILayout.BeginHorizontal();
        for (int i = 0; i < army.waves.Count; i++)
        {
            DisplayWave(i+1, army.waves[i]);
        }
        GUILayout.EndHorizontal();
        EditorGUILayout.EndScrollView();
        if (GUILayout.Button("Add Wave"))
        {
            army.waves.Add(new WaveManagement.Army.Wave());
        }
        
    }

    void DisplayWave(int number, WaveManagement.Army.Wave wave)
    {
        EditorGUILayout.BeginVertical();
        GUILayout.Label("Wave " + number);
        GUILayout.Label("Worth " + wave.Bounty);
        for (int i = 0; i < wave.units.Count; i++)
        {
            if (wave.units[i].qty == 0)
                wave.units.RemoveAt(i);
            else if (wave.units[i].enemy != null)
                DisplayUnit(wave.units[i]);
            else
                wave.units.RemoveAt(i);
            
        }

        wave.units.Add(new WaveManagement.Army.Unit());
        
        GameObject newUnit = EditorGUILayout.ObjectField(null, typeof(GameObject), false) as GameObject;
        if (newUnit != null)
            wave.units.Add(new WaveManagement.Army.Unit()
            {
                enemy = newUnit,
                qty = 1,
            });
        EditorGUILayout.EndVertical();
    }

    void DisplayUnit(WaveManagement.Army.Unit unit)
    {
        unit.enemy = EditorGUILayout.ObjectField(unit.enemy, typeof(GameObject), false) as GameObject;
        unit.qty = EditorGUILayout.IntField("Number", unit.qty);
        // unit.delay = EditorGUILayout.FloatField("Delay", unit.delay);
    }
}
