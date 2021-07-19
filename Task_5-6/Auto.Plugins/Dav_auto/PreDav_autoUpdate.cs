using Auto.Plugins.Dav_auto.Handlers;
using Auto.Plugins.Core;
using Entities;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auto.Plugins.Dav_auto
{
    public sealed class PreDav_autoUpdate : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            PluginConfiguration plugin = new PluginConfiguration(serviceProvider);
            var target = plugin.target.ToEntity<dav_auto>();

            try
            {
                Dav_autoService autoService = new Dav_autoService(plugin.service);
                autoService.CopyNameToVin(target);
            }
            catch (Exception ex)
            {
                plugin.tracingService.Trace(ex.ToString());
                throw new InvalidPluginExecutionException(ex.Message);
            }
        }
    }
}
