//Fournisseur
function OpenFournisseurEditor(ID) {
    debugger;
    $.ajax({
        type: "GET",
        url: "/Achat/OpenFournisseurEditor",
        data:{ID:ID},
        success: function (data) {
            debugger;

            $('#GenericModel').modal();
            $("#ModalTitle").text("Fournisseur");
            $("#ModalBody").html(data);

        },
        failure: function (response) { }
    });
}
function AddOrUpdateFournisseur() {
    debugger;
    var formData = $("#ParamForm").serializeArray();
    if (formData[1].value == "" || formData[1].value == null || formData[1].value == undefined) {
        toastr.error('Veuillez renseigner le champs Nom fournisseur', 'error', { progressBar: true, showDuration: 100 });
        return;
    }
    $.ajax({
        type: 'POST',
        url: "/Achat/AddOrUpdateFournisseur",
        data: formData,
        encode: true,
        success: function (response) {
            debugger;
            //var res = JSON.parse(response);

            $('#GenericModel').modal('hide');
            $('#FournisseursGrid').DataTable().ajax.reload();

        },
    })
}
function LoadFournisseurData() {
    debugger;
    var TypeFournisseur = $("#TypeFournisseur").val();
    var table = $('#FournisseursGrid').DataTable({
        "order": [[0, "desc"]],
        "autoWidth": true,
        ajax: {
            url: "/Achat/GetAllFournisseurs",
            data: {TypeFournisseur:TypeFournisseur},
            dataSrc: ''
        },
        columns: [
             
            { "data": "ID" },
             {
                 "data": "",
                 "render": function (data, type, row) {
                     debugger
                     var ID = row.ID;
                     return "<div style='display:inline-flex'><button onclick=OpenFournisseurEditor('" + ID + "') style='width:30px;height:30px;' class='d-none d-sm-inline-block btn btn-sm btn-info shadow-sm'><span class='icon text-white-30'><i class='fas fa-edit'></i></span></button></div>"
                 }
             },
                  { "data": "OwnerName" },
                  { "data": "Adresse" },
                  { "data": "CodePostal" },
                  { "data": "IBAN" },
                  { "data": "Tel1" },
                  { "data": "Tel2" },
                  { "data": "Fax" },
                  { "data": "MatriculeFiscal" },
                  { "data": "IsActive" }

        ],
        "columnDefs": [
         {
             "targets": [0,10],
             "visible": false,
             "searchable": false
         }],
         "fnRowCallback": function (row, aData) {
        debugger;
        if (aData.IsActive == false) {
            $(row).css({ "background-color": "#ffc0cb" });
        }

    }
    });

}
function DeleteFournisseur(ID) {
    debugger;
    if (confirm('Vous confirmez la suppression de ce fournisseur??')) {
        var formData = $("#ParamForm").serializeArray();
        $.ajax({

            url: "/Vente/DeleteFournisseur",
            data: { ID: ID },


            success: function (response) {
                debugger;
                toastr.success('Supprimé avec succès', 'Succès', { progressBar: true, showDuration: 100 });
                $('#FournisseursGrid').DataTable().ajax.reload();

            },
        });
    }
}

