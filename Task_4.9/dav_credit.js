var Navicon = Navicon || {}

const yearInMs = 31536000000;

Navicon.dav_credit = (function()
{
    let datestartOnChange = function(context)
    {
        let formContext = context.getFormContext();
        let datestartAttr = formContext.getAttribute("dav_datestart");

        if (datestartAttr.getValue() !== null && datestartAttr.isValid())
            formContext.getControl("dav_dateend").setDisabled(false);
        else
            formContext.getControl("dav_dateend").setDisabled(true);
    }


    let dateendOnChange = function(context)
    {
        let formContext = context.getFormContext();
        let datestartAttr = formContext.getAttribute("dav_datestart");
        let dateendAttr = formContext.getAttribute("dav_dateend");
        let dateendControl = formContext.getControl("dav_dateend");

        if (dateendAttr.getValue() !== null)
        {
            if ((dateendAttr.getValue() - datestartAttr.getValue()) < yearInMs)
            {
                dateendControl.addNotification({
                    messages: ['Разница между датой начала и окончания кредита должна быть не менее 1 года'],
                    notificationLevel: 'ERROR',
                    uniqueId: 'dav_dateend_yearerror'
                });
            }
            else
            {
                dateendControl.clearNotification("dav_dateend_yearerror");
            }
        }
    }


    


    return {
        
        onLoad : function(context)
        {
            let formContext = context.getFormContext();

            formContext.getControl("dav_dateend").setDisabled(true);

            let datestartAttr = formContext.getAttribute("dav_datestart");
            datestartAttr.addOnChange(datestartOnChange);

            let dateendAttr = formContext.getAttribute("dav_dateend");
            dateendAttr.addOnChange(dateendOnChange);
        }
    }
})()  