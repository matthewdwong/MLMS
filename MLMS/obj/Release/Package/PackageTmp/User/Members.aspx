<%@ Page Language="C#" MasterPageFile="~/MasterPage/TemplateDefault.Master" AutoEventWireup="true" CodeBehind="Members.aspx.cs" Inherits="MLMS.User.Members" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtoolkit" %>
<%@ Register Src="~/UserControls/CallHistory.ascx" TagName="CallHistory" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cph1" runat="server">
    <script type="text/javascript" src="../Scripts/jquery-1.9.1.js"></script>
    <script type="text/javascript" src="../Scripts/jquery-ui-1.10.3.custom.min.js"></script>
    <script type="text/javascript" src="../Scripts/jquery.autocomplete.js"></script>
    <script type="text/javascript" src="../Scripts/jquery.jqpagination.js"></script>
    <link href="../App_Themes/bdsu/jquery-ui-1.10.3.custom.css" rel='stylesheet' />
    <script type="text/javascript">
        function pageLoad() {
        }

        $(document).ready(function () {
            $.ajaxSetup({ cache: false });

            $('#<%=txtBxMDOB.ClientID %>').datepicker({
                changeMonth: true,
                changeYear: true,
                yearRange: '-70:+0',
                onClose: function (selectedDate) {
                    isValidDate(selectedDate);
                }
            });

            $('#<%=txtBxSearchStartDate.ClientID %>').datepicker({
                onClose: function (selectedDate) {
                    $('#<%=txtBxSearchEndDate.ClientID %>').datepicker("option", "minDate", selectedDate);
                    isValidDate(selectedDate);
                }
            });

            $('#<%=txtBxSearchEndDate.ClientID %>').datepicker({
                onClose: function (selectedDate) {
                    $('#<%=txtBxSearchStartDate.ClientID %>').datepicker("option", "maxDate", selectedDate);
                    isValidDate(selectedDate);
                }
            });

            $('#<%=txtBxMeetingStartDate.ClientID %>').datepicker({
                onClose: function (selectedDate) {
                    $('#<%=txtBxMeetingEndDate.ClientID %>').datepicker("option", "maxDate", selectedDate);
                    isValidDate(selectedDate);
                },
                onSelect: function () {
                    $('#<%=txtBxMeetingEndDate.ClientID %>')[0].value = $('#<%=txtBxMeetingStartDate.ClientID %>')[0].value;
                }
            });

            $('#<%=txtBxMeetingEndDate.ClientID %>').datepicker({
                onClose: function (selectedDate) {
                    $('#<%=txtBxMeetingStartDate.ClientID %>').datepicker("option", "maxDate", selectedDate);
                    isValidDate(selectedDate);
                }
            });

            $('#txtLearner').autocomplete("MemberFilterHandler.ashx", {
                minLength: 2,
                max: 50,
                appendTo: '#txtLearner'
            })
        .result(function (event, item) {
            $('#<%= hdMemberID.ClientID %>')[0].value = item[1];
            return false;
        }
        );

            if ($('#<%= hdMemberID.ClientID %>')[0].value != "")
                LoadMember();

            var select = $("#leadQuality");
            var slider = $("<div id='slider' style='float:left; margin-top:5px; width:150px;'></div>").insertBefore(select).slider({
                min: 1,
                max: 10,
                range: "min",
                value: select[0].selectedIndex + 1,
                slide: function (event, ui) {
                    select[0].selectedIndex = ui.value - 1;
                }
            });
            $("#leadQuality").change(function () {
                slider.slider("value", this.selectedIndex + 1);
            });
        });

        function validate() {
            showLoading();
            var MemberID = $('#<%=hdMemberID.ClientID %>').val();
            var FirstName = $('#<%=txtBxFirstName.ClientID %>').val();
            var LastName = $('#<%= txtBxLastName.ClientID %>').val();
            var Adult = $('#<%= chkBxAdult.ClientID %>')[0].checked;
            var PGFirstName = $('#<%= txtBxPGFName.ClientID %>').val();
            var PGLastName = $('#<%= txtBxPGLName.ClientID %>').val();
            var DOB = $('#<%= txtBxMDOB.ClientID %>').val();
            var PrimaryNumber = $('#<%=txtBxMPrimNumb.ClientID %>').val();
            var SecondaryNumber = $('#<%= txtBxMSecNumb.ClientID %>').val();
            var PrefCallBack = $('#<%= ddlPreferCallBack.ClientID %>').val();
            var HowDidYouHear = $('#<%= ddlHowDidYouHear.ClientID %>').val();
            var EmailAddress = $('#<%= txtBxEmailAdd.ClientID %>').val();
            var Quality = $("#slider").slider("option", "value");
            var Notes = $('#<%= txtBxNotes.ClientID %>').val();
            var MemberTypeID = $('#<%= hdMemberTypeID.ClientID %>').val();
            var AchieveList = "[";

            //Validate Dates
            if (!isValidDate(DOB)) {
                showMessageBox('Date of birth is not a valid Date', 2);
                hideLoading();
                return false;
            }

            //loop through achieve to get selected values
            $("#<%= cblAchieve.ClientID %> input:checkbox:checked").each(function () {
                AchieveList = AchieveList + this.parentElement.attributes["mainvalue"].value + ",";
            });

            if (AchieveList.charAt(AchieveList.length - 1) == ",")
                AchieveList = AchieveList.substring(0, AchieveList.length - 1);

            AchieveList = AchieveList + "]";

            //loop through checklist to get selected values
            var CheckList = "[";

            $('#<%= cblChecklist.ClientID %> input:checkbox').each(function () {
                CheckList = CheckList + this.parentElement.attributes["mainvalue"].value + "|" + this.checked + ",";
            });

            if (CheckList.charAt(CheckList.length - 1) == ",")
                CheckList = CheckList.substring(0, CheckList.length - 1);

            CheckList = CheckList + "]";

            if ($.trim(FirstName).length == 0 || $.trim(PrimaryNumber).length == 0) {
                showMessageBox('First Name and Primary Number can not be blank', 2);
                hideLoading();
                return false;
            }

            var btn = $('#<%= btnUpdate.ClientID %>')[0].value;
            if (btn == "Add") {
                $.ajax({
                    type: 'POST',
                    url: '../Services/CalendarWebService.asmx/InsertMember',
                    data: '{ "FirstName":"' + FirstName + '", "LastName":"' + LastName + '", "Adult":"' + Adult + '", "PGFirstName":"' + PGFirstName + '", "PGLastName":"' + PGLastName + '", "DOB":"' + DOB + '", "PrimaryNumber":"' + PrimaryNumber + '", "SecondaryNumber":"' + SecondaryNumber + '", "PreferredCallBackID":"' + PrefCallBack + '", "HowDidYouHearID":"' + HowDidYouHear + '", "Quality":"' + Quality + '", "EmailAddres":"' + EmailAddress + '", "MemberTypeID":"' + MemberTypeID + '", "Notes":"' + Notes + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        showMessageBox('Member info saved', 1);
                        $('#<%= btnUpdate.ClientID %>')[0].value = "Update";
                        $('#<%=hdMemberID.ClientID %>')[0].value = data.d;
                        $('#<%=tblSearch.ClientID %>').show();
                    },
                    error: function () {
                        showMessageBox('Error creating member', 2);
                    }
                }).always(function () {
                    hideLoading();
                });;
            }
            else if (btn == "Update") {
                $.ajax({
                    type: 'POST',
                    url: '../Services/CalendarWebService.asmx/UpdateMember',
                    data: '{ "MemberID":"' + MemberID + '", "FirstName":"' + FirstName + '", "LastName":"' + LastName + '", "Adult":"' + Adult + '", "PGFirstName":"' + PGFirstName + '", "PGLastName":"' + PGLastName + '", "DOB":"' + DOB + '", "PrimaryNumber":"' + PrimaryNumber + '", "SecondaryNumber":"' + SecondaryNumber + '", "PreferredCallBackID":"' + PrefCallBack + '", "HowDidYouHearID":"' + HowDidYouHear + '", "Quality":"' + Quality + '", "EmailAddres":"' + EmailAddress + '", "MemberTypeID":"' + MemberTypeID + '", "Notes":"' + Notes + '", "AchieveList":"' + AchieveList + '", "Checklist":"' + CheckList + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        showMessageBox('Member info updated', 1);
                    },
                    error: function () {
                        showMessageBox('Error updating member', 2);
                    }
                }).always(function () {
                    hideLoading();
                });;
            }
        }
        function LoadMember() {
            showLoading();
            var id = $('#<%= hdMemberID.ClientID %>')[0].value;

            if (id != "") {
                $.ajax({
                    type: 'POST',
                    url: '../Services/CalendarWebService.asmx/GetMemberInfo',
                    data: '{"MemberID":"' + id + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        //parse string to objects
                        var obj = jQuery.parseJSON(data.d);
                        var MemberID = obj[0].MemberID;
                        var FirstName = obj[0].FirstName;
                        var LastName = obj[0].LastName;
                        var Adult = obj[0].Adult;
                        var PGFirstName = obj[0].PGFirstName;
                        var PGLastName = obj[0].PGLastName;
                        var DOB = obj[0].DOB;
                        var PrimaryNumber = obj[0].PrimaryNumber;
                        var SecondaryNumber = obj[0].SecondaryNumber;
                        var PrefCallBack = obj[0].PrefCallBack;
                        var HowDidYouHear = obj[0].HowDidYouHear;
                        var EmailAddress = obj[0].EmailAddress;
                        var Quality = obj[0].Quality;
                        var Notes = obj[0].Notes;
                        var MemberTypeID = obj[0].MemberTypeID;
                        var achieve = new Array();
                        var i = 0;
                        if (!jQuery.isEmptyObject(obj[1])) {
                            $.each(obj[1], function () {
                                achieve[i] = obj[1][i].AchieveID + "|" + obj[1][i].AchieveName;
                                i++;
                            });
                        }

                        var checklist = new Array();
                        var j = 0;
                        if (!jQuery.isEmptyObject(obj[2])) {
                            $.each(obj[2], function () {
                                checklist[j] = obj[2][j].MemberChecklistOptionID + "|" + obj[2][j].UserCheckListOptionID;
                                j++;
                            });
                        }

                        //Set table visibility
                        //$('#<%= tblSearch.ClientID %>')[0].style.display = "none";
                        $('#<%= tblDetails.ClientID %>').find("input,select,textarea").prop("disabled", false);
                        $('#<%= btnUpdate.ClientID %>')[0].style.display = "";
                        $('#<%= btnUpdate.ClientID %>')[0].value = "Update";

                        //uncheck all checkboxes as default
                        $('input[type=checkbox]:checked').prop("checked", false);

                        //Set values
                        $('#<%= txtBxFirstName.ClientID %>')[0].value = FirstName;
                        $('#<%= txtBxLastName.ClientID %>')[0].value = LastName;
                        $('#<%= chkBxAdult.ClientID %>')[0].checked = Adult;
                        $('#<%= txtBxPGFName.ClientID %>')[0].value = PGFirstName;
                        $('#<%= txtBxPGLName.ClientID %>')[0].value = PGLastName;
                        $('#<%= txtBxMDOB.ClientID %>')[0].value = DOB;
                        $('#<%= txtBxMPrimNumb.ClientID %>')[0].value = PrimaryNumber;
                        $('#<%= txtBxMSecNumb.ClientID %>')[0].value = SecondaryNumber;
                        $('#<%= ddlPreferCallBack.ClientID %>').val(PrefCallBack);
                        $('#<%= ddlHowDidYouHear.ClientID %>').val(HowDidYouHear);
                        $('#<%= txtBxEmailAdd.ClientID %>')[0].value = EmailAddress;
                        $('#slider').slider("value", Quality);
                        $('#leadQuality').val(Quality);
                        $('#<%= txtBxNotes.ClientID %>')[0].value = Notes;
                        $('#<%= hdMemberID.ClientID %>')[0].value = MemberID;
                        $('#<%= hdMemberTypeID.ClientID %>')[0].value = MemberTypeID;
                        $('#<%= ucCallHistory.ClientID %>_hdLeadID').val(MemberID);
                        LoadCallLog();

                        achieve.forEach(function (value) {
                            $('#<%= cblAchieve.ClientID %> input:checkbox').each(function () {
                                var splitValue = value.split('|')
                                if (this.parentElement.attributes["mainvalue"].value == splitValue[0])
                                    this.checked = true;
                            });
                        });

                        checklist.forEach(function (value) {
                            $('#<%= cblChecklist.ClientID %> input:checkbox').each(function () {
                                var splitValue = value.split('|')
                                if (this.parentElement.attributes["mainvalue"].value == splitValue[1])
                                    this.checked = true;
                            });
                        });

                        //Gets the calendar events for a member
                        $(function () {
                            $.ajax({
                                type: "POST",
                                url: "../Services/CalendarWebService.asmx/GetCalendarEventsForMember",
                                data: '{"MemberID":"' + id + '"}',
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                success: OnSuccess,
                                failure: function (response) {
                                    showMessageBox(response.d, 1);
                                },
                                error: function (response) {
                                    showMessageBox(response.d, 2);
                                }
                            });
                        });

                    },
                    error: function () {
                        showMessageBox('Error opening event', 2);
                    }
                });
                }
            }

            function AddEvent() {
                showLoading();
                var _MemberID = $('#<%= hdMemberID.ClientID %>')[0].value;
                var _EventName = $('#<%= txtBxEventName.ClientID %>')[0].value;
                var _AllDayEvent = $('#<%= chkBxAllDayEvent.ClientID %>')[0].checked;
                var _StartDate = $('#<%= txtBxMeetingStartDate.ClientID %>');
                var _EventDateStart = _StartDate[0].value + " " + $('#<%= ddlStart.ClientID %>').val();
                var _EndDate = $('#<%= txtBxMeetingEndDate.ClientID %>');
                var _EventDateEnd = _EndDate[0].value + " " + $('#<%= ddlEnd.ClientID %>').val();
                var _EventDescription = $('#<%= txtBxMeetDesc.ClientID %>')[0].value;
                var _FollowUp = $('#<%= chkBxFollowUp.ClientID %>')[0].checked;
                var _Notes = $('#<%= txtBxNotes.ClientID %>')[0].value;
                var _CalendarEventID = $('#<%= hdCEID.ClientID %>')[0].value;

                $('#<%= tblDetails.ClientID %>').find("input").css("border", "");

                //Validate Dates and Blank values
                if (_StartDate[0].value.length == 0) {
                    showMessageBox('Start Date can not be blank', 2);
                    _StartDate[0].style.border = "2px solid red";
                    hideLoading();
                    return false;
                }
                else if (!isValidDate(_StartDate.val())) {
                    showMessageBox('Start date is not a valid Date', 2);
                    hideLoading();
                    return false;
                }

                if (_EndDate[0].value.length == 0) {
                    showMessageBox('End Date can not be blank', 2);
                    _EndDate[0].style.border = "2px solid red";
                    hideLoading();
                    return false;
                }
                else if (!isValidDate(_EndDate.val())) {
                    showMessageBox('End date is not a valid Date', 2);
                    hideLoading();
                    return false;
                }

                if (_EventName.length == 0) {
                    showMessageBox('Event Name can not be blank', 2);
                    $('#<%= txtBxEventName.ClientID %>')[0].style.border = "2px solid red";
                    hideLoading();
                    return false;
                }

                $.ajax({
                    type: 'POST',
                    url: '../Services/CalendarWebService.asmx/InsertCalendarEventFromMemberPage',
                    data: '{ "MemberID":"' + _MemberID + '", "EventName":"' + _EventName + '", "StartDate":"' + _EventDateStart + '", "EndDate":"' + _EventDateEnd + '", "EventDescription":"' + _EventDescription + '", "AllDayEvent":"' + _AllDayEvent + '", "Notes":"' + _Notes + '", "FollowUp":"' + _FollowUp + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        var split = data.d.split("|");

                        showMessageBox(split[1], 1);
                        $find("PE").hide();
                        LoadMember();
                    },
                    error: function () {
                        showMessageBox('Error creating event', 2);
                    }
                }).always(function () {
                    hideLoading();
                });;
            }

            function UpdateEvent() {
                showLoading();
                var _EventName = $('#<%= txtBxEventName.ClientID %>')[0].value;
        var _AllDayEvent = $('#<%= chkBxAllDayEvent.ClientID %>')[0].checked;
        var _StartDate = $('#<%= txtBxMeetingStartDate.ClientID %>');
        var _EventDateStart = _StartDate[0].value + " " + $('#<%= ddlStart.ClientID %>').val();
        var _EndDate = $('#<%= txtBxMeetingEndDate.ClientID %>');
        var _EventDateEnd = _EndDate[0].value + " " + $('#<%= ddlEnd.ClientID %>').val();
        var _EventDescription = $('#<%= txtBxMeetDesc.ClientID %>')[0].value;
        var _FollowUp = $('#<%= chkBxFollowUp.ClientID %>')[0].checked;
        var _Notes = $('#<%= txtBxNotes.ClientID %>')[0].value;
        var _CalendarEventID = $('#<%= hdCEID.ClientID %>')[0].value;

        $('#<%= tblDetails.ClientID %>').find("input").css("border", "");

        if (_StartDate[0].value.length == 0) {
            showMessageBox('Start Date can not be blank', 2);
            _StartDate[0].style.border = "2px solid red";
            hideLoading();
            return false;
        }
        else if (!isValidDate(_StartDate.val())) {
            showMessageBox('Start date is not a valid Date', 2);
            hideLoading();
            return false;
        }
        if (_EndDate[0].value.length == 0) {
            showMessageBox('End Date can not be blank', 2);
            _EndDate[0].style.border = "2px solid red";
            hideLoading();
            return false;
        }
        else if (!isValidDate(_EndDate.val())) {
            showMessageBox('End date is not a valid Date', 2);
            hideLoading();
            return false;
        }

        if (_EventName.length == 0) {
            showMessageBox('Event Name can not be blank', 2);
            $('#<%= txtBxEventName.ClientID %>')[0].style.border = "2px solid red";
            hideLoading();
            return false;
        }

        $.ajax({
            type: 'POST',
            url: '../Services/CalendarWebService.asmx/UpdateCalendarEventFromMemberPage',
            data: '{ "CalendarEventID":"' + _CalendarEventID + '", "EventName":"' + _EventName + '", "StartDate":"' + _EventDateStart + '", "EndDate":"' + _EventDateEnd + '", "EventDescription":"' + _EventDescription + '", "AllDayEvent":"' + _AllDayEvent + '", "Notes":"' + _Notes + '", "FollowUp":"' + _FollowUp + '"}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                showMessageBox(data.d, 1);

                $find("PE").hide();
                LoadMember();
            },
            error: function () {
                showMessageBox('Error creating event', 2);
            }
        }).always(function () {
            hideLoading();
        });;
    }

    function SearchMemberEventDateRange() {
        showLoading();
        var id = $('#<%= hdMemberID.ClientID %>')[0].value;

    var _StartDate = $('#<%=txtBxSearchStartDate.ClientID %>').val();
    var _EndDate = $('#<%=txtBxSearchEndDate.ClientID %>').val();

    //Validate Dates
    if (!isValidDate(_StartDate)) {
        showMessageBox('Start date is not a valid Date', 2);
        hideLoading();
        return false;
    }
    if (!isValidDate(_EndDate)) {
        showMessageBox('End date is not a valid Date', 2);
        hideLoading();
        return false;
    }

    $(function () {
        $.ajax({
            type: "POST",
            url: "../Services/CalendarWebService.asmx/GetCalendarEventsByDateRange",
            data: '{"MemberID":"' + id + '", "StartDate":"' + _StartDate + '", "EndDate":"' + _EndDate + '"}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: OnSuccess,
            failure: function (response) {
                showMessageBox(response.d, 1);
            },
            error: function (response) {
                showMessageBox(response.d, 2);
            }
        }).always(function () {
            hideLoading();
        });;
    });
}

