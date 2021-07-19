using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auto.Workflows.CreatePaymentScheduleWorkflow
{
    public class CheckAnyPaidInvoicesActivity : CodeActivity
    {
        [Input("Agreement")]
        [RequiredArgument]
        [ReferenceTarget("dav_agreement")]
        public InArgument<EntityReference> AgreementReference { get; set; }

        [Output("Has an agreement any linked paid invoices")]
        public OutArgument<bool> HasLinkedPaidInvoices { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            var serviceFactory = context.GetExtension<IOrganizationServiceFactory>();

            var service = serviceFactory.CreateOrganizationService(null);

            var agreementRef = AgreementReference.Get(context);

            var fetchXml = $@"
                <fetch no-lock='true'>
                  <entity name='dav_agreement'>
                    <attribute name='dav_name' />
                    <filter>
                      <condition attribute='dav_agreementid' operator='eq' value='{agreementRef.Id}'/>
                    </filter>
                    <link-entity name='dav_invoice' from='dav_dogovorid' to='dav_agreementid'>
                      <filter>
                        <condition attribute='dav_fact' operator='eq' value='{1}'/>
                      </filter>
                    </link-entity>
                  </entity>
                </fetch>";

            var result = service.RetrieveMultiple(new FetchExpression(fetchXml));

            if (result?.Entities?.Count() != 0)
                HasLinkedPaidInvoices.Set(context, true);
            else
                HasLinkedPaidInvoices.Set(context, false);
        }
    }
}
