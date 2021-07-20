using Auto.Plugins.Core;
using Auto.Plugins.Dav_auto.Handlers;
using Entities;
using Microsoft.Xrm.Sdk;
using System;

namespace Auto.Plugins.Dav_auto
{
    public sealed class PreDav_autoUpdate : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            PluginConfiguration plugin = new PluginConfiguration(serviceProvider);
            var target = plugin.Target.ToEntity<dav_auto>();

            try
            {
                Dav_autoService autoService = new Dav_autoService(plugin.Service);
                autoService.CopyNameToVin(target);
            }
            catch (Exception ex)
            {
                plugin.TrasingService.Trace(ex.ToString());
                throw new InvalidPluginExecutionException(ex.Message);
            }
        }
    }
}
