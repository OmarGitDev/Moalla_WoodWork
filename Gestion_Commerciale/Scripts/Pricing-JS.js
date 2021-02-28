function OpenEditorToAddPricing(PricingType) {

    Title = "Ajout " + PricingType
    $.ajax({
        type: "GET",
        url: "/Vente/OpenPricingEditorToAdd",
        data: { Type: PricingType },
        success: function (data) {
            debugger;

            $('#GenericModel').modal();
            $("#ModalTitle").text(Title);
            $("#ModalBody").html(data);

        },
        failure: function (response) { }
    });
}
function AddNewPricing() {
    debugger;
    var formData = $("#ParamForm").serializeArray();
   
        if (formData[7].value == "" || formData[7].value == null || formData[7].value == undefined) {
            toastr.error('Veuillez renseigner le champs Libellé', 'error', { progressBar: true, showDuration: 100 });
            return;
        }
        if (formData[8].value == "" || formData[8].value == null || formData[8].value == undefined || formData[8].value == "0") {
            toastr.error('Veuillez renseigner le champs Nom client', 'error', { progressBar: true, showDuration: 100 });
            return;
        }
        if (formData[9].value == "" || formData[9].value == null || formData[9].value == undefined) {
            toastr.error('Veuillez renseigner le champs date depuis', 'error', { progressBar: true, showDuration: 100 });
            return;
        }
        if (formData[10].value == "" || formData[10].value == null || formData[10].value == undefined) {
            toastr.error('Veuillez renseigner le champs date jusqu\'à', 'error', { progressBar: true, showDuration: 100 });
            return;
        }
   
    $.ajax({
        type: 'POST',
        url: "/Vente/AddNewPricing",
        data: formData,
        //dataType: 'json',
        //encode: true,
        success: function (response) {
            debugger;
            $('#GenericModel').modal('hide');
            $('#PricingListGrid').DataTable().ajax.reload();
            window.open("/Vente/PricingDetails?ID=" + response, "_self");
        },
    })
}
function LoadPricingListData(type) {

    var table = $('#PricingListGrid').DataTable({
        "order": [[2, "desc"]],
        "autoWidth": true,
        // "ScrollX":false,

        ajax: {
            url: "/Vente/GetAllPricings",
            data: { Type: type },
            dataSrc: ''
        },
        columns: [


        { "data": "ID" },
        {
            "data": "",
            "render": function (data, type, row) {
                debugger
                var ID = row.ID;
                return "<div style='display:inline-flex'><button onclick='OpenPricingDetail(" + "\"" + ID + "\"" + ")' style='width:30px;height:30px;' class='d-none d-sm-inline-block btn btn-sm btn-info shadow-sm'><span class='icon text-white-30'><i class='fas fa-list'></i></span><button onclick='DeletePricing(" + "\"" + ID + "\"" + ")' style='width:30px;height:30px;margin-left:10px;'  class='d-none d-sm-inline-block btn btn-sm btn-danger shadow-sm'><span class='icon text-white-30'><i class='fas fa-trash'></i></span></button></div>"
            }
        },
        {
            "data": "CodePricing"
        },
        { "data": "Libelle" },
        { "data": "Statut" },
        { "data": "NomClient" },
        {
            "data": "ValidFrom",
            render: function (data, type, row) {
                if (type === "sort" || type === "type") {
                    return data;
                }
                return moment(data).format("YYYY-MM-DD");
            }
        },
        {
            "data": "ValidUntil",
            render: function (data, type, row) {
                if (type === "sort" || type === "type") {
                    return data;
                }
                return moment(data).format("YYYY-MM-DD");
            }
        }
        ],
        "columnDefs": [
         {
             "targets": [0],
             "visible": false,
             "searchable": false
         }]
    });
    $('#PricingListGrid tbody').on('click', 'tr', function () {
        debugger;


        var tr = $(this).closest('tr');
        var row = $('#PricingListGrid').DataTable().row(tr);

        if (row.child.isShown()) {
            // This row is already open - close it
            row.child.hide();
            tr.removeClass('shown');
            //   $(this).closest("td").removeClass('fa fa-minus').addClass("fa fa-plus");
        }
        else {
            $(this).addClass('selected');
            // Open this row
            row.child(format(row.data())).show();
            //  $(this).closest("td").removeClass('fa fa-plus').addClass("fa fa-minus")
        }
    });

}
function DeletePricing(ID) {
    if (confirm('Veuillez confirmez la suppression de ce Pricing')) {
        $.ajax({
            type: 'POST',
            url: "/Vente/DeletePricing",
            data: { ID: ID },
            success: function (response) {
                toastr.success('Supprimé avec succès', 'Succès', { progressBar: true, showDuration: 100 });
                $('#PricingListGrid').DataTable().ajax.reload();

            },
        });
    }
}
function OpenPricingDetail(ID) {
    window.open("/Vente/PricingDetails?ID=" + ID, "_self");
}
//function PrintPricingDetails() {
    
