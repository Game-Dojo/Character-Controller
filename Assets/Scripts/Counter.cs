using System;
using TMPro;
using UnityEngine;

public class Counter : MonoBehaviour
{
    private TMP_Text _textCounter;
    private int _counter = 0;
    private void Awake()
    {
        _textCounter = GetComponent<TMP_Text>();
    }

    private void UpdateTextCounter()
    {
        _textCounter.text = _counter.ToString();
    }

    public void AddPinguin()
    {
        _counter += 1;
        UpdateTextCounter();
    }

    public void RemovePinguin()
    {
        _counter -= 1;
        UpdateTextCounter();
    }
}
