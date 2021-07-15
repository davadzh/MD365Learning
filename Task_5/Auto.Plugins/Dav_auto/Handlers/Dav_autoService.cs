using Entities;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auto.Plugins.Dav_auto.Handlers
{
    public class Dav_autoService
    {
        private readonly IOrganizationService service;

        public Dav_autoService(IOrganizationService service)
        {
            this.service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public void CopyNameToVin(dav_auto target)
        {
            string name = target.dav_name;
            throw new InvalidPluginExecutionException(name);
        }
    }
}