//Client
function OpenClientEditor(ID) {
    debugger;
    $.ajax({
        type: "GET",
        url: "/Vente/OpenClientEditor",
        data:{ID:ID},
        success: function (data) {
            debugger;

            $('#GenericModel').modal();
            $("#ModalTitle").text("Client");
            $("#ModalBody").html(data);

        },
        failure: function (response) { }
    });
}
function AddOrUpdateClient() {
    debugger;
    var formData = $("#ParamForm").serializeArray();
    if (formData[1].value == "" || formData[1].value == null || formData[1].value == undefined ) {
        toastr.error('Veuillez renseigner le champs Nom client', 'error', { progressBar: true, showDuration: 100 });
        return;
    }
    $.ajax({
        type: 'POST',
        url: "/Vente/AddOrUpdateClient",
        data: formData,
        dataType: 'json',
        encode: true,
        success: function (response) {
            debugger;
            $('#GenericModel').modal('hide');
            $('#ClientsGrid').DataTable().ajax.reload();
            toastr.success('Opération réussite', 'Succès', { progressBar: true, showDuration: 100 });

        },
    });
}
function LoadClientsData() {
    debugger;
    var table = $('#ClientsGrid').DataTable({
        "order": [[0, "desc"]],
        "autoWidth": true,
        ajax: {
            url: "/Vente/GetAllClients",
            dataSrc: ''
        },
        columns: [

            { "data": "ID" },
                        {
                            "data": "",
                            "render": function (data, type, row) {
                                debugger
                                var ID = row.ID;
                                return "<div style='display:inline-flex'><button onclick=OpenClientEditor('" + ID + "') style='width:30px;height:30px;' class='d-none d-sm-inline-block btn btn-sm btn-info shadow-sm'><span class='icon text-white-30'><i class='fas fa-edit'></i></span></button></div>"

                            }
                        },

                  { "data": "OwnerName" },
                  { "data": "Adresse" },
                  { "data": "IBAN" },
                  { "data": "Tel1" },
                  { "data": "Tel2" },
                  { "data": "Fax" },
                  { "data": "MatriculeFiscal" },
                  { "data": "IsActive" },
                  { "data": "Route" }
                  
        
        ],
        "columnDefs": [
         {
             "targets": [0,10],
             "visible": false,
             "searchable": false
         }],
        //datatable color row with condition
        "fnRowCallback": function (row, aData) {
            debugger;
            if (aData.IsActive == false) {
                $(row).css({ "background-color": "#ffc0cb" });
            }

        }
    });

}
function DeleteClient(ID)
{
    debugger;
    if (confirm('Vous confirmez la suppression de ce client??')) {
        var formData = $("#ParamForm").serializeArray();
        $.ajax({

            url: "/Vente/DeleteClient",
            data: { ID: ID },


            success: function (response) {
                debugger;
                toastr.success('Supprimé avec succès', 'Succès', { progressBar: true, showDuration: 100 });
                $('#ClientsGrid').DataTable().ajax.reload();

            },
        });
    }
}

// Categories Produits
function DeleteCategoriesProduit(ID) {
    debugger;
    if (confirm('Vous confirmez la suppression de cette catégorie?')) {
        var formData = $("#ParamForm").serializeArray();
        $.ajax({

            url: "/Administration/DeleteCategoriesProduit",
            data: { ID: ID },

            success: function (response) {
                debugger;
                toastr.success('Supprimé avec succès', 'Succès', { progressBar: true, showDuration: 100 });
                $('#CategoriesProduitsGrid').DataTable().ajax.reload();

            },
        });
    }
}
function OpenCategoriesProduitsEditor(ID) {
    debugger;
    $.ajax({
        type: "GET",
        url: "/Administration/OpenCategorieProduitsEditor",
        data: { ID: ID },
        success: function (data) {
            debugger;

            $('#GenericModel').modal();
            $("#ModalTitle").text("Catégorie produit");
            $("#ModalBody").html(data);

        },
        failure: function (response) { }
    });
}
function AddOrUpdateCategoriesProduit() {
    debugger;
    var formData = $("#ParamForm").serializeArray();
    if (formData[1].value == "" || formData[1].value == null || formData[1].value == undefined) {
        toastr.error('Veuillez renseigner le champs Libellé', 'error', { progressBar: true, showDuration: 100 });
        return;
    }
    $.ajax({
        type: 'POST',
        url: "/Administration/AddOrUpdateCategoriesProduit",
        data: formData,
        encode: true,
        success: function (response) {
            debugger;
            //var res = JSON.parse(response);

            $('#GenericModel').modal('hide');
            toastr.success('Opération réussite', 'Succès', { progressBar: true, showDuration: 100 });
            $('#CategoriesProduitsGrid').DataTable().ajax.reload();


        },
    })
}
function LoadCategoriesProduitsData() {
    debugger;
    var table = $('#CategoriesProduitsGrid').DataTable({
        "order": [[0, "desc"]],
        "autoWidth": true,
        ajax: {
            url: "/Administration/GetAllCategorieProduits",
            dataSrc: ''
        },
        columns: [{ "data": "ID" },
         {
             "data": "",
             "render": function (data, type, row) {
                 debugger
                 var ID = row.ID;
                 return "<div style='display:inline-flex'><button onclick=OpenCategoriesProduitsEditor('" + ID + "') style='width:30px;height:30px;' class='d-none d-sm-inline-block btn btn-sm btn-info shadow-sm'><span class='icon text-white-30'><i class='fas fa-edit'></i></span><button style='width:30px;height:30px;margin-left:10px;'onclick=DeleteCategoriesProduit('" + ID + "')  class='d-none d-sm-inline-block btn btn-sm btn-danger shadow-sm'><span class='icon text-white-30'><i class='fas fa-trash'></i></span></button></div>"
             }
         },
      
        { "data": "Libelle" }




        ],
        "columnDefs": [
         {
             "targets": [0],
             "visible": false,
             "searchable": false
         }]
    });

}

