var Navicon = Navicon || {}

const contactTypes = {
    phone: 810610003,
    email: 810610004
}

Navicon.dav_communication = (function()
{
    let typeOnChange = function(context)
    {
        let formContext = context.getFormContext();

        let phoneControl = formContext.getControl("dav_phone");
        let emailControl = formContext.getControl("dav_email");
        
        let phoneAttr = formContext.getAttribute("dav_phone");
        let emailAttr = formContext.getAttribute("dav_email");
        let typeAttr = formContext.getAttribute("dav_type");

        phoneControl.setVisible(false);
        emailControl.setVisible(false);

        switch (typeAttr.getValue()) {
            case null:
                phoneAttr.setValue(null);
                emailAttr.setValue(null);
                break;
        
            case contactTypes.phone:
                emailAttr.setValue(null);
                phoneControl.setVisible(true);
                break;

            case contactTypes.email:
                phoneAttr.setValue(null);
                emailControl.setVisible(true);
                break;

            default:
                break;
        }
    }

    return {
        
        onLoad : function(context)
        {
            let formContext = context.getFormContext();

            formContext.getControl("dav_phone").setVisible(false);
            formContext.getControl("dav_email").setVisible(false);

            let typeAttr = formContext.getAttribute("dav_type");
            typeAttr.addOnChange(typeOnChange);
        }
    }
})()  