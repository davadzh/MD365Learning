using Auto.Common.Commands;
using Entities;
using Microsoft.Xrm.Sdk;
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

        public Dav_invoiceService(IOrganizationService service)
        {
            this.service = service;
        }

        public void SetDefaultInvoiceType(dav_invoice davInvoice)
        {
            InvoiceCommands.SetDefaultInvoiceType(davInvoice);
        }
    }
}
