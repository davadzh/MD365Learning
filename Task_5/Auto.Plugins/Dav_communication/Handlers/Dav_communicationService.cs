using Entities;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auto.Plugins.Dav_communication.Handlers
{
    public class Dav_communicationService
    {
        private readonly IOrganizationService service;
        private readonly ITracingService tracingService;

        public Dav_communicationService(IOrganizationService service,
                                        ITracingService tracingService)
        {
            this.service = service;
            this.tracingService = tracingService;
        }

        public void CheckMainPhoneOrEmailUniqueness(dav_communication davCommunication)
        {
            if (davCommunication.dav_main == true)
            {
                var query = new QueryExpression();
                query.EntityName = dav_communication.EntityLogicalName;
                query.ColumnSet = new ColumnSet("dav_contactid");
                query.Criteria.AddCondition("dav_contactid", ConditionOperator.Equal, davCommunication.dav_contactid.Id);

                if (davCommunication.dav_type == dav_communication_dav_type.Email)
                {
                    query.Criteria.AddCondition("dav_type", ConditionOperator.Equal, dav_communication_dav_type.Email);
                }
                else if (davCommunication.dav_type == dav_communication_dav_type._) // Телефон
                {
                    query.Criteria.AddCondition("dav_type", ConditionOperator.Equal, dav_communication_dav_type._);
                }

                var result = service.RetrieveMultiple(query);
                if (result.Entities.Count > 0)
                    throw new InvalidPluginExecutionException("Указаный контакт уже имеет основное средство связи с таким типом");
            }
        }
    }
}
