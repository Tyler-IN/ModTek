﻿using BattleTechModLoader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ModTek
{
    public static class Loading
    {
        public static void LoadMod(ModDef modDef, StreamWriter logWriter = null)
        {
            logWriter.WriteLine("Attempting to load {0}", modDef.Name);

            // load mod dll
            if (modDef.DLL != null)
            {
                var dllPath = Path.Combine(modDef.Directory, modDef.DLL);

                if (File.Exists(dllPath))
                {
                    if (modDef.DLLEntryPoint == null)
                    {
                        BTModLoader.LoadDLL(dllPath, logWriter);
                    }
                    else
                    {
                        string typeName = null;
                        string methodName = "Init";
                        int pos = modDef.DLLEntryPoint.LastIndexOf('.');

                        if (pos == -1)
                            methodName = modDef.DLLEntryPoint;
                        else
                        {
                            typeName = modDef.DLLEntryPoint.Substring(0, pos - 1);
                            methodName = modDef.DLLEntryPoint.Substring(pos + 1);
                        }

                        // TODO: pass in param for modDef.Path, and modDef.Settings if available
                        BTModLoader.LoadDLL(dllPath, logWriter, methodName, typeName);
                    }
                }
                else
                {
                    logWriter.WriteLine("{0} has a DLL specified ({1}), but it's missing! Aborting load.", modDef.Name, dllPath);
                    return;
                }
            }

            // load other parts of mod!

            logWriter.WriteLine("Loaded {0}", modDef.Name);
        }
    }
}