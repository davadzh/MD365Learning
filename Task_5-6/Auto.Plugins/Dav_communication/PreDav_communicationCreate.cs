using Auto.Plugins.Core;
using Auto.Plugins.Dav_communication.Handlers;
using Entities;
using Microsoft.Xrm.Sdk;
using System;

namespace Auto.Plugins.Dav_communication
{
    public sealed class PreDav_communicationCreate : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            PluginConfiguration plugin = new PluginConfiguration(serviceProvider);
            var target = plugin.Target.ToEntity<dav_communication>();

            try
            {
                Dav_communicationService davInvoiceService = new Dav_communicationService(plugin.Service, plugin.TrasingService);
                davInvoiceService.CheckMainPhoneOrEmailUniqueness(target);
            }
            catch (Exception ex)
            {
                plugin.TrasingService.Trace(ex.ToString());
                throw new InvalidPluginExecutionException(ex.Message);
            }
        }
    }
}
