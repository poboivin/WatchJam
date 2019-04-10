using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[System.Serializable]
public struct SoundBite
{

    //public string name;
    public AudioClip clip;
    [Space]
    [Range(0, 1)]
    public float volume;
    //    public AudioSource source;
}
public class SoundPlayer : MonoBehaviour
{
   
   public SoundBite jumpSound;
   public SoundBite RunSound;
   public SoundBite DeathSound;


   // public SoundBite[] Bites;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

//[CustomEditor(typeof(SoundPlayer))]

//public class SoundPlayerEditor : Editor
//{
//    //string[] _choices = new[] { "foo", "foobar" };
//    int _choiceIndex = 0;

//    public override void OnInspectorGUI()
//    {
//        // Draw the default inspector
//        DrawDefaultInspector();
//        var someClass = target as SoundPlayer;
//        if (someClass.Bites != null && someClass.Bites.Length != 0)
//        {

//            string[] _choices = new string[someClass.Bites.Length];
//            for (int i = 0; i < someClass.Bites.Length; i++)
//            {
//                if (someClass.Bites[i].clip != null)
//                {
//                    _choices[i] = someClass.Bites[i].clip.name;


//                }
//                else
//                {
//                    _choices[i] = "null";
//                }
//            }
//            EditorGUILayout.Space();

//            EditorGUILayout.BeginHorizontal();
//            EditorGUILayout.LabelField("jumpSound");
//            _choiceIndex = EditorGUILayout.Popup(_choiceIndex, _choices);
//            EditorGUILayout.EndHorizontal();

//            // Update the selected choice in the underlying object
//            // someClass.jumpSound = _choices[_choiceIndex];
//            someClass.jumpSound = someClass.Bites[_choiceIndex];

//            // Save the changes back to the object
//            EditorUtility.SetDirty(target);
//        }
    
//    }
//}