// Types Produits
function OpenTypesProduitsEditor(ID) {
    debugger;
    $.ajax({
        type: "GET",
        url: "/Administration/OpenTypeProduitsEditor",
        data: { ID: ID },
        success: function (data) {
            debugger;

            $('#GenericModel').modal();
            $("#ModalTitle").text("Type produit");
            $("#ModalBody").html(data);

        },
        failure: function (response) { }
    });
}
function DeleteTypesProduit(ID) {
    debugger;
    if (confirm('Vous confirmez la suppression de ce type??')) {
        var formData = $("#ParamForm").serializeArray();
        $.ajax({

            url: "/Administration/DeleteTypeProduit",
            data: { ID: ID },

            success: function (response) {
                debugger;
                toastr.success('Supprimé avec succès', 'Succès', { progressBar: true, showDuration: 100 });
                $('#TypesProduitsGrid').DataTable().ajax.reload();

            },
        });
    }
}
function AddOrUpdateTypesProduit() {
    debugger;
    var formData = $("#ParamForm").serializeArray();
    if (formData[1].value == "" || formData[1].value == null || formData[1].value == undefined) {
        toastr.error('Veuillez renseigner le champs Libellé', 'error', { progressBar: true, showDuration: 100 });
        return;
    }
    if (formData[2].value == "" || formData[2].value == null || formData[2].value == undefined || formData[2].value == "0") {
        toastr.error('Veuillez renseigner le champs Catégorie', 'error', { progressBar: true, showDuration: 100 });
        return;
    }
    $.ajax({
        type: 'POST',
        url: "/Administration/AddOrUpdateTypeProduit",
        data: formData,
        encode: true,
        success: function (response) {
            debugger;
            //var res = JSON.parse(response);

            $('#GenericModel').modal('hide');
            toastr.success('Opération réussite', 'Succès', { progressBar: true, showDuration: 100 });
            $('#TypesProduitsGrid').DataTable().ajax.reload();


        },
    })
}
function LoadTypesProduitsData() {
    debugger;
    var table = $('#TypesProduitsGrid').DataTable({
        "order": [[0, "desc"]],
        "autoWidth": true,
        ajax: {
            url: "/Administration/GetAllTypeProduits",
            dataSrc: ''
        },
        columns: [{ "data": "ID" },
         {
             "data": "",
             "render": function (data, type, row) {
                 debugger
                 var ID = row.ID;
                 return "<div style='display:inline-flex'><button onclick=OpenTypesProduitsEditor('" + ID + "') style='width:30px;height:30px;' class='d-none d-sm-inline-block btn btn-sm btn-info shadow-sm'><span class='icon text-white-30'><i class='fas fa-edit'></i></span><button style='width:30px;height:30px;margin-left:10px;'onclick=DeleteTypesProduit('" + ID + "')  class='d-none d-sm-inline-block btn btn-sm btn-danger shadow-sm'><span class='icon text-white-30'><i class='fas fa-trash'></i></span></button></div>"
             }
         },
        
        { "data": "Libelle" },
        { "data": "CategorieLibelle" }




        ],
        "columnDefs": [
         {
             "targets": [0],
             "visible": false,
             "searchable": false
         }]
    });

}

