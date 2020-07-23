
var arr = [,];
var ad=0;
$(document).ready(function () {
   

    var param = "{}";
    var listitems;
    var $select = $('#Frequency');
    $.ajax({
        type: 'POST',
        url: '/DoctorAppointment/GetPrescriptionFrequency',
        data: param,
        dataType: 'json',
        success: function (response) {
            if (response.Success == true) {

                if (response.Frequency != null) {
                    $.each(response.Frequency, function (item) {
                        listitems += '<option value=' + response.Frequency[item].FrequencyName + '>' + response.Frequency[item].FrequencyName + '</option>';

                    });
                    $select.append(listitems);

                }
            }
            //else {response.Message;}

            return false;
        },
        //error: errorRes

    });
    
    $("#txtDrugName").autocomplete({
        //var key=$(this).val();
        source: function (request, response) {
            $.ajax({
                type: "POST",

                url: "/DoctorAppointment/GetDrugSearchResult",

                dataType: "json",
                data: { "P_Name": request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        
                        return { label: item.DisplayName, value: item.FullName};
                        
                    }))
                }
            });
        }
        
    }); 




    //$("#txtDrugName").chosen();
    //$("#choosenStandard_chosen").find(".chosen-search > input[type='text']").keyup(function (e) {
    //    //debugger;
    //    //var Index = $('#choosenICDCodestableIndex').val();
    //    var key = $(this).val();
    //    var SmartSearchStandard = {
    //        SmartSearch: key
    //    }
    //    if ((key.length >= 1) && !(e.keyCode === 38 || e.keyCode === 40)) {
    //        ajaxStart();
    //        alert("felix");
    //        //AjaxCallAsync("/Compliance/GetStandardNameSmartSearch", "POST", SmartSearchStandard, GetStandardNameSmartSearchSuccessCallBack, '', '');


    //    }
    //});


    //$("#btnAdd").click(function (value) {
    //    var Shedule = null;
    //    var Food = null;
    //    for (i = 1; i <= 4; i++) {
    //        if ($('#Sh'+i).is(':checked')) {
    //            if (Shedule != null)
    //                Shedule = Shedule+' , '+ $('#Sh' + i).val();
    //            else
    //                Shedule = $('#Sh' + i).val();
    //        }
    //    }
        
    //    for (i = 1; i <= 2; i++) {
    //        if ($('#Fs' + i).is(':checked')) {
    //            if (Food != null)
    //                Food = Food + ' , ' + $('#Fs' + i).val();
    //            else
    //                Food = $('#Fs' + i).val();
    //        }
    //    }

    //    var Ferquency = $("#Frequency").val();
    //   // alert($("btnAdd").val())
    //    var sel = {
            
    //        "No": ad = ad + 1,
    //        "AppId": $("btnAdd").val(),
    //        "DrugName": $("#txtDrugName").val(),
    //        "DrugFrequency": Ferquency,
    //        "Schedule": Shedule,
    //        "Dose": $("#txtDose").val(),
    //        "Days": $("#txtDays").val(),
    //        "Quantity": $("#txtQuantity").val(),
    //        "FoodStatus": Food,
            
    //    };
    //    arr.push(sel);
    //    //if (ad != null) {
    //    //    arr.push(sel);
    //    //    alert(arr);
    //    //    ad = ad + ',' + JSON.stringify(sel);
    //    //   // alert(ad);
    //    //}
    //    //else
    //    //     ad = sel;

    //    //if (RemovedItem != null) {

    //    //    bindDescriptionTable();
    //    //    var result = arrayRemove(array, 6);
    //    //}
    //   // else
    //        bindDescriptionTable();
    //});

    ////$('#Descriptiontable').DataTable({
    //    'order': [[0, 'asc']],
    //    'columnDefs': [
    //        { visible: false, targets: [0] }

    //    ],
    //    "bAutoWidth": false, // Disable the auto width calculation 
    //    "aoColumns": [
            
    //        { "sWidth": "30%" },
    //        { "sWidth": "15%" },
    //        { "sWidth": "15%" },
    //        { "sWidth": "15%" },
    //        { "sWidth": "10%" },
    //        { "sWidth": "10%" }
    //    ]
    //});
    //$("#SavePrescription").click(function () {
    //    var cUrl ='@Url.'
    //        '@Url.Action("SaveDrugPrescription","DoctorAppointment")';
    //    $.post(cUrl, arr, function (oResult) {
    //        if (oResult.response.success) {
    //            alert("Success");
    //        }
    //        // $("#divPharmacySearchBody").html(oResult);
    //        // $('#mdlPharmacy').modal('show');
    //    });

    //});
});



