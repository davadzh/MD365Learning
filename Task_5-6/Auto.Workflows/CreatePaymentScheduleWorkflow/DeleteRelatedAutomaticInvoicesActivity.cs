using Entities;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using System.Linq;
using System;

namespace Auto.Workflows.CreatePaymentScheduleWorkflow
{
    public class DeleteRelatedAutomaticInvoicesActivity : CodeActivity
    {
        [Input("Agreement")]
        [RequiredArgument]
        [ReferenceTarget("dav_agreement")]
        public InArgument<EntityReference> AgreementReference { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            try
            {
                var serviceFactory = context.GetExtension<IOrganizationServiceFactory>();

                var service = serviceFactory.CreateOrganizationService(null);

                var agreementRef = AgreementReference.Get(context);

                var dav_type = "810610001";

                var fetchXml = $@"
                <fetch>
                  <entity name='dav_invoice'>
                    <attribute name='dav_invoiceid' />
                    <filter>
                      <condition attribute='dav_dogovorid' operator='eq' value='{agreementRef.Id}'/>
                      <condition attribute='dav_type' operator='eq' value='{dav_type}'/>
                    </filter>
                  </entity>
                </fetch>";

                var result = service.RetrieveMultiple(new FetchExpression(fetchXml));

                foreach (var el in result?.Entities?.Select(x => x.ToEntity<dav_invoice>()))
                {
                    service.Delete(dav_invoice.EntityLogicalName, el.Id);
                }
            }
            catch (Exception e)
            {
                throw new InvalidWorkflowException(e.Message);
            }
        }
    }
}