//Motifs congé
function OpenMotifsCongesEditor(ID) {
    debugger;
    $.ajax({
        type: "GET",
        url: "/Administration/OpenMotifCongeEditor",
        data: { ID: ID },
        success: function (data) {
            debugger;

            $('#GenericModel').modal();
            $("#ModalTitle").text("Motif congé");
            $("#ModalBody").html(data);

        },
        failure: function (response) { }
    });
}
function AddOrUpdateMotifsConges() {
    debugger;
    var formData = $("#ParamForm").serializeArray();
    if (formData[1].value == "" || formData[1].value == null || formData[1].value == undefined) {
        toastr.error('Veuillez renseigner le champs Libellé', 'error', { progressBar: true, showDuration: 100 });
        return;
    }
    $.ajax({
        type: 'POST',
        url: "/Administration/AddOrUpdateMotifConge",
        data: formData,
        encode: true,
        success: function (response) {
            debugger;
            //var res = JSON.parse(response);

            $('#GenericModel').modal('hide');
            toastr.success('Opération réussite', 'Succès', { progressBar: true, showDuration: 100 });
            $('#MotifsCongesGrid').DataTable().ajax.reload();


        },
    })
}
function LoadMotifsCongesData() {
    debugger;
    var table = $('#MotifsCongesGrid').DataTable({
        "order": [[0, "desc"]],
        "autoWidth": true,
        ajax: {
            url: "/Administration/GetAllMotifConge",
            dataSrc: ''
        },
        columns: [{ "data": "ID" },
         {
             "data": "",
             "render": function (data, type, row) {
                 debugger
                 var ID = row.ID;
                 return "<div style='display:inline-flex'><button onclick=OpenMotifsCongesEditor('" + ID + "') style='width:30px;height:30px;' class='d-none d-sm-inline-block btn btn-sm btn-info shadow-sm'><span class='icon text-white-30'><i class='fas fa-edit'></i></span><button style='width:30px;height:30px;margin-left:10px;'onclick=DeleteMotifsConges('" + ID + "')  class='d-none d-sm-inline-block btn btn-sm btn-danger shadow-sm'><span class='icon text-white-30'><i class='fas fa-trash'></i></span></button></div>"
             }
         },
       
        { "data": "Libelle" },




        ],
        "columnDefs": [
         {
             "targets": [0],
             "visible": false,
             "searchable": false
         }]
    });

}
function DeleteMotifsConges(ID) {
    debugger;
    if (confirm('Vous confirmez la suppression de ce motif de congé??')) {
        var formData = $("#ParamForm").serializeArray();
        $.ajax({

            url: "/Administration/DeleteMotifConge",
            data: { ID: ID },

            success: function (response) {
                debugger;
                toastr.success('Supprimé avec succès', 'Succès', { progressBar: true, showDuration: 100 });
                $('#MotifsCongesGrid').DataTable().ajax.reload();

            },
        });
    }
}

