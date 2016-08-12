﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Reflection;

namespace Huoyaoyuan.AdmiralRoom.Composition
{
    internal class ModuleHost
    {
        private ModuleHost()
        {
            var a = new AssemblyCatalog(Assembly.GetExecutingAssembly());
            ComposablePartCatalog catalog;
            try
            {
                var d = new DirectoryCatalog("modules");
                catalog = new AggregateCatalog(a, d);
            }
            catch { catalog = a; }
            container = new CompositionContainer(catalog);
            container.ComposeParts(this);
        }
        public static ModuleHost Instance { get; } = new ModuleHost();
        [ImportMany(typeof(ModuleInfo))]
        public IList<Lazy<ModuleInfo, IModuleMetadata>> Modules { get; } = new List<Lazy<ModuleInfo, IModuleMetadata>>();
        [ImportMany(typeof(ISubView))]
        public IList<ISubView> SubViews { get; } = new List<ISubView>();
        [ImportMany(typeof(ISubWindow))]
        public IList<ISubWindow> SubWindows { get; } = new List<ISubWindow>();
        private CompositionContainer container;
        public void Dispose()
        {
            foreach (var module in Modules)
                (module.Value as IDisposable)?.Dispose();
        }
    }
}
