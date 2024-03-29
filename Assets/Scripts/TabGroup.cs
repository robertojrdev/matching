﻿using UnityEngine;

/// <summary>
/// Controll child Tabs
/// </summary>
public class TabGroup : MonoBehaviour
{
    /// <summary>
    /// The first tab to be selected at start, if null nothing will be selected
    /// </summary>
    [SerializeField] private Tab initialTab; 

    private Tab[] tabs;
    private Tab currentActive;

    private void OnEnable()
    {
        tabs = GetComponentsInChildren<Tab>();
        for (int i = 0; i < tabs.Length; i++)
        {
            tabs[i].onClickEvent += SelectTab;
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < tabs.Length; i++)
        {
            tabs[i].onClickEvent -= SelectTab;
        }
    }

    private void Start()
    {
        if(initialTab)
            SelectTab(initialTab);
    }

    public void SelectTab(Tab selected)
    {
        if (selected == currentActive)
            return;

        if (currentActive != null)
        {
            currentActive.Leave();
            currentActive = null;
        }

        currentActive = selected;
        currentActive.Select();
    }
}