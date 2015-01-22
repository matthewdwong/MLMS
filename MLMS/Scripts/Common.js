function ShowCompanyOther() {
    var txtID = '';
    var lblID = '';
    var lstID = '';

    var elements = document.forms[0].elements;

    for (var i = 0; i < elements.length; i++) {
        var elem = elements[i];

        if (elem.id.indexOf('ddlCompanyID') > 0)
            lstID = elem.id;

        if (elem.id.indexOf('txtCompanyOther') > 0)
            txtID = elem.id;

        if (elem.id.indexOf('lblCompanyOther') > 0)
            lblID = elem.id;

    }

    var txt = document.getElementById(txtID);
    var lbl = document.getElementById(lblID);
    if (txt != null && lbl != null) {
        if (IsOtherSelected(lstID)) {
            txt.style.display = "block";
            lbl.style.display = "block";
        }
        else {
            txt.value = '';
            txt.style.display = "none";
            lbl.style.display = "none";
        }
    }

    return false;
}

function IsOtherSelected(lstID) {
    var lst = document.getElementById(lstID);
    var len = lst.options.length;

    for (var i = 0; i < len; i++) {
        if (lst[i].selected && lst[i].text == 'Other') {

            return true;
        }
    }
    return false;
}

function ChangeInternational() {
    var stateID = '';
    var zipID = '';
    var countryID = '';

    var elements = document.forms[0].elements;

    for (var i = 0; i < elements.length; i++) {
        var elem = elements[i];

        if (elem.id.indexOf('lblState') > 0)
            stateID = elem.id;
        if (elem.id.indexOf('lblZip') > 0)
            zipID = elem.id;
        if (elem.id.indexOf('ddlCountry') > 0)
            countryID = elem.id;
    }

    if (countryID != '') {
        var ddlCountry = document.getElementById(countryID);
        var lblState = document.getElementById(stateID);
        var lblZip = document.getElementById(zipID);

        switch (ddlCountry.value) {
            case "CAN":
                if (lblState != null)
                    lblState.value = 'Prov.:';
                if (lblZip != null)
                    lblZip.value = 'Postal Code: ';
                break;
            case "MEX":
                if (lblState != null)
                    lblState.value = 'State:';
                if (lblZip != null)
                    lblZip.value = 'Postal Code: ';
                break;
            case "USA":
                if (lblState != null)
                    lblState.value = 'State:';
                if (lblZip != null)
                    lblZip.value = 'Zip Code: ';
                break;
            default:
                if (lblState != null)
                    lblState.value = 'State:';
                if (lblZip != null)
                    lblZip.value = 'Zip Code: ';
                break;
        }
    }
    return false;
}

function ShowChainOther() {
    var chainID = '';
    var lblChainID = '';
    var btnID = '';
    var lstID = '';

    var elements = document.forms[0].elements;

    for (var i = 0; i < elements.length; i++) {
        var elem = elements[i];

        if (elem.id.indexOf('ddlChainID') > 0)
            lstID = elem.id;

        if (elem.id.indexOf('txtChain') > 0)
            chainID = elem.id;

        if (elem.id.indexOf('lblChain') > 0)
            lblChainID = elem.id;

        if (elem.id.indexOf('btnSearchChain') > 0)
            btnID = elem.id;

    }

    var chain = document.getElementById(chainID);
    var lblChain = document.getElementById(lblChainID);
    var btn = document.getElementById(btnID);
    if (chain != null && lblChain != null) {
        if (IsFirstSelected(lstID)) {
            chain.style.display = "none";
            lblChain.style.display = "none";
            btn.style.display = "none";
        }
        else {
            chain.style.display = "block";
            lblChain.style.display = "block";
            btn.style.display = "block";
        }
    }

    return false;
}

function IsFirstSelected(lstID) {
    var lst = document.getElementById(lstID);

    var len = lst.options.length;

    for (var i = 0; i < len; i++) {
        if (lst[i].selected && lst[i].textContent == ' ') {

            return true;
        }
    }
    return false;
}

function showMessageBox(message, messageType) {
    //Stop any previous menu
    hideMessageBox();

    //Pop up menu with message
    // 1 is successfull message add fade time
    // 2 is error message do not add fade time
    if (messageType == 1) {
        $('.messageBox').fadeIn().delay(10000).fadeOut();
        $('.messageBox').css("color", "black");
    }
    else {
        $('.messageBox').fadeIn();
        $('.messageBox').css("color", "red");
    }
    $('#messageBox').html(message);
}

function hideMessageBox() {
    $('.messageBox').stop();
    $('#messageBoxContainer').hide();
}

function showLoading() {
    $('#loading').show();
}

function hideLoading() {
    $('#loading').hide();
}

// Checks a string to see if it in a valid date format
function isValidDate(date) {
    var matches = /^(\d{1,2})[-\/](\d{1,2})[-\/](\d{4})$/.exec(date);
    if (matches == null && date.trim().length > 0) {
        showMessageBox('Date range is not valid', 2);
        return false;
    }
    return true;
}