function OnSuccess(response) {
    var xmlDoc = $.parseXML(response.d);
    var xml = $(xmlDoc);
    var calendarEvents = xml.find("Table");
    var row = $("[id*=events] tbody tr:last-child").clone(true);
    var hiddenRow = $("[id*=events] tbody tr:last-child").clone(true);
    row.css("display", "");
    $("[id*=events] tbody tr").remove();
    if (calendarEvents.length > 0) {
        var rowCount = 1;
        $.each(calendarEvents, function () {
            var Events = $(this);
            var allDayEvent;
            if ($(this).find("AllDayEvent").text() == 'false') allDayEvent = "No";
            else allDayEvent = "Yes";
            $("td", row).eq(0).find('input').prop("value", $(this).find("CalendarEventID").text());
            $("td", row).eq(0).find('input').prop("id", "hdCalendarEventID" + rowCount);
            $("td", row).eq(0).find('button').prop("id", rowCount);
            $("td", row).eq(1).html(dateFromString($(this).find("StartDate").text()));
            $("td", row).eq(2).html(dateFromString($(this).find("EndDate").text()));
            $("td", row).eq(3).html($(this).find("EventName").text());
            $("td", row).eq(4).html($(this).find("EventDescription").text());
            $("td", row).eq(5).html(allDayEvent);
            $("td", row).eq(6).html($(this).find("Type").text());
            $("td", row).eq(7).find('button').prop("id", rowCount);
            row.prop("style", "");
            $("[id*=events]").append(row);
            row = $("[id*=events] tbody tr:last-child").clone(true);

            rowCount++;
        });
    }

    $('#<%= txtBxSearchStartDate.ClientID %>').prop("disabled", false);
    $('#<%= txtBxSearchEndDate.ClientID %>').prop("disabled", false);
    $('#btnGetEvents').prop("disabled", false);
    $('#<%= btnAddEvents.ClientID %>').prop("disabled", false);

    // select the table rows
    $table_rows = $('#events tbody tr');

    var table_row_limit = 10;

    var page_table = function (page) {

        // calculate the offset and limit values
        var offset = (page - 1) * table_row_limit,
            limit = page * table_row_limit;

        // hide all table rows
        $table_rows.hide();

        // show only the n rows
        $table_rows.slice(offset, limit).show();

    }

    $('.pagination').jqPagination({
        max_page: Math.round($table_rows.length / table_row_limit),
        paged: page_table
    });

    // set the initial table state to page 1
    page_table(1);

    hiddenRow.css("display", "none");
    $("[id*=events] tbody:last-child").append(hiddenRow);

    hideLoading();
}

