var Navicon = Navicon || {}

var lookupOptions = 
{
    defaultEntityType: "dav_credit",
	entityTypes: ["dav_credit"],
    allowMultiSelect: false,
	searchText:"Allison",
	filters: [{filterXml: "<filter type='or'><condition attribute='name' operator='like' value='A%' /></filter>",entityLogicalName: "dav_credit"}]
};

Navicon.dav_agreement = (function()
{

    var autoidOrContactOnChange = function(context)
    {
        let formContext = context.getFormContext();

        let autoidAttr = formContext.getAttribute("dav_autoid");
        let contactAttr = formContext.getAttribute("dav_contact");

        let creditTabControl = formContext.ui.tabs.get("credittab");
        let creditidControl = formContext.getControl("dav_creditid");

        if (autoidAttr.getValue() !== null && autoidAttr.isValid() 
            && contactAttr.getValue() !== null && contactAttr.isValid())
        {
            creditTabControl.setVisible(true);
            creditidControl.setDisabled(false);
        }
        else
        {
            creditTabControl.setVisible(false);
            creditidControl.setDisabled(true);
        }
    }


    var creditidOnChange = function(context)
    {
        let formContext = context.getFormContext();

        let creditidAttr = formContext.getAttribute("dav_creditid");

        if (creditidAttr.getValue() !== null && creditidAttr.isValid())
        {
            formContext.getControl("dav_creditperiod").setDisabled(false);
            formContext.getControl("dav_creditamount").setDisabled(false);
            formContext.getControl("dav_fullcreditamount").setDisabled(false);
            formContext.getControl("dav_initialfee").setDisabled(false);
            formContext.getControl("dav_creditperiod").setDisabled(false);
            formContext.getControl("dav_factsumma").setDisabled(false);
            formContext.getControl("dav_paymentplandate").setDisabled(false);
        }
    }


    let agreementnumberOnChange = function(context)
    {
        let formContext = context.getFormContext();
        let agreementnumberAttr = formContext.getAttribute("dav_agreementnumber");

        let val = agreementnumberAttr.getValue();
        if (val !== null)
            agreementnumberAttr.setValue(val.replace(/[^-\d]/g, ''));
    }


    let autoidOnChange = function(context)
    {
        let formContext = context.getFormContext();
        let autoArray = formContext.getAttribute("dav_autoid").getValue();

        


        if (autoArray !== null)
        {
            let autoRef = autoArray[0];

            var fetchData = {
                dav_autoid: autoRef.id
            };

            var fetchXml = [
        "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='true'>",
        "<entity name='dav_credit'>",
        "<attribute name='dav_creditid' />",
        "<attribute name='dav_name' />",
        "<attribute name='createdon' />",
        "<order attribute='dav_name' descending='false' />",
        "<link-entity name='dav_dav_credit_dav_auto' from='dav_creditid' to='dav_creditid' visible='false' intersect='true'>",
        "<link-entity name='dav_auto' from='dav_autoid' to='dav_autoid' alias='ab'>",
        "<filter type='and'>",
        "<condition attribute='dav_autoid' operator='eq' value='", fetchData.dav_autoid, "'/>",
        "</filter>",
        "</link-entity>",
        "</link-entity>",
        "</entity>",
        "</fetch>",
            ].join("");

            fetchXml = "?fetchXml=" + encodeURIComponent(fetchXml);

            let creditids = [];
            Xrm.WebApi.retrieveMultipleRecords("dav_credit", fetchXml).then(
                function(entityResult)
                {
                    for (let i = 0; i < entityResult.entities.length; i++) {
                        creditids.push(entityResult.entities[i].dav_name);
                    }

                        var filterXml = `<filter type='and'>` +
                        `<condition attribute='dav_creditid' operator='in'>`;
                        creditids.forEach(id => {
                                filterXml += `<value>${id}</value>`
                        });
                        filterXml += `</condition></filter>`;

                        formContext.getControl("dav_creditid").addCustomFilter(filterXml, "dav_credit");
                }
            );
               
        }
    }


    return {
        
        onLoad : function(context)
        {
            let formContext = context.getFormContext();

            let amountControl = formContext.getControl("dav_amount");
            let factControl = formContext.getControl("dav_fact");
            let creditidControl = formContext.getControl("dav_creditid");
            let creditTabControl = formContext.ui.tabs.get("credittab");

            formContext.getControl("dav_creditperiod").setDisabled(true);
            formContext.getControl("dav_creditamount").setDisabled(true);
            formContext.getControl("dav_fullcreditamount").setDisabled(true);
            formContext.getControl("dav_initialfee").setDisabled(true);
            formContext.getControl("dav_creditperiod").setDisabled(true);
            formContext.getControl("dav_factsumma").setDisabled(true);
            formContext.getControl("dav_paymentplandate").setDisabled(true);

            amountControl.setDisabled(true);
            factControl.setDisabled(true);
            creditidControl.setDisabled(true);
            creditTabControl.setVisible(false);

            let autoidAttr = formContext.getAttribute("dav_autoid");
            let contactAttr = formContext.getAttribute("dav_contact");
            autoidAttr.addOnChange(autoidOrContactOnChange);
            contactAttr.addOnChange(autoidOrContactOnChange);

            autoidAttr.addOnChange(autoidOnChange);

            let creditidAttr = formContext.getAttribute("dav_creditid");
            creditidAttr.addOnChange(creditidOnChange);

            let agreementnumberAttr = formContext.getAttribute("dav_agreementnumber");
            agreementnumberAttr.addOnChange(agreementnumberOnChange);
        }
    }
})()    