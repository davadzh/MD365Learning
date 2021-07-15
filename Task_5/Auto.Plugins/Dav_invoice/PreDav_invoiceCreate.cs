using Auto.Plugins.Core;
using Auto.Plugins.Dav_invoice.Handlers;
using Entities;
using Microsoft.Xrm.Sdk;
using System;

namespace Auto.Plugins.Dav_invoice
{
    public sealed class PreDav_invoiceCreate : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            PluginConfiguration plugin = new PluginConfiguration(serviceProvider);
            var target = plugin.target.ToEntity<dav_invoice>();

            try
            {
                Dav_invoiceService davInvoiceService = new Dav_invoiceService(plugin.service);
                davInvoiceService.SetDefaultInvoiceType(target);
            }
            catch (Exception ex)
            {
                plugin.tracingService.Trace(ex.ToString());
                throw new InvalidPluginExecutionException(ex.Message);
            }
        }
    }
}
