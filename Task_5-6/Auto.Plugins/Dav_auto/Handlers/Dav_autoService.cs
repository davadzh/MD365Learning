using Entities;
using Microsoft.Xrm.Sdk;
using System;

namespace Auto.Plugins.Dav_auto.Handlers
{
    public class Dav_autoService
    {
        private readonly IOrganizationService _service;

        public Dav_autoService(IOrganizationService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public void CopyNameToVin(dav_auto target)
        {
            string name = target.dav_name;
            throw new InvalidPluginExecutionException(name);
        }
    }
}
