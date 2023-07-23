using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
[CreateAssetMenu(fileName = "LogControlScriptableObject", menuName = "ScriptableObject/LogControlScriptableObject", order = 0)]
public class LogControlScriptableObject : ScriptableObject
{
    public bool testAble = false;
    public bool infoAble = false;
    public bool warningAble = false;
    public bool errorAble = false;
    public bool fatalAble = false;

    public List<string> textDisableMark = new List<string>();
    public List<string> textEnableMark = new List<string>();

    public List<string> infoDisableMark = new List<string>();
    public List<string> infoEnableMark = new List<string>();

    public List<string> warningDisableMark = new List<string>();
    public List<string> warningEnableMark = new List<string>();

    public List<string> errorDisableMark = new List<string>();
    public List<string> errorEnableMark = new List<string>();

    public List<string> fatalDisableMark = new List<string>();
    public List<string> fatalEnableMark = new List<string>();

    public List<string> globalDisableMark = new List<string>();
    public List<string> globalEnableMark = new List<string>();

    public List<string> markList = new List<string>();
}
