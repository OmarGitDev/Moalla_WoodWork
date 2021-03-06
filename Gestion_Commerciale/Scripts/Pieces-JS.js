
function TransformToCFAC()
{
    var NumPiece = $("#NumPiece").val();
    debugger;
    $.ajax({
        type: "GET",
        url: "/Common/TransformToCFAC",
        data: {  NumPiece: NumPiece },
        success: function (data) {
            debugger;
            toastr.success('Opération réussite', 'Succès', { progressBar: true, showDuration: 100 });

            window.location.href = "/Common/FactureDetails?NumPiece=" + data + "&Type=CFAC";

        },
        failure: function (response) { }
    });
}
function RefreshExternalReglementsGrid(Owner)
{
    debugger;
    $('#ExternalReglementsGrid').DataTable().destroy();
    LoadExternalReglementsData(Owner);
}
function OpenEditorToAddFacture(PieceType) {
    var Title = "";
    //var PieceType = $("#TypePieces").val();
    if(PieceType == 'CFAC' || PieceType == 'FFAC')
        Title = "Nouvelle facture"
    else if (PieceType == "BCOM")
    {
        Title = "Bon de commande"
    }
    else if (PieceType == "BLIV") {
        Title = "Bon de livraison"
    }
    $.ajax({
        type: "GET",
        url: "/Common/OpenPieceEditorToAdd",
        data: { Type: PieceType },
        success: function (data) {
            debugger;

            $('#GenericModel').modal();
            $("#ModalTitle").text(Title);
            $("#ModalBody").html(data);

        },
        failure: function (response) { }
    });
}
function AddNewPiece(TypePieces) {
    debugger;
    var formData = $("#ParamForm").serializeArray();
    var typePiece = formData[8].value;
    if (typePiece == 'FFAC' || typePiece == 'FFAC' || typePiece == 'BLIV' || typePiece == 'BCOM') {
        if (formData[9].value == "" || formData[9].value == null || formData[9].value == undefined || formData[9].value == "0") {
            toastr.error('Veuillez renseigner le champs Fournisseur', 'error', { progressBar: true, showDuration: 100 });
            return;
        }
    }
    else {
        if (formData[10].value == "" || formData[10].value == null || formData[10].value == undefined || formData[10].value == "0") {
            toastr.error('Veuillez renseigner le champs Client', 'error', { progressBar: true, showDuration: 100 });
            return;
        }
    }

   
    $.ajax({
        type: 'POST',
        url: "/Common/AddNewPiece",
        data: formData,
        //dataType: 'json',
        //encode: true,
        success: function (response) {
            debugger;
            $('#GenericModel').modal('hide');
            $('#PieceListGrid').DataTable().ajax.reload();
            window.open("/Common/FactureDetails?NumPiece=" + response + "&Type=" + TypePieces,"_self");
        },
    })
}
function OpenDetailsPieceEditor(ID)
{
    var NumPiece = $("#NumPiece").val();
    debugger;
    $.ajax({
        type: "GET",
        url: "/Common/OpenDetailsPieceEditor",
        data: { ID: ID,NumPiece:NumPiece },
        success: function (data) {
            debugger;

            $('#GenericModel').modal();
            $("#ModalTitle").text("Detail Piece ");
            $("#ModalBody").html(data);

        },
        failure: function (response) { }
    });
}
function AddOrUpdateDetailsPiece() {
    debugger;
    var formData = $("#ParamForm").serializeArray();
    //if (formData[2].value == "" || formData[2].value == null || formData[2].value == undefined) {
    //    toastr.error('Veuillez renseigner le champs Code', 'error', { progressBar: true, showDuration: 100 });
    //    return;
    //}
    if (formData[3].value == "" || formData[3].value == null || formData[3].value == undefined) {
        toastr.error('Veuillez renseigner le champs Libellé', 'error', { progressBar: true, showDuration: 100 });
        return;
    }
    if (formData[4].value == "" || formData[4].value == null || formData[4].value == undefined) {
        toastr.error('Veuillez renseigner le champs Quantité', 'error', { progressBar: true, showDuration: 100 });
        return;
    }
    if (formData[5].value == "" || formData[5].value == null || formData[5].value == undefined) {
        toastr.error('Veuillez renseigner le champs Remise', 'error', { progressBar: true, showDuration: 100 });
        return;
    }
      if (formData[6].value == "" || formData[6].value == null || formData[6].value == undefined) {
        toastr.error('Veuillez renseigner le champs Prix unitaire', 'error', { progressBar: true, showDuration: 100 });
        return;
    }
    if (formData[7].value == "" || formData[7].value == null || formData[7].value == undefined || formData[7].value == "0") {
        toastr.error('Veuillez renseigner le champs Taxe', 'error', { progressBar: true, showDuration: 100 });
        return;
    }
  
   
    $.ajax({
        type: 'POST',
        url: "/Common/AddOrUpdateDetailsPiece",
        data: formData,
        dataType: 'json',
        encode: true,
        success: function (data) {
            var t1 = data.Val1;
            var t2 = data.Val2;
            var t3 = data.Val3;
            $("#MontantTotalValue").text(t1);
            $("#MontantFinalValue").text(t2);
            $("#RASValue").text(t3);
            debugger;
            $('#GenericModel').modal('hide');
            $('#FacturesDetailsGrid').DataTable().ajax.reload();
            toastr.success('Opération réussite', 'Succès', { progressBar: true, showDuration: 100 });

        },
    });
}
function DeleteDetailsPiece(ID)
{
    if (confirm('Veuillez confirmer la suppression de ce détail')) {
        var formData = $("#ParamForm").serializeArray();
        $.ajax({

            url: "/Common/DeleteDetailsPiece",
            data: { ID: ID },


            success: function (data) {
                var t1 = data.Val1;
                var t2 = data.Val2;
                var t3 = data.Val3;
                $("#MontantTotalValue").text(t1);
                $("#MontantFinalValue").text(t2);
                $("#RASValue").text(t3);
                debugger;
                toastr.success('Supprimé avec succès', 'Succès', { progressBar: true, showDuration: 100 });
                $('#FacturesDetailsGrid').DataTable().ajax.reload();
                RefreshServicesGrid();
                
            },
        });
    }
}
function LoadPieceDetailsData(StatutPiece) {
    if (StatutPiece == "ECR")
    {
        var table = $('#FacturesDetailsGrid').DataTable({
            "order": [[1, "desc"]],
            "autoWidth": true,
            responsive: true,
            
            ajax: {
                url: "/Common/GetFactureDetails",
                data: { NumPiece: $("#NumPiece").val() },
                dataSrc: ''
            },
            columns: [{ "data": "ID" },
            {
                "data": "",
                "render": function (data, type, row) {
                    debugger
                    var ID = row.ID;
                   
                    return "<div style='display:inline-flex'>" +
                        "<a onclick='OpenDetailsPieceEditor(" + ID + ")'  href='#' class='btn btn-warning btn-circle btn-sm HiddenIfVLD'>" + "<i class='fas fa-edit'></i> </a>" +
                        "<a  style='cursor:pointer;margin:0px 5px 0px 5px' onclick=DeleteDetailsPiece('" + ID + "') href='#' class='btn btn-danger btn-circle btn-sm'>" + "<i class='fas fa-trash'></i> </a>" +
                        "</div>"
                }
            },
            { "data": "Libelle" },
            { "data": "Quantite" },
            { "data": "RemiseString" },
            { "data": "pourcentageTaxe" },
            { "data": "MontantTotal", "render": function (data, type, row) { 
                if(data != null)
                    return data.toFixed(3);
                else {
                    return data;
                }
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
    }
    else
    {
        var table = $('#FacturesDetailsGrid').DataTable({
            "order": [[1, "desc"]],
            "autoWidth": true,
            responsive: true,
            
            ajax: {
                url: "/Common/GetFactureDetails",
                data: { NumPiece: $("#NumPiece").val() },
                dataSrc: ''
            },
            columns: [{ "data": "ID" },
            //{ "data": "CodeDetailPiece" },
            { "data": "Libelle" },
            { "data": "Quantite" },
            { "data": "RemiseString" },
            { "data": "pourcentageTaxe" },
            { "data": "MontantHorsTaxe" }
            ],
            "columnDefs": [
             {
                 "targets": [0],
                 "visible": false,
                 "searchable": false
             }]
        });
    }
    
    
}
function LoadPieceListData() {
    var type = $("#TypePieces").val();
    var ClientFilter = "";
    var FournisseurFilter = "";
    var CodeFilter = "";
    var DateFromFilter = "";
    var DateToFilter = "";
    var MontantMinFilter = "";
    var MontantMaxFilter = "";
    debugger;
    var table;
    var etat = $("#Etat").val();
  
    if (type == 'FFAC' || type == 'FNOC' || type == 'BLIV' || type == 'BCOM') {

        if (type == 'FNOC') {
            ClientFilter = $("#ClientFilterFFAC").val();
            FournisseurFilter = $("#FournisseurFilterFFAC").val();
            CodeFilter = $("#CodeFilterFFAC").val();
            DateFromFilter = $("#DateFromFilterFFAC").val();
            DateToFilter = $("#DateToFilterFFAC").val();
            MontantMinFilter = $("#MontantMinFilterFFAC").val();
            MontantMaxFilter = $("#MontantMaxFilterFFAC").val();
            StatusFilter = $("#StatusFilterFFAC").val();
        }
        else {
            ClientFilter = $("#ClientFilter" + type).val();
            FournisseurFilter = $("#FournisseurFilter" + type).val();
            CodeFilter = $("#CodeFilter" + type).val();
            DateFromFilter = $("#DateFromFilter" + type).val();
            DateToFilter = $("#DateToFilter" + type).val();
            MontantMinFilter = $("#MontantMinFilter" + type).val();
            MontantMaxFilter = $("#MontantMaxFilter" + type).val();
            StatusFilter = $("#StatusFilterCFAC").val();
        }
    table = $('#PieceListGrid').DataTable({
        "order": [[2, "desc"]],
        "autoWidth": true,
        "searchable": false,

        responsive: true,

        ajax: {
            url: "/Common/GetAllPieces",
            data: {
                Type: type,
                CodeFilter: CodeFilter,
                DateFromFilter: DateFromFilter,
                DateToFilter: DateToFilter,
                MontantMinFilter: MontantMinFilter,
                MontantMaxFilter: MontantMaxFilter,
                ClientFilter: ClientFilter,
                FournisseurFilter: FournisseurFilter,
                etat: etat,
                StatusFilter: StatusFilter
               
            },
            dataSrc: ''
        },
        columns: [


            { "data": "ID" },
            {
                "data": "",
                "render": function (data, type, row) {
                    debugger
                    var NumPiece = row.NumPiece;
               //     return "<div style='display:inline-flex'><button onclick='OpenFactureDetail(" + "\"" + NumPiece + "\"" + ")' style='width:30px;height:30px;' class='d-none d-sm-inline-block btn btn-sm btn-info shadow-sm'><span class='icon text-white-30'><i class='fas fa-list'></i></span></div>"

                    return "<div style='display:inline-flex'>" +
                        "<a onclick='OpenFactureDetail(" + "\"" + NumPiece + "\"" + ")'  href='#' class='btn btn-info btn-circle btn-sm'>" + "<i class='fas fa-list'></i> </a>" +
                       "</div>"
                }
            },
            {
                "data": "NumPiece"
            },
            { "data": "Statut" },
            { "data": "NomFournisseur" },
            {
                "data": "MontantFinal",
                "render": function (data, type, row) {
                    if(data != null)
                        return data.toFixed(3);
                    else {
                        return data ;
            }
                }
            },
            {
                "data": "Solde",
                "render": function (data, type, row) {
                    if(data != null)
                        return data.toFixed(3);
                    else {
                        return data ;
                    }
                }
            }
        ],
        "columnDefs": [
            {
                "targets": [0],
                "visible": false,
                "searchable": false
            }],
        "footerCallback": function (row, data, start, end, display) {
            debugger;
            var api = this.api(), data;

            // Remove the formatting to get integer data for summation
            var intVal = function (i) {
                return typeof i === 'string' ?
                    i.replace(/[\$,]/g, '') * 1 :
                    typeof i === 'number' ?
                        i : 0;
            };

            // Total over all pages
            total = api
                .column(5)
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);

            // Total over this page
            pageTotal = api
                .column(5, { page: 'current' })
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);

            // Update footer
            $(api.column(5).footer()).html(
                pageTotal + ' TND' + ' ( ' + total + ' TND total)'
            );
        }
    });

}
    else {
        if (type == "CNOC")
        {
             ClientFilter = $("#ClientFilterCFAC").val();
             FournisseurFilter = $("#FournisseurFilterCFAC").val();
             CodeFilter = $("#CodeFilterCFAC").val();
             DateFromFilter = $("#DateFromFilterCFAC").val();
             DateToFilter = $("#DateToFilterCFAC").val();
             MontantMinFilter = $("#MontantMinFilterCFAC").val();
             MontantMaxFilter = $("#MontantMaxFilterCFAC").val();
             StatusFilter = $("#StatusFilterCFAC").val();
        }
        else
        {
             ClientFilter = $("#ClientFilter" + type).val();
             FournisseurFilter = $("#FournisseurFilter" + type).val();
             CodeFilter = $("#CodeFilter" + type).val();
             DateFromFilter = $("#DateFromFilter" + type).val();
             DateToFilter = $("#DateToFilter" + type).val();
             MontantMinFilter = $("#MontantMinFilter" + type).val();
             MontantMaxFilter = $("#MontantMaxFilter" + type).val();
             StatusFilter = $("#StatusFilterCFAC").val();
        }
        table = $('#PieceListGrid').DataTable({
            "order": [[2, "desc"]],
         
            // "ScrollX":false,
             "searchable":false,
            "autoWidth": true,
            responsive: true,
            
            ajax: {
                url: "/Common/GetAllPieces",
                data: {
                    Type: type,
                    CodeFilter: CodeFilter,
                    DateFromFilter: DateFromFilter,
                    DateToFilter: DateToFilter,
                    MontantMinFilter: MontantMinFilter,
                    MontantMaxFilter: MontantMaxFilter,
                    ClientFilter: ClientFilter,
                    FournisseurFilter: FournisseurFilter,
                    etat:etat,
                    StatusFilter: StatusFilter
                },
                dataSrc: ''
            },
            columns: [


            { "data": "ID" },
            {
                "data": "",
                "render": function (data, type, row) {
                    debugger
                    var NumPiece = row.NumPiece;
                    return "<div style='display:inline-flex'><button onclick='OpenFactureDetail(" + "\"" + NumPiece + "\"" + ")' style='width:30px;height:30px;' class='d-none d-sm-inline-block btn btn-sm btn-info shadow-sm'><span class='icon text-white-30'><i class='fas fa-list'></i></span><button onclick='PrintFacture(" + "\"" + NumPiece + "\"" + ")' style='width:30px;height:30px;margin-left:10px;'  class='d-none d-sm-inline-block btn btn-sm btn-info shadow-sm'><span class='icon text-white-30'><i class='fas fa-print'></i></span></button></div>"

                    return "<div style='display:inline-flex'>" +
                        "<a style='cursor:pointer' onclick='OpenFactureDetail(" + "\"" + NumPiece + "\"" + ")'  href='#' class='btn btn-info btn-circle btn-sm'>" + "<i class='fas fa-list'></i> </a>" +
                        "<a style='cursor:pointer;margin:0px 5px 0px 5px' onclick='PrintFacture(" + "\"" + NumPiece + "\"" + ")'  href='#' class='btn btn-primary btn-circle btn-sm'>" + "<i class='fas fa-print'></i> </a>" +
                        "</div>"

                }
            },
            {
                "data": "NumPiece"
            },
            { "data": "Statut" },
            { "data": "NomClient" },
            {
                "data": "MontantFinal",
                "render": function (data, type, row) {
                    if(data != null)
                    return data.toFixed(3);
                    else {
                        return data ;
                    }
                }
            },
                {
                    "data": "Solde",
                    "render": function (data, type, row) {
                        if (data != null)
                            return data.toFixed(3);
                        else {
                            return data;
                        }
                    }
                }
            ],
            "columnDefs": [
             {
                 "targets": [0],
                 "visible": false,
                 "searchable": false
                }],
            "footerCallback": function (row, data, start, end, display) {
                debugger;
                var api = this.api(), data;

                // Remove the formatting to get integer data for summation
                var intVal = function (i) {
                    return typeof i === 'string' ?
                        i.replace(/[\$,]/g, '') * 1 :
                        typeof i === 'number' ?
                            i : 0;
                };

                // Total over all pages
                total = api
                    .column(5)
                    .data()
                    .reduce(function (a, b) {
                        return intVal(a) + intVal(b);
                    }, 0);

                // Total over this page
                pageTotal = api
                    .column(5, { page: 'current' })
                    .data()
                    .reduce(function (a, b) {
                        return intVal(a) + intVal(b);
                    }, 0);

                // Update footer
                $(api.column(5).footer()).html(
                     pageTotal + ' TND'+' ( ' + total + ' TND total)'
                );
            }
        });
    }
  
}
function OpenFactureDetail(NumPiece) {
    var TypePieces = $("#TypePieces").val();
    window.open("/Common/FactureDetails?NumPiece=" + NumPiece + "&Type=" + TypePieces, "_self");
}
function SearchTypeAndOpenFactureDetail(NumPiece)
{
    $.ajax({
        type: 'POST',
        url: "/Common/GetPieceType",
        data: { NumPiece: NumPiece },
        success: function (response) {
            if (response != null && response != "")
            {
                window.open("/Common/FactureDetails?NumPiece=" + NumPiece + "&Type=" + response, "_self");
            }
            else
            {
                toastr.error('Pièce introuvable', 'Erreur !', { progressBar: true, ShowDuration: 500 });
            }


        },
    });
}
function DeleteFacture(NumPiece) {
    if (confirm('Veuillez confirmez la suppression de cette facture'))
    {
        $.ajax({
            type: 'POST',
            url: "/Common/DeleteFacture",
            data: { NumPiece: NumPiece },
            success: function (response) {
                toastr.success('Supprimé avec succès', 'Succès', { progressBar: true, showDuration: 100 });
                $('#PieceListGrid').DataTable().ajax.reload();

            },
        });
    }
}
function format(d) {
    debugger;
    // `d` is the original data object for the row
    return '<table>' +
        '<tr>' +
            '<td>Montant Total:</td>' +
            '<td>' + d.MontantTotal + '</td>' +
        '</tr>' +
        '<tr>' +
            '<td>Date Creation:</td>' +
            '<td>' + moment(d.DateCreation).format("YYYY-MM-DD") + '</td>' +
        '</tr>' +
         '<tr>' +
            '<td>Date Edition:</td>' +
            '<td>' + moment(d.LastEditTime).format("YYYY-MM-DD") + '</td>' +
        '</tr>'
    '</table>'
    ;
}

function SaveReglementsChanges() {
    var Montant = $("#Montant").val();
    var DateReglement = $("#DateReglement").val();
    var OwnerId = $("#OwnerId").val();
    var Reference = $("#Reference").val();
    var Sens = $("#Sens").val();
    var Remarques = $("#Remarques").val();
    var DateEcheance = $("#DateEcheance").val();
    var Banque = $("#Banque").val();
    var ReglementID = $("#ReglementID").val();
    debugger;
    $.ajax({
        type:"POST",
        url: "/Common/SaveReglementsChanges",
        data: { ReglementID: ReglementID, Montant: Montant, DateReglement: DateReglement, OwnerId: OwnerId, Reference: Reference, Sens: Sens, Remarques: Remarques, DateEcheance: DateEcheance, Banque: Banque },
        success: function (response) {
            //debugger;
            //var LibelleRes = response[0];
            //$("#Libelle").val(LibelleRes);
            //var CodeClientRes = response[2];
            //$("#CodeClient").val(CodeClientRes);
            //var CodeFournisseurRes = response[3];
            //var RemiseRes = response[4];
            toastr.success('Enregistré avec succès', 'Succès', { progressBar: true, ShowDuration: 500 });
            //window.location.reload();                
        },
    })
}
var Loader = function () { }
Loader.prototype = {
    require: function (scripts, callback) {
        this.loadCount = 0;
        this.totalRequired = scripts.length;
        this.callback = callback;

        for (var i = 0; i < scripts.length; i++) {
            this.writeScript(scripts[i]);
        }
    },
    loaded: function (evt) {
        this.loadCount++;

        if (this.loadCount == this.totalRequired && typeof this.callback == 'function') this.callback.call();
    },
    writeScript: function (src) {
        var self = this;
        var s = document.createElement('script');
        s.type = "text/javascript";
        s.async = true;
        s.src = src;
        s.addEventListener('load', function (e) { self.loaded(e); }, false);
        var head = document.getElementsByTagName('head')[0];
        head.appendChild(s);
    }
}
function SaveFactureChanges() {
    var NumPiece = $("#NumPiece").val();
    var Libelle = $("#Libelle").val();
    var CodeClient = $("#CodeClient").val();
    var CodeFournisseur = $("#CodeFournisseur").val();
    var Reference = $("#Reference").val();
    var Remise = $("#Remise").val();

    $.ajax({

        url: "/Common/SaveFactureChanges",
        data: { NumPiece: NumPiece, Libelle: Libelle, CodeClient: CodeClient, CodeFournisseur: CodeFournisseur, Remise: Remise, Reference: Reference },
        
        success: function (response) {
            //debugger;
            //var LibelleRes = response[0];
            //$("#Libelle").val(LibelleRes);
            //var CodeClientRes = response[2];
            //$("#CodeClient").val(CodeClientRes);
            //var CodeFournisseurRes = response[3];
            //var RemiseRes = response[4];
            toastr.success('Enregistré avec succès', 'Succès', { progressBar: true, ShowDuration: 500 });
            //window.location.reload();                
        },
    })
}

function LoadFactureDetailsData() {
    debugger;


}
function editRow(CodeDetailPiece) {
    //var oTable = $('#FacturesDetailsGrid').DataTable();
    var element = $("td:contains(" + CodeDetailPiece + ")");
    var MyRow = element.parent();
    debugger;
    MyRow[4].innerHTML = '<input type="text" >';
    MyRow[5].innerHTML = '<input type="text" >';
    MyRow[6].innerHTML = '<input type="text" >';
    MyRow[1].innerHTML = "<button style='width:30px;height:30px;' class='d-none d-sm-inline-block btn btn-sm btn-info shadow-sm'></button>";
}

function AddNewDetailPiece()
{
    var formData = $("#ParamForm").serializeArray();
    $.ajax({
        type: 'POST',
        url: "/Common/AddNewDetailPiece",
        data: formData,
        //dataType: 'json',
        //encode: true,
        success: function (response) {
            debugger;
            $('#GenericModel').modal('hide');
            $('#FacturesDetailsGrid').DataTable().ajax.reload();
            toastr.success('Détail facture ajouté avec succès', 'Succès', { progressBar: true, showDuration: 1000 });
        },
    })
}
function RefreshTarifsSelectorGrid() {
    debugger;
    $('#TarifsSelectorGrid').DataTable().destroy();
    LoadTarifsSelectorData();
}
function RefreshDevisSelectorGrid() {
    debugger;
    $('#DevisSelectorGrid').DataTable().destroy();
    LoadDevisSelectorData();
}
function LoadDevisSelectorData() {
    debugger;
    var DevisSelector = $("#DevisSelector").val();
    if (DevisSelector == undefined || DevisSelector == null) {
        DevisSelector = 0;
    }
    var tableP = $('#DevisSelectorGrid').DataTable({
        "order": [[0, "desc"]],

        "autoWidth": true,
        responsive: true,
        
        ajax: {
            url: "/Common/GetAllPricingDetailsSelector",
            data: { PricingSelector: DevisSelector},
            dataSrc: ''
        },
        columns: [
        { "data": "ID" },
         { "data": "CodeDetail" },
        { "data": "Libelle" },
        { "data": "Quantite" },
         { "data": "MontantUnitaire",
         "render": function (data, type, row) {
             if(data != null)
                 return data.toFixed(3);
             else {
                 return data ;
             }

         }
         },
        ],
    });

}

function OpenEditorToFillFacture() {
    
    var SelectedServices = $("#ServicesSelectedItems").val();
    var test = "************************************************";
    if (SelectedServices == "" || test.includes(SelectedServices)) {
        toastr.warning('Veuillez sélectionner des détails', 'Erreur!', { progressBar: true, showDuration: 100 });
        return;
    }
    var numPiece = $("#NumPiece").val();
    $.ajax({
        type: "GET",
        url: "/Common/OpenEditorToFillFacture",
        data: { SelectedServices: SelectedServices, numPiece: numPiece },
        success: function (data) {
            $('#GenericModel').modal();
            $("#ModalTitle").text("Lier la liste des Produits");
            $("#ModalBody").html(data);
            $("#ServicesSelectedItems").val("");
            $(".selected").removeClass("selected");

        },
        failure: function (response) { }
    });
}

function AddReglementMappings() {
    debugger;
    var formData = $("#ParamForm").serialize();
    $.ajax({
        type: 'POST',
        url: "/Common/AddReglementMappings",
        data: formData,
        error: function (response) {
            debugger;
        },
        success: function (data) {
           /* var t1 = data.Val1;
            var t2 = data.Val2;
            $("#MontantTotalValue").text(t1);
            $("#MontantFinalValue").text(t2);
            debugger;
            */
            $('#GenericModel').modal('hide');
            $('#MappingReglementDetailsGrid').DataTable().ajax.reload();
            RefreshMappingGrid();
            toastr.success('Opération réussite', 'Succès', { progressBar: true, showDuration: 100 });

        },
    });

}
function LinkServicesToInvoice() {
    debugger;
    var formData = $("#ParamForm").serialize();
    $.ajax({
        type: 'POST',
        url: "/Common/LinkServicesToInvoice",
        data: formData,
        error: function (response) {
            debugger;
        },
        success: function (data) {
            debugger;
            var t1 = data.Val1;
            var t2 = data.Val2;
            var t3 = data.Val3;
            $("#MontantTotalValue").text(t1);
            $("#MontantFinalValue").text(t2);
            $("#RASValue").text(t3);
            debugger;
            $('#GenericModel').modal('hide');
            $('#FacturesDetailsGrid').DataTable().ajax.reload();
            RefreshServicesGrid();
            
            toastr.success('Opération réussite', 'Succès', { progressBar: true, showDuration: 100 });

        },
    });

}
function RefreshServicesGrid() {
    debugger;
    $('#ServicesSelectorGrid').DataTable().destroy();
    LoadServicesSelectorData();
}
function RefreshMappingGrid() {
    debugger;
    $('#MappingReglementDetailsGrid').DataTable().destroy();
    LoadMappingsRData();
}
function LoadExternalReglementsData(Owner) {
    debugger;
    var OwnerFilter = $("#OwnerFilter").val();
     var LibelleFilter = $("#LibelleFilter").val();
     var DateFromFilter = $("#DateFromFilter").val();
     var DateToFilter = $("#DateToFilter").val();
     var MontantMinFilter = $("#MontantMinFilter").val();
     var MontantMaxFilter = $("#MontantMaxFilter").val();
    var table = $('#ExternalReglementsGrid').DataTable({
        "order": [[1, "desc"]],
        "autoWidth": true,
        responsive: true,
        
        ajax: {
            url: "/Common/GetExternalReglements",
            data: { Owner: Owner, OwnerFilter: OwnerFilter, LibelleFilter: LibelleFilter, DateFromFilter: DateFromFilter, DateToFilter: DateToFilter, MontantMinFilter: MontantMinFilter, MontantMaxFilter: MontantMaxFilter },
            dataSrc: ''
        },
        columns: [{ "data": "ID" },
        {
            "data": "",
            "render": function (data, type, row) {
                debugger
                var ID = row.ID;
                return "<div style='display:inline-flex'><button onclick='OpenExternalReglementsDetails(" + ID + ")' style='width:30px;height:30px;' class='d-none d-sm-inline-block btn btn-sm btn-info shadow-sm'><span class='icon text-white-30'><i class='fas fa-list'></i></span></button><button onclick='DeleteExternalReglements(" + ID + ")' style='width:30px;height:30px;margin-left:10px;'  class='d-none d-sm-inline-block btn btn-sm btn-danger shadow-sm'><span class='icon text-white-30'><i class='fas fa-trash'></i></span></button><button onclick='PrintReglement(" + "\"" + ID + "\"" + ")' style='width:30px;height:30px;margin-left:10px;'  class='d-none d-sm-inline-block btn btn-sm btn-info shadow-sm'><span class='icon text-white-30'><i class='fas fa-print'></i></span></button></div>"
            }
        },
        { "data": "LibelleTypeReglement" },
        { "data": "NomBanque" },
        {
            "data": "DateReglement",
            render: function (data, type, row) {
                if (type === "sort" || type === "type") {
                    return data;
                }
                return moment(data).format("DD/MM/YYYY");
            }
        },
        {
            "data": "DateEcheance",
            render: function (data, type, row) {
                if (type === "sort" || type === "type") {
                    return data;
                }
                return moment(data).format("DD/MM/YYYY");
            }
        },
        { "data": "Remarques" },
        {
            "data": "Montant",
            "render": function (data, type, row) {
                if(data != null)
                    return data.toFixed(3);
                else{
                    return data ;
                }
            }
        },
        ],
        "columnDefs": [
         {
             "targets": [0],
             "visible": false,
             "searchable": false
            }],
        "footerCallback": function (row, data, start, end, display) {
            debugger;
            var api = this.api(), data;

            // Remove the formatting to get integer data for summation
            var intVal = function (i) {
                return typeof i === 'string' ?
                    i.replace(/[\$,]/g, '') * 1 :
                    typeof i === 'number' ?
                        i : 0;
            };

            // Total over all pages
            total = api
                .column(7)
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);

            // Total over this page
            pageTotal = api
                .column(7, { page: 'current' })
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);

            // Update footer
            $(api.column(7).footer()).html(
                pageTotal + ' TND' + ' ( ' + total + ' TND total)'
            );
        }
    });
}
function OpenExternalReglementsEditor(OwnerType) {
    
    debugger;
    $.ajax({
        type: "GET",
        url: "/Common/OpenExternalReglementsEditor",
        data: { OwnerType: OwnerType },
        success: function (data) {
            debugger;

            $('#GenericModel').modal();
            $("#ModalTitle").text("Règlement ");
            $("#ModalBody").html(data);

        },
        failure: function (response) { }
    });
}
function AddNewExternalReglement() {
    debugger;
    var formData = $("#ParamForm").serializeArray();
    var Ownertype = formData[1].value;
    var Regtype = formData[2].value;
    
   
        if (formData[3].value == "" || formData[3].value == null || formData[3].value == undefined || formData[3].value == "0") {
            if (Ownertype == 'C') {
                toastr.error('Veuillez renseigner le champs Client', 'error', { progressBar: true, showDuration: 100 });
            }
            else { toastr.error('Veuillez renseigner le champs Fournisseur', 'error', { progressBar: true, showDuration: 100 });
            }
            return;
        }
   
    
    if (Regtype == '3') {
        if (formData[10].value == "" || formData[10].value == null || formData[10].value == undefined ) {
            toastr.error('Veuillez renseigner le champs Date échéance', 'error', { progressBar: true, showDuration: 100 });
            return;
        }
        if (formData[11].value == "" || formData[11].value == null || formData[11].value == undefined || formData[11].value == "0") {
            toastr.error('Veuillez renseigner le champs Banque', 'error', { progressBar: true, showDuration: 100 });
            return;
        }
    }
    if (formData[6].value == "" || formData[6].value == null || formData[6].value == undefined) {
        toastr.error('Veuillez renseigner le champs Référece', 'error', { progressBar: true, showDuration: 100 });
        return;
    }
    if (formData[7].value == "" || formData[7].value == null || formData[7].value == undefined) {
        toastr.error('Veuillez renseigner le champs Date règlement', 'error', { progressBar: true, showDuration: 100 });
        return;
    }
    if (formData[4].value == "" || formData[4].value == null || formData[4].value == undefined || formData[4].value == "0") {
        toastr.error('Veuillez renseigner le champs Montant', 'error', { progressBar: true, showDuration: 100 });
        return;
    }
    if (formData[2].value == "" || formData[2].value == null || formData[2].value == undefined || formData[2].value == "0") {
        toastr.error('Veuillez renseigner le champs Type règlement', 'error', { progressBar: true, showDuration: 100 });
        return;
    }
    

    $.ajax({
        type: 'POST',
        url: "/Common/AddNewExternalReglement",
        data: formData,
        //dataType: 'json',
        //encode: true,
        success: function (response) {
            debugger;
            if (response.Text == 'KO') {
                toastr.error(response.Value, 'error', { progressBar: true, showDuration: 100 });
            }
            else {
                var automaticAttach = $("#automaticAttach").is(":checked");
                $('#GenericModel').modal('hide');
                $('#ExternalReglementsGrid').DataTable().ajax.reload();
                if (automaticAttach == false) {

                    window.open("/Common/ExternalReglementDetails?ExternalReglementID=" + response.Value, "_self");
                }
            }
        },
    })
}
function DeleteExternalReglements(ID) {
    var numPiece = $("#NumPiece").val();
    if (confirm('Veuillez confirmer la suppression de ce réglement')) {
        var formData = $("#ParamForm").serializeArray();
        $.ajax({

            url: "/Common/DeleteExternalReglements",
            data: { ID: ID, numPiece: numPiece },


            success: function (data) {
                toastr.success('Supprimé avec succès', 'Succès', { progressBar: true, showDuration: 100 });
                $('#ReglementsGrid').DataTable().ajax.reload();


            },
        });
    }
}

function LoadReglementsData()
{
    var table = $('#ReglementsGrid').DataTable({
        "order": [[1, "desc"]],
        "autoWidth": true,
        responsive: true,
        
        ajax: {
            url: "/Common/GetReglementsFacture",
            data: { NumPiece: $("#NumPiece").val() },
            dataSrc: ''
        },
        columns: [{ "data": "ID" },
        {
            "data": "",
            "render": function (data, type, row) {
                debugger
                var ID = row.ID;
                return "<div style='display:inline-flex'><button onclick='OpenReglementsDtails(" + ID + ")' style='width:30px;height:30px;margin-right:10px;' class='d-none d-sm-inline-block btn btn-sm btn-info shadow-sm'><span class='icon text-white-50'><i class='fas fa-list'></i></span></button><button onclick='OpenReglementsEditor(" + ID + ")' style='width:30px;height:30px;' class='d-none d-sm-inline-block btn btn-sm btn-info shadow-sm'><span class='icon text-white-50'><i class='fas fa-edit'></i></span></button><button onclick='DeleteReglements(" + ID + ")' style='width:30px;height:30px;margin-left:10px;'  class='d-none d-sm-inline-block btn btn-sm btn-danger shadow-sm'><span class='icon text-white-50'><i class='fas fa-trash'></i></span></button></div>"
            }
        },
        { "data": "Reference" },
        { "data": "LibelleTypeReglement" },
        { "data": "DateReglement",
            render: function (data, type, row) {
                if (type === "sort" || type === "type") {
                    return data;
                }
                return moment(data).format("DD/MM/YYYY");
            }
        },
        { "data": "Montant" ,
        "render": function (data, type, row) {
            if(data != null)
                return data.toFixed(3);
            else {
                return data ;
            }
        }
        },
        { "data": "MontantMapping" ,
        "render": function (data, type, row) {
            if(data != null)
                return data.toFixed(3);
            else {
                return data ;
            }
        }}
        ],
        "columnDefs": [
         {
             "targets": [0],
             "visible": false,
             "searchable": false
         }]
    });
}
function OpenReglementsDtails(ID)
{
    $.ajax({
        type: "GET",
        url: "/Common/GetReglementIDFormDetail",
        data: { ID: ID},
        success: function (data) {
            window.open("/Common/ExternalReglementDetails?ExternalReglementID=" + data, "_self");
        },
        failure: function (response) { }
    });
}
function OpenReglementsEditor(ID) {
    var NumPiece = $("#NumPiece").val();
    debugger;
    $.ajax({
        type: "GET",
        url: "/Common/OpenReglementsEditor",
        data: { ID: ID, NumPiece: NumPiece },
        success: function (data) {
            debugger;

            $('#GenericModel').modal();
            $("#ModalTitle").text("Reglement ");
            $("#ModalBody").html(data);

        },
        failure: function (response) { }
    });
}
function AddOrUpdateReglements() {
    debugger;
    var formData = $("#ParamForm").serializeArray();
    var IDReg = formData[0].value;
    if (IDReg == "0") {
        
        var Regtype = formData[5].value;

       
        if (Regtype == '3') {
            if (formData[9].value == "" || formData[9].value == null || formData[9].value == undefined) {
                toastr.error('Veuillez renseigner le champs Date échéance', 'error', { progressBar: true, showDuration: 100 });
                return;
            }
            if (formData[8].value == "" || formData[8].value == null || formData[8].value == undefined || formData[8].value == "0") {
                toastr.error('Veuillez renseigner le champs Banque', 'error', { progressBar: true, showDuration: 100 });
                return;
            }
        }
        if (formData[4].value == "" || formData[4].value == null || formData[4].value == undefined) {
            toastr.error('Veuillez renseigner le champs Référece', 'error', { progressBar: true, showDuration: 100 });
            return;
        }
        if (formData[7].value == "" || formData[7].value == null || formData[7].value == undefined) {
            toastr.error('Veuillez renseigner le champs Date règlement', 'error', { progressBar: true, showDuration: 100 });
            return;
        }
        if (formData[6].value == "" || formData[6].value == null || formData[6].value == undefined || formData[6].value == "0") {
            toastr.error('Veuillez renseigner le champs Montant', 'error', { progressBar: true, showDuration: 100 });
            return;
        }
        if (formData[5].value == "" || formData[5].value == null || formData[5].value == undefined || formData[5].value == "0") {
            toastr.error('Veuillez renseigner le champs Type règlement', 'error', { progressBar: true, showDuration: 100 });
            return;
        }
    } else {
        if (formData[4].value == "" || formData[4].value == null || formData[4].value == undefined || formData[4].value == "0") {
            toastr.error('Veuillez renseigner le champs Montant', 'error', { progressBar: true, showDuration: 100 });
            return;
        }
    }
    $.ajax({
        type: 'POST',
        url: "/Common/AddOrUpdateReglements",
        data: formData,
        dataType: 'json',
        encode: true,
        error: function(data){
            debugger;
        },
        success: function (data) {
            debugger;
            var statut = data.Text;
            var msg = data.Value;
            
            if (statut == 'KO')
            {
                toastr.warning(msg, 'Erreur', { progressBar: true, showDuration: 100 });
            }
            else
            {

                $('#GenericModel').modal('hide');
                $('#ReglementsGrid').DataTable().ajax.reload();
                toastr.success('Opération réussite', 'Succès', { progressBar: true, showDuration: 100 });
            }

        },
    });
}
function DeleteReglements(ID) {
    debugger;
    var numPiece = $("#NumPiece").val();
    if (confirm('Veuillez confirmer la suppression de ce réglement')) {
        var formData = $("#ParamForm").serializeArray();
        $.ajax({

            url: "/Common/DeleteReglements",
            data: { ID: ID, numPiece: numPiece },


            success: function (data) {
                toastr.success('Supprimé avec succès', 'Succès', { progressBar: true, showDuration: 100 });
                $('#ReglementsGrid').DataTable().ajax.reload();
                $('#MappingReglementDetailsGrid').DataTable().ajax.reload();

            },
        });
    }
}

function LoadTarifsSelectorData() {
    debugger;
    var TarifSelector = $("#TarfisSelector").val();
    if (TarifSelector == undefined || TarifSelector == null) {
        TarifSelector = 0;
    }
    var tableP = $('#TarifsSelectorGrid').DataTable({
        "order": [[0, "desc"]],
        "autoWidth": true,
        responsive: true,
        
        ajax: {
            url: "/Common/GetAllPricingDetailsSelector",
            data: { PricingSelector: TarifSelector},
            dataSrc: ''
        },
        columns: [
        {
            "data": "ID"
        },
         { "data": "CodeDetail" },
        { "data": "Libelle" },
        {
            "data": "MontantUnitaire",
            "render": function (data, type, row) {
                if(data != null)
                    return data.toFixed(3);
                else {
                    return data ;
                }
            }
        },

        ],

    });

}
function ChangeSourceType(Type) {

    $(".btnSources").removeClass("btn-primary");
    $(".btnSources").addClass("btn-default");
    $("#" + Type + "SourceBtn").removeClass("btn-default");
    $("#" + Type + "SourceBtn").addClass("btn-primary");
    $(".DivsSources").css("display", "none");
    $("#" + Type + "ListDiv").css("display", "block");

}

function LoadServicesSelectorData() {
    debugger;
    var TypePiece = $("#TypePiece").val();
    var Achat = false;
    var Vente = false;
    if (TypePiece == "CFAC")
    {
        Vente = true;
    }
    if (TypePiece == "FFAC" ) {
        Achat = true;
    }
    var libelle = $("#LibelleFilter").val();
    var code = $("#ReferenceFilter").val();
    var tableP = $('#ServicesSelectorGrid').DataTable({
        "order": [[0, "desc"]],
        "searchable": false,
        "autoWidth": true,
        ajax: {
            url: "/Stock/GetAllServices",
            data: { libelle: libelle, code: code, Vente: Vente, Achat: Achat },
            dataSrc: ''
        },
        columns: [{
            "data": "ID"

        },
        { "data": "Libelle" },
        ],

    });

}
function OpenEditorToFillFactureFromDevisDetails() {
    debugger;
    var SelectedDevis = $("#DevisSelectedItems").val();
    var test = "*****************************************************";
    if (SelectedDevis == "" || test.includes(SelectedDevis))
    {
        toastr.warning('Veuillez sélectionner des détails', 'Erreur!', { progressBar: true, showDuration: 100 });
        return;
    }
    var numPiece = $("#NumPiece").val();
    $.ajax({
        type: "GET",
        url: "/Common/OpenEditorToFillFactureFromPricing",
        data: { SelectedPricing: SelectedDevis, numPiece: numPiece },
        success: function (data) {
            $('#GenericModel').modal();
            $("#ModalTitle").text("Lier la liste des détails devis");
            $("#ModalBody").html(data);
            $("#DevisSelectedItems").val("");
            $(".selected").removeClass("selected");

        },
        failure: function (response) { }
    });
}
function OpenEditorToFillFactureFromTarif() {
    var SelectedTarifs = $("#TarifsSelectedItems").val();
    var test = "************************************************";
    if (SelectedTarifs == "" || test.includes(SelectedTarifs))
    {
        toastr.warning('Veuillez sélectionner des détails', 'Erreur!', { progressBar: true, showDuration: 100 });
        return;
    }
    var numPiece = $("#NumPiece").val();
    $.ajax({
        type: "GET",
        url: "/Common/OpenEditorToFillFactureFromPricing",
        data: { SelectedPricing: SelectedTarifs, numPiece: numPiece },
        success: function (data) {
            $('#GenericModel').modal();
            $("#ModalTitle").text("Lier la liste des détails tarifs");
            $("#ModalBody").html(data);
            $("#TarifsSelectedItems").val("");
            $(".selected").removeClass("selected");

        },
        failure: function (response) { }
    });
}
function LinkPricingDetailsToInvoice() {
    debugger;
    var formData = $("#ParamForm").serialize();
    $.ajax({
        type: 'POST',
        url: "/Common/LinkPricingDetailsToInvoice",
        data: formData,
        
        error: function (response) {
            debugger;
        },
        success: function (response) {
            debugger;
            $('#GenericModel').modal('hide');
            $('#FacturesDetailsGrid').DataTable().ajax.reload();
            RefreshServicesGrid();
            
            toastr.success('Opération réussite', 'Succès', { progressBar: true, showDuration: 100 });

        },
    });

}

function FillFactureFromDevis(numPiece) {
    debugger;
    var devisId = $("#DevisSelector").val();
    if (devisId == 0) {
        toastr.warning('Veuillez sélectionner un devis', 'Erreur!', { progressBar: true, showDuration: 100 });
        return;
    }
    $.ajax({
        type: 'POST',
        url: "/Common/LinkDevisToInvoice",
        data: { devisId: devisId, numPiece: numPiece },
        error: function (response) {
            debugger;
        },
        success: function (response) {
            debugger;
            $('#GenericModel').modal('hide');
            $('#FacturesDetailsGrid').DataTable().ajax.reload();
            toastr.success('Opération réussite', 'Succès', { progressBar: true, showDuration: 100 });

        },
    });
}

function ChangeFactureStatus(status)
{
    var numPiece = $("#NumPiece").val();
    $.ajax({
        type: 'POST',
        url: "/Common/ChangeFactureStatus",
        data: { numPiece: numPiece, status: status },
        error: function (response) {
            debugger;
        },
     /*   beforeSend: function(){
            $('#page').modal('show');
            
        },
        complete: function(){
            $('#page').modal('hide');
        },*/
        
        success: function (response) {
            debugger;
            if (response == "OK") {
                toastr.success('Opération réussite', 'Succès', { progressBar: true, showDuration: 100 });
                window.location.reload();
            }else 
            { toastr.error(response, 'Erreur', { progressBar: true, showDuration: 100 }); }
        },
    });

}
function UpdatePrixVente(i)
{
    debugger;
    var marge = $(".marge_"+i).val();
    var pAchat = $(".pAchat_"+i).val();
    $(".pVente_"+i).val(pAchat * (1 + (marge / 100)));
}
function UpdateMarge(i) {
    debugger;
    var pVente = $(".pVente_"+i).val();
    var pAchat = $(".pAchat_" + i).val();
    $(".marge_" + i).val(pVente - pAchat / pAchat * 100);
}
function DeleteAllDetails()
{
    var numPiece = $("#NumPiece").val();
    if (confirm('Veuillez confirmer la suppression des détails')) {
        var formData = $("#ParamForm").serializeArray();
        $.ajax({

            url: "/Common/DeleteAllDetailsPiece",
            data: { numPiece: numPiece },


            success: function (data) {
                var t1 = data.Val1;
                var t2 = data.Val2;
                var t3 = data.Val3;
                $("#MontantTotalValue").text(t1);
                $("#MontantFinalValue").text(t2);
                $("#RASValue").text(t3);
                debugger;
                toastr.success('Supprimé avec succès', 'Succès', { progressBar: true, showDuration: 100 });
                $('#FacturesDetailsGrid').DataTable().ajax.reload();
                RefreshServicesGrid();
               
            },
        });
    }
}
function ShowMore()
{
    $("#moreDetailsDiv").show();
    $("#MoreBtn").css("display", "none");
    $("#LessBtn").css("display", "block");
}
function ShowLess() {
    $("#moreDetailsDiv").hide();
    $("#MoreBtn").css("display", "block");
    $("#LessBtn").css("display", "none");
}
function ShowPieceDetailsDiv()
{
        $("#ReglementsSourceBtn").removeClass("btn-primary");
    $("#ReglementsSourceBtn").addClass("btn-default");
    $("#DetailsSourceBtn").removeClass("btn-default");
    $("#DetailsSourceBtn").addClass("btn-primary");
    $("#PieceDetailsDiv").show();
    $("#PieceReglementsDiv").hide();
}
function ShowPieceReglementsDiv()
{

    $("#DetailsSourceBtn").removeClass("btn-primary");
    $("#DetailsSourceBtn").addClass("btn-default");
    $("#ReglementsSourceBtn").removeClass("btn-default");
    $("#ReglementsSourceBtn").addClass("btn-primary");
    $("#PieceReglementsDiv").show();
    $("#PieceDetailsDiv").hide();
}
function ChangeReglementSourceType(Type) {

    $(".btnSourcesR").removeClass("btn-primary");
    $(".btnSourcesR").addClass("btn-default");
    $("#" + Type + "SourceBtn").removeClass("btn-default");
    $("#" + Type + "SourceBtn").addClass("btn-primary");
    $(".DivsSourcesR").css("display", "none");
    $("#" + Type + "ListDiv").css("display", "block");

}



function LoadFactureClientSelectorData() {
    debugger;
    var Type = $("#FactureType").val();
    var ClientFilter = $("#OwnerId").val();
    var FournisseurFilter = 0;
    var CodeFilter = $("#CodeFilterCFAC").val();
    var LibelleFilter = $("#LibelleFilterCFAC").val();
    var DateFromFilter = $("#DateFromFilterCFAC").val();
    var DateToFilter = $("#DateToFilterCFAC").val();
    var MontantMinFilter = $("#MontantMinFilterCFAC").val();
    var MontantMaxFilter = $("#MontantMaxFilterCFAC").val();
    var RAS = $("#RAS").val();
    var tableP = $('#FactureClientSelectorGrid').DataTable({
        "order": [[0, "desc"]],
        "autoWidth": true,
        responsive: true,
        
        ajax: {
            url: "/Common/GetAllPiecesForReglements",
            data: { Type: Type, CodeFilter: CodeFilter, LibelleFilter: LibelleFilter, DateFromFilter: DateFromFilter, DateToFilter: DateToFilter, MontantMinFilter: MontantMinFilter, MontantMaxFilter: MontantMaxFilter, ClientFilter: ClientFilter, FournisseurFilter: FournisseurFilter, RAS: RAS },
            dataSrc: ''
        },
        columns: [{
            "data": "ID"

        },
        { "data": "NumPiece" },
        { "data": "MontantFinal",
        "render": function (data, type, row) {
            if(data != null)
                return data.toFixed(3);
            else {
                return data ;
            }
        }
        },
       

        ],

    });

}
function LoadFactureFournisseurSelectorData() {
    debugger;
    var ClientFilter = 0;
    var Type = $("#FactureType").val();
    var FournisseurFilter = $("#OwnerId").val();
    var CodeFilter = $("#CodeFilterFFAC").val();
    var LibelleFilter = $("#LibelleFilterFFAC").val();
    var DateFromFilter = $("#DateFromFilterFFAC").val();
    var DateToFilter = $("#DateToFilterFFAC").val();
    var MontantMinFilter = $("#MontantMinFilterFFAC").val();
    var MontantMaxFilter = $("#MontantMaxFilterFFAC").val();
    var RAS = $("#RAS").val();
    var tableP = $('#FactureFournisseurSelectorGrid').DataTable({
        "order": [[0, "desc"]],
        "autoWidth": true,
        responsive: true,
        
        ajax: {
            url: "/Common/GetAllPiecesForReglements",
            data: { Type: Type, CodeFilter: CodeFilter, LibelleFilter: LibelleFilter, DateFromFilter: DateFromFilter, DateToFilter: DateToFilter, MontantMinFilter: MontantMinFilter, MontantMaxFilter: MontantMaxFilter, ClientFilter: ClientFilter, FournisseurFilter: FournisseurFilter },
            dataSrc: ''
        },
        columns: [{
            "data": "ID"

        },
        { "data": "NumPiece" },
        {
            "data": "MontantFinal",
            "render": function (data, type, row) {
                if(data != null)
                    return data.toFixed(3);
                else {
                    return data
                }
            }
        },
       

        ],

    });

}
function LoadMATSelectorRData(reglementID) {
    

    var tableP = $('#MATSelectorGridR').DataTable({
        "order": [[0, "desc"]],

        "autoWidth": true,
        ajax: {
            url: "/Common/GetAllMaterialDetailsByReglement",
            data: { reglementID: reglementID },
            dataSrc: ''
        },
        columns: [{
            "data": "ID"

        },
            {
                "data": "",
                "render": function (data, type, row) {
                    debugger
                    var ID = row.ID;
                    return "<div style='display:inline-flex'><button onclick=OpenMaterialReglementEditor('" + ID + "') style='width:30px;height:30px;' class='d-none d-sm-inline-block btn btn-sm btn-info shadow-sm'><span class='icon text-white-30'><i class='fas fa-edit'></i></span><button style='width:30px;height:30px;margin-left:10px;'onclick=DeleteMaterialReglement('" + ID + "')  class='d-none d-sm-inline-block btn btn-sm btn-danger shadow-sm'><span class='icon text-white-30'><i class='fas fa-trash'></i></span></button></div>"
                }
            },
            { "data": "ProductName" },
        {
            "data": "Amount",
            "render": function (data, type, row) {
                if (data != null)
                    return data.toFixed(3);
                else {
                    return data;
                }
            }
        },


        ],
        "columnDefs": [
            {
                "targets": [0],
                "visible": false,
                "searchable": false
            }]
    });

}
function OpenMaterialReglementEditor(id) {
    var reglementID = $("#ReglementID").val();
    $.ajax({
        type: "GET",
        url: "/Common/OpenMaterialReglementEditor",
        data: { id: id, reglementID: reglementID},
        success: function (data) {
            debugger;

            $('#GenericModel').modal();
            $("#ModalTitle").text("Matériels du règlement");
            $("#ModalBody").html(data);

        },
        failure: function (response) { }
    });
}
function AddOrUpdateMaterialReglement() {

    debugger;
    var formData = $("#ParamForm").serializeArray();
    if (formData[1].value == "" || formData[1].value == null || formData[1].value == undefined) {
        toastr.error('Veuillez renseigner le champs produit', 'error', { progressBar: true, showDuration: 100 });
        return;
    }
    if (formData[2].value == "" || formData[2].value == null || formData[2].value == undefined || formData[2].value == "0") {
        toastr.error('Veuillez renseigner le champs montant', 'error', { progressBar: true, showDuration: 100 });
        return;
    }
    $.ajax({
        type: 'POST',
        url: "/Common/AddOrUpdateMaterialReglement",
        data: formData,
        encode: true,
        success: function (response) {
            debugger;
            //var res = JSON.parse(response);

            $('#GenericModel').modal('hide');
            toastr.success('Opération réussite', 'Succès', { progressBar: true, showDuration: 100 });
            $('#MATSelectorGridR').DataTable().ajax.reload();


        },
    })
}
function DeleteMaterialReglement(ID) {
    debugger;
    if (confirm('Veuillez confirmez la suppression du détail')) {
        var formData = $("#ParamForm").serializeArray();
        $.ajax({

            url: "/Common/DeleteMaterialReglement",
            data: { ID: ID },

            success: function (response) {
                debugger;
                toastr.success('Supprimé avec succès', 'Succès', { progressBar: true, showDuration: 100 });
                $('#MATSelectorGridR').DataTable().ajax.reload();

            },
        });
    }
}
function LoadBLSelectorRData() {
    debugger;
    var ClientFilter = $("#OwnerId").val();
    var FournisseurFilter =0;
    var CodeFilter = $("#CodeFilterBLIV").val();
    var LibelleFilter = $("#LibelleFilterBLIV").val();
    var DateFromFilter = $("#DateFromFilterBLIV").val();
    var DateToFilter = $("#DateToFilterBLIV").val();
    var MontantMinFilter = $("#MontantMinFilterBLIV").val();
    var MontantMaxFilter = $("#MontantMaxFilterBLIV").val();
    var RAS = $("#RAS").val();

    var tableP = $('#BLSelectorGridR').DataTable({
        "order": [[0, "desc"]],

        "autoWidth": true,
        ajax: {
            url: "/Common/GetAllPiecesForReglements",
            data: { Type: "BLIV", CodeFilter: CodeFilter, LibelleFilter: LibelleFilter, DateFromFilter: DateFromFilter, DateToFilter: DateToFilter, MontantMinFilter: MontantMinFilter, MontantMaxFilter: MontantMaxFilter, ClientFilter: ClientFilter, FournisseurFilter: FournisseurFilter },
            dataSrc: ''
        },
        columns: [{
            "data": "ID"

        },
        { "data": "NumPiece" },
        {
            "data": "MontantFinal",
            "render": function (data, type, row) {
                if(data != null)
                    return data.toFixed(3);
                else {
                    return data;
                }
            }
        },
       

        ],

    });

}
function LoadBCSelectorRData() {
    debugger;
    var ClientFilter = $("#OwnerId").val();
    var FournisseurFilter = 0;
    var CodeFilter = $("#CodeFilterBCOM").val();
    var LibelleFilter = $("#LibelleFilterBCOM").val();
    var DateFromFilter = $("#DateFromFilterBCOM").val();
    var DateToFilter = $("#DateToFilterBCOM").val();
    var MontantMinFilter = $("#MontantMinFilterBCOM").val();
    var MontantMaxFilter = $("#MontantMaxFilterBCOM").val();
    var RAS = $("#RAS").val();

    var tableP = $('#BCSelectorGridR').DataTable({
        "order": [[0, "desc"]],
        "autoWidth": true,
        responsive: true,
        
        ajax: {
            url: "/Common/GetAllPiecesForReglements",
            data: { Type: "BCOM", CodeFilter: CodeFilter, LibelleFilter: LibelleFilter, DateFromFilter: DateFromFilter, DateToFilter: DateToFilter, MontantMinFilter: MontantMinFilter, MontantMaxFilter: MontantMaxFilter, ClientFilter: ClientFilter, FournisseurFilter: FournisseurFilter },
            dataSrc: ''
        },
        columns: [{
            "data": "ID"

        },
        { "data": "NumPiece" },
        {
            "data": "MontantFinal",
            "render": function (data, type, row) {
                if(data != null)
                    return data.toFixed(3);
                else {
                    return data;
                }
            }
        },

        ],

    });

}
function LoadMappingsRData()
{
    debugger;
    var ReglementID = $("#ReglementID").val();
    var tableP = $('#MappingReglementDetailsGrid').DataTable({
        "order": [[0, "desc"]],
        "autoWidth": true,
        responsive: true,
        
        ajax: {
            url: "/Common/GetAllMappingsForReglements",
            data: { ReglementID: ReglementID },
            dataSrc: ''
        },
        columns: [{
            "data": "ID"

        },
                {
                    "data": "",
                    "render": function (data, type, row) {
                        debugger
                        var ID = row.ID;
                        var PieceID = row.PieceID;
                        return "<div style='display:inline-flex'><button onclick='SearchTypeAndOpenFactureDetail(" + "\"" + PieceID + "\"" + ")' style='width:30px;height:30px;' class='d-none d-sm-inline-block btn btn-sm btn-info shadow-sm'><span class='icon text-white-50'><i class='fas fa-list'></i></span></button><button onclick='DeleteReglements(" + ID + ")' style='width:30px;height:30px;margin-left:10px;'  class='d-none d-sm-inline-block btn btn-sm btn-danger shadow-sm'><span class='icon text-white-50'><i class='fas fa-trash'></i></span></button></div>"
                    }
                },
        { "data": "PieceID" },
        {
            "data": "Montant",
            "render": function (data, type, row) {
                if (data != null)
                    return data.toFixed(3);
                else {
                    return data;
                }
            }
        },
        ],
        "columnDefs": [
         {
             "targets": [0],
             "visible": false,
             "searchable": false
         }]
    });

}
function RefreshFactureGrid(type) {
    debugger;

    var src = $("#Value").val();
    if (src == "0") {
        if (type == "CFAC") {
            $('#FactureClientSelectorGrid').DataTable().destroy();
            LoadFactureClientSelectorData();
        }
        else if (type == "FFAC") {
            $('#FactureFournisseurSelectorGrid').DataTable().destroy();
            LoadFactureFournisseurSelectorData();
        }
        else if (type == "BLIV") {
            $('#BLSelectorGridR').DataTable().destroy();
            LoadBLSelectorRData();
        }
        else if (type == "BCOM") {
            $('#BCSelectorGridR').DataTable().destroy();
            LoadBCSelectorRData();
        }
    }
    else {

        $('#PieceListGrid').DataTable().destroy();
        LoadPieceListData();
    }
    $(".dataTables_length").hide();
    $(".dataTables_filter").hide();
}
function OpenExternalReglementsDetails(ID)
{
    window.open("/Common/ExternalReglementDetails?ExternalReglementID=" + ID, "_self");
}
function OpenEditorToFillReglementFromFacture()
{
    var SelectedItems = $("#FactureSelectedItems").val();
    OpenEditorToFillMapping(SelectedItems, 'factures');
}
function OpenEditorToFillReglementFromBC() {
    var SelectedItems = $("#BCSelectedItems").val()
    OpenEditorToFillMapping(SelectedItems,'bons de commande');
}
function OpenEditorToFillReglementFromBL() {
    var SelectedItems = $("#BLSelectedItems").val()
    OpenEditorToFillMapping(SelectedItems,'bons de livraison');
}
function OpenEditorToFillMapping(SelectedItems,type)
{
    var test = "************************************************";
    if (SelectedItems == "" || test.includes(SelectedItems)) {
        toastr.warning('Veuillez sélectionner des détails'+type, 'Erreur!', { progressBar: true, showDuration: 100 });
        return;
    }
    var ReglementID = $("#ReglementID").val();
    $.ajax({
        type: "GET",
        url: "/Common/OpenEditorToFillMappings",
        data: { SelectedItems: SelectedItems, ReglementID: ReglementID },
        success: function (data) {
            $('#GenericModel').modal();
            $("#ModalTitle").text("Lier la liste des Produits");
            $("#ModalBody").html(data);
            $(".selected").removeClass("selected");
            $("#FactureSelectedItems").val("");
            $("#BCSelectedItems").val("");
            $("#BLSelectedItems").val("");
        },
        failure: function (response) { }
    });
}
function CheckMontantAregler(i)
{
    debugger;
    var montantReglement = parseInt($("#montantReglement").val());
    var SumPieces = parseInt($("#SumPieces").val());
    var NewElementValue = parseInt($(".MontantARegler_" + i).val());
    var OldElementValue = parseInt($(".MontantARegler_" + i)[0].defaultValue);
    var MontantFinal = parseInt($(".MontantFinal_" + i).val());
    var MontantReglee = parseInt($(".MontantReglee_" + i).val());
    if ((NewElementValue + MontantReglee )> MontantFinal)
    {
        toastr.warning('Le montant à régler a dépassé le montant de la facture' , 'Erreur!', { progressBar: true, showDuration: 100 });
        $(".MontantARegler_" + i).val(OldElementValue);
        //$(".MontantFinal_" + i).trigger("change");
        return;
    }
    if((SumPieces-OldElementValue +  NewElementValue)> montantReglement)
    {
        toastr.warning('Le montant à régler a dépassé le montant de réglement' , 'Erreur!', { progressBar: true, showDuration: 100 });
        $(".MontantARegler_" + i).val(OldElementValue);
        return;
    }
    $("#SumPieces").val(SumPieces - OldElementValue + NewElementValue);
}

function SetInAccounted() {
    var LibelleTypePiece = $("#LibelleTypePiece").val();
    if (confirm("Voulez vous vraiment supprimer les mouvements stock attachés à ce " + LibelleTypePiece))
        var NumPiece = $("#NumPiece").val();
    $.ajax({
        type: "GET",
        url: "/Common/SetInAccounted",
        data: { NumPiece: NumPiece },
        success: function (data) {
            var statut = data.Text;
            var msg = data.Value;

            if (statut == 'KO') {
                toastr.warning(msg, 'Erreur', { progressBar: true, showDuration: 100 });
            }
            else {
                toastr.success('Mouvements stock supprimés', 'Succès!', { progressBar: true, showDuration: 100 });
            }
        },
        failure: function (response) { }
    });
}
function SetAccounted()
{
    var LibelleTypePiece = $("#LibelleTypePiece").val();
    if (confirm("Voulez vous vraiment créer les mouvements stock nécesaires pour ce "+ LibelleTypePiece ))
    var NumPiece = $("#NumPiece").val();
    $.ajax({
        type: "GET",
        url: "/Common/SetAccounted",
        data: { NumPiece: NumPiece },
        success: function (data) {
            var statut = data.Text;
            var msg = data.Value;

            if (statut == 'KO') {
                toastr.warning(msg, 'Erreur', { progressBar: true, showDuration: 100 });
            }
            else {
                toastr.success('Mouvements stock créés', 'Succès!', { progressBar: true, showDuration: 100 });
            }
        },
        failure: function (response) { }
    });
}

function ChangeListPiecesSourceType(Type) {

    $(".btnSourcesF").removeClass("btn-primary");
    $(".btnSourcesF").addClass("btn-default");
    $("#" + Type + "Btn").removeClass("btn-default");
    $("#" + Type + "Btn").addClass("btn-primary");
    $("#TypePieces").val(Type);
    RefreshListPiecesGrid();

}
function RefreshListPiecesGrid() {
    debugger;
    $('#PieceListGrid').DataTable().destroy();
    LoadPieceListData();
}
function PrintFacture(NumPiece) {
    $.ajax({
        url: "/Print/ImprimerRapportFactureClient",
        beforeSend: function () {
            $.blockUI({ message: 'Patientez un peu...' });


        },
        complete: function () {
            $.unblockUI();
        },
        data: { CodeFacture: NumPiece },

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
function PrintFactureDetails()
{
    var NumPiece = $("#NumPiece").val();
    $.ajax({
        url: "/Print/ImprimerRapportFactureClient",
        beforeSend: function () {
            $.blockUI({ message: 'Patientez un peu...' });
      
       
   },
   complete: function(){
       $.unblockUI();
   },
        data: { CodeFacture: NumPiece },

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
function PrintReglement(ReglementID) {
    $.ajax({
        url: "/Print/ImprimerRapportReglement",
        beforeSend: function () {
            $.blockUI({ message: 'Patientez un peu...' });


        },
        complete: function () {
            $.unblockUI();
        },
        data: { ReglementID: ReglementID },

        error: function (xhr, textStatus, errorThrown) {

            
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
function PrintReglementDetails() {
    debugger;
    var ReglementID = $("#ReglementID").val();
    $.ajax({
        url: "/Print/ImprimerRapportReglement",
        beforeSend: function () {
            $.blockUI({ message: 'Patientez un peu...' });


        },
        complete: function () {
            $.unblockUI();
        },
        data: { ReglementID: ReglementID },

        error: function (xhr, textStatus, errorThrown) {

            
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
                OPEN_URL_IN_BLANK(Value );
            }
        }
    });

}
function OpenReglementsGridSelector()
{
    var TypePiece = $("#TypePiece").val();
    var NumPiece = $("#NumPiece").val();
    $.ajax({
        type: "GET",
        url: "/Common/OpenReglementsGridSelector",
        data: { NumPiece: NumPiece, TypePiece: TypePiece },
        success: function (data) {
            $('#GenericModel').modal();
            $("#ModalTitle").text("Importer règlement");
            $("#ModalBody").html(data);
           
        },
        failure: function (response) { }
    });
}
function ImportFromExisingReglement()
{
    var selectedReglement = $("#SelectedReglementDiv").val();
    var NumPiece = $("#NumPiece").val();
    $.ajax({
        type: 'POST',
        url: "/Common/ImportFromExisingReglement",
        data: { selectedReglement: selectedReglement, NumPiece: NumPiece },
        dataType: 'json',
        encode: true,
        error: function (data) {
            debugger;
        },
        success: function (data) {
            debugger;
            var statut = data.Text;
            var msg = data.Value;

            if (statut == 'KO') {
                toastr.warning(msg, 'Erreur', { progressBar: true, showDuration: 100 });
            }
            else {

                $('#GenericModel').modal('hide');
                $('#ReglementsGrid').DataTable().ajax.reload();
                toastr.success('Opération réussite', 'Succès', { progressBar: true, showDuration: 100 });
            }

        },
    });
}
function GetPassagePerMonth()
{
    var Month = $("#dateLog").val();
    $.ajax({
        type: 'GET',
        url: "/Vente/GetPassagePerMonth",
        data: { Month: Month },
        success: function (data) {
            $("#PassagePerMonth").html(data);
        },
    });
}
var Div = "<div class='container' style='margin-top:10px;margin-bottom:10px;'><div class='row'><div onclick=UpdateCaseStatus('" + 'REA' + "') title='REALISER' style='background-color:lime;cursor:pointer;border-radius:10px;margin-right:8px;height:15px;' class='col-2'></div><div onclick=UpdateCaseStatus('" + 'RAP' + "')   title='RAPPORTER' style='background-color:#FFFF33;cursor:pointer;border-radius:10px;;margin-right:8px' class='col-2'></div><div onclick=UpdateCaseStatus('" + 'NRL' + "')  title='NON REALISER'style='background-color:red;cursor:pointer;border-radius:10px;;margin-right:8px' class='col-2'></div><div onclick=UpdateCaseStatus('" + 'PLA' + "')  style='border-radius:10px;background-color:#9A9A9A;cursor:pointer;margin-right:8px' title='PLANIFIER' class='col-2'><center></center></div><div onclick=UpdateCaseStatus('" + 'VIDE' + "')  style='border-radius:10px;cursor:pointer;border:2px #9A9A9A solid;' title='VIDE' class='col-2'></div></div></div>";
function ChangeTdContentToChoose(Client, Jour) {
    $("#SelectedCase").val("PassageClient_" + Client + "_" + Jour);
    $("#SelectedClient").val(Client);
    $("#SelectedDay").val(Jour);
    $('#SmallModel').modal();
    $("#SmallModalTitle").text("Passages");
    $("#SmallModalBody").html(Div);
}
function UpdateCaseStatus(Status) {
    debugger;
    var Client =  $("#SelectedClient").val();
    var Day = $("#SelectedDay").val();
    var Month = $("#dateLog").val();
    $.ajax({
        type: 'GET',
        url: "/Vente/UpdateCaseStatus",
        data: { Client: Client,Month:Month,Day:Day,Status:Status },
        success: function (data) {
            debugger;
            if (Status == "REA") {
                $("#" + $("#SelectedCase").val()).css("background-color", "lime");
            }
            else if (Status == "PLA") {
                $("#" + $("#SelectedCase").val()).css("background-color", "#9A9A9A");
            }
            else if (Status == "RAP") {
                $("#" + $("#SelectedCase").val()).css("background-color", "#FFFF33");
            }
            else if (Status == "NRL") {
                $("#" + $("#SelectedCase").val()).css("background-color", "red");
            }
            else {
                $("#" + $("#SelectedCase").val()).css("background-color", "#f8f9fc");
            }
            $('#SmallModel').modal('hide');

            $("#SelectedClient").val("");
            ("#SelectedDay").val("");
            $("#dateLog").val("");
        },
    });

    

}
$(".AttachFacture").click(function () {
    debugger;
    if ($(this).is(":checked")) {
        $("#ParametresSupplementaiesDIV").show();
        if ($("#TypeReglement option:selected").text() == "Chèque") {
            $("#ChequeParamsDiv").show();
        }
    }
    else {

        $("#ParametresSupplementaiesDIV").hide();


    }
});
$("#TypeReglement").change(function () {
    debugger;
    if ($("#TypeReglement option:selected").text() == "Chèque" || $("#TypeReglement option:selected").text() == "Traite") {
        $("#ChequeParamsDiv").show();
        $("#VirementParamsDiv").hide();
    }
    else if ($("#TypeReglement option:selected").text() == "Virement" ) {
        $("#VirementParamsDiv").show();
        $("#ChequeParamsDiv").hide();
        changeCompte();
    }

    else {
        $("#ChequeParamsDiv").hide();
        $("#VirementParamsDiv").hide();
    }
    if ( $("#TypeReglement option:selected").text() == "Retenue à la source") {
        $.ajax({
            type: "GET",
            url: "/Common/GetDefaultRASAmmount",
            data: { numPiece: $("#numPiece").val() },
            success: function (data) {
                data = data.replace(".", ",");
                    $("#Montant").val(data);

                
            },

        });
    }
    else {
        $.ajax({
            type: "GET",
            url: "/Common/GetDefaultAmmount",
            data: { numPiece: $("#numPiece").val() },
            success: function (data) {
                debugger;
                data = data.replace(".", ",");
                    $("#Montant").val(data);

            },

        });
    }
})
function changeCompte() {
    var Sens = $("#Sens")[0].elements[0].checked == true ? "1" : "-1";
    debugger;
    $.ajax({
        type: "GET",
        url: "/Common/GetPartialViewComptes",
        data: { OwnerType: $("#OwnerType").val(), Sens: Sens, OwnerID: $("#OwnerId").val() },
        success: function (data) {
            debugger;
            $("#VirementParamsDiv").html(data);
        },

    });


}
$('input[type=radio][name=Sens]').change(function () {
    changeCompte();
});
$('#OwnerId').change(function () {
    changeCompte();
});