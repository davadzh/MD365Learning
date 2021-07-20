using Entities;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using System.Collections.Generic;

namespace Auto.Workflows.CreatePaymentScheduleWorkflow
{
    public class CreateInvoicesForPaymentScheduleActivity : CodeActivity
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

                var agreementEntity = service.Retrieve(dav_agreement.EntityLogicalName, agreementRef.Id, new Microsoft.Xrm.Sdk.Query.ColumnSet("dav_name", "dav_creditperiod", "dav_creditamount", "dav_date"));
                var agreement = agreementEntity.ToEntity<dav_agreement>();

                if (agreement.dav_creditperiod.HasValue && agreement.dav_date.HasValue)
                {
                    var paymentMonths = agreement.dav_creditperiod.Value * 12;

                    var averagePayment = agreement.dav_creditamount.Value / paymentMonths;

                    List<DateTime> dates = new List<DateTime>();
                    for (int i = 0; i < paymentMonths; i++)
                    {
                        dates.Add(agreement.dav_date.Value.AddMonths(i));
                    }

                    foreach (var el in dates)
                    {
                        var davInvoice = new dav_invoice
                        {
                            dav_name = $"Счет на оплату договора {agreement.dav_name} за {el.ToShortDateString()}",
                            dav_date = el,
                            dav_dogovorid = agreementRef,
                            dav_amount = new Money(averagePayment),
                            dav_type = dav_invoice_dav_type.__810610000,
                            dav_paydate = el,
                            dav_fact = false
                        };

                        service.Create(davInvoice);
                    }
                }
            }
            catch (Exception e)
            {
                throw new InvalidWorkflowException(e.Message);
            }

        }
    }
}
