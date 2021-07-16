using Auto.Common.Commands;
using Entities;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auto.Plugins.Dav_invoice.Handlers
{
    public class Dav_invoiceService
    {
        private readonly IOrganizationService service;
        private readonly ITracingService tracingService;

        public Dav_invoiceService(IOrganizationService service,
                                  ITracingService tracingService)
        {
            this.service = service;
            this.tracingService = tracingService;
        }


        public void SetDefaultInvoiceType(dav_invoice davInvoice)
        {
            InvoiceCommands.SetDefaultInvoiceType(davInvoice);
        }


        public void SetAgreementAmountDependingOnStatusOnUpdate(dav_invoice davInvoice)
        {
            if (davInvoice.dav_fact == true)
            {
                var davInvoiceEntity = service.Retrieve(dav_invoice.EntityLogicalName, davInvoice.Id, new ColumnSet("dav_amount", "dav_dogovorid"));
                var davOldInvoice = davInvoiceEntity.ToEntity<dav_invoice>();

                var increase = davInvoice?.dav_amount?.Value ?? davOldInvoice.dav_amount.Value;

                var davAgreementEntity = service.Retrieve(dav_agreement.EntityLogicalName, davOldInvoice.dav_dogovorid.Id, new ColumnSet("dav_amount", "dav_factsumma"));

                var davAgreement = davAgreementEntity.ToEntity<dav_agreement>();

                var newFactSumma = davAgreement.dav_factsumma.Value + increase;

                bool isPaid = false;

                if (newFactSumma > davAgreement.dav_amount.Value)
                    throw new InvalidPluginExecutionException("Общая оплаченная сумма не должна превышать сумму в договоре");
                else if (newFactSumma == davAgreement.dav_amount.Value)
                    isPaid = true;

                var agreementToUpdate = new dav_agreement
                {
                    Id = davOldInvoice.dav_dogovorid.Id,
                    dav_factsumma = new Money(newFactSumma),
                    dav_fact = isPaid
                };

                service.Update(agreementToUpdate);
            }
        }

        public void SetAgreementAmountDependingOnStatusOnCreate(dav_invoice davInvoice)
        {
            if (davInvoice.dav_fact == true)
            {
                var increase = davInvoice?.dav_amount?.Value ?? 0;

                var davAgreementEntity = service.Retrieve(dav_agreement.EntityLogicalName, davInvoice.dav_dogovorid.Id, new ColumnSet("dav_amount", "dav_factsumma"));

                var davAgreement = davAgreementEntity.ToEntity<dav_agreement>();

                var newFactSumma = davAgreement.dav_factsumma.Value + increase;

                bool isPaid = false;

                if (newFactSumma > davAgreement.dav_amount.Value)
                    throw new InvalidPluginExecutionException("Общая оплаченная сумма не должна превышать сумму в договоре");
                else if (newFactSumma == davAgreement.dav_amount.Value)
                    isPaid = true;

                var agreementToUpdate = new dav_agreement
                {
                    Id = davInvoice.dav_dogovorid.Id,
                    dav_factsumma = new Money(newFactSumma),
                    dav_fact = isPaid
                };

                service.Update(agreementToUpdate);
            }
        }

    }
}
