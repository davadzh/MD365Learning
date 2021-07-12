var Navicon = Navicon || {}

// const fieldNames = {
//     amount: "dav_amount",
//     creditamount: "dav_creditamount",
//     initialfee: "dav_initialfee"
// }

Navicon.nav_building_ribbon = (function()
{

    var getAttributeValue = function (context, field)
    {
        return context.getAttribute(field).getValue();
    }


    var setAttributeValue = function (context, field, value)
    {
        context.getAttribute(field).setValue(value);
    }


    var isAttributeValid = function (context, field)
    {
        return context.getAttribute(field).isValid();
    }

    return {
        
        ShowAlert : function(primaryControl)
        {
            var formContext = primaryControl; 
 
            let amount = getAttributeValue(formContext, 'dav_amount');
            let initialfee = getAttributeValue(formContext, 'dav_initialfee');

            if (amount !== null && isAttributeValid(formContext, 'dav_amount')
             && initialfee !== null && isAttributeValid(formContext, 'dav_initialfee'))
            {
                setAttributeValue(formContext, 'dav_creditamount', amount - initialfee)
            }

            let creditid = getAttributeValue(formContext, 'dav_creditid');
            let creditperiod = getAttributeValue(formContext, 'dav_creditperiod');
            let creditamount = getAttributeValue(formContext, 'dav_creditamount');


            if (creditid !== null && 
                creditperiod !== null &&
                creditamount !== null)
            {
                Xrm.WebApi.retrieveRecord("dav_credit", creditid[0].id)
                .then(
                    function(result)
                    {
                        setAttributeValue(formContext, 'dav_fullcreditamount', (result.dav_percent/100 
                                                                                * creditperiod * creditamount) + creditamount);
                    },
                    function(error)
                    {
                        console.log(error.message);
                    }
                )
            }
        }
    }
})();