var Navicon = Navicon || {}


const fieldNames = {
    amount: "dav_amount",
    fact: "dav_fact",
    creditid: "dav_creditid",
    creditperiod: "dav_creditperiod",
    creditamount: "dav_creditamount",
    fullcreditamount: "dav_fullcreditamount",
    initialfee: "dav_initialfee",
    factsumma: "dav_factsumma",
    paymentplandate: "dav_paymentplandate",
    autoid: "dav_autoid",
    contact: "dav_contact",
    agreementnumber: "dav_agreementnumber",
    date: "dav_date"
}


Navicon.dav_agreement = (function()
{

    // Открываем вкладку кредит, если выбраны автомобиль и контакт
    let creditTabVisibleOnFieldsChange = function(context)
    {
        let formContext = context.getFormContext();

        let creditTabControl = formContext.ui.tabs.get("credittab");

        if (   getAttributeValue(context, fieldNames.autoid) !== null && isAttributeValid(context, fieldNames.autoid) 
            && getAttributeValue(context, fieldNames.contact) !== null && isAttributeValid(context, fieldNames.contact)
            && getAttributeValue(context, fieldNames.date) !== null && isAttributeValid(context, fieldNames.date))
        {
            creditTabControl.setVisible(true);
            changeFieldsDisabling(context, false, fieldNames.creditid);
        }
        else
        {
            creditTabControl.setVisible(false);
            changeFieldsDisabling(context, true, fieldNames.creditid);
        }
    }


    // Открываем/закрываем доступ к полям в зависимости от того, 
    // выбрана ли кредитная программа
    let creditidOnChange = function(context)
    {
        let formContext = context.getFormContext();

        let creditid = getAttributeValue(context, fieldNames.creditid);

        if (creditid !== null)
        {
            let creditidControl = formContext.getControl(fieldNames.creditid);
            let date = getAttributeValue(context, fieldNames.date);

            Xrm.WebApi.retrieveRecord("dav_credit", creditid[0].id).then(
                function(result)
                {
                    if (date - new Date(result.dav_dateend) > 0)
                    {
                        creditidControl.addNotification({
                            messages: ['Срок действия кредитной программы истек'],
                            notificationLevel: 'ERROR',
                            uniqueId: 'dav_date_creditdateendconflict'
                        });

                        changeFieldsDisabling(context, true,
                            fieldNames.creditperiod,
                            fieldNames.creditamount,
                            fieldNames.fullcreditamount,
                            fieldNames.fullcreditamount,
                            fieldNames.initialfee,
                            fieldNames.factsumma,
                            fieldNames.paymentplandate);
                    }
                    else
                    {
                        creditidControl.clearNotification("dav_date_creditdateendconflict");

                        changeFieldsDisabling(context, false,
                            fieldNames.creditperiod,
                            fieldNames.creditamount,
                            fieldNames.fullcreditamount,
                            fieldNames.fullcreditamount,
                            fieldNames.initialfee,
                            fieldNames.factsumma,
                            fieldNames.paymentplandate);

                        // Подставляем срок кредита из кредитной программы во поле
                        // из вкладки "Кредит"
                        setAttributeValue(context, fieldNames.creditperiod, result.dav_creditperiod);    
                    }
                }
            );

            
        }
        else
        {
            changeFieldsDisabling(context, true,
                fieldNames.creditperiod,
                fieldNames.creditamount,
                fieldNames.fullcreditamount,
                fieldNames.fullcreditamount,
                fieldNames.initialfee,
                fieldNames.factsumma,
                fieldNames.paymentplandate);
        }
    }

    
    // Авторасчет стоимости автомобиля в договоре
    let updateAmountWithAutoOnChange = function(context)
    {
        let formContext = context.getFormContext();

        var modelid;

        let autoid = getAttributeValue(context, fieldNames.autoid);

        debugger

        Xrm.WebApi.retrieveRecord("dav_auto", autoid[0].id)
        .then(
            function(result)
            {
                debugger
                console.log(result.dav_used);
                console.log(result.dav_anount);
                modelid = result.dav_modelid
            } 
        )
        .then(
            function(result)
            {
                debugger
                console.log(result.dav_recommendedamount);
            } 
        );
    }


    // Форматируем номер договора (только цифры и знаки тире)
    let agreementnumberOnChange = function(context)
    {
        let currentValue = getAttributeValue(context, fieldNames.agreementnumber);
        
        if (currentValue !== null)
            setAttributeValue(
                context, 
                fieldNames.agreementnumber,
                currentValue.replace(/[^-\d]/g, '')
            );
    }


    // Изменение списка кредитных программ в зависимости от автомобиля
    let autoidOnChange = function(context)
    {
        let formContext = context.getFormContext();

        let autoArray = getAttributeValue(context, fieldNames.autoid);

        if (autoArray !== null)
        {
            // Получаем id выбранного в форме автомобиля
            let autoid = autoArray[0].id;

            let viewId = "{7A047D7A-D76F-4080-B035-4EDB276C59F5}";
 
            let entity = "dav_credit";
                               
            let viewDisplayName = "Кредитные программы";

            // Запрос на получение кредитных программ в зависимости от выбранного автомобиля
            let fetchXml = [
                "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='true'>",
                    "<entity name='dav_credit'>",
                        "<attribute name='dav_creditid' />",
                        "<attribute name='dav_name' />",
                        "<attribute name='createdon' />",
                    "<order attribute='dav_name' descending='false' />",
                    "<link-entity name='dav_dav_credit_dav_auto' from='dav_creditid' to='dav_creditid' visible='false' intersect='true'>",
                        "<link-entity name='dav_auto' from='dav_autoid' to='dav_autoid' alias='ab'>",
                            "<filter type='and'>",
                                "<condition attribute='dav_autoid' operator='eq' value='", autoid, "'/>",
                            "</filter>",
                        "</link-entity>",
                    "</link-entity>",
                    "</entity>",
                "</fetch>",
            ].join("");

            // Сабгрид для отображения нового лукапа
            let layout = "<grid name='resultset' object='8' jump='dav_name' select='1' icon='1' preview='1'>" +
            "<row name='result' id='dav_creditid'>" +
            "<cell name='dav_name' width='300'/>" +
            "</row></grid>";
       
            // Добавляем кастомное представление для лукапа с отфильтрованными данными
            formContext.getControl(fieldNames.creditid)
                       .addCustomView(viewId, entity, viewDisplayName, fetchXml, layout, true);
        }
    }


    var changeFieldsDisabling = function (context, makeDisabled, ...fields)
    {
        let formContext = context.getFormContext();

        if (makeDisabled === true)
        {
            for (let i = 0; i < fields.length; i++) {
                formContext.getControl(fields[i]).setDisabled(true);
            }
        }
        else
        {
            for (let i = 0; i < fields.length; i++) {
                formContext.getControl(fields[i]).setDisabled(false);
            }
        }
    }


    var getAttributeValue = function (context, field)
    {
        return context.getFormContext().getAttribute(field).getValue();
    }


    var setAttributeValue = function (context, field, value)
    {
        context.getFormContext().getAttribute(field).setValue(value);
    }


    var isAttributeValid = function (context, field)
    {
        return context.getFormContext().getAttribute(field).isValid();
    }


    return {
        
        onLoad : function(context)
        {
            let formContext = context.getFormContext();

            // Блокируем все поля кредита, а также некоторые поля с основной вкладки
            changeFieldsDisabling(context,  true,
                                    fieldNames.amount,
                                    fieldNames.fact,
                                    fieldNames.creditperiod,
                                    fieldNames.creditamount,
                                    fieldNames.fullcreditamount,
                                    fieldNames.initialfee,
                                    fieldNames.factsumma,
                                    fieldNames.paymentplandate,
                                    fieldNames.creditid);

            // Скрываем вкладку "Кредит"
            let creditTabControl = formContext.ui.tabs.get("credittab");
            creditTabControl.setVisible(false);

            let autoidAttr = formContext.getAttribute(fieldNames.autoid);
            let contactAttr = formContext.getAttribute(fieldNames.contact);
            let dateAttr = formContext.getAttribute(fieldNames.date);

            // Проверяем изменение автомобиля/контакта/даты для открытия вкладки "Кредит"
            autoidAttr.addOnChange(creditTabVisibleOnFieldsChange);
            contactAttr.addOnChange(creditTabVisibleOnFieldsChange);
            dateAttr.addOnChange(creditTabVisibleOnFieldsChange);

            // Проверяем автомобиль для выборки кредитных программ
            autoidAttr.addOnChange(autoidOnChange);
            
            // Открываем доступ к полям кредита при наличии кредитной программы
            let creditidAttr = formContext.getAttribute(fieldNames.creditid);
            creditidAttr.addOnChange(creditidOnChange);

            // Форматируем номер договора (только цифры и знаки тире)
            let agreementnumberAttr = formContext.getAttribute(fieldNames.agreementnumber);
            agreementnumberAttr.addOnChange(agreementnumberOnChange);
        }
    }
})()    