function dateFromString(s) {
    var bits = s.split(/[-T:+]/g);
    var d = new Date(bits[0], bits[1] - 1, bits[2]);
    d.setHours(bits[3], bits[4], bits[5]);

    // Get supplied time zone offset in minutes
    var offsetMinutes = bits[6] * 60 + Number(bits[7]);
    var sign = /\d\d-\d\d:\d\d$/.test(s) ? '-' : '+';

    // Apply the sign
    offsetMinutes = 0 + (sign == '-' ? -1 * offsetMinutes : offsetMinutes);

    // Apply offset and local timezone
    d.setMinutes(d.getMinutes() - offsetMinutes - d.getTimezoneOffset())

    // d is now a local time equivalent to the supplied time
    return d;
}

function ClearPopUp() {

    //Makes search table visible
    $('#<%= tblSearch.ClientID %>')[0].style.display = "";
    $('#<%=btnSelectMember.ClientID %>')[0].style.display = "";

    //Disable member table till search is complete
    $('#<%= tblDetails.ClientID %>').find("input,select,textarea").prop("disabled", true).css("border", "");

    $('#txtLearner')[0].value = "";
    $('#<%= txtBxFirstName.ClientID %>')[0].value = "";
            $('#<%= txtBxLastName.ClientID %>')[0].value = "";
    $('#<%= chkBxAdult.ClientID %>')[0].checked = false;
    $('#<%= txtBxPGFName.ClientID %>')[0].value = "";
    $('#<%= txtBxPGLName.ClientID %>')[0].value = "";
    $('#<%= txtBxMDOB.ClientID %>')[0].value = "";
    $('#<%= txtBxMPrimNumb.ClientID %>')[0].value = "";
    $('#<%= txtBxMSecNumb.ClientID %>')[0].value = "";
    $('#<%= ddlPreferCallBack.ClientID %>').val("Morning");
    $('#<%= ddlHowDidYouHear.ClientID %>').val("");
    $('#<%= txtBxEmailAdd.ClientID %>')[0].value = "";

    $('#<%= txtBxNotes.ClientID %>')[0].value = "";
    $('#<%= btnUpdate.ClientID %>')[0].value = "Add Learner";

    $('#<%= hdCEID.ClientID %>')[0].value = "";
    hideLoading();
}

