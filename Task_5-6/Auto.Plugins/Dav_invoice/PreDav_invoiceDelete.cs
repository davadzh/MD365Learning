using Auto.Plugins.Core;
using Auto.Plugins.Dav_invoice.Handlers;
using Microsoft.Xrm.Sdk;
using System;

namespace Auto.Plugins.Dav_invoice
{
    public sealed class PreDav_invoiceDelete : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            PluginConfiguration plugin = new PluginConfiguration(serviceProvider, true);
            var target = plugin.Target.ToEntityReference();

            try
            {
                Dav_invoiceService davInvoiceService = new Dav_invoiceService(plugin.Service, plugin.TrasingService);
                //davInvoiceService.SetAgreementAmountDependingOnStatus(target);
            }
            catch (Exception ex)
            {
                plugin.TrasingService.Trace(ex.ToString());
                throw new InvalidPluginExecutionException(ex.Message);
            }
        }
    }
}
