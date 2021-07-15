using Microsoft.Xrm.Sdk;
using System;

namespace Auto.Plugins.Core
{
    public class PluginConfiguration
    {
        public ITracingService tracingService;
        public IOrganizationService service;
        public Entity target;

        public PluginConfiguration(IServiceProvider serviceProvider)
        {
            try
            {
                tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

                var pluginContext = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
                target = (Entity)pluginContext.InputParameters["Target"];

                var serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                service = serviceFactory.CreateOrganizationService(Guid.Empty); //Если от системного, то null
            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException(ex.Message);
            }
        }
    }
}
