var Navicon = Navicon || {}

const fieldNames = {
    phone: "dav_phone",
    email: "dav_email",
    type: "dav_type"
}

const contactTypes = {
    phone: 810610003,
    email: 810610004
}

Navicon.dav_communication = (function()
{
    let typeOnChange = function(context)
    {
        let formContext = context.getFormContext();

        let phoneControl = formContext.getControl(fieldNames.phone);
        let emailControl = formContext.getControl(fieldNames.email);
        
        let phoneAttr = formContext.getAttribute(fieldNames.phone);
        let emailAttr = formContext.getAttribute(fieldNames.email);
        let typeAttr = formContext.getAttribute(fieldNames.type);

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

            formContext.getControl(fieldNames.phone).setVisible(false);
            formContext.getControl(fieldNames.email).setVisible(false);

            let typeAttr = formContext.getAttribute(fieldNames.type);
            typeAttr.addOnChange(typeOnChange);
        }
    }
})()  