//Taxes
function OpenTaxesEditor(ID) {
    debugger;
    $.ajax({
        type: "GET",
        url: "/Administration/OpenTaxesEditor",
        data: { ID: ID },
        success: function (data) {
            debugger;

            $('#GenericModel').modal();
            $("#ModalTitle").text("Taxes");
            $("#ModalBody").html(data);

        },
        failure: function (response) { }
    });
}
function AddOrUpdateTaxes() {
    debugger;
    var formData = $("#ParamForm").serializeArray();
    $.ajax({
        type: 'POST',
        url: "/Administration/AddOrUpdateTaxes",
        data: formData,
        encode: true,
        success: function (response) {
            debugger;
            //var res = JSON.parse(response);

            $('#GenericModel').modal('hide');
            toastr.success('Opération réussite', 'Succès', { progressBar: true, showDuration: 100 });
            $('#TaxesGrid').DataTable().ajax.reload();


        },
    })
}
function LoadTaxesData() {
    debugger;
    var table = $('#TaxesGrid').DataTable({
        "order": [[0, "desc"]],
        "autoWidth": true,
        ajax: {
            url: "/Administration/GetAllTaxes",
            dataSrc: ''
        },
        columns: [{ "data": "ID" },
         {
             "data": "",
             "render": function (data, type, row) {
                 debugger
                 var ID = row.ID;
                 return "<div style='display:inline-flex'><button onclick=OpenTaxesEditor('" + ID + "') style='width:30px;height:30px;' class='d-none d-sm-inline-block btn btn-sm btn-info shadow-sm'><span class='icon text-white-30'><i class='fas fa-edit'></i></span><button style='width:30px;height:30px;margin-left:10px;'onclick=DeleteTaxes('" + ID + "')  class='d-none d-sm-inline-block btn btn-sm btn-danger shadow-sm'><span class='icon text-white-30'><i class='fas fa-trash'></i></span></button></div>"
             }
         },
       
        { "data": "NomTaxe" },
        { "data": "Pourcentage" },
        { "data": "IsDefault" }




        ],
        "columnDefs": [
         {
             "targets": [0],
             "visible": false,
             "searchable": false
         }]
    });

}
function DeleteTaxes(ID) {
    debugger;
    if (confirm('Vous confirmez la suppression de ce taxe??')) {
        var formData = $("#ParamForm").serializeArray();
        $.ajax({

            url: "/Administration/DeleteTaxes",
            data: { ID: ID },

            success: function (response) {
                debugger;
                toastr.success('Supprimé avec succès', 'Succès', { progressBar: true, showDuration: 100 });
                $('#TaxesGrid').DataTable().ajax.reload();

            },
        });
    }
}

//JoursFeries
function OpenJoursFeriesEditor(ID) {
    debugger;
    $.ajax({
        type: "GET",
        url: "/Administration/OpenJoursFeriesEditor",
        data: { ID: ID },
        success: function (data) {
            debugger;

            $('#GenericModel').modal();
            $("#ModalTitle").text("JoursFeries");
            $("#ModalBody").html(data);

        },
        failure: function (response) { }
    });
}
function AddOrUpdateJoursFeries() {
    debugger;
    var formData = $("#ParamForm").serializeArray();
    $.ajax({
        type: 'POST',
        url: "/Administration/AddOrUpdateJoursFeries",
        data: formData,
        encode: true,
        success: function (response) {
            debugger;
            //var res = JSON.parse(response);

            $('#GenericModel').modal('hide');
            toastr.success('Opération réussite', 'Succès', { progressBar: true, showDuration: 100 });
            $('#JoursFeriesGrid').DataTable().ajax.reload();


        },
    })
}
function LoadJoursFeriesData() {
    debugger;
    var table = $('#JoursFeriesGrid').DataTable({
        "order": [[0, "desc"]],
        "autoWidth": true,
        ajax: {
            url: "/Administration/GetAllJoursFeries",
            dataSrc: ''
        },
        columns: [{ "data": "ID" },
         {
             "data": "",
             "render": function (data, type, row) {
                 debugger
                 var ID = row.ID;
                 return "<div style='display:inline-flex'><button onclick=OpenJoursFeriesEditor('" + ID + "') style='width:30px;height:30px;' class='btn btn-info btn-icon-split'><span class='icon text-white-30'><i class='fas fa-edit'></i></span><button style='width:30px;height:30px;margin-left:10px;'onclick=DeleteJoursFeries('" + ID + "')  class='btn btn-danger btn-icon-split'><span class='icon text-white-30'><i class='fas fa-trash'></i></span></button></div>"
             }
         },

        { "data": "Libelle" },
        {
            "data": "Date",
            render: function (data, type, row) {
                if (type === "sort" || type === "type") {
                    return data;
                }
                return moment(data).format("DD/MM/YYYY");
            }
        },
        { "data": "NbrJours" }




        ],
        "columnDefs": [
         {
             "targets": [0],
             "visible": false,
             "searchable": false
         }]
    });

}
function DeleteJoursFeries(ID) {
    debugger;
    if (confirm('Vous confirmez la suppression de ce jour férié??')) {
        var formData = $("#ParamForm").serializeArray();
        $.ajax({

            url: "/Administration/DeleteJoursFeries",
            data: { ID: ID },

            success: function (response) {
                debugger;
                toastr.success('Supprimé avec succès', 'Succès', { progressBar: true, showDuration: 100 });
                $('#JoursFeriesGrid').DataTable().ajax.reload();

            },
        });
    }
}

