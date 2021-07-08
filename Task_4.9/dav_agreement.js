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
    agreementnumber: "dav_agreementnumber"
}


Navicon.dav_agreement = (function()
{

    // Открываем вкладку кредит, если выбраны автомобиль и контакт
    let autoidOrContactOnChange = function(context)
    {
        let formContext = context.getFormContext();

        let autoidAttr = formContext.getAttribute(fieldNames.autoid);
        let contactAttr = formContext.getAttribute(fieldNames.contact);

        let creditTabControl = formContext.ui.tabs.get("credittab");

        if (autoidAttr.getValue() !== null && autoidAttr.isValid() 
            && contactAttr.getValue() !== null && contactAttr.isValid())
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
        if (getAttributeValue(context, fieldNames.creditid) !== null 
            && checkAttributeValid(context, fieldNames.creditid))
        {
            changeFieldsDisabling(context, false,
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


    var checkAttributeValid = function (context, field)
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

            // Проверяем изменение автомобиля/контакта для открытия вкладки "Кредит"
            autoidAttr.addOnChange(autoidOrContactOnChange);
            contactAttr.addOnChange(autoidOrContactOnChange);

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