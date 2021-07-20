using Entities;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using System;

namespace Auto.Workflows.CreatePaymentScheduleWorkflow
{
    public class SetNewPaymentScheduleDateActivity : CodeActivity
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

                dav_agreement davAgreementToUpdate = new dav_agreement()
                {
                    Id = agreementRef.Id,
                    dav_paymentplandate = DateTime.Now.AddDays(1)
                };

                service.Update(davAgreementToUpdate);
            }
            catch (Exception e)
            {

                throw new InvalidWorkflowException(e.Message);
            }
        }
    }
}
