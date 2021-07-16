using Auto.Plugins.Core;
using Auto.Plugins.Dav_communication.Handlers;
using Entities;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auto.Plugins.Dav_communication
{
    public sealed class PreDav_communicationCreate : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            PluginConfiguration plugin = new PluginConfiguration(serviceProvider);
            var target = plugin.target.ToEntity<dav_communication>();

            try
            {
                Dav_communicationService davInvoiceService = new Dav_communicationService(plugin.service, plugin.tracingService);
                davInvoiceService.CheckMainPhoneOrEmailUniqueness(target);
            }
            catch (Exception ex)
            {
                plugin.tracingService.Trace(ex.ToString());
                throw new InvalidPluginExecutionException(ex.Message);
            }
        }
    }
}
