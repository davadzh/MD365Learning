using Auto.Plugins.Core;
using Auto.Plugins.Dav_agreement.Handlers;
using Entities;
using Microsoft.Xrm.Sdk;
using System;

namespace Auto.Plugins.Dav_agreement
{
    public sealed class PreDav_agreementCreate : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            PluginConfiguration plugin = new PluginConfiguration(serviceProvider);
            var target = plugin.Target.ToEntity<dav_agreement>();

            try
            {
                Dav_agreementService davAgreementService = new Dav_agreementService(plugin.Service);
                davAgreementService.TrySetContactFirstAgreement(target);
            }
            catch (Exception ex)
            {
                plugin.TrasingService.Trace(ex.ToString());
                throw new InvalidPluginExecutionException(ex.Message);
            }
        }
    }
}