//     window.open("/Print/PrintPricingDetails?CodePricing=" + CodePricing);
//}
function PrintPricingDetails() {
    var CodePricing = $("#CodePricing").val();
    $.ajax({
        url: "/Print/ImprimerRapportPricing",
        beforeSend: function () {
            $('#page').modal('show');

        },
        complete: function () {
            $('#page').modal('hide');
        },
        data: { CodePricing: CodePricing },

        error: function (xhr, textStatus, errorThrown) {
            alert2('Error while trying to Insert the invoice!', 'KO');
        },
        success: function (data) {
            debugger;
            //var resultArray = ReadJsonResult(data);
            var Status = data.Text;
            var Value = data.Value;
            if (Status != 'OK' && Status != '') {
                alert2(Value, Status);
            }
            else {
                OPEN_URL_IN_BLANK(Value);
            }
        }
    });

}
function SavePricingChanges() {
    var ID = $("#CurrentPricingID").val();
    var Libelle = $("#Libelle").val();
    var ClientID = $("#ClientID").val();
    var ValidFrom = $("#ValidFrom").val();
    var ValidUntil = $("#ValidUntil").val();
    if (Libelle == "" || Libelle == null || Libelle == undefined) {
        toastr.error('Le champs Libellé est non valide', 'error', { progressBar: true, showDuration: 100 });
        return;
    }
    if (ClientID == "" || ClientID == null || ClientID == undefined || ClientID == "0") {
        toastr.error('Le champs client est non valide', 'error', { progressBar: true, showDuration: 100 });
        return;
    }
    if (ValidFrom == "" || ValidFrom == null || ValidFrom == undefined) {
        toastr.error('Le champs date depuis est non valide', 'error', { progressBar: true, showDuration: 100 });
        return;
    }
    if (ValidUntil == "" || ValidUntil == null || ValidUntil == undefined) {
        toastr.error('Le champs date jusqu\'à est non valide', 'error', { progressBar: true, showDuration: 100 });
        return;
    }
    $.ajax({

        url: "/Vente/SavePricingChanges",
        data: { ID: ID, Libelle: Libelle, ClientID: ClientID, ValidFrom: ValidFrom, ValidUntil: ValidUntil },
        success: function (response) {
            toastr.success('Enregistré avec succès', 'Succès', { progressBar: true, ShowDuration: 500 });              
        },
    })
}
function OpenDetailsPricingEditor(ID) {
    var PricingID = $("#CurrentPricingID").val();
    debugger;
    $.ajax({
        type: "GET",
        url: "/Vente/OpenDetailsPricingEditor",
        data: { ID: ID, PricingID: PricingID },
        success: function (data) {
            debugger;

            $('#GenericModel').modal();
            $("#ModalTitle").text("Detail Pricing ");
            $("#ModalBody").html(data);

        },
        failure: function (response) { }
    });
}

