<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Calendar.aspx.cs" Inherits="MLMS.User.Calendar" MasterPageFile="~/MasterPage/TemplateDefault.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtoolkit" %>
<asp:Content runat="server" ID="contentArea" ContentPlaceHolderID="cph1">
    <link href="../App_Themes/bdsu/fullcalendar.css" rel='stylesheet' />
    <script type="text/javascript" src="../Scripts/jquery-1.9.1.js"></script>
    <script type="text/javascript" src="../Scripts/jquery-ui-1.10.3.custom.min.js"></script>
    <script type="text/javascript" src="../Scripts/jquery.ui.core.js"></script>
    <script type="text/javascript" src="../Scripts/jquery.ui.menu.js"></script>
    <script type="text/javascript" src="../Scripts/jquery.ui.widget.js"></script>
    <script type="text/javascript" src="../Scripts/jquery.ui.position.js"></script>
    <script type="text/javascript" src="../Scripts/jquery.autocomplete.js"></script>
    <link href="../App_Themes/bdsu/jquery-ui-1.10.3.custom.css" rel='stylesheet' />
    <script type="text/javascript" src="../Scripts/fullcalendar.min.js"></script>
    <style type="text/css">
        .eventColor1,
        .fc-agenda .eventColor1 .fc-event-time,
        .eventColor1 a {
            background-color: rgb(0, 128, 0);
            background-color: rgba(0, 128, 0,0.3) !important; /* green  */
        }

        .eventColor2,
        .fc-agenda .eventColor2 .fc-event-time,
        .eventColor2 a {
            background-color: rgb(255, 0, 0);
            background-color: rgba(255,0,0,0.3) !important; /* red  */
        }

        .eventColor3,
        .fc-agenda .eventColor3 .fc-event-time,
        .eventColor3 a {
            background-color: rgb(0,104,139);
            background-color: rgba(0,104,139,0.3) !important; /* blue  */
        }

        .eventColor4,
        .fc-agenda eventColor4 .fc-event-time,
        .eventColor4 a {
            background-color: rgb(255,165,0);
            background-color: rgba(255,165,0,0.3) !important; /* orange */
        }
    </style>
    <script type="text/javascript">

        function pageLoad() {
            //var mpe = $find("PE");
            //var background = $find("PE")._backgroundElement;
            //background.onclick = function () { $find("PE").hide(); }
        }

        $(document).ready(function () {

            x = 1;
            $('#<%=txtBxMDOB.ClientID %>').datepicker({
                changeMonth: true,
                changeYear: true,
                yearRange: '-70:+0',
                onClose: function (selectedDate) {
                    isValidDate(selectedDate);
                }
            });

            $('#<%=txtBxMeetingStartDate.ClientID %>').datepicker({
                onClose: function (selectedDate) {
                    $('#<%=txtBxMeetingEndDate.ClientID %>').datepicker("option", "minDate", selectedDate);
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

            $('#<%=txtBxFollowUpStartDate.ClientID %>').datepicker({
                onClose: function (selectedDate) {
                    $('#<%=txtBxFollowUpEndDate.ClientID %>').datepicker("option", "minDate", selectedDate);
                    isValidDate(selectedDate);
                },
                onSelect: function () {
                    $('#<%=txtBxFollowUpEndDate.ClientID %>')[0].value = $('#<%=txtBxFollowUpStartDate.ClientID %>')[0].value;
                }
            });

            $('#<%= txtBxFollowUpEndDate.ClientID %>').datepicker({
                onClose: function (selectedDate) {
                    $('#<%=txtBxFollowUpStartDate.ClientID %>').datepicker("option", "maxDate", selectedDate);
                    isValidDate(selectedDate);
                }
            });

            LoadCalendar();

            $('#txtLearner').autocomplete("MemberFilterHandler.ashx", {
                minLength: 2,
                max: 50,
                appendTo: '#txtLearner'
            })
        .result(function (event, item) { $('#<%= hdMemberID.ClientID %>')[0].value = item[1]; return false; }
        );

            $('#menuEvent').click(function () {
                $('#<%= tblEventDetails.ClientID %>').css({ "visibility": "visible", "height": "475px" });
                $('#<%= tblDetails.ClientID %>').css({ "visibility": "hidden", "height": "0px" });
                $(this).removeClass('calendarMenu');
                $(this).addClass('calendarMenuSelected');

                $('#menuMember').removeClass('calendarMenu calendarMenuSelected');
                $('#menuMember').addClass('calendarMenu');
            });

            $('#menuMember').click(function () {
                $('#<%= tblEventDetails.ClientID %>').css({ "visibility": "hidden", "height": "0px" });
                $('#<%= tblDetails.ClientID %>').css({ "visibility": "visible", "height": "475px" });
                $(this).removeClass('calendarMenu calendarMenuSelected');
                $(this).addClass('calendarMenuSelected');

                $('#menuEvent').removeClass('calendarMenu calendarMenuSelected');
                $('#menuEvent').addClass('calendarMenu');
            });
        });

        function LoadCalendar() {
            $('#calendar').fullCalendar({
                header: {
                    left: 'prev,next, today',
                    center: 'title',
                    right: 'month,agendaWeek,agendaDay'
                },
                weekMode: 'liquid',
                defaultView: 'agendaWeek',
                editable: true,
                eventClick: function (calEvent, jsEvent, view) {
                    $find('PE').show();
                    LoadEvent(calEvent.id);
                },
                eventResize: function (event, dayDelta, minuteDelta, revertFunc) {
                    var tempDate = new Date(event.start);
                    if (event.end == null) event.end = new Date(tempDate.setHours(tempDate.getHours() + 1));
                    event.start = (event.start).format('dd/MM/yyyy HH:mm:ss');
                    event.end = (event.end).format('dd/MM/yyyy HH:mm:ss');
                    $.ajax({
                        type: 'POST',
                        url: '../Services/CalendarWebService.asmx/UpdateCalendarEvent',
                        data: '{ "CalendarEventID":"' + event.id + '", "StartDate":"' + event.start + '", "EndDate":"' + event.end + '"}',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                            $('#calendar').fullCalendar('refetchEvents');
                        },
                        error: function () {

                        }
                    });
                    $(this).css('z-index', 8);
                    $('.tooltipevent').remove();

                },
                eventDrop: function (event, dayDelta, minuteDelta, allDay, revertFunc) {
                    var tempDate = new Date(event.start);
                    if (event.end == null) event.end = new Date(tempDate.setHours(tempDate.getHours() + 1));
                    showLoading();
                    event.start = (event.start).format('dd/MM/yyyy HH:mm:ss');
                    event.end = (event.end).format('dd/MM/yyyy HH:mm:ss');
                    $.ajax({
                        type: 'POST',
                        url: '../Services/CalendarWebService.asmx/UpdateCalendarEvent',
                        data: '{ "CalendarEventID":"' + event.id + '", "StartDate":"' + event.start + '", "EndDate":"' + event.end + '"}',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                            $('#calendar').fullCalendar('refetchEvents');

                        },
                        error: function () {
                        }
                    });
                    $(this).css('z-index', 8);
                    $('.tooltipevent').remove();
                },
                eventRender: function (event, element) {
                    showLoading();
                },
                eventAfterRender: function (event, element, view) {
                    hideLoading();
                },
                eventAfterAll: function Renderfunction(event, element, view) {
                    hideLoading();
                },
                dayClick: function (date, allDay, jsEvent, view) {
                    $find('PE').show();
                    ClearPopUp();
                    clearYesNo();
                },
                //events: "CalendarHandler.ashx",
                eventMouseover: function (calEvent, jsEvent) {
                    var tooltip = '<div class="tooltipevent">' + calEvent.description + '</div>';
                    $("body").append(tooltip);
                    $(this).mouseover(function (e) {
                        $(this).css('z-index', 10000);
                        $('.tooltipevent').css('overflow', 'hidden');
                        $('.tooltipevent').css('height', '500px');
                        $('.tooltipevent').fadeIn('500');
                        $('.tooltipevent').fadeTo('10', 1.9);
                    }).mousemove(function (e) {
                        $('.tooltipevent').css('top', e.pageY + 10);
                        $('.tooltipevent').css('left', e.pageX + 20);
                    });
                },
                eventMouseout: function (calEvent, jsEvent) {
                    $(this).css('z-index', 8);
                    $('.tooltipevent').remove();
                },
                loading: function (bool) {
                    if (bool)
                        showLoading();
                    else
                        hideLoading();
                },
            });

            $('#calendar').fullCalendar('addEventSource', "CalendarHandler.ashx");

        }

        function AddEvent() {
            var _MemberID = $('#<%= hdMemberID.ClientID %>')[0].value;
            var _FirstName = $('#<%= txtBxFirstName.ClientID %>')[0].value;
            var _LastName = $('#<%= txtBxLastName.ClientID %>')[0].value;
            var _Adult = $('#<%= chkBxAdult.ClientID %>')[0].checked;
            var _PGFirstName = $('#<%= txtBxPGFName.ClientID %>')[0].value;
            var _PGLastName = $('#<%= txtBxPGLName.ClientID %>')[0].value;
            var _DOB = $('#<%= txtBxMDOB.ClientID %>')[0].value;
            var _PrimaryNumber = $('#<%= txtBxMPrimNumb.ClientID %>')[0].value;
            var _SecondaryNumber = $('#<%= txtBxMSecNumb.ClientID %>')[0].value;
            var _PrefCallBack = $('#<%= ddlPreferCallBack.ClientID %>').val();
            var _HowDidYouHear = $('#<%= ddlHowDidYouHear.ClientID %>').val();
            var _Quality = $('#<%= txtBxQuality.ClientID %>')[0].value;
            var _Confirmed = $('#<%= chkBxConfirmed.ClientID %>')[0].checked;
            var _EmailAddress = $('#<%= txtBxEmailAdd.ClientID %>')[0].value;
            var _MemberTypeID = $('#<%= hdMemberTypeID.ClientID %>')[0].value;
            var _EventName = $('#<%= txtBxEventName.ClientID %>')[0].value;
            var _AllDayEvent = $('#<%= chkBxAllDayEvent.ClientID %>')[0].checked;
            var _StartDate = $('#<%= txtBxMeetingStartDate.ClientID %>');
            var _EventDateStart = _StartDate[0].value + " " + $('#<%= ddlStart.ClientID %>').val();
            var _EndDate = $('#<%= txtBxMeetingEndDate.ClientID %>');
            var _EventDateEnd = _EndDate[0].value + " " + $('#<%= ddlEnd.ClientID %>').val();
            var _EventDescription = $('#<%= txtBxMeetDesc.ClientID %>')[0].value;
            var _FollowUp = $('#<%= chkBxIsFollowUp.ClientID %>')[0].checked;
            var _EventType = $('#<%= hdEventType.ClientID %>')[0].value;
            if (_FollowUp == true) _EventType = 2;
            if (_EventType == "") _EventType = 3;
            var _Notes = $('#<%= txtBxNotes.ClientID %>')[0].value;
            var _CalendarEventID = $('#<%= hdCalendarEventID.ClientID %>')[0].value;
            var button = $('#<%= btnUpdate.ClientID %>')[0].value;

            var _AdditionalNotes = $('#<%= txtBxAddNotes.ClientID %>')[0].value;
            var _Objections = $('#<%= ddlObjection.ClientID %>').val();

            $('#<%= tblDetails.ClientID %>').find("input").css("border", "");

            if ($('#<%= txtBxMeetingStartDate.ClientID %>')[0].value.length == 0) {
                showMessageBox("Start Date can not be blank", 2);
                $('#<%= txtBxMeetingStartDate.ClientID %>')[0].style.border = "2px solid red";
                hideLoading();
                return false;
            }
            else if (!isValidDate(_StartDate.val())) {
                showMessageBox('Start date is not a valid Date', 2);
                hideLoading();
                return false;
            }
            if (_EndDate[0].value.length == 0) {
                showMessageBox("End Date can not be blank", 2);
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
                showMessageBox("Event Name can not be blank", 2);
                $('#<%= txtBxEventName.ClientID %>')[0].style.border = "2px solid red";
                hideLoading();
                return false;
            }

    if (button == "Add Event") {
        $.ajax({
            type: 'POST',
            url: '../Services/CalendarWebService.asmx/InsertCalendarEvent',
            data: '{ "MemberID":"' + _MemberID + '", "FirstName":"' + _FirstName + '", "LastName":"' + _LastName + '", "Adult":"' + _Adult + '", "PGFirstName":"' + _PGFirstName + '", "PGLastName":"' + _PGLastName + '", "DOB":"' + _DOB + '", "PrimaryNumber":"' + _PrimaryNumber + '", "SecondaryNumber":"' + _SecondaryNumber + '", "PreferredCallBackID":"' + _PrefCallBack + '", "HowDidYouHearID":"' + _HowDidYouHear + '", "Confirmed":"' + _Confirmed + '", "Quality":"' + _Quality + '", "EmailAddres":"' + _EmailAddress + '", "EventName":"' + _EventName + '", "StartDate":"' + _EventDateStart + '", "EndDate":"' + _EventDateEnd + '", "EventDescription":"' + _EventDescription + '", "AllDayEvent":"' + _AllDayEvent + '", "EventType":"' + _EventType + '", "Notes":"' + _Notes + '"}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                var split = data.d.split("|");

                var eventClass = "";
                if (_EventType == "2") eventClass = "eventColor3";

                $('#calendar').fullCalendar('renderEvent', {
                    id: split[0],
                    title: _EventName,
                    start: _EventDateStart,
                    end: _EventDateEnd,
                    allDay: _AllDayEvent,
                    description: _EventDescription,
                    className: eventClass
                });

                showMessageBox(split[1], 1);
                $find("PE").hide();
            },
            error: function () {
                showMessageBox("Error creating event", 2);
            }
        });
    }
    else {
        var yesChk = $('#<%= yesChk.ClientID %>');
        var noChk = $('#<%= noChk.ClientID %>');
        if (yesChk.is(':checked') || noChk.is(':checked')) {
            if (yesChk.is(':checked')) {
                $.ajax({
                    type: 'POST',
                    url: '../Services/CalendarWebService.asmx/UpdateMemberAndCalendarEvent',
                    data: '{ "CalendarEventID":"' + _CalendarEventID + '", "MemberID":"' + _MemberID + '", "FirstName":"' + _FirstName + '", "LastName":"' + _LastName + '", "Adult":"' + _Adult + '", "PGFirstName":"' + _PGFirstName + '", "PGLastName":"' + _PGLastName + '", "DOB":"' + _DOB + '", "PrimaryNumber":"' + _PrimaryNumber + '", "SecondaryNumber":"' + _SecondaryNumber + '", "PreferredCallBackID":"' + _PrefCallBack + '", "HowDidYouHearID":"' + _HowDidYouHear + '", "Confirmed":"' + _Confirmed + '", "Quality":"' + _Quality + '", "EmailAddres":"' + _EmailAddress + '", "MemberTypeID":"' + $('#<%= ddlMemberTypeID.ClientID %>').val() + '", "EventName":"' + _EventName + '", "StartDate":"' + _EventDateStart + '", "EndDate":"' + _EventDateEnd + '", "EventDescription":"' + _EventDescription + '", "AllDayEvent":"' + _AllDayEvent + '", "EventType":"' + _EventType + '", "Notes":"' + _AdditionalNotes + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        showMessageBox(data.d, 1);

                        $find("PE").hide();
                        $('#calendar').fullCalendar('refetchEvents');
                    },
                    error: function () {
                        showMessageBox("Error adding event", 2);
                    }
                });
            }
            else if (noChk.is(':checked')) {
                $.ajax({
                    type: 'POST',
                    url: '../Services/CalendarWebService.asmx/InactivateLead',
                    data: '{ "MemberID":"' + _MemberID + '", "Objection":"' + _Objections + '", "AdditionalNotes":"' + _AdditionalNotes + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        showMessageBox(data.d, 1);

                        $find("PE").hide();
                        $('#calendar').fullCalendar('refetchEvents');
                    },
                    error: function () {
                        showMessageBox("Error inactivating lead", 2);
                    }
                });
            }

            //Update if follow up date is there
            //First check if dates are blank
            var _FollowUpStartDate = $('#<%= txtBxFollowUpStartDate.ClientID %>')[0].value;
            var _FollowUpEndDate = $('#<%= txtBxFollowUpEndDate.ClientID %>')[0].value;

            if (!isValidDate(_FollowUpStartDate)) {
                showMessageBox('Follow up start date is not a valid Date', 2);
                hideLoading();
                return false;
            }
            if (!isValidDate(_FollowUpEndDate)) {
                showMessageBox('Follow up end date is not a valid Date', 2);
                hideLoading();
                return false;
            }

            if ($.trim(_FollowUpStartDate).length > 0 && $.trim(_FollowUpEndDate).length > 0) {
                _FollowUpStartDate = _FollowUpStartDate + " " + $('#<%= ddlFollowUpTimeStart.ClientID %>').val();
                _FollowUpEndDate = _FollowUpEndDate + " " + $('#<%= ddlFollowUpEndTime.ClientID %>').val();

                $.ajax({
                    type: 'POST',
                    url: '../Services/CalendarWebService.asmx/InsertCalendarEventFromMemberPage',
                    data: '{ "MemberID":"' + _MemberID + '", "EventName":"' + "Follow Up Meeting" + '", "StartDate":"' + _FollowUpStartDate + '", "EndDate":"' + _FollowUpEndDate + '", "EventDescription":"' + "" + '", "AllDayEvent":"' + "false" + '", "FollowUp":"' + "true" + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        var split = data.d.split("|");

                        showMessageBox(split[1], 1);
                        $find("PE").hide();
                        $('#calendar').fullCalendar('renderEvent', {
                            id: split[0],
                            title: "Follow Up Meeting",
                            start: _FollowUpStartDate,
                            end: _FollowUpEndDate,
                            allDay: false,
                            description: "",
                            className: "eventColor3"
                        });
                    },
                    error: function () {
                        showMessageBox('Error creating event', 2);
                    }
                });
            }
        }
        else {
            $.ajax({
                type: 'POST',
                url: '../Services/CalendarWebService.asmx/UpdateMemberAndCalendarEvent',
                data: '{ "CalendarEventID":"' + _CalendarEventID + '", "MemberID":"' + _MemberID + '", "FirstName":"' + _FirstName + '", "LastName":"' + _LastName + '", "Adult":"' + _Adult + '", "PGFirstName":"' + _PGFirstName + '", "PGLastName":"' + _PGLastName + '", "DOB":"' + _DOB + '", "PrimaryNumber":"' + _PrimaryNumber + '", "SecondaryNumber":"' + _SecondaryNumber + '", "PreferredCallBackID":"' + _PrefCallBack + '", "HowDidYouHearID":"' + _HowDidYouHear + '", "Confirmed":"' + _Confirmed + '", "Quality":"' + _Quality + '", "EmailAddres":"' + _EmailAddress + '", "MemberTypeID":"' + _MemberTypeID + '", "EventName":"' + _EventName + '", "StartDate":"' + _EventDateStart + '", "EndDate":"' + _EventDateEnd + '", "EventDescription":"' + _EventDescription + '", "AllDayEvent":"' + _AllDayEvent + '", "EventType":"' + _EventType + '", "Notes":"' + _Notes + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    showMessageBox(data.d, 1);

                    $find("PE").hide();
                    $('#calendar').fullCalendar('refetchEvents');
                },
                error: function () {
                    showMessageBox('Error updating member', 2);
                }
            });
        }
    }
}