function EditEvent(id) {
    showLoading();
    var _CalendarEventID = $('#hdCalendarEventID' + id)[0].value;

    $.ajax({
        type: 'POST',
        url: '../Services/CalendarWebService.asmx/GetMemberEvent',
        data: '{"CalendarEventID":"' + _CalendarEventID + '"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            //parse string to objects
            var obj = jQuery.parseJSON(data.d);
            var EventName = obj[0].EventName;
            var AllDayEvent = obj[0].AllDayEvent;
            var EventDateStart = obj[0].EventDateStart;
            var EventDateEnd = obj[0].EventDateEnd;
            var EventDateStartTime = obj[0].EventDateStartTime;
            var EventDateEndTime = obj[0].EventDateEndTime;
            var EventDescription = obj[0].EventDescription;
            var Notes = obj[0].Notes;

            //Set values
            $('#<%= txtBxEventName.ClientID %>')[0].value = EventName;
                    $('#<%= chkBxAllDayEvent.ClientID %>')[0].checked = AllDayEvent;
                    $('#<%= txtBxMeetingStartDate.ClientID %>')[0].value = EventDateStart;
                    $('#<%= txtBxMeetingEndDate.ClientID %>')[0].value = EventDateEnd;
                    $('#<%= ddlStart.ClientID %>').val(EventDateStartTime);
                    $('#<%= ddlEnd.ClientID %>').val(EventDateEndTime);
                    $('#<%= txtBxMeetDesc.ClientID %>')[0].value = EventDescription;
                    $('#<%= txtBxNotes.ClientID %>')[0].value = Notes;
                    $('#<%= btnUpdateEvent.ClientID %>').prop("style", "");
                    $('#<%= btnCreateEvent.ClientID %>').prop("style", "display:none;");
                    $('#<%= hdCEID.ClientID %>')[0].value = _CalendarEventID;

                    //Show popup
                    $find('PE').show();
                },
                error: function () {
                    showMessageBox('Error opening event', 2);
                }
            }).always(function () {
                hideLoading();
            });;
        }

        function DeleteEvent(id) {
            if (confirm("Are you sure you want to delete this event?")) {
                showLoading();
                var _CalendarEventID = $('#hdCalendarEventID' + id)[0].value;

                $.ajax({
                    type: 'POST',
                    url: '../Services/CalendarWebService.asmx/DeleteCalendarEvent',
                    data: '{ "CalendarEventID":"' + _CalendarEventID + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: SearchMemberEventDateRange,
                    error: function () {
                        showMessageBox('Error deleting event', 2);
                    }
                }).always(function () {
                    hideLoading();
                });;
            }
        }

        function resizeModal() {
            d = document.getElementById('<%= pnlCallLog.ClientID%>');
            d.style.height = "80%";
        }

        function ToggleButton() {
            $('#<%= btnUpdateEvent.ClientID %>').prop("style", "display:none;");
    $('#<%= btnCreateEvent.ClientID %>').prop("style", "");
}