//Banque
function OpenBanqueEditor(ID) {
    debugger;
    $.ajax({
        type: "GET",
        url: "/Administration/OpenBanqueEditor",
        data: { ID: ID },
        success: function (data) {
            debugger;

            $('#GenericModel').modal();
            $("#ModalTitle").text("Banque");
            $("#ModalBody").html(data);

        },
        failure: function (response) { }
    });
}
function AddOrUpdateBanque() {
    debugger;
    var formData = $("#ParamForm").serializeArray();
    if (formData[1].value == "" || formData[1].value == null || formData[1].value == undefined) {
        toastr.error('Veuillez renseigner le champs Libellé', 'error', { progressBar: true, showDuration: 100 });
        return;
    }
    $.ajax({
        type: 'POST',
        url: "/Administration/AddOrUpdateBanque",
        data: formData,
        encode: true,
        success: function (response) {
            debugger;
            //var res = JSON.parse(response);

            $('#GenericModel').modal('hide');
            toastr.success('Opération réussite', 'Succès', { progressBar: true, showDuration: 100 });
            $('#BanqueGrid').DataTable().ajax.reload();


        },
    })
}
function LoadBanqueData() {
    debugger;
    var table = $('#BanqueGrid').DataTable({
        "order": [[0, "desc"]],
        "autoWidth": true,
        ajax: {
            url: "/Administration/GetAllBanque",
            dataSrc: ''
        },
        columns: [{ "data": "ID" },
         {
             "data": "",
             "render": function (data, type, row) {
                 debugger
                 var ID = row.ID;
                 return "<div style='display:inline-flex'><button onclick=OpenBanqueEditor('" + ID + "') style='width:30px;height:30px;' class='d-none d-sm-inline-block btn btn-sm btn-info shadow-sm'><span class='icon text-white-30'><i class='fas fa-edit'></i></span><button style='width:30px;height:30px;margin-left:10px;'onclick=DeleteBanque('" + ID + "')  class='d-none d-sm-inline-block btn btn-sm btn-danger shadow-sm'><span class='icon text-white-30'><i class='fas fa-trash'></i></span></button></div>"
             }
         },

        { "data": "NomBanque" },
        
        { "data": "Adresse" }




        ],
        "columnDefs": [
         {
             "targets": [0],
             "visible": false,
             "searchable": false
         }]
    });

}
function DeleteBanque(ID) {
    debugger;
    if (confirm('Vous confirmez la suppression de cette banque??')) {
        var formData = $("#ParamForm").serializeArray();
        $.ajax({

            url: "/Administration/DeleteBanque",
            data: { ID: ID },

            success: function (response) {
                debugger;
                toastr.success('Banque supprimée avec succès', 'Succès', { progressBar: true, showDuration: 100 });
                $('#BanqueGrid').DataTable().ajax.reload();

            },
        });
    }
}

