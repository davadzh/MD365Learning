var Navicon = Navicon || {}

const yearInMs = 31536000000;

const fieldNames = {
    datestart: "dav_datestart",
    dateend: "dav_dateend"
}

const dateendNotification = {
    messages: ['Разница между датой начала и окончания кредита должна быть не менее 1 года'],
    notificationLevel: 'ERROR',
    uniqueId: 'dav_dateend_yearerror'
}


Navicon.dav_credit = (function()
{

    let datestartOnChange = function(context)
    {
        let formContext = context.getFormContext();
        let datestartAttr = formContext.getAttribute(fieldNames.datestart);

        if (datestartAttr.getValue() !== null && datestartAttr.isValid())
            formContext.getControl(fieldNames.dateend).setDisabled(false);
        else
            formContext.getControl(fieldNames.dateend).setDisabled(true);
    }


    let dateendOnChange = function(context)
    {
        let formContext = context.getFormContext();
        let datestartAttr = formContext.getAttribute(fieldNames.datestart);
        let dateendAttr = formContext.getAttribute(fieldNames.dateend);
        let dateendControl = formContext.getControl(fieldNames.dateend);

        if (dateendAttr.getValue() !== null)
        {
            if ((dateendAttr.getValue() - datestartAttr.getValue()) < yearInMs)
            {
                dateendControl.addNotification({
                    dateendNotification
                });
            }
            else
            {
                dateendControl.clearNotification(dateendNotification.uniqueId);
            }
        }
    }


    


    return {
        
        onLoad : function(context)
        {
            let formContext = context.getFormContext();

            formContext.getControl(fieldNames.dateend).setDisabled(true);

            let datestartAttr = formContext.getAttribute(fieldNames.datestart);
            datestartAttr.addOnChange(datestartOnChange);

            let dateendAttr = formContext.getAttribute(fieldNames.dateend);
            dateendAttr.addOnChange(dateendOnChange);
        }
    }
})()  