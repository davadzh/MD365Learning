var Navicon = Navicon || {}


Navicon.dav_auto = (function()
{

    let usedOnChange = function(context)
    {
        let formContext = context.getFormContext();

        let kmControl = formContext.getControl("dav_km");
        let ownerscountControl = formContext.getControl("dav_ownerscount");
        let isdamagedControl = formContext.getControl("dav_isdamaged");

        let isUsed = formContext.getAttribute("dav_used").getValue();

        if (isUsed)
        {
            kmControl.setVisible(true);
            ownerscountControl.setVisible(true);
            isdamagedControl.setVisible(true);
        }
        else
        {
            let kmAttr = formContext.getAttribute("dav_km");
            let ownerscountAttr = formContext.getAttribute("dav_ownerscount");
            let isdamagedAttr = formContext.getAttribute("dav_isdamaged");

            kmAttr.setValue(null);
            ownerscountAttr.setValue(null);
            isdamagedAttr.setValue(null);

            kmControl.setVisible(false);
            ownerscountControl.setVisible(false);
            isdamagedControl.setVisible(false);
        }
    }


    return {
        
        onLoad : function(context)
        {
            let formContext = context.getFormContext();

            formContext.getControl("dav_km").setVisible(false);
            formContext.getControl("dav_ownerscount").setVisible(false);
            formContext.getControl("dav_isdamaged").setVisible(false);

            let usedAttr = formContext.getAttribute("dav_used");
            usedAttr.addOnChange(usedOnChange)
        }
    }
})()