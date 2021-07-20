var Navicon = Navicon || {}

const fieldNames = {
    km: "dav_km",
    ownerscount: "dav_ownerscount",
    isdamaged: "dav_isdamaged",
    used: "dav_used"
}


Navicon.dav_auto = (function()
{

    let usedOnChange = function(context)
    {
        let formContext = context.getFormContext();

        let kmControl = formContext.getControl(fieldNames.km);
        let ownerscountControl = formContext.getControl(fieldNames.ownerscount);
        let isdamagedControl = formContext.getControl(fieldNames.isdamaged);

        let isUsed = formContext.getAttribute(fieldNames.used).getValue();

        if (isUsed)
        {
            kmControl.setVisible(true);
            ownerscountControl.setVisible(true);
            isdamagedControl.setVisible(true);
        }
        else
        {
            setAttributeValue(context, fieldNames.km, null);
            setAttributeValue(context, fieldNames.ownerscount, null);
            setAttributeValue(context, fieldNames.isdamaged, null);

            kmControl.setVisible(false);
            ownerscountControl.setVisible(false);
            isdamagedControl.setVisible(false);
        }
    }


    var setAttributeValue = function (context, field, value)
    {
        context.getFormContext().getAttribute(field).setValue(value);
    }


    return {
        
        onLoad : function(context)
        {
            let formContext = context.getFormContext();

            formContext.getControl(fieldNames.km).setVisible(false);
            formContext.getControl(fieldNames.ownerscount).setVisible(false);
            formContext.getControl(fieldNames.isdamaged).setVisible(false);

            let usedAttr = formContext.getAttribute(fieldNames.used);
            usedAttr.addOnChange(usedOnChange)
        }
    }
})()