function DeleteEvent() {
    var _MemberID = $('#<%= hdMemberID.ClientID %>')[0].value;

    if (confirm("Are you sure you want to delete this event?")) {
        var _CalendarEventID = $('#<%= hdCalendarEventID.ClientID %>')[0].value;

        $.ajax({
            type: 'POST',
            url: '../Services/CalendarWebService.asmx/DeleteCalendarEvent',
            data: '{ "CalendarEventID":"' + _CalendarEventID + '"}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                $('#calendar').fullCalendar('refetchEvents');
                showMessageBox(data.d, 1);
                $find("PE").hide();
            },
            error: function () {
                showMessageBox('Error deleting event', 2);
            }
        });
    }
}

function LoadEvent(id) {
    $.ajax({
        type: 'POST',
        url: '../Services/CalendarWebService.asmx/GetMemberEvent',
        data: '{"CalendarEventID":"' + id + '"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            //parse string to objects
            var obj = jQuery.parseJSON(data.d);
            var MemberID = obj[0].MemberID;
            var FirstName = obj[0].FirstName;
            var LastName = obj[0].LastName;
            var Adult = obj[0].Adult;
            var AdultBool = false;
            if (Adult.toLowerCase() == "true") AdultBool = true;
            var PGFirstName = obj[0].PGFirstName;
            var PGLastName = obj[0].PGLastName;
            var DOB = obj[0].DOB;
            var PrimaryNumber = obj[0].PrimaryNumber;
            var SecondaryNumber = obj[0].SecondaryNumber;
            var PrefCallBack = obj[0].PrefCallBack;
            var HowDidYouHear = obj[0].HowDidYouHear;
            var Quality = obj[0].Quality;
            var MemberTypeID = obj[0].MemberTypeID;
            var EmailAddress = obj[0].EmailAddress;
            var EventName = obj[0].EventName;
            var AllDayEvent = obj[0].AllDayEvent;
            var AllDayEventBool = false;
            if (AllDayEvent.toLowerCase() == "true") AllDayEventBool = true;
            var EventDateStart = obj[0].EventDateStart;
            var EventDateEnd = obj[0].EventDateEnd;
            var EventDateStartTime = obj[0].EventDateStartTime;
            var EventDateEndTime = obj[0].EventDateEndTime;
            var EventDescription = obj[0].EventDescription;
            var Confirmed = obj[0].Confirmed;
            var ConfirmedBool = false;
            if (Confirmed.toLowerCase() == "true") ConfirmedBool = true;
            var EventType = obj[0].EventType;
            var chkBxIsFollowUpBool = false;
            if (EventType == "2") chkBxIsFollowUpBool = true;
            var Notes = obj[0].Notes;

            //Set table visibility
            $('#<%= pnlPopUp.ClientID %>').find("table,input,select,textarea,span").prop('disabled', false);
            $('#<%= tblSearch.ClientID %>').find("input,select,textarea").prop("disabled", true).css("border", "");
            $('#<%= btnUpdate.ClientID %>')[0].style.display = "";
            $('#<%= btnUpdate.ClientID %>')[0].value = "Update";
            $('#<%= btnDelete.ClientID %>')[0].style.display = "";

            //Set values
            $('#<%= txtBxFirstName.ClientID %>')[0].value = FirstName;
            $('#<%= txtBxLastName.ClientID %>')[0].value = LastName;
            $('#<%= chkBxAdult.ClientID %>')[0].checked = AdultBool;
            $('#<%= txtBxPGFName.ClientID %>')[0].value = PGFirstName;
            $('#<%= txtBxPGLName.ClientID %>')[0].value = PGLastName;
            $('#<%= txtBxMDOB.ClientID %>')[0].value = DOB;
            $('#<%= txtBxMPrimNumb.ClientID %>')[0].value = PrimaryNumber;
            $('#<%= txtBxMSecNumb.ClientID %>')[0].value = SecondaryNumber;
            $('#<%= ddlPreferCallBack.ClientID %>').val(PrefCallBack);
            $('#<%= ddlHowDidYouHear.ClientID %>').val(HowDidYouHear);
            $('#<%= txtBxQuality.ClientID %>')[0].value = Quality;
            $('#<%= txtBxEmailAdd.ClientID %>')[0].value = EmailAddress;
            $('#<%= hdMemberTypeID.ClientID %>')[0].value = MemberTypeID;
            $('#<%= txtBxEventName.ClientID %>')[0].value = EventName;
            $('#<%= chkBxAllDayEvent.ClientID %>')[0].checked = AllDayEventBool;
            $('#<%= txtBxMeetingStartDate.ClientID %>')[0].value = EventDateStart;
            $('#<%= txtBxMeetingEndDate.ClientID %>')[0].value = EventDateEnd;
            $('#<%= ddlStart.ClientID %>').val(EventDateStartTime);
            $('#<%= ddlEnd.ClientID %>').val(EventDateEndTime);
            $('#<%= txtBxMeetDesc.ClientID %>')[0].value = EventDescription;
            $('#<%= chkBxConfirmed.ClientID %>')[0].checked = ConfirmedBool;
            $('#<%= chkBxIsFollowUp.ClientID %>')[0].checked = chkBxIsFollowUpBool;
            $('#<%= txtBxNotes.ClientID %>')[0].value = Notes;
            $('#<%= hdEventType.ClientID %>')[0].value = EventType;
            $('#<%= hdCalendarEventID.ClientID %>')[0].value = id;
            $('#<%= hdMemberID.ClientID %>')[0].value = MemberID;

            clearYesNo();
            if (MemberTypeID == '1') {
                $('#<%= spanYes.ClientID %>').show();
                $('#<%= spanNo.ClientID %>').show();
                $('#<%= yesChk.ClientID %>').show();
                $('#<%= noChk.ClientID %>').show();
                $('#<%=FollowUpNotesSection.ClientID %>').hide();
                $('#<%= lblBecomeMember.ClientID %>').show();
            }

        },
        error: function () {
            showMessageBox('Error opening event', 2);
        }
    });
}

