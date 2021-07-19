using Entities;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var serviceFactory = context.GetExtension<IOrganizationServiceFactory>();

            var service = serviceFactory.CreateOrganizationService(null);

            var agreementRef = AgreementReference.Get(context);

            dav_agreement davAgreementToUpdate = new dav_agreement();
            davAgreementToUpdate.Id = agreementRef.Id;
            davAgreementToUpdate.dav_paymentplandate = davAgreementToUpdate.dav_paymentplandate.Value.AddDays(1);

            service.Update(davAgreementToUpdate);
        }
    }
}
