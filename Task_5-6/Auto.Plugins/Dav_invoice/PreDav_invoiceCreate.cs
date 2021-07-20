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
            var target = plugin.Target.ToEntity<dav_invoice>();

            try
            {
                Dav_invoiceService davInvoiceService = new Dav_invoiceService(plugin.Service, plugin.TrasingService);
                davInvoiceService.SetDefaultInvoiceType(target);
                davInvoiceService.SetAgreementAmountDependingOnStatusOnCreate(target);
            }
            catch (Exception ex)
            {
                plugin.TrasingService.Trace(ex.ToString());
                throw new InvalidPluginExecutionException(ex.Message);
            }
        }
    }
}