function LoadMemeber() {

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
                var Notes = obj[0].Notes;

                //Set table visibility
                $('#<%= tblSearch.ClientID %>').find("input,select,textarea").prop("disabled", true).css("border", "");
                $('#<%= pnlPopUp.ClientID %>').find("input,select,textarea").prop("disabled", false);
                $('#<%= btnUpdate.ClientID %>')[0].style.display = "";

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
                $('#<%= txtBxNotes.ClientID %>')[0].value = Notes;
                clearYesNo();

            },
            error: function () {
                showMessageBox('Error opening event', 2);
            }
        });
    }
}

function ClearPopUp() {

    //Makes search table visible
    $('#<%= tblSearch.ClientID %>').find("input,select,textarea").prop("disabled", false);

    //Disable member table till search is complete
    $('#<%= tblEventDetails.ClientID %>').find("input,select,textarea").prop("disabled", true).css("border", "");
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
    $('#<%= txtBxEventName.ClientID %>')[0].value = "";
    $('#<%= chkBxAllDayEvent.ClientID %>')[0].checked = false;
    $('#<%= txtBxMeetingStartDate.ClientID %>')[0].value = "";
    $('#<%= txtBxMeetingEndDate.ClientID %>')[0].value = "";
    $('#<%= ddlStart.ClientID %>').val("0:00:00");
    $('#<%= ddlEnd.ClientID %>').val("0:00:00");
    $('#<%= txtBxMeetDesc.ClientID %>')[0].value = "";
    $('#<%= txtBxNotes.ClientID %>')[0].value = "";
    $('#<%= btnUpdate.ClientID %>')[0].value = "Add Event";
    $('#<%=txtBxMeetingEndDate.ClientID %>').datepicker("option", "minDate", "");
    $('#<%=txtBxMeetingStartDate.ClientID %>').datepicker("option", "maxDate", "");
    $('#<%= hdCalendarEventID.ClientID %>')[0].value = "";


}
function showYesNo(value) {

    var yesChk = $('#<%= yesChk.ClientID %>');
    var noChk = $('#<%= noChk.ClientID %>');

    if (value) {
        $('#<%=FollowUpNotesSection.ClientID %>').show();
        $('#<%= ddlMemberTypeID.ClientID %>').show();
        $('#<%= ddlObjection.ClientID %>').hide();

        if (yesChk.is(':checked')) {
            noChk.prop("disabled", true);
        }
        else {
            noChk.prop("disabled", false);
            $('#<%=FollowUpNotesSection.ClientID %>').hide();
            $('#<%= ddlMemberTypeID.ClientID %>').hide();
        }
    }
    else {
        $('#<%=FollowUpNotesSection.ClientID %>').show();
        $('#<%= ddlObjection.ClientID %>').show();
        $('#<%= ddlMemberTypeID.ClientID %>').hide();

        if (noChk.is(':checked')) {
            yesChk.prop("disabled", true);
        }
        else {
            yesChk.prop("disabled", false);
            $('#<%=FollowUpNotesSection.ClientID %>').hide();
            $('#<%= ddlObjection.ClientID %>').hide();
            $('#<%= ddlMemberTypeID.ClientID %>').hide();
        }
    }
}