function AddTable(AppId)
{
    if ($("#txtDrugName").val() == '')
    {
        showAlert("Alert", "Please Select DrugName !!!");
        return;
    }
    var Ferquency = $("#Frequency option:selected").text();
    if (Ferquency == '') {
        showAlert("Alert", "Please Select DrugName !!!");
        return;
    }
    if ($("#txtDose").val() == '') {
        showAlert("Alert", "Please Fill Dose !!!");
        return;
    }
    if ($("#txtDays").val() == '') {
        showAlert("Alert", "Please Fill Days !!!");
        return;
    }
    if ($("#txtQuantity").val() == '') {
        showAlert("Alert", "Please Fill Quantity !!!");
        return;
    }
   
    var Shedule = null;
    var Food = null;
    for (i = 1; i <= 4; i++) {
        if ($('#Sh' + i).is(':checked')) {
            if (Shedule != null) {
                Shedule = Shedule + ' , ' + $('#Sh' + i).val();
                $("#Sh" + i).prop("checked", false);
            }
            else {
                Shedule = $('#Sh' + i).val();
                $("#Sh" + i).prop("checked", false);
            }
        }
    }
    if (Shedule == null) {
        showAlert("Alert", "Please Select Shedule !!!");
        return;
    }
    for (i = 1; i <= 2; i++) {
        if ($('#Fs' + i).is(':checked')) {
            if (Food != null) {
                Food = Food + ' , ' + $('#Fs' + i).val();
                $("#Fs" + i).prop("checked", false);
            }
            else {
                Food = $('#Fs' + i).val();
                $("#Fs" + i).prop("checked", false);
            }
        }
    }
    if (Food == null) {
        showAlert("Alert", "Please Select Food Status !!!");
        return;
    }

    
    // alert($("btnAdd").val())
    var sel = {

        "No": ad = ad + 1,
        "AppId": AppId,
        "DrugName": $("#txtDrugName").val(),
        "DrugFrequency": Ferquency,
        "Schedule": Shedule,
        "Dose": $("#txtDose").val(),
        "Days": $("#txtDays").val(),
        "Quantity": $("#txtQuantity").val(),
        "FoodStatus": Food,
        "FullName": FullName,
        "DrugCode": DrugCode,
        "DrugName": DisplayName
    };
    arr.push(sel);
    //if (ad != null) {
    //    arr.push(sel);
    //    alert(arr);
    //    ad = ad + ',' + JSON.stringify(sel);
    //   // alert(ad);
    //}
    //else
    //     ad = sel;

    //if (RemovedItem != null) {

    //    bindDescriptionTable();
    //    var result = arrayRemove(array, 6);
    //}
    // else
    $('#txtDrugName').val('');
    $('#txtDose').val('');
    $("#txtDays").val('');
    $("#txtQuantity").val('');
    $("#txtQuantity").val('');
    $("#Frequency").val('');
    
    $("#lblDescription").val('');
    bindDescriptionTable();



}
var arr = [];

