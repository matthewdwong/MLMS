<%@ Page Language="C#" MasterPageFile="~/MasterPage/TemplateDefault.Master" AutoEventWireup="true" CodeBehind="Leads.aspx.cs" Inherits="MLMS.User.Leads" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtoolkit" %>
<%@ Register Src="~/UserControls/CallHistory.ascx" TagName="CallHistory" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cph1" runat="server">
    <script type="text/javascript" src="../Scripts/jquery-1.9.1.js"></script>
    <script type="text/javascript" src="../Scripts/jquery-ui-1.10.3.custom.min.js"></script>
    <script type="text/javascript" src="../Scripts/jquery.autocomplete.js"></script>
    <link href="../App_Themes/bdsu/jquery-ui-1.10.3.custom.css" rel='stylesheet' />
    <script type="text/javascript">
        function pageLoad() {
            var mpe = $find("PELead");
            var background = $find("PELead")._backgroundElement;
            background.onclick = function () { $find("PELead").hide(); }
        }

        $(document).ready(function () {
            $('#<%=txtBxMDOB.ClientID %>').datepicker({
                changeMonth: true,
                changeYear: true,
                yearRange: '-70:+0',
                onClose: function (selectedDate) {
                    isValidDate(selectedDate);
                }
            });

            $('#<%=txtBxIntroStartDate.ClientID %>').datepicker({
                onClose: function (selectedDate) {
                    $('#<%=txtBxIntroEndDate.ClientID %>').datepicker("option", "minDate", selectedDate);
                    isValidDate(selectedDate);
                },
                onSelect: function () {
                    $('#<%=txtBxIntroEndDate.ClientID %>')[0].value = $('#<%=txtBxIntroStartDate.ClientID %>')[0].value;
                }
            });

            $('#<%=txtBxIntroEndDate.ClientID %>').datepicker({
                onClose: function (selectedDate) {
                    $('#<%=txtBxIntroStartDate.ClientID %>').datepicker("option", "maxDate", selectedDate);
                    isValidDate(selectedDate);
                }
            });

            $('#txtLearner').autocomplete("LeadSearchHandler.ashx", {
                minLength: 2,
                max: 50,
                appendTo: '#txtLearner'
            })
        .result(function (event, item) { $('#<%= hdMemberID.ClientID %>')[0].value = item[1]; return false; }
        );

            $('#menuLead').click(function () {
                $('#<%= tblDetails.ClientID %>').css({ "visibility": "visible", "height": "525px" });
                $('#<%= pnlCallLog.ClientID %>').css({ "visibility": "hidden", "height": "0px" });
                $(this).removeClass('calendarMenu');
                $(this).addClass('calendarMenuSelected');

                $('#menuCallLog').removeClass('calendarMenu calendarMenuSelected');
                $('#menuCallLog').addClass('calendarMenu');
            });

            $('#menuCallLog').click(function () {
                $('#<%= tblDetails.ClientID %>').css({ "visibility": "hidden", "height": "0px" });
                $('#<%= pnlCallLog.ClientID %>').css({ "visibility": "visible", "height": "525px" });
                $(this).removeClass('calendarMenu calendarMenuSelected');
                $(this).addClass('calendarMenuSelected');

                $('#menuLead').removeClass('calendarMenu calendarMenuSelected');
                $('#menuLead').addClass('calendarMenu');
            });

            var select = $('#<%=leadQuality.ClientID %>')
            var slider = $("<div id='slider' style='float:left; margin-top:5px; width:150px;'></div>").insertBefore(select).slider({
                min: 1,
                max: 10,
                range: "min",
                value: select[0].selectedIndex + 1,
                slide: function (event, ui) {
                    select[0].selectedIndex = ui.value - 1;
                }
            });
            $('#<%=leadQuality.ClientID %>').change(function () {
                slider.slider("value", this.selectedIndex + 1);
            });
        });

        function validate() {
            showLoading();
            var FirstName = $('#<%=txtBxFirstName.ClientID %>').val();
            var PrimaryNumb = "(" + $('#<%=txtBxMPrimNumbArea.ClientID %>').val() + ")" + $('#<%=txtBxMPrimNumb.ClientID %>').val() + "-" + $('#<%=txtBxMPrimNumbLast4.ClientID %>').val();
            if ($.trim(FirstName).length == 0 || $.trim(PrimaryNumb).length == 0) {
                showMessageBox('First Name and Primary Number must be filled', 2);
                hideLoading();
                return false;
            }
            //Validate Dates
            var DOB = $('#<%=txtBxMDOB.ClientID %>').val();
            var IntroStart = $('#<%=txtBxIntroStartDate.ClientID %>').val();
            var IntroEnd = $('#<%=txtBxIntroEndDate.ClientID %>').val();
            if (!isValidDate(DOB)) {
                showMessageBox('Date of birth is not a valid Date', 2);
                hideLoading();
                return false;
            }
            if (!isValidDate(IntroStart)) {
                showMessageBox('Intro start date is not a valid Date', 2);
                hideLoading();
                return false;
            }
            if (!isValidDate(IntroEnd)) {
                showMessageBox('Intro end date is not a valid Date', 2);
                hideLoading();
                return false;
            }
        }

        function LoadMemeber() {
            showLoading();
            var id = $('#<%= hdMemberID.ClientID %>')[0].value;

            if (id != "") {
                $.ajax({
                    type: 'POST',
                    url: '../Services/CalendarWebService.asmx/GetLeadInfo',
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
                        var EventName = obj[0].EventName;
                        var AllDayEvent = obj[0].AllDayEvent;
                        var EventDateStart = obj[0].EventDateStart;
                        var EventDateEnd = obj[0].EventDateEnd;
                        var EventDateStartTime = obj[0].EventDateStartTime;
                        var EventDateEndTime = obj[0].EventDateEndTime;
                        var EventDescription = obj[0].EventDescription;
                        var Notes = obj[0].Notes;

                        //Set table visibility
                        $('#<%= tblSearch.ClientID %>')[0].style.display = "none";
                        $('#<%= tblDetails.ClientID %>').find("input,select,textarea").prop("disabled", false);
                        $('#<%= btnUpdate.ClientID %>')[0].style.display = "";
                        $('#<%= btnUpdate.ClientID %>')[0].value = "Update";

                        //Set values
                        $('#<%= txtBxFirstName.ClientID %>')[0].value = FirstName;
                        $('#<%= txtBxLastName.ClientID %>')[0].value = LastName;
                        $('#<%= chkBxAdult.ClientID %>')[0].checked = Adult;
                        $('#<%= txtBxPGFName.ClientID %>')[0].value = PGFirstName;
                        $('#<%= txtBxPGLName.ClientID %>')[0].value = PGLastName;
                        $('#<%= txtBxMDOB.ClientID %>')[0].value = DOB;
                        var splitPrimNumbArea = PrimaryNumber.split(')');
                        var splitPrimNumb = splitPrimNumbArea.split('-');
                        if (splitPrimNumbArea.length > 0) {
                            $('#<%= txtBxMPrimNumbArea.ClientID %>')[0].value = splitPrimNumbArea[0].slice(1);
                            $('#<%= txtBxMPrimNumb.ClientID %>')[0].value = splitPrimNumb[0];
                            if (splitPrimNumb.length > 0) $('#<%= txtBxMPrimNumbLast4.ClientID %>')[0].value = splitPrimNumb[1];
                        }
                        var splitSecNumbArea = SecondaryNumber.split(')');
                        var splitSecNumb = splitSecNumbArea.split('-');
                        if (splitSecNumbArea.length > 0) {
                            $('#<%= txtBxMPrimNumbArea.ClientID %>')[0].value = splitSecNumbArea[0].slice(1);
                            $('#<%= txtBxMPrimNumb.ClientID %>')[0].value = splitSecNumb[0];
                            if (splitSecNumb.length > 0) $('#<%= txtBxMPrimNumbLast4.ClientID %>')[0].value = splitSecNumb[1];
                        }
                        $('#<%= ddlPreferCallBack.ClientID %>').val(PrefCallBack);
                        $('#<%= ddlHowDidYouHear.ClientID %>').val(HowDidYouHear);
                        $('#<%= txtBxEmailAdd.ClientID %>')[0].value = EmailAddress;
                        $('#<%= txtBxIntroStartDate.ClientID %>')[0].value = EventDateStart;
                        $('#<%= txtBxIntroEndDate.ClientID %>')[0].value = EventDateEnd;
                        $('#<%= ddlStart.ClientID %>').val(EventDateStartTime);
                        $('#<%= ddlEnd.ClientID %>').val(EventDateEndTime);
                        $('#<%= txtBxNotes.ClientID %>')[0].value = Notes;
                        $('#<%= hdCalendarEventID.ClientID %>')[0].value = id;
                        $('#<%= hdMemberID.ClientID %>')[0].value = MemberID;
                        $('#<%= ucCallHistory.ClientID %>_hdLeadID').val(MemberID);
                        LoadCallLog();

                    },
                    error: function () {
                        showMessageBox('Error loading lead', 2);
                    }
                }).always(hideLoading());
            }
        }

        function showNewLead() {
            showLoading();
            $find("PELead").show();
            var btn = $('#<%= btnUpdate.ClientID %>');
            btn[0].value = "Save";
            var hdUpdate = $('#<%=hdUpdate.ClientID %>');
            hdUpdate[0].value = "Save";
            ClearPopUp();
            $('#<%= tblSearch.ClientID %>')[0].style.display = "none";
                $('#<%= tblDetails.ClientID %>').find("input,select,textarea").prop("disabled", false);
            hideLoading();
        }

        function showSearchLead() {
            if ($("#<%=searchPanel.ClientID %>:first").is(":hidden")) {
                $("#<%=searchPanel.ClientID %>").slideDown("slow");
            } else {
                $("#<%=searchPanel.ClientID %>").slideUp("slow");
            }

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
            $('#<%= txtBxMPrimNumbArea.ClientID %>')[0].value = "";
            $('#<%= txtBxMPrimNumb.ClientID %>')[0].value = "";
            $('#<%= txtBxMPrimNumbLast4.ClientID %>')[0].value = "";
            $('#<%= txtBxMSecNumbArea.ClientID %>')[0].value = "";
            $('#<%= txtBxMSecNumb.ClientID %>')[0].value = "";
            $('#<%= txtBxMSecNumbLast4.ClientID %>')[0].value = "";
                $('#<%= ddlPreferCallBack.ClientID %>').val("Morning");
                $('#<%= ddlHowDidYouHear.ClientID %>').val("");
                $('#<%= txtBxEmailAdd.ClientID %>')[0].value = "";
                $('#<%= txtBxIntroStartDate.ClientID %>')[0].value = "";
                $('#<%= txtBxIntroEndDate.ClientID %>')[0].value = "";
                $('#<%= ddlStart.ClientID %>').val("0:00:00");
                $('#<%= ddlEnd.ClientID %>').val("0:00:0    0");
                $('#<%= txtBxNotes.ClientID %>')[0].value = "";
                $('#<%= btnUpdate.ClientID %>')[0].value = "Add Lead";
                $('#<%=txtBxIntroEndDate.ClientID %>').datepicker("option", "minDate", "");
                $('#<%=txtBxIntroStartDate.ClientID %>').datepicker("option", "maxDate", "");
                $('#<%= hdCalendarEventID.ClientID %>')[0].value = "";
        }

        function resizeModal()
        {
            d = document.getElementById('<%= pnlCallLog.ClientID%>');
            d.style.height = "80%";
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
    <div>
        <a href="#" id="lnkSearchLead" onclick="showSearchLead(); return false;">Search</a>
        | 
        <a href="#" id="lnkCreateLead" onclick="showNewLead(); return false;">New Lead</a>
    </div>
    <div id="searchPanel" style="display: none;" runat="server">
        <div style="float: left; width: 50%;">
            <asp:Label runat="server" ID="lblFirstNameSearch" Text="First Name"></asp:Label>
            <asp:TextBox runat="server" ID="txtBxFirstNameSearch"></asp:TextBox>
        </div>
        <div style="float: left;">
            <asp:Label runat="server" ID="lblLastNameSearch" Text="Last Name"></asp:Label>
            <asp:TextBox runat="server" ID="txtBxLastNameSearch"></asp:TextBox>
        </div>
        <div style="clear: both; float: left; width: 50%;">
            <asp:Label runat="server" ID="lblPhoneNumberSearch" Text="Phone Number"></asp:Label>
            <asp:TextBox runat="server" ID="txtBxPhoneNumberSearch"></asp:TextBox>
        </div>
        <div style="float: left;">
            <asp:Label runat="server" ID="lblPreferredCallBackSearch" Text="Preferred Call Back"></asp:Label>
            <asp:DropDownList runat="server" ID="ddlPreferredCallBackSearch"></asp:DropDownList>
        </div>
        <div style="clear: both; float: left; width: 50%"></div>
        <div style="float: left; width: 50%;">
            <asp:Button runat="server" ID="btnSearch" OnClick="btnSearch_Click" Text="Search" OnClientClick="showLoading();" />
        </div>
    </div>
    <div style="clear: both;">
        <asp:LinkButton ID="hlSchIntro" runat="server" Text="Schedule Intro" OnClick="hlSchIntro_Click" OnClientClick="showLoading();"></asp:LinkButton>
        <asp:LinkButton ID="hlNeedConfirm" runat="server" Text="Confirm Intro" OnClick="hlNeedConfirm_Click" OnClientClick="showLoading();"></asp:LinkButton>
    </div>
    <div>
        <asp:Button runat="server" ID="btnPopUp" Style="display: none;" />
        <asp:GridView runat="server" ID="grdCallList" GridLines="Both" AutoGenerateColumns="false" AllowPaging="true" OnPageIndexChanging="grdCallList_PageIndexChanging" PageSize="25"
            AllowSorting="false" OnRowCommand="grdCallList_RowCommand" DataKeyNames="MemberID" OnRowDeleting="grdCallList_RowDeleting">
            <Columns>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <label>Details</label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Button runat="server" ID="btnDetails" Text="Details" CommandName="Select" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <label>Name</label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblName" Text='<%#Eval("FullName")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <label>Parent/Guardian Name</label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblPGFullName" Text='<%#Eval("PGFullName")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <label>Number</label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblPrimNumber" Text='<%#Eval("PrimaryNumber")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <label>Preferred Call Back Time</label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblPCB" Text='<%#Eval("PreferredCallBack")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <label>Meeting Date</label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%--<asp:Label runat="server" ID="lblIntroMeeting" Text='<%#Eval("IntroMeetDate", "{0:M-dd-yyyy}")%>'></asp:Label>--%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <label>Delete</label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Button runat="server" ID="btnDelete" Text="Delete" CommandName="Delete" OnClientClick="return confirm('Do you want to delete this lead ? ');" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <asp:Panel runat="server" ID="pnlPopUp" CssClass="modalPopup" Style="display: none;">
        <asp:Table runat="server" Width="100%">
            <asp:TableRow>
                <asp:TableCell>
                    <span class="calendarMenuSelected" id="menuLead">
                        Lead Info
                    </span>
                    <span class="calendarMenu" id="menuCallLog">
                        Call History
                    </span>
                </asp:TableCell>
                <asp:TableCell ColumnSpan="4" HorizontalAlign="Right">
                    <asp:ImageButton runat="server" ID="btnCancel" CssClass="btnCancel" ImageUrl="~/App_Themes/bdsu/Images/close-button.png" />
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <asp:Table runat="server" ID="tblSearch" Style="display: none;">
            <asp:TableRow>
                <asp:TableCell ColumnSpan="2" HorizontalAlign="Left">
                    <asp:Label runat="server" ID="lblSearchFirstName" Text="Last Name:"></asp:Label>
                    <input type="text" id="txtLearner" style="width: 200px" />
                    <asp:Button runat="server" ID="btnSelectMember" Text="Search" OnClientClick="LoadMemeber(); return false;" />
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <asp:Table runat="server" ID="tblDetails" Style="visibility: visible; display: block; height: 525px;">
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
                    (<asp:TextBox Width="25" runat="server" ID="txtBxMPrimNumbArea" MaxLength="3" onkeypress="return isNumberKey(event)"></asp:TextBox>)
                    <asp:TextBox Width="25" runat="server" ID="txtBxMPrimNumb" MaxLength="3" onblur="ConvertPhone(this);"  />-
                    <asp:TextBox Width="30" runat="server" ID="txtBxMPrimNumbLast4" MaxLength="4" onblur="ConvertPhone(this);"  />
                </asp:TableCell>
                <asp:TableCell>
                    <asp:Label runat="server" ID="lblSecNumb" Width="150" Text="Secondary Number: " />
                </asp:TableCell>
                <asp:TableCell>
                    (<asp:TextBox Width="25" runat="server" ID="txtBxMSecNumbArea" MaxLength="3" onkeypress="return isNumberKey(event)"></asp:TextBox>)
                    <asp:TextBox Width="25" runat="server" ID="txtBxMSecNumb" MaxLength="3" onblur="ConvertPhone(this);"  />-
                    <asp:TextBox Width="30" runat="server" ID="txtBxMSecNumbLast4" MaxLength="4" onblur="ConvertPhone(this);"  />
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
                    <select style="margin-left:15px;" name="leadQuality" runat="server" id="leadQuality">
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
                <asp:TableCell VerticalAlign="Top" ColumnSpan="2">
                    <asp:Label runat="server" ID="lblAchieve" Text="What do they want to Achieve: " Style="float: left;" /><br />
                    <asp:CheckBoxList runat="server" ID="cblAchieve"></asp:CheckBoxList>
                </asp:TableCell>
                <asp:TableCell ColumnSpan="2">
                    <asp:Label runat="server" ID="lblNotes" Text="Notes: " Style="float: left;" /><br />
                    <asp:TextBox runat="server" ID="txtBxNotes" TextMode="MultiLine" Width="300" Height="150" Style="float: left; max-width: 300px; max-height: 200px; min-height: 200px; min-width: 300px;"></asp:TextBox>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell ID="tdConfirmLbl" Visible="false">
                    <asp:Label runat="server" ID="Label1" Text="Confirm Meeting Date: " />
                </asp:TableCell>
                <asp:TableCell ID="tdConfirmChk" Visible="false">
                    <asp:CheckBox runat="server" ID="chkBxConfirmMeeting" />
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell HorizontalAlign="Left" ColumnSpan="1">
                    <asp:Label runat="server" ID="Label3" Text="Meeting Date: " />
                </asp:TableCell>
                <asp:TableCell ColumnSpan="1">
                    <asp:TextBox runat="server" ID="txtBxIntroStartDate" CssClass="datepicker"></asp:TextBox>
                    <asp:DropDownList runat="server" ID="ddlStart" OnSelectedIndexChanged="ddlStart_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                </asp:TableCell>
                <asp:TableCell ColumnSpan="1">
                    <asp:Label runat="server" ID="lblEndDate" Text="Meeting End Date:"></asp:Label>
                </asp:TableCell>
                <asp:TableCell ColumnSpan="1">
                    <asp:TextBox runat="server" ID="txtBxIntroEndDate" CssClass="datepicker"></asp:TextBox>
                    <asp:DropDownList runat="server" ID="ddlEnd"></asp:DropDownList>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>

                    <asp:Button runat="server" ID="btnUpdate" Text="Update" Style="float: left;" OnClick="btnUpdate_Click" OnClientClick="return validate();" />
                    <asp:HiddenField runat="server" ID="hdMemberID" />
                    <asp:HiddenField runat="server" ID="hdHearAbout" />
                    <asp:HiddenField runat="server" ID="hdUpdate" Value="Update" />
                    <asp:HiddenField runat="server" ID="hdCalendarEventID" />
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <asp:Panel runat="server" ID="pnlCallLog" Style="visibility: hidden; display: block; height: 0px;">
            <uc:CallHistory runat="server" ID="ucCallHistory"></uc:CallHistory>
        </asp:Panel>
    </asp:Panel>
    <ajaxtoolkit:ModalPopupExtender ID="MPE" runat="server"
        BehaviorID="PELead"
        TargetControlID="btnPopUp"
        PopupControlID="pnlPopUp"
        BackgroundCssClass="modalBackground"
        CancelControlID="btnCancel"
        RepositionMode="RepositionOnWindowResizeAndScroll">
    </ajaxtoolkit:ModalPopupExtender>
</asp:Content>