function clearYesNo() {
    $('#<%= spanYes.ClientID %>').hide();
            $('#<%= spanNo.ClientID %>').hide();
            $('#<%= yesChk.ClientID %>').hide();
            $('#<%= noChk.ClientID %>').hide();
            $('#<%= lblBecomeMember.ClientID %>').hide();
            $('#<%=FollowUpNotesSection.ClientID %>').hide();
            $('#<%= ddlObjection.ClientID %>').hide();
            $('#<%= ddlMemberTypeID.ClientID %>').hide();

            $('#<%= yesChk.ClientID %>').prop('checked', false);
            $('#<%= noChk.ClientID %>').prop('checked', false);
            $('#<%= ddlObjection.ClientID %>').prop('selectedIndex', 0);
            $('#<%= ddlMemberTypeID.ClientID %>').prop('selectedIndex', 0);
            $('#<%= txtBxAddNotes.ClientID %>').html('');
            $('#<%= txtBxFollowUpStartDate.ClientID %>').val('');
            $('#<%= txtBxFollowUpEndDate.ClientID %>').val('');
            $('#<%= ddlFollowUpTimeStart.ClientID %>').prop('selectedIndex', 0);
            $('#<%= ddlFollowUpEndTime.ClientID %>').prop('selectedIndex', 0);
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
        body {
            text-align: center;
            font-size: 14px;
            font-family: "Lucida Grande",Helvetica,Arial,Verdana,sans-serif;
        }

        #calendar {
            width: 980px;
            margin: 0 auto;
        }

        .tooltipevent {
            border: 1px solid #3a87ad;
            min-width: 100px;
            min-height: 30px;
            max-width: 100px;
            max-height: 100px;
            background: #ffffff;
            position: absolute;
            z-index: 10001;
        }
    </style>
    <div id='calendar'></div>
    <asp:HiddenField runat="server" ID="calendarEvents" />
    <asp:Button runat="server" ID="btnPopUp" Style="display: none;" />
    <asp:Panel runat="server" ID="pnlPopUp" CssClass="modalPopup" Style="display: none;">
        <asp:Table runat="server" ID="tblCancel" Width="710">
            <asp:TableRow>
                <asp:TableCell>
                    <span class="calendarMenuSelected" id="menuEvent">
                        Event Details
                    </span>
                    <span class="calendarMenu" id="menuMember">
                        Member Info
                    </span>
                </asp:TableCell>
                <asp:TableCell ColumnSpan="2" HorizontalAlign="Right">
                    <asp:ImageButton runat="server" ID="btnCancel" CssClass="btnCancel" ImageUrl="~/App_Themes/bdsu/Images/close-button.png" />
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <asp:Table runat="server" ID="tblSearch">
            <asp:TableRow>
                <asp:TableCell>
                    <asp:Label runat="server" ID="lblSearchFirstName" Text="Last Name:"></asp:Label>
                    <input type="text" id="txtLearner" style="width: 200px" />
                    <asp:HiddenField runat="server" ID="hdMemberID" />
                    <asp:Button runat="server" ID="btnSelectMember" Text="Search" OnClientClick="LoadMemeber(); return false;" />
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <asp:UpdatePanel runat="server" ID="upPanel" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Table runat="server" ID="tblEventDetails" Enabled="false" Style="visibility: visible; display: block; height: 475px;">
                    <asp:TableRow>
                        <asp:TableCell HorizontalAlign="Left">
                            <asp:Label runat="server" ID="Label1" Width="200" Text="Event Start Date: " />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox runat="server" ID="txtBxMeetingStartDate" Class="datetimepicker" Width="62"></asp:TextBox>
                            <asp:DropDownList runat="server" ID="ddlStart" OnSelectedIndexChanged="ddlStart_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Label runat="server" ID="lblMeetEnd" Width="150" Text="Event End Date: " />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox runat="server" ID="txtBxMeetingEndDate" CssClass="datepicker" Width="62"></asp:TextBox>
                            <asp:DropDownList runat="server" ID="ddlEnd"></asp:DropDownList>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label runat="server" ID="lblEventName" Text="EventName:"></asp:Label>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox runat="server" ID="txtBxEventName"></asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell ColumnSpan="2" VerticalAlign="Top">
                            <asp:Label runat="server" ID="lblMeetingDescription" Text="Event Description: " Style="float: left;" /><br />
                            <asp:TextBox runat="server" ID="txtBxMeetDesc" TextMode="MultiLine" Width="300" Height="150" Style="float: left; max-width: 300px; max-height: 200px; min-height: 200px; min-width: 300px;"></asp:TextBox>
                        </asp:TableCell>
                        <asp:TableCell VerticalAlign="Top" ColumnSpan="2">
                            <asp:CheckBox runat="server" ID="chkBxAllDayEvent" Text="All Day Event" TextAlign="Left" /><br />
                            <asp:CheckBox runat="server" ID="chkBxConfirmed" Text="Confirmed" TextAlign="Left" /><br />
                            <asp:CheckBox runat="server" ID="chkBxIsFollowUp" Text="Is meeting a follow up:" TextAlign="Left" /><br />
                            <asp:Label runat="server" ID="lblBecomeMember" Text="Did they sign up?"></asp:Label>
                            <div>
                                <div style="float: left;">
                                    <span id="spanYes" runat="server">Yes</span><input type="checkbox" id="yesChk" name="rbYesNo" value="Yes" onclick="showYesNo(1);" runat="server">
                                </div>
                                <div style="float: right;">
                                    <span id="spanNo" runat="server">No</span><input type="checkbox" id="noChk" name="rbYesNo" value="No" onclick="showYesNo(0);" runat="server">
                                </div>
                            </div>
                            <div style="clear: both;">
                                <asp:DropDownList runat="server" ID="ddlObjection" Style="display: none;"></asp:DropDownList>
                                <asp:DropDownList runat="server" ID="ddlMemberTypeID" Style="display: none;">
                                    <asp:ListItem Text="Member" Value="2" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Customer" Value="3"></asp:ListItem>
                                </asp:DropDownList><br />
                                <div id="FollowUpNotesSection" style="display: none;" runat="server">
                                    <asp:TextBox runat="server" ID="txtBxAddNotes" TextMode="MultiLine" Width="300" Height="150" Style="float: left; max-width: 300px; max-height: 200px; min-height: 200px; min-width: 300px;"></asp:TextBox><br />
                                    <asp:Label runat="server" ID="lblFollowUpDate" Text="Set a follow up date"></asp:Label><br />
                                    <asp:Label runat="server" ID="lblFollowUpStartDate" Text="Start Date:"></asp:Label>
                                    <asp:TextBox runat="server" ID="txtBxFollowUpStartDate"></asp:TextBox>
                                    <asp:DropDownList runat="server" ID="ddlFollowUpTimeStart" OnSelectedIndexChanged="ddlFollowUpTimeStart_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList><br />
                                    <asp:Label runat="server" ID="lblFollowUpEndDate" Text="End Date:"></asp:Label>
                                    <asp:TextBox runat="server" ID="txtBxFollowUpEndDate"></asp:TextBox>
                                    <asp:DropDownList runat="server" ID="ddlFollowUpEndTime"></asp:DropDownList>
                                </div>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:Table runat="server" ID="tblDetails" Enabled="false" Style="visibility: hidden; display: block; height: 0px;">
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
                    <asp:TextBox runat="server" ID="txtBxMDOB" class="datepicker" />
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
                    <asp:Label runat="server" ID="lblSecNumb" Width="150" Text="Secondary Number: " />
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
                    <asp:TextBox runat="server" ID="txtBxQuality" onkeydown="return OnlyNumeric(event,this);" MaxLength="2"></asp:TextBox>
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
                    <asp:Label runat="server" ID="lblPGFName" Width="200" Text="Parent/Guardian First Name: " />
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
                <asp:TableCell ColumnSpan="2">
                    <asp:Label runat="server" ID="lblNotes" Text="Notes: " Style="float: left;" /><br />
                    <asp:TextBox runat="server" ID="txtBxNotes" TextMode="MultiLine" Width="300" Height="150" Style="float: left; max-width: 300px; max-height: 200px; min-height: 200px; min-width: 300px;"></asp:TextBox>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>
                    <asp:HiddenField runat="server" ID="hdEventType" />
                    <asp:HiddenField runat="server" ID="hdHearAbout" />
                    <asp:HiddenField runat="server" ID="hdMemberTypeID" />
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <asp:Table runat="server" ID="tblBtn" Style="margin-bottom: 10px;">
            <asp:TableRow>
                <asp:TableCell>
                    <asp:HiddenField runat="server" ID="hdCalendarEventID" />
                    <asp:Button runat="server" ID="btnUpdate" Text="Update" OnClientClick="AddEvent();return false;" Style="display: none;" />
                    <asp:Button runat="server" ID="btnDelete" Text="Delete" OnClientClick="DeleteEvent();return false;" Style="display: none;" />
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </asp:Panel>
    <ajaxtoolkit:ModalPopupExtender ID="MPE" runat="server"
        BehaviorID="PE"
        TargetControlID="btnPopUp"
        PopupControlID="pnlPopUp"
        BackgroundCssClass="modalBackground"
        CancelControlID="btnCancel">
    </ajaxtoolkit:ModalPopupExtender>
</asp:Content>
