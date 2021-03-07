

function OpenServicesEditor(ID) {
    debugger;
    $.ajax({
        type: "GET",
        url: "/Stock/OpenServicesEditor",
        data:{ID:ID},
        success: function (data) {
            debugger;

            $('#GenericModel').modal();
            $("#ModalTitle").text("Produit");
            $("#ModalBody").html(data);

        },
        failure: function (response) { }
    });
}
function OpenMVTStockEditor(ID)
{
    debugger;
    $.ajax({
        type: "GET",
        url: "/Stock/OpenMVTStockEditor",
        data: { ID: ID },
        success: function (data) {
            debugger;

            $('#GenericModel').modal();
            $("#ModalTitle").text("Mouvements stock");
            $("#ModalBody").html(data);

        },
        failure: function (response) { }
    });
}
function AddOrUpdateMVTStock() {
    debugger;
    var formData = $("#ParamForm").serializeArray();
    if (formData[1].value == "" || formData[1].value == null || formData[1].value == undefined || formData[1].value == "0") {
        toastr.error('Veuillez renseigner le champs Service', 'error', { progressBar: true, showDuration: 100 });
        return;
    }
    if (formData[5].value == "" || formData[5].value == null || formData[5].value == undefined || formData[5].value == "0") {
        toastr.error('Veuillez renseigner le champs Quantité', 'error', { progressBar: true, showDuration: 100 });
        return;
    }
    $.ajax({
        type: 'POST',
        url: "/Stock/AddOrUpdateMVTStock",
        data: formData,
        encode: true,
        success: function (response) {
            debugger;
            //var res = JSON.parse(response);
            if (response.Text == "KO")
            {
                toastr.error(response.Value, 'error', { progressBar: true, showDuration: 100 });
            }
            else
            {
                $('#GenericModel').modal('hide');
                toastr.success('Opération réussite', 'Succès', { progressBar: true, showDuration: 100 });
                $('#MVTStockGrid').DataTable().ajax.reload();
            }


        },
    })
}
function AddOrUpdateService() {
    debugger;
    var formData = $("#ParamForm").serializeArray();
    if (formData[0].value == "" || formData[0].value == null || formData[0].value == undefined) {
        toastr.error('Veuillez renseigner le champs Référence', 'error', { progressBar: true, showDuration: 100 });
        return;
    }
    if (formData[1].value == "" || formData[1].value == null || formData[1].value == undefined) {
        toastr.error('Veuillez renseigner le champs Libellé', 'error', { progressBar: true, showDuration: 100 });
        return;
    }

    $.ajax({
        type: 'POST',
        url: "/Stock/AddOrUpdateService",
        data: formData,
        encode: true,
        success: function (response) {
            debugger;
            //var res = JSON.parse(response);
            if (response.Text == "KO") {
                toastr.error(response.Value, 'error', { progressBar: true, showDuration: 100 });
            }
            else {
                $('#GenericModel').modal('hide');
                toastr.success('Opération réussite', 'Succès', { progressBar: true, showDuration: 100 });
                $('#ServicesGrid').DataTable().ajax.reload();
            }

        },
    })
}
function LoadMVTStockData()
{
    
    var table = $('#MVTStockGrid').DataTable({
        "order": [[0, "desc"]],
        "autoWidth": true,
        ajax: {
            url: "/Stock/GetAllMVTStock",
            dataSrc: ''
        },
        columns: [{ "data": "ID" },
         {
             "data": "",
             "render": function (data, type, row) {
               
                 var ID = row.ID;
                // return "<div style='display:inline-flex'><button onclick=OpenMVTStockEditor('" + ID + "') style='width:30px;height:30px;' class='d-none d-sm-inline-block btn btn-sm btn-info shadow-sm'><span class='icon text-white-30'><i class='fas fa-edit'></i></span><button style='width:30px;height:30px;margin-left:10px;'onclick=DeleteMVTStockt('" + ID + "')  class='d-none d-sm-inline-block btn btn-sm btn-danger shadow-sm'><span class='icon text-white-30'><i class='fas fa-trash'></i></span></button></div>"

                 return "<div style='display:inline-flex'>" +
                     "<a style='cursor:pointer' onclick=OpenMVTStockEditor('" + ID + "')  href='#' class='btn btn-warning btn-circle btn-sm'>" + "<i class='fas fa-edit'></i> </a>" +
                     "<a style='cursor:pointer;margin:0px 5px 0px 5px' onclick=DeleteMVTStockt('" + ID + "')  href='#' class='btn btn-danger btn-circle btn-sm'>" + "<i class='fas fa-trash'></i> </a>" +
                     "</div>";
             }
         },
        { "data": "ServiceNames" },
        {
            "data": "prixUnitaire",
            "render": function (data, type, row) {
                if (data != null)
                    return data.toFixed(3);
                else {
                    return data;
                }
            }
        },
        { "data": "Quantite" },
        { "data": "Observation" },
        {
            "data": "Date",
            render: function (data, type, row) {
                if (type === "sort" || type === "type") {
                    return data;
                }
                return moment(data).format("YYYY-MM-DD HH:mm");
            }

        },
        { "data": "Sens" }

        ],

        "columnDefs": [
         {
             "targets": [0,7],
             "visible": false,
             "searchable": false
         }],
        
        //datatable color row with condition
        "fnRowCallback" : function(row, aData)
        {
            debugger;
            if (aData.Sens == "-1")
            {
                $(row).css({ "background-color": "#ffc0cb" });
            }
            else
            {
                $(row).css({ "background-color": "#98fb98" });
            }
}
    });

}
function RefreshServicesGrid() {
    debugger;
    $('#ServicesGrid').DataTable().destroy();
    LoadServicesData();
}
function LoadServicesData() {
    var libelle = $("#LibelleFilter").val();
    var Status = $("#StatusFilter").val();
    var Vente = true;
    var Achat = true;
    var AchatAndVente = false;
    if (Status == "1") {
        Vente = false;
    }
    if (Status == "2") {
        Achat = false;
    }
    if (Status == "3") {
        AchatAndVente = true;
    }
    var code = $("#ReferenceFilter").val();

    var table = $('#ServicesGrid').DataTable({
        "order": [[0, "desc"]],
        "autoWidth": true,
        ajax: {
            url: "/Stock/GetAllServices",
            data: { libelle: libelle, code: code, Vente: Vente, Achat: Achat, AchatAndVente: AchatAndVente },
            dataSrc: ''
        },
        columns: [{ "data": "ID" },
         {
             "data": "",
             "render": function (data, type, row) {
                 debugger
                 var ID = row.ID;
              //   return "<div style='display:inline-flex'><button onclick=OpenServicesEditor('" + ID + "') style='width:30px;height:30px;' class='d-none d-sm-inline-block btn btn-sm btn-info shadow-sm'><span class='icon text-white-30'><i class='fas fa-edit'></i></span></button></div>"

                 return "<div style='display:inline-flex'>" +
                     "<a style='cursor:pointer' onclick=OpenServicesEditor('" + ID + "')  href='#' class='btn btn-warning btn-circle btn-sm'>" + "<i class='fas fa-edit'></i> </a>" +
                   //  "<a style='cursor:pointer;margin:0px 5px 0px 5px' onclick=DeleteMVTStockt('" + ID + "')  href='#' class='btn btn-danger btn-circle btn-sm'>" + "<i class='fas fa-trash'></i> </a>" +
                     "</div>";
             }
         },
         //{ "data": "Reference" },
        { "data": "Libelle" },
        {
            "data": "Montant",
            "render": function (data, type, row) {
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

function DeleteMVTStockt(ID)
{
    debugger;
    var formData = $("#ParamForm").serializeArray();
    $.ajax({

        url: "/Stock/DeleteMVTStockt",
        data: { ID: ID },

        success: function (response) {
            debugger;
            toastr.success('Supprimé avec succès', 'Succès', { progressBar: true, showDuration: 100 });
            $('#MVTStockGrid').DataTable().ajax.reload();

        },
    });

}
function DeleteService(ID) {
    debugger;
    var formData = $("#ParamForm").serializeArray();
    $.ajax({
        
        url: "/Stock/DeleteService",
        data: { ID: ID },
       
        success: function (response) {
            debugger;
            toastr.success('Supprimé avec succès', 'Succès', { progressBar: true, showDuration: 100 });
            $('#ServicesGrid').DataTable().ajax.reload();

        },
    });
}
