using Auto.Plugins.Core;
using Auto.Plugins.Dav_agreement.Handlers;
using Entities;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auto.Plugins.Dav_agreement
{
    public sealed class PreDav_agreementCreate : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            PluginConfiguration plugin = new PluginConfiguration(serviceProvider);
            var target = plugin.target.ToEntity<dav_agreement>();

            try
            {
                Dav_agreementService davAgreementService = new Dav_agreementService(plugin.service);
                davAgreementService.TrySetContactFirstAgreement(target);
            }
            catch (Exception ex)
            {
                plugin.tracingService.Trace(ex.ToString());
                throw new InvalidPluginExecutionException(ex.Message);
            }
        }
    }
}
