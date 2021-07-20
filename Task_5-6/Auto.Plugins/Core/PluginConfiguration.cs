using Microsoft.Xrm.Sdk;
using System;

namespace Auto.Plugins.Core
{
    public class PluginConfiguration
    {
        public ITracingService TrasingService { get; private set; }
        public IOrganizationService Service { get; private set; }
        public Entity Target { get; private set; }
        public EntityReference DeletionTarget { get; private set; }

        public PluginConfiguration(IServiceProvider serviceProvider, bool isDeletionTarget = false)
        {
            try
            {
                TrasingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

                var pluginContext = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

                if (isDeletionTarget)
                    DeletionTarget = (EntityReference)pluginContext.InputParameters["Target"];
                else
                    Target = (Entity)pluginContext.InputParameters["Target"];

                var serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                Service = serviceFactory.CreateOrganizationService(Guid.Empty); //Если от системного, то null
            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException(ex.Message);
            }
        }
    }
}