function bindDescriptionTable() {
    var dataArray = arr;
   // var dataArray = $.parseJSON('[' + Description + ']');
    //arr = dataArray;
    $('#Descriptiontable').DataTable().clear();
    for (var i = 0; i < dataArray.length; i++) {
        $('#Descriptiontable').dataTable().fnAddData([
            //dataArray[i].No,
            dataArray[i].DrugName,
            dataArray[i].Quantity,
            dataArray[i].DrugFrequency,
            dataArray[i].Schedule,
            dataArray[i].FoodStatus,
            dataArray[i].Dose,
            dataArray[i].Days,
            //ToJavaScriptDateMedicine(dataArray[i].reporteddate),
            //"<div class='btn-group'> <button type='button' class='btn btn-primary'>Remove</button>" +
            ////"<button type='button' class='btn btn-primary'  aria-expanded='false' onclick='Remove(" + i + ")'>"
            //+
            //"<span class='caret'></span>" +
            //"<span class='sr-only'>Toggle Dropdown</span>" +
            //"</button>" +
            //"<ul class='dropdown-menu' role='menu'>" +
            
            //"<li>" +
            //"<a href='javascript:void(0)' id='delete' onclick='Remove(" + i + ");'>Delete</a></li>" +
            ////"<button id='delete' type='button' class='btn btn-link submit' style='border-bottom:none' onclick='deleteMedicine(" + Medications[i].medicationID + ");'>Delete</button></li>" +
            //"</ul>" +
            //"</div>"

            "<a class='btn btn-primary editbtn' onclick='Remove(" + dataArray[i].No + ")'>Remove</a>"

            //"<div class='btn-group'> <button type='button' class='btn btn-primary'>Remove</button>" +
            //"<button type='button' class='btn btn-primary dropdown-toggle' data-toggle='dropdown' aria-expanded='false'>" +
            //"<span class='caret'></span>" +
            //"<span class='sr-only'>Toggle Dropdown</span>" +
            //"</button>" +
            //"<ul class='dropdown-menu' role='menu'>" +
            //"<li>" +
            //"<a class='editbtn' onclick='Remove(" + dataArray[i].No + ")'>Remove</a>" +
            //"</li>" +
            ////"<li>" +
            ////"<a href='javascript:void(0)' id='delete' onclick='deleteMedicine(" + Medications[i].medicationID + ");'>Delete</a></li>" +
            ////"<button id='delete' type='button' class='btn btn-link submit' style='border-bottom:none' onclick='deleteMedicine(" + Medications[i].medicationID + ");'>Delete</button></li>" +
            //"</ul>" +
            //"</div>"

        ]);
    }
    $('#Descriptiontable').DataTable().draw();
}
//var RemovedItem = null;
function Remove(Rom) {
    

    arr = jQuery.grep(arr, function (n) {
        return n.No != Rom;   //or n[1] for second item in two item array
    });
    arr;

    //for (var i = 0; i < arr.length; i++) {
    //    if (arr[i] === Rom)
    //    { arr.splice(i, 1); }
    //}
   // _.remove(arr, function (e) {
    //    return e.Rom == Rom;
        
    //});
   // delete arr[Rom] == Rom;
    //arr.remove(Rom);
    bindDescriptionTable();
    //if (RemovedItem==null)
    //    RemovedItem = Rom;
    //else
    //    RemovedItem = RemovedItem + ',' + Rom;
 }

var DrugCode;
var FullName;
var DisplayName;
function GetDrugDescription() {
    DrugCode = null;
    FullName = null;
    DisplayName = null;
    var prefix = $("#txtDrugName").val();
        $.ajax({
            type: 'POST',
            url: '/DoctorAppointment/GetDrugDescription',
            data: { "prefix": prefix },
            dataType: 'json',
            success: function (response) {
                if (response.Success == true) {

                    if (response.Medicines.length > 0) {
                        medicines = response.Medicines;
                        var medicinesArray = $.map(medicines, function (el) {

                            //return el.medicineName;
                            $('#lblDescription').val(el.Description);
                            DrugCode = el.DrugCode;
                            FullName = el.FullName;
                            DisplayName = el.DisplayName;
                        //$('#lblDescription').autocomplete({
                        //    lookup: el.Description
                        //    });
                        });
                    }

                }
                return false;
                // else {alert(data.ErrorMessage);}


            },
            //error: errorRes

        });

    


  



}

function SavePrescription() {
    $('#Descriptiontable').DataTable().clear();

    arr = JSON.stringify({ 'arr': arr });
    showLoader();
    $.ajax({
        contentType: 'application/json; charset=utf-8',
        type: 'POST',
        url: '/DoctorAppointment/SaveDrugPrescription',
        data: arr,
        dataType: 'json',
        success: function (response) {
            if (response.Message != undefined) {
                new PNotify({
                    title: 'Error',
                    text: response.Message,
                    type: 'error',
                    styling: 'bootstrap3'
                });
            }
            if (response.Success == true) {
                if (response.ApiResultModel.message == "")
                    new PNotify({
                        title: 'Success',
                        text: "Prescription Saved successfully.",
                        type: 'info', addclass: 'dark',
                        styling: 'bootstrap3'
                    });
                //removeMedicine(response.ApiResultModel.ID);
                //bindMedicinesTable(_medicationTable);
                $('#Descriptiontable').hide();

                //var cCallLogUrl = 'DoctorAppointment/Index';
                //$.post(cCallLogUrl);
                arr = null;
                hideLoader();
               // window.location.href = "http://localhost:9080/DoctorAppointment/Index"
                
                //bindDescriptionTable();

            }
            else if (response.ApiResultModel.message != "") {
                new PNotify({
                    title: 'Error',
                    text: response.ApiResultModel.message,
                    type: 'error',
                    styling: 'bootstrap3'
                });
                $('#Descriptiontable').DataTable().clear();
                arr = null;
                var cCallLogUrl = 'DoctorAppointment/Index';
                $.post(cCallLogUrl);
                hideLoader();
            }




        },
        //hideLoader();
        //error: errorRes


    });



}