//Comptes bancaires
function OpenCompteBancaireEditor(ID) {
    debugger;
    var OwnerType = $("#OwnerType").val();

    $.ajax({
        type: "GET",
        url: "/Administration/OpenCompteBancaireEditor",
        data: { ID: ID, OwnerType: OwnerType },
        success: function (data) {
            debugger;

            $('#GenericModel').modal();
            $("#ModalTitle").text("Compte bancaire");
            $("#ModalBody").html(data);

        },
        failure: function (response) { }
    });
}
function AddOrUpdateCompteBancaire() {
    debugger;
    var formData = $("#ParamForm").serializeArray();
    $.ajax({
        type: 'POST',
        url: "/Administration/AddOrUpdateCompteBancaire",
        data: formData,
        encode: true,
        success: function (response) {
            debugger;
            //var res = JSON.parse(response);

            $('#GenericModel').modal('hide');
            toastr.success('Opération réussite', 'Succès', { progressBar: true, showDuration: 100 });
            $('#CompteBancaireGrid').DataTable().ajax.reload();


        },
    })
}
function LoadCompteBancaireData() {
    debugger;
    var OwnerType = $("#OwnerType").val();
    var table = $('#CompteBancaireGrid').DataTable({
        "order": [[0, "desc"]],
        "autoWidth": true,
        ajax: {
            url: "/Administration/GetAllCompteBancaire",
            data:{OwnerType:OwnerType},
            dataSrc: ''
        },
        columns: [{ "data": "ID" },
         {
             "data": "",
             "render": function (data, type, row) {
                 debugger
                 var ID = row.ID;
                 return "<div style='display:inline-flex'><button onclick=OpenCompteBancaireEditor('" + ID + "') style='width:30px;height:30px;' class='d-none d-sm-inline-block btn btn-sm btn-info shadow-sm'><span class='icon text-white-30'><i class='fas fa-edit'></i></span><button style='width:30px;height:30px;margin-left:10px;'onclick=DeleteCompteBancaire('" + ID + "')  class='d-none d-sm-inline-block btn btn-sm btn-danger shadow-sm'><span class='icon text-white-30'><i class='fas fa-trash'></i></span></button></div>"
             }
         },

        { "data": "Libelle" },

        { "data": "Owner" },
        
        { "data": "OwnerName" },
        { "data": "NomBanque" },
        { "data": "RIB" }




        ],
        "columnDefs": [
         {
             "targets": [0],
             "visible": false,
             "searchable": false
         }]
    });

}
function DeleteCompteBancaire(ID) {
    debugger;
    if (confirm('Vous confirmez la suppression de ce compte bancaire??')) {
        var formData = $("#ParamForm").serializeArray();
        $.ajax({

            url: "/Administration/DeleteCompteBancaire",
            data: { ID: ID },

            success: function (response) {
                debugger;
                toastr.success('Compte bancaire supprimé avec succès', 'Succès', { progressBar: true, showDuration: 100 });
                $('#CompteBancaireGrid').DataTable().ajax.reload();

            },
        });
    }
}