function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (charCode != 46 && charCode > 31
      && (charCode < 48 || charCode > 57))
        return false;

    return true;
}

function ConvertPhone(id) {
    id.value = id.value.replace(/(\d{3})(\d{3})(\d{4})/, '($1)$2-$3');
}

function OnlyNumeric(evt, ctrl) {

    if (evt.keyCode >= 48 && evt.keyCode <= 57) //Numbers
        return true;
    else if (evt.keyCode >= 96 && evt.keyCode <= 105)//Numeric Keypad
        return true;
    else if (evt.keyCode >= 37 && evt.keyCode <= 40)//Arrow Keys
        return false;
    else if (evt.keyCode == 8 ||//backspace
                    evt.keyCode == 46) //Del
        return true;
    else
        return false;
}
    </script>
    <style type="text/css">
        #events {
            border-collapse: collapse;
            border: 1px solid black;
        }

            #events th {
                border-collapse: collapse;
                border: 1px solid black;
                width: 200px;
            }

            #events td {
                border-collapse: collapse;
                border: 1px solid black;
            }
    </style>
    <asp:Panel runat="server" ID="pnlMember">
        <asp:Table runat="server" ID="tblSearch">
            <asp:TableRow>
                <asp:TableCell ColumnSpan="2" HorizontalAlign="Left">
                    <asp:Label runat="server" ID="lblSearchFirstName" Text="Last Name:"></asp:Label>
                    <input type="text" id="txtLearner" style="width: 180px" />
                    <asp:Button runat="server" ID="btnSelectMember" Text="Search" OnClientClick="LoadMember(); return false;" />
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <asp:Table runat="server" ID="tblDetails" Width="100%">
            <asp:TableRow>
                <asp:TableCell HorizontalAlign="Left">
                    <span style="color: red;">*</span><asp:Label runat="server" ID="lblFirstName" Text="First Name: " />
                </asp:TableCell>
                <asp:TableCell>
                    <asp:TextBox runat="server" ID="txtBxFirstName" />
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>
                    <asp:Label runat="server" ID="lblLastName" Text="Last Name: " />
                </asp:TableCell>
                <asp:TableCell>
                    <asp:TextBox runat="server" ID="txtBxLastName" />
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell HorizontalAlign="Left">
                    <asp:Label runat="server" ID="Label2" Text="Adult: " />
                </asp:TableCell>
                <asp:TableCell HorizontalAlign="Left">
                    <asp:CheckBox runat="server" ID="chkBxAdult" />
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell HorizontalAlign="Left">
                    <asp:Label runat="server" ID="lblDOB" Text="DOB: " />
                </asp:TableCell>
                <asp:TableCell>
                    <asp:TextBox runat="server" ID="txtBxMDOB" CssClass="datepicker" />
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell HorizontalAlign="Left">
                    <span style="color: red;">*</span><asp:Label runat="server" ID="lblPrimNumb" Text="Primary Number: " />
                </asp:TableCell>
                <asp:TableCell>
                    <asp:TextBox runat="server" ID="txtBxMPrimNumb" MaxLength="10" onblur="ConvertPhone(this);" onkeypress="return isNumberKey(event)" />
                </asp:TableCell>
                <asp:TableCell>
                    <asp:Label runat="server" ID="lblSecNumb" Text="Secondary Number: " />
                </asp:TableCell>
                <asp:TableCell>
                    <asp:TextBox runat="server" ID="txtBxMSecNumb" MaxLength="10" onblur="ConvertPhone(this);" onkeypress="return isNumberKey(event)" />
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell HorizontalAlign="Left">
                    <asp:Label runat="server" ID="lblEmailAdd" Text="Email Address: " />
                </asp:TableCell>
                <asp:TableCell>
                    <asp:TextBox runat="server" ID="txtBxEmailAdd"></asp:TextBox>
                </asp:TableCell>
                <asp:TableCell HorizontalAlign="Left">
                    <asp:Label runat="server" ID="lblQuality" Text="Quality: " />
                </asp:TableCell>
                <asp:TableCell>
                    <select style="margin-left:15px;" name="leadQuality" id="leadQuality">
                        <option>1</option>
                        <option>2</option>
                        <option>3</option>
                        <option>4</option>
                        <option>5</option>
                        <option>6</option>
                        <option>7</option>
                        <option>8</option>
                        <option>9</option>
                        <option>10</option>
                    </select>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell HorizontalAlign="Left">
                    <asp:Label runat="server" ID="lblPreferCallBack" Text="Preferred Call Back: " />
                </asp:TableCell>
                <asp:TableCell HorizontalAlign="Left">
                    <asp:DropDownList runat="server" ID="ddlPreferCallBack"></asp:DropDownList>
                </asp:TableCell>
                <asp:TableCell HorizontalAlign="Left">
                    <asp:Label runat="server" ID="lblHowDidYouHear" Text="How Did You Hear: " />
                </asp:TableCell>
                <asp:TableCell HorizontalAlign="Left">
                    <asp:DropDownList runat="server" ID="ddlHowDidYouHear"></asp:DropDownList>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell HorizontalAlign="Left">
                    <asp:Label runat="server" ID="lblPGFName" Width="180" Text="Parent/Guardian First Name: " />
                </asp:TableCell>
                <asp:TableCell>
                    <asp:TextBox runat="server" ID="txtBxPGFName" />
                </asp:TableCell>
                <asp:TableCell>
                    <asp:Label runat="server" ID="lblPGLName" Text="Last Name: " />
                </asp:TableCell>
                <asp:TableCell>
                    <asp:TextBox runat="server" ID="txtBxPGLName" />
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell VerticalAlign="Top">
                    <asp:Label runat="server" ID="lblAchieve" Text="What do they want to Achieve: " Style="float: left;" /><br />
                    <asp:CheckBoxList runat="server" ID="cblAchieve"></asp:CheckBoxList>
                </asp:TableCell>
                <asp:TableCell ColumnSpan="2" VerticalAlign="Top">
                    <asp:Label runat="server" ID="lblNotes" Text="Notes: " Style="float: left;" /><br />
                    <asp:TextBox runat="server" ID="txtBxNotes" TextMode="MultiLine" Style="float: left; max-width: 100%; max-height: 200px; min-height: 200px; min-width: 330px;"></asp:TextBox>
                </asp:TableCell>
                <asp:TableCell VerticalAlign="Top">
                    <asp:UpdatePanel runat="server" ID="upCbl" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label runat="server" ID="lblChecklist" Text="Select a checklist: " Style="float: left;"></asp:Label><br />
                            <asp:DropDownList runat="server" ID="ddlCl" OnSelectedIndexChanged="ddlCL_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList><br />
                            <asp:CheckBoxList runat="server" ID="cblChecklist"></asp:CheckBoxList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell ColumnSpan="4">
                    <hr />
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell ColumnSpan="2">
                    <asp:Button runat="server" ID="btnUpdate" Text="Add" Style="float: left;" OnClientClick="validate(); return false;" />
                    <asp:Button runat="server" ID="btnReset" Text="Clear" OnClick="btnReset_Click" />
                    <asp:Button runat="server" ID="btnCallLog" Text="Call Log" />
                    <asp:HiddenField runat="server" ID="hdMemberID" />
                    <asp:HiddenField runat="server" ID="hdMemberTypeID" />
                </asp:TableCell>
                <asp:TableCell ColumnSpan="2" HorizontalAlign="Right">
                    <asp:Button runat="server" ID="btnDelete" Text="Delete" OnClick="btnDelete_Click" />
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <hr />
        <asp:TextBox runat="server" ID="txtBxSearchStartDate" Enabled="false"></asp:TextBox>
        <asp:TextBox runat="server" ID="txtBxSearchEndDate" Enabled="false"></asp:TextBox>
        <button id="btnGetEvents" onclick="SearchMemberEventDateRange(); return false;" disabled>Load Events</button>
        <asp:Button runat="server" ID="btnAddEvents" Text="Create New Event" Enabled="false" />
        <table id="events" style="border-collapse: collapse; border: 1px solid black;">
            <thead>
                <tr>
                    <th>Edit</th>
                    <th>Start</th>
                    <th>End</th>
                    <th>Event Name</th>
                    <th>Event Description</th>
                    <th>All Day Event</th>
                    <th>Type of Meeeting</th>
                    <th>Delete</th>
                </tr>
            </thead>
            <tbody>
                <tr style="display: none;">
                    <td>
                        <button name="Edit" onclick="EditEvent(this.id);return false;">Edit</button><input type="hidden" /></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td>
                        <button name="Delete" onclick="DeleteEvent(this.id);return false;">Delete</button></td>
                </tr>
            </tbody>
        </table>
        <div class="pagination">
            <a href="#" class="first" data-action="first">&laquo;</a>
            <a href="#" class="previous" data-action="previous">&lsaquo;</a>
            <input type="text" readonly="readonly" data-max-page="40" />
            <a href="#" class="next" data-action="next">&rsaquo;</a>
            <a href="#" class="last" data-action="last">&raquo;</a>
        </div>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlEvent" CssClass="modalPopup" Style="display: none;">
        <asp:Table runat="server" ID="tblCancel" Width="710">
            <asp:TableRow>
                <asp:TableCell ColumnSpan="4" HorizontalAlign="Right">
                    <asp:ImageButton runat="server" ID="btnCancel" CssClass="btnCancel" ImageUrl="~/App_Themes/bdsu/Images/close-button.png" />
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <asp:Table runat="server" ID="tblEvent">
            <asp:TableRow>
                <asp:TableCell HorizontalAlign="Left">
                    <asp:Label runat="server" ID="lblMeetStart" Text="Event Start Date: " />
                </asp:TableCell>
                <asp:TableCell ColumnSpan="1">
                    <asp:TextBox runat="server" ID="txtBxMeetingStartDate" Class="datetimepicker" Width="75"></asp:TextBox>
                    <asp:DropDownList runat="server" ID="ddlStart" OnSelectedIndexChanged="ddlStart_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                </asp:TableCell>
                <asp:TableCell ID="tdConfirmLbl" Visible="false">
                    <asp:Label runat="server" ID="lblConfirmIntro" Text="Confirm Meeting Date: " />
                </asp:TableCell>
                <asp:TableCell ID="tdConfirmChk" Visible="false">
                    <asp:CheckBox runat="server" ID="chkBxConfirmMeet" />
                </asp:TableCell>
                <asp:TableCell HorizontalAlign="Left">
                    <asp:Label runat="server" ID="lblMeetEnd" Text="Event End Date: " />
                </asp:TableCell>
                <asp:TableCell>
                    <asp:TextBox runat="server" ID="txtBxMeetingEndDate" CssClass="datepicker" Width="75"></asp:TextBox>
                    <asp:DropDownList runat="server" ID="ddlEnd"></asp:DropDownList>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell HorizontalAlign="Left">
                    <asp:Label runat="server" ID="lblEventName" Text="EventName:"></asp:Label>
                </asp:TableCell>
                <asp:TableCell HorizontalAlign="Left">
                    <asp:TextBox runat="server" ID="txtBxEventName"></asp:TextBox>
                </asp:TableCell>
                <asp:TableCell HorizontalAlign="Left">
                    <asp:Label runat="server" ID="lblAllDayEvent" Text="All Day Event"></asp:Label>
                </asp:TableCell>
                <asp:TableCell HorizontalAlign="Left">
                    <asp:CheckBox runat="server" ID="chkBxAllDayEvent" />
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell ColumnSpan="2">
                    <asp:Label runat="server" ID="lblMeetingDescription" Text="Event Description: " Style="float: left;" /><br />
                    <asp:TextBox runat="server" ID="txtBxMeetDesc" TextMode="MultiLine" Width="300" Height="150" Style="float: left; max-width: 300px; max-height: 200px; min-height: 200px; min-width: 300px;"></asp:TextBox>
                    <asp:HiddenField runat="server" ID="hdCEID" />
                </asp:TableCell>
                <asp:TableCell>
                    <asp:Label runat="server" ID="lblFollowUP" Text="Is this a follow up meeting"></asp:Label>
                    <asp:CheckBox runat="server" ID="chkBxFollowUp" />
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>
                    <asp:Button runat="server" ID="btnUpdateEvent" Text="Update" Style="display: none;" OnClientClick="UpdateEvent(); return false;" />
                    <asp:Button runat="server" ID="btnCreateEvent" Text="Create Event" OnClientClick="AddEvent(); return false;" />
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlCallLog" CssClass="modalPopup" Style="display: none;">
        <asp:Table runat="server" ID="Table1" Width="100%">
            <asp:TableRow>
                <asp:TableCell ColumnSpan="4" HorizontalAlign="Right">
                    <asp:ImageButton runat="server" ID="btnCancelCallLog" CssClass="btnCancel" ImageUrl="~/App_Themes/bdsu/Images/close-button.png" />
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <uc:CallHistory runat="server" ID="ucCallHistory" />
    </asp:Panel>
    <ajaxtoolkit:ModalPopupExtender ID="MPE" runat="server"
        BehaviorID="PE"
        TargetControlID="btnAddEvents"
        PopupControlID="pnlEvent"
        BackgroundCssClass="modalBackground"
        CancelControlID="btnCancel"
        RepositionMode="RepositionOnWindowResize">
    </ajaxtoolkit:ModalPopupExtender>
    <ajaxtoolkit:ModalPopupExtender ID="MPECallLog" runat="server"
        BehaviorID="MPE"
        TargetControlID="btnCallLog"
        PopupControlID="pnlCallLog"
        BackgroundCssClass="modalBackground"
        CancelControlID="btnCancelCallLog">
    </ajaxtoolkit:ModalPopupExtender>
</asp:Content>

