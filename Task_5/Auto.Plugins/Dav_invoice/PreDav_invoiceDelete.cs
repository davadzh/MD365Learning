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
    public sealed class PreDav_invoiceDelete : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            PluginConfiguration plugin = new PluginConfiguration(serviceProvider, true);
            var target = plugin.target.ToEntityReference();

            try
            {
                Dav_invoiceService davInvoiceService = new Dav_invoiceService(plugin.service, plugin.tracingService);
                //davInvoiceService.SetAgreementAmountDependingOnStatus(target);
            }
            catch (Exception ex)
            {
                plugin.tracingService.Trace(ex.ToString());
                throw new InvalidPluginExecutionException(ex.Message);
            }
        }
    }
}
