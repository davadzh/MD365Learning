using Auto.Plugins.Core;
using Auto.Plugins.Dav_invoice.Handlers;
using Entities;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auto.Plugins.Dav_invoice
{
    public sealed class PreDav_invoiceUpdate : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            PluginConfiguration plugin = new PluginConfiguration(serviceProvider);
            var target = plugin.target.ToEntity<dav_invoice>();

            try
            {
                Dav_invoiceService davInvoiceService = new Dav_invoiceService(plugin.service, plugin.tracingService);
                davInvoiceService.SetAgreementAmountDependingOnStatusOnUpdate(target);
            }
            catch (Exception ex)
            {
                plugin.tracingService.Trace(ex.ToString());
                throw new InvalidPluginExecutionException(ex.Message);
            }
        }
    }
}
