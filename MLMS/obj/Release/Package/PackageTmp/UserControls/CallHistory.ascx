<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CallHistory.ascx.cs" Inherits="MLMS.UserControls.CallHistory" %>
<script type="text/javascript" src="../Scripts/jquery.jqpagination.js"></script>
<script type="text/javascript">
    $(document).ready(function () {
        $('#<%=txtBxCallLogDate.ClientID %>').datepicker({
            onClose: function (selectedDate) {
                isValidDate(selectedDate);
            }
        });
    });

    function validateCallLog() {
        showLoading();
        var date = $.trim($('#<%=txtBxCallLogDate.ClientID %>').val());
        var memberID = $('#<%=hdLeadID.ClientID %>').val();

        if ($('#<%= txtBxCallLogDate.ClientID %>')[0].value.length == 0) {
            showMessageBox("Call log date can not be blank", 2);
            $('#<%= txtBxCallLogDate.ClientID %>')[0].style.border = "2px solid red";
            hideLoading();
            return false;
        }
        else if (!isValidDate($('#<%= txtBxCallLogDate.ClientID %>').val())) {
            showMessageBox('Call log date is not a valid Date', 2);
            hideLoading();
            return false;
        }
        if (memberID != "") {
            if (date.length > 0) {
                $.ajax({
                    type: 'POST',
                    url: '../Services/MemberWebService.asmx/AddCallHistory',
                    data: '{ "LeadID":"' + memberID + '", "Date":"' + $('#<%=txtBxCallLogDate.ClientID %>').val() + '", "Time":"' + $('#<%=ddlCallStart.ClientID %>').val() + '", "Notes":"' + $('#<%=txtBxCallNotes.ClientID %>').val() + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        showMessageBox('Member info saved.', 1);
                        CancelCallHistory();
                        $("#callHistory").find('tr').slice(1, -1).remove();
                        ($('#callHistory > tbody')).prepend(data.d);

                        $table_rows_CallHistory = $('#callHistory  tbody tr');

                        var table_row_limit_CallHistory = 10;

                        var page_tableCallHistory = function (page) {

                            // calculate the offset and limit values
                            var offsetCallHistory = (page - 1) * table_row_limit_CallHistory,
                                limitCallHistory = page * table_row_limit_CallHistory;

                            // hide all table rows
                            $table_rows_CallHistory.hide();

                            // show only the n rows
                            $table_rows_CallHistory.slice(offsetCallHistory, limitCallHistory).show();

                        }

                        $('.paginationCallHistory').jqPagination({
                            max_page: Math.round($table_rows_CallHistory.length / table_row_limit_CallHistory),
                            paged: page_tableCallHistory
                        });

                        // set the initial table state to page 1
                        page_tableCallHistory(1);

                        $('#callHistory > tbody tr:last').css("display", "none");
                        hideLoading();
                    },
                    error: function () {
                        showMessageBox('Error creating call log', 2);
                        hideLoading();
                    }
                });
            }
            else {
                showMessageBox('Can not add a call log without a date', 2);
                hideLoading();
            }
        }
        else {
            showMessageBox('Error creating call log', 2);
            hideLoading();
        }
    }

    function LoadCallLog() {

        var memberID = $('#<%=hdLeadID.ClientID %>').val();
        if (memberID != "") {
            $.ajax({
                type: 'POST',
                url: '../Services/MemberWebService.asmx/GetCallHistory',
                data: '{ "LeadID":"' + memberID + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    CancelCallHistory();

                    $("#callHistory").find('tr').slice(1, -1).remove();
                    ($('#callHistory > tbody')).prepend(data.d);

                    $table_rows_CallHistory = $('#callHistory tbody tr');

                    var table_row_limit_CallHistory = 5;

                    var page_tableCallHistory = function (page) {

                        // calculate the offset and limit values
                        var offsetCallHistory = (page - 1) * table_row_limit_CallHistory,
                            limitCallHistory = page * table_row_limit_CallHistory;

                        // hide all table rows
                        $table_rows_CallHistory.hide();

                        // show only the n rows
                        $table_rows_CallHistory.slice(offsetCallHistory, limitCallHistory).show();

                    }

                    $('.paginationCallHistory').jqPagination({
                        max_page: Math.round($table_rows_CallHistory.length / table_row_limit_CallHistory),
                        paged: page_tableCallHistory
                    });

                    // set the initial table state to page 1
                    page_tableCallHistory(1);

                    $('#callHistory > tbody tr:last').css("display", "none");

                },
                error: function () {
                    showMessageBox('Error getting call log', 2);

                }
            });
        }
    }

    function EditCallHistory(id) {
        showLoading();
        var i = id.slice(-1);
        var btn = $('#' + id);
        var btnText = $('#' + id).val();

        if (btnText == "Edit") {
            CancelCallHistory();
            var notes = $('#lblNotes' + i);

            //Show text box and cancel button
            $('#txtBxNotesUpdate' + i).show().val(notes.text());
            $('#btnCancel' + i).show();

            //Hide label on that row
            notes.hide();

            //Change current row to Update
            btn.val("Update");

            btn.parent().parent().addClass("selected");
            hideLoading();
        }
        else {
            var notes = $('#txtBxNotesUpdate' + i).val();

            var memberID = $('#hdMemberCallLogID' + i).val()
            if (memberID != "") {
                $.ajax({
                    type: 'POST',
                    url: '../Services/MemberWebService.asmx/UpdateCallHistory',
                    data: '{ "MemberCallLog":"' + memberID + '", "Notes":"' + notes + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        CancelCallHistory();

                        $('#lblNotes' + i).text(notes);
                        showMessageBox('Member info updated', 1);
                        hideLoading();
                    },
                    error: function () {
                        showMessageBox('Error updating member', 2);
                        hideLoading();
                    }
                });
            }
            else {
                showMessageBox('Error updating member', 2);
                hideLoading();
            }
        }
    }

    function DeleteCallHistory(id) {
        var c = confirm("Are you sure you want to delete?");
        if (c == true) {
            showLoading();
            var i = id.slice(-1);
            var memberID = $('#hdMemberCallLogID' + i).val();

            if (memberID != "") {
                $.ajax({
                    type: 'POST',
                    url: '../Services/MemberWebService.asmx/DeleteCallHistory',
                    data: '{ "MemberCallLog":"' + memberID + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function () {
                        CancelCallHistory();

                        $('#' + id).parent().parent().remove();
                        showMessageBox('Member info deleted', 1);
                        hideLoading();
                    },
                    error: function () {
                        showMessageBox('Error deleting member', 2);
                        hideLoading();
                    }
                });
            }
            else {
                showMessageBox('Error deleting call log', 2);
                hideLoading();
            }
        }
        else {
            return;
        }
    }

    function CancelCallHistory() {
        $('.editCallHistory').val("Edit");
        $('.cancelCallHistory').hide();

        //Hide all other textboxes
        $('#callHistory textarea').hide().val("");
        $('#callHistory input:text').hide().val("");
        //Show all other labels that were previously hidden
        $('#callHistory span').show().val("");
        //Remove styles on rows
        $('#callHistory tr').removeClass();

        $('#addNewCallHistory').hide();
    }

    function ShowAddRow() {
        CancelCallHistory();

        $('#addNewCallHistory').show();
        $('#addNewCallHistory textarea').show();
        $('#addNewCallHistory input:text').show();

        resizeModal();
    }
