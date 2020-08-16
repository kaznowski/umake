using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityMake;

public sealed class EditTargetGroupDefineSymbols : UMakeBuildAction
{
    [SerializeField] private bool clearAllBeforeLogic = false;
    [SerializeField] private BuildTargetGroup targetGroup;
    [SerializeField] private List<DefineSymbolsConfig> defineSymbolsConfigs;

    public override void Execute(UMake umake, UMakeTarget target)
    {
        string defineSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);
        if (clearAllBeforeLogic)
            defineSymbols = "";

        foreach (DefineSymbolsConfig config in defineSymbolsConfigs)
        {
            if (config.Include)
            {
                if (defineSymbols.Contains(config.Symbol))
                    continue;

                defineSymbols += ";" + config.Symbol;
                continue;
            }

            if (!defineSymbols.Contains(config.Symbol))
                continue;

            defineSymbols = defineSymbols.Replace(";" + config.Symbol, "");
            defineSymbols = defineSymbols.Replace(config.Symbol + ";", "");
            defineSymbols = defineSymbols.Replace(config.Symbol, "");
        }

        PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, defineSymbols);
    }

    [Serializable]
    private class DefineSymbolsConfig
    {
        public string Symbol;
        public bool Include;
    }
}