function ChangeBancOwnerSourceType(Type) {

    $(".btnSources").removeClass("btn-primary");
    $(".btnSources").addClass("btn-default");
    $("#" + Type + "OwnerSourceBtn").removeClass("btn-default");
    $("#" + Type + "OwnerSourceBtn").addClass("btn-primary");
    $("#OwnerType").val(Type);
    RefreshCompteGrid();

}
function ChangeFournisseurType(Type) {

    $(".btnSources").removeClass("btn-primary");
    $(".btnSources").addClass("btn-default");
    $("#" + Type + "Btn").removeClass("btn-default");
    $("#" + Type + "Btn").addClass("btn-primary");
    $("#TypeFournisseur").val(Type);
    RefreshFournisseur();

}
function RefreshCompteGrid() {
    debugger;
    $('#CompteBancaireGrid').DataTable().destroy();
    LoadCompteBancaireData();
}
function RefreshFournisseur() {
    debugger;
    $('#FournisseursGrid').DataTable().destroy();
    LoadFournisseurData();
}
//Types règlements
function OpenTypeReglementEditor(ID) {
    debugger;
    $.ajax({
        type: "GET",
        url: "/Administration/OpenTypeReglementEditor",
        data: { ID: ID },
        success: function (data) {
            debugger;

            $('#GenericModel').modal();
            $("#ModalTitle").text("Type règlement");
            $("#ModalBody").html(data);

        },
        failure: function (response) { debugger; }
    });
}
function AddOrUpdateTypeReglement() {
    debugger;
    var formData = $("#ParamForm").serializeArray();
    $.ajax({
        type: 'POST',
        url: "/Administration/AddOrUpdateTypeReglement",
        data: formData,
        encode: true,
        success: function (response) {
            debugger;
            //var res = JSON.parse(response);

            $('#GenericModel').modal('hide');
            toastr.success('Opération réussite', 'Succès', { progressBar: true, showDuration: 100 });
            $('#TypeReglementGrid').DataTable().ajax.reload();


        },
    })
}
function LoadTypeReglementData() {
    debugger;
    var table = $('#TypeReglementGrid').DataTable({
        "order": [[0, "desc"]],
        "autoWidth": true,
        ajax: {
            url: "/Administration/GetAllTypeReglement",
            dataSrc: ''
        },
        columns: [{ "data": "ID" },
         {
             "data": "",
             "render": function (data, type, row) {
                 debugger
                 var ID = row.ID;
                 return "<div style='display:inline-flex'><button onclick=OpenTypeReglementEditor('" + ID + "') style='width:30px;height:30px;' class='d-none d-sm-inline-block btn btn-sm btn-info shadow-sm'><span class='icon text-white-30'><i class='fas fa-edit'></i></span><button style='width:30px;height:30px;margin-left:10px;'onclick=DeleteTypeReglement('" + ID + "')  class='d-none d-sm-inline-block btn btn-sm btn-danger shadow-sm'><span class='icon text-white-30'><i class='fas fa-trash'></i></span></button></div>"
             }
         },

        { "data": "Libelle" },
        ],
        "columnDefs": [
         {
             "targets": [0],
             "visible": false,
             "searchable": false
         }]
    });

}
function DeleteTypeReglement(ID) {
    debugger;
    if (confirm('Vous confirmez la suppression de ce type de règlement??')) {
        var formData = $("#ParamForm").serializeArray();
        $.ajax({

            url: "/Administration/DeleteTypeReglement",
            data: { ID: ID },

            success: function (response) {
                debugger;
                toastr.success('Type règlement supprimé avec succès', 'Succès', { progressBar: true, showDuration: 100 });
                $('#TypeReglementGrid').DataTable().ajax.reload();

            },
        });
    }
}
function RefreshReclamations() {
    $('#ReclamationsGrid').DataTable().destroy();
    LoadReclamations();
}
//Reclamation
function LoadReclamations()
{
    debugger;
    var DateFromFilter = $("#DateFromFilter").val();
    var DateToFilter = $("#DateToFilter").val();
    var ClientFilter = $("#ClientFilter").val();
    var table = $('#ReclamationsGrid').DataTable({
        "order": [[0, "desc"]],
        "autoWidth": true,
        ajax: {
            url: "/Administration/GetAllReclamations",
            data: { DateFromFilter: DateFromFilter, DateToFilter: DateToFilter, ClientFilter: ClientFilter },
            dataSrc: ''
        },
        columns: [{ "data": "ID" },
        { "data": "Client" },
         {
             "data": "",
             "render": function (data, type, row) {
                 debugger
                 var Date = row.Date;
                 return moment(Date).format("YYYY-MM-DD")
             }

         },
          { "data": "Reclamation" },
        ],
        "columnDefs": [
         {
             "targets": [0],
             "visible": false,
             "searchable": false
         }]
    });
}