</script>
<style type="text/css">
    .events {
        border-collapse: collapse;
        border: 1px solid black;
    }

        .events th {
            border-collapse: collapse;
            border: 1px solid black;
            width: 200px;
        }

        .events td {
            border-collapse: collapse;
            border: 1px solid black;
        }

    .selected {
        background-color: lightgray;
    }
</style>
<div id="callLogHeader">
    <asp:HiddenField runat="server" ID="hdLeadID" />
    <button id="btnAdd" onclick="ShowAddRow(); return false;">Add Call</button>
</div>

<table id="callHistory" class="events" style="border-collapse: collapse; border: 1px solid black;">
    <thead>
        <tr>
            <th>Edit</th>
            <th>Date Called</th>
            <th>Notes</th>
            <th>Delete</th>
        </tr>
    </thead>
    <tbody>
        <tr id="addNewCallHistory" style="display: none;">
            <td>
                <asp:Button runat="server" ID="btnAddCall" Text="Add" OnClientClick="validateCallLog(); return false;" />
                <button id="cancelAddCall" onclick="CancelCallHistory(); return false">Cancel</button>
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtBxCallLogDate" class="callDatePicker"></asp:TextBox>
                <asp:DropDownList runat="server" ID="ddlCallStart" AutoPostBack="false"></asp:DropDownList>
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtBxCallNotes" TextMode="MultiLine" Style="max-width: 300px; max-height: 200px; min-height: 200px; min-width: 300px;"></asp:TextBox></td>
            <td></td>
        </tr>
    </tbody>
</table>
<div class="paginationCallHistory">
    <a href="#" class="first" data-action="first">&laquo;</a>
    <a href="#" class="previous" data-action="previous">&lsaquo;</a>
    <input type="text" readonly="readonly" data-max-page="40" />
    <a href="#" class="next" data-action="next">&rsaquo;</a>
    <a href="#" class="last" data-action="last">&raquo;</a>
</div>
