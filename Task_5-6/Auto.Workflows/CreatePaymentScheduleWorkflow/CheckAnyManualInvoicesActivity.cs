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
    public class CheckAnyManualInvoicesActivity : CodeActivity
    {
        [Input("Agreement")]
        [RequiredArgument]
        [ReferenceTarget("dav_agreement")]
        public InArgument<EntityReference> AgreementReference { get; set; }

        [Output("Has an agreement any linked manual type invoices")]
        public OutArgument<bool> HasLinkedManualTypeInvoices { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            var serviceFactory = context.GetExtension<IOrganizationServiceFactory>();

            var service = serviceFactory.CreateOrganizationService(null);

            var agreementRef = AgreementReference.Get(context);

            //Тип создания счета "Вручную"
            var dav_type = "810610000";

            var fetchXml = $@"
                <fetch>
                  <entity name='dav_agreement'>
                    <attribute name='dav_name' />
                    <filter>
                      <condition attribute='dav_agreementid' operator='eq' value='{agreementRef.Id}'/>
                    </filter>
                    <link-entity name='dav_invoice' from='dav_dogovorid' to='dav_agreementid'>
                      <filter>
                        <condition attribute='dav_type' operator='eq' value='{dav_type}'/>
                      </filter>
                    </link-entity>
                  </entity>
                </fetch>";

            var result = service.RetrieveMultiple(new FetchExpression(fetchXml));

            if (result?.Entities?.Count() != 0)
                HasLinkedManualTypeInvoices.Set(context, true);
            else
                HasLinkedManualTypeInvoices.Set(context, false);
        }
    }
}