function LinkServicesToPricing() {
    debugger;
    var formData = $("#ParamForm").serialize();
    $.ajax({
        type: 'POST',
        url: "/Vente/LinkServicesToPricing",
        data: formData,
        error: function (response) {
            debugger;
        },
        success: function (data) {
            var t1 = data.Val1;
            var t2 = data.Val2;
            $("#MontantTotalValue").text(t1);
            $("#MontantFinalValue").text(t2);

            debugger;
            $('#GenericModel').modal('hide');
            $('#PricingDetailsGrid').DataTable().ajax.reload();
            RefreshServicesGrid();

            toastr.success('Opération réussite', 'Succès', { progressBar: true, showDuration: 100 });

        },
    });

}
function RefreshServicesGrid() {
    debugger;
    $('#PricingServicesSelectorGrid').DataTable().destroy();
    LoadPricingServicesSelectorData();
}
function OpenEditorToFillPricing() {
    debugger;
    var SelectedServices = $("#PricingServicesSelectedItems").val();
    var test = "************************************************";
    if (SelectedServices == "" || test.includes(SelectedServices)) {
        toastr.warning('Veuillez sélectionner des détails', 'Erreur!', { progressBar: true, showDuration: 100 });
        return;
    }
    var numPricing = $("#CurrentPricingID").val();
    $.ajax({
        type: "GET",
        url: "/Vente/OpenEditorToFillPricing",
        data: { SelectedServices: SelectedServices, codePricing: numPricing },
        success: function (data) {
            $('#GenericModel').modal();
            $("#ModalTitle").text("Lier la liste des Produits");
            $("#ModalBody").html(data);
            $("#PricingServicesSelectedItems").val("");
            $(".selected").removeClass("selected");

        },
        failure: function (response) { }
    });
}
function AddOrUpdateDetailsPricing() {
    debugger;
    var formData = $("#ParamForm").serializeArray();
    if (formData[6].value == "" || formData[6].value == null || formData[6].value == undefined) {
        toastr.error('Veuillez renseigner le champs Code', 'error', { progressBar: true, showDuration: 100 });
        return;
    }
    if (formData[7].value == "" || formData[7].value == null || formData[7].value == undefined) {
        toastr.error('Veuillez renseigner le champs Libellé', 'error', { progressBar: true, showDuration: 100 });
        return;
    }
    if (formData[8].value == "" || formData[8].value == null || formData[8].value == undefined) {
        toastr.error('Veuillez renseigner le champs Remise', 'error', { progressBar: true, showDuration: 100 });
        return;
    }
    if (formData[9].value == "" || formData[9].value == null || formData[9].value == undefined || formData[9].value == "0") {
        toastr.error('Veuillez renseigner le champs Taxe', 'error', { progressBar: true, showDuration: 100 });
        return;
    }
    if (formData[11].value == "" || formData[11].value == null || formData[11].value == undefined) {
        toastr.error('Veuillez renseigner le champs Prix unitaire', 'error', { progressBar: true, showDuration: 100 });
        return;
    }
    if (formData[12].value == "" || formData[12].value == null || formData[12].value == undefined) {
        toastr.error('Veuillez renseigner le champs Quantité', 'error', { progressBar: true, showDuration: 100 });
        return;
    }
    $.ajax({
        type: 'POST',
        url: "/Vente/AddOrUpdatePricingDetails",
        data: formData,
        dataType: 'json',
        encode: true,
        success: function (data) {
            debugger;
            var t1 = data.Val1;
            var t2 = data.Val2;

            $("#MontantTotalValue").text(t1);
            $("#MontantFinalValue").text(t2);

            $('#GenericModel').modal('hide');
            $('#PricingDetailsGrid').DataTable().ajax.reload();
            toastr.success('Opération réussite', 'Succès', { progressBar: true, showDuration: 100 });

        },
    });
}
function LoadPricingServicesSelectorData() {
    debugger;
    var libelle = $("#LibelleFilter").val();
    var code = $("#ReferenceFilter").val();
    var tableP = $('#PricingServicesSelectorGrid').DataTable({
        "order": [[0, "desc"]],

        "autoWidth": true,
        ajax: {
            url: "/Stock/GetAllServices",
            data: { libelle: libelle, code: code,Vente:true,Achat:false },
            dataSrc: ''
        },
        columns: [{
            "data": "ID"

        },
        { "data": "Libelle" },

        ],

    });

}
function DeleteDetailsPricing(ID) {
    if (confirm('Veuillez confirmer la suppression de ce détail')) {
        var formData = $("#ParamForm").serializeArray();
        $.ajax({

            url: "/Vente/DeleteDetailsPricing",
            data: { ID: ID },


            success: function (data) {
                var t1 = data.Val1;
                var t2 = data.Val2;
                $("#MontantTotalValue").text(t1);
                $("#MontantFinalValue").text(t2);

                debugger;
                toastr.success('Supprimé avec succès', 'Succès', { progressBar: true, showDuration: 100 });
                $('#PricingDetailsGrid').DataTable().ajax.reload();
                RefreshServicesGrid();

            },
        });
    }
}
function LoadPricingDetailsData() {
    var table = $('#PricingDetailsGrid').DataTable({
        "order": [[1, "desc"]],
        "autoWidth": true,

        ajax: {
            url: "/Vente/GetAllPricingDetails",
            data: { ID: $("#CurrentPricingID").val() },
            dataSrc: ''
        },
        columns: [{ "data": "ID" },
        {
            "data": "",
            "render": function (data, type, row) {
                debugger
                var ID = row.ID;
                return "<div style='display:inline-flex'><button onclick='OpenDetailsPricingEditor(" + ID + ")' style='width:30px;height:30px;' class='d-none d-sm-inline-block btn btn-sm btn-info shadow-sm'><span class='icon text-white-30'><i class='fas fa-list'></i></span><button onclick='DeleteDetailsPricing(" + ID + ")' style='width:30px;height:30px;margin-left:10px;'  class='d-none d-sm-inline-block btn btn-sm btn-danger shadow-sm'><span class='icon text-white-30'><i class='fas fa-trash'></i></span></button></div>"
            }
        },
        { "data": "CodeDetail" },
        { "data": "Libelle" },
        { "data": "Quantite" },
        {
            "data": "MontantHorsTaxe",
            "render": function (data, type, row) {
                if (data != null)
                    return data.toFixed(3);
                else {
                    return data;
                }
            }
        },
        {
            "data": "MontantTotal",
            "render": function (data, type, row) {
                if (data != null)
                    return data.toFixed(3);
                else {
                    return data;
                }
            }
        },
        { "data": "Description" }
        ],
        "columnDefs": [
         {
             "targets": [0],
             "visible": false,
             "searchable": false
         }]
    });
}
function DeleteAllDetailsPricing() {
    var PricingID = $("#CurrentPricingID").val();
    if (confirm('Veuillez confirmer la suppression des détails')) {
        
        $.ajax({

            url: "/Vente/DeleteAllDetailsPricing",
            data: { PricingID: PricingID },


            success: function (data) {
                var t1 = data.Val1;
                var t2 = data.Val2;

                $("#MontantTotalValue").text(t1);
                $("#MontantFinalValue").text(t2);

                debugger;
                toastr.success('Supprimé avec succès', 'Succès', { progressBar: true, showDuration: 100 });
                $('#PricingDetailsGrid').DataTable().ajax.reload();
                RefreshServicesGrid();

            },
        });
    }
}
function ConvertDevisToInvoice()
{
    var PricingID = $("#CurrentPricingID").val();
        
        $.ajax({

            url: "/Vente/ConvertDevisToInvoice",
            data: { PricingID: PricingID },


            success: function (data) {
                window.open("/Common/FactureDetails?NumPiece=" + data + "&Type=CFAC", "_self");
            },
        });
    
}