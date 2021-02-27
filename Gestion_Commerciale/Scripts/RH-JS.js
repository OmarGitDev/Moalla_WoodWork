
 // Personnes
 function DeletePerson(ID) {
     debugger;
     if (confirm('Vous confirmez la suppression de cette personne??')) {
         var formData = $("#ParamForm").serializeArray();
         $.ajax({

             url: "/RH/DeletePerson",
             data: { ID: ID },

             success: function (response) {
                 debugger;
                 toastr.success('Supprimé avec succès', 'Succès', { progressBar: true, showDuration: 100 });
                 $('#PersonsGrid').DataTable().ajax.reload();

             },
         });
     }
 }
 function OpenPersonEditor(ID) {
     debugger;
     $.ajax({
         type: "GET",
         url: "/RH/OpenPersonnesEditor",
         data: { ID: ID },
         success: function (data) {
             debugger;

             $('#GenericModel').modal();
             $("#ModalTitle").text("Personne");
             $("#ModalBody").html(data);

         },
         failure: function (response) { }
     });
 }
 function AddOrUpdatePerson() {
     debugger;


     
     var formData = $("#ParamForm").serializeArray();
         if (formData[1].value == "" || formData[1].value == null || formData[1].value == undefined) {
             toastr.error('Veuillez renseigner le champs Nom', 'error', { progressBar: true, showDuration: 100 });
             return;
         }
         if (formData[2].value == "" || formData[2].value == null || formData[2].value == undefined)
         {
             toastr.error('Veuillez renseigner le champs Prénom', 'error', { progressBar: true, showDuration: 100 });
             return;
         }
         if (formData[3].value == "" || formData[3].value == null || formData[3].value == undefined) {
             toastr.error('Veuillez renseigner le champs Fonction', 'error', { progressBar: true, showDuration: 100 });
             return;
         }
         if (formData[4].value == "" || formData[4].value == null || formData[4].value == undefined) {
             toastr.error('Veuillez renseigner le champs Adresse Email', 'error', { progressBar: true, showDuration: 100 });
             return;
         }
         if (formData[5].value == "" || formData[5].value == null || formData[5].value == undefined) {
             toastr.error('Veuillez renseigner le champs Tel1', 'error', { progressBar: true, showDuration: 100 });
             return;
         } if (formData[7].value == "" || formData[7].value == null || formData[7].value == undefined) {
             toastr.error('Veuillez renseigner le champs Date debut', 'error', { progressBar: true, showDuration: 100 });
             return;
         }
         if (formData[8].value == "" || formData[8].value == null || formData[8].value == undefined) {
             toastr.error('Veuillez renseigner le champs Solde congé', 'error', { progressBar: true, showDuration: 100 });
             return;
         }
     $.ajax({
         type: 'POST',
         url: "/RH/AddOrUpdatePerson",
         data: formData,
         encode: true,
         success: function (response) {
             debugger;
             //var res = JSON.parse(response);

             $('#GenericModel').modal('hide');
             toastr.success('Opération réussite', 'Succès', { progressBar: true, showDuration: 100 });
             $('#PersonsGrid').DataTable().ajax.reload();


         },
     })
     
 }
 function LoadPersonData() {
     debugger;
     var table = $('#PersonsGrid').DataTable({
         "order": [[0, "desc"]],
         "autoWidth": true,
         ajax: {
             url: "/RH/GetAllPersonnes",
             dataSrc: ''
         },
         columns: [{ "data": "ID" },
          {
              "data": "",
              "render": function (data, type, row) {
                  debugger
                  var ID = row.ID;
                  return "<div style='display:inline-flex'><button onclick=OpenPersonEditor('" + ID + "') style='width:30px;height:30px;' class='btn btn-info btn-icon-split'><span class='icon text-white-30'><i class='fas fa-edit'></i></span><button style='width:30px;height:30px;margin-left:10px;'onclick=DeletePerson('" + ID + "')  class='btn btn-danger btn-icon-split'><span class='icon text-white-30'><i class='fas fa-trash'></i></span></button></div>"
              }
          },
                    
         { "data": "Nom" },
         { "data": "Prenom" },
         { "data": "Fonction" },
         { "data": "Tel1" },
         { "data": "Tel2" },
         { "data": "SoldeCongeAnnuel" },
         { "data": "SoldeCongeRestant" },
         { "data": "AdresseEmail" },
         {
             "data": "DateDebutContrat",
             render: function (data, type, row) {
                 if (type === "sort" || type === "type") {
                     return data;
                 }
                 return moment(data).format("DD-MM-YYYY");
             }
         },
         {
             "data": "DateFinContrat",
             render: function (data, type, row) {
                 if (type === "sort" || type === "type") {
                     return data;
                 }
                 return moment(data).format("YYYY-MM-DD HH:mm");
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

//Congés
 function DeleteConges(ID) {
     debugger;
     if (confirm('Vous confirmez la suppression de cette Conges??')) {
         var formData = $("#ParamForm").serializeArray();
         $.ajax({

             url: "/RH/DeleteConges",
             data: { ID: ID },

             success: function (response) {
                 debugger;
                 toastr.success('Supprimé avec succès', 'Succès', { progressBar: true, showDuration: 100 });
                 $('#CongesGrid').DataTable().ajax.reload();

             },
         });
     }
 }
 function OpenCongesEditor(ID) {
     debugger;
     $.ajax({
         type: "GET",
         url: "/RH/OpenCongesEditor",
         data: { ID: ID },
         success: function (data) {
             debugger;

             $('#GenericModel').modal();
             $("#ModalTitle").text("Congé");
             $("#ModalBody").html(data);

         },
         failure: function (response) { }
     });
 }
 function AddOrUpdateConges() {
     debugger;
     var formData = $("#ParamForm").serializeArray();
     if (formData[1].value == "" || formData[1].value == null || formData[1].value == undefined) {
         toastr.error('Veuillez renseigner le champs Libellé', 'error', { progressBar: true, showDuration: 100 });
         return;
     }
     if (formData[2].value == "" || formData[2].value == null || formData[2].value == undefined || formData[2].value == "0") {
         toastr.error('Veuillez renseigner le champs Personne', 'error', { progressBar: true, showDuration: 100 });
         return;
     }
     if (formData[3].value == "" || formData[3].value == null || formData[3].value == undefined || formData[3].value == "0") {
         toastr.error('Veuillez renseigner le champs Motif', 'error', { progressBar: true, showDuration: 100 });
         return;
     }
     if (formData[4].value == "" || formData[4].value == null || formData[4].value == undefined) {
         toastr.error('Veuillez renseigner le champs Date debut', 'error', { progressBar: true, showDuration: 100 });
         return;
     }
     if (formData[5].value == "" || formData[5].value == null || formData[5].value == undefined) {
         toastr.error('Veuillez renseigner le champs Date fin', 'error', { progressBar: true, showDuration: 100 });
         return;
     }
         $.ajax({
         type: 'POST',
         url: "/RH/AddOrUpdateConges",
         data: formData,
         encode: true,
         success: function (response) {
             debugger;
             //var res = JSON.parse(response);

             $('#GenericModel').modal('hide');
             toastr.success('Opération réussite', 'Succès', { progressBar: true, showDuration: 100 });
             $('#CongesGrid').DataTable().ajax.reload();


         },
     })
 }
 function LoadCongesData() {
     debugger;
     var table = $('#CongesGrid').DataTable({
         "order": [[0, "desc"]],
         "autoWidth": true,
         ajax: {
             url: "/RH/GetAllConges",
             dataSrc: ''
         },
         columns: [{ "data": "ID" },
          {
              "data": "",
              "render": function (data, type, row) {
                  debugger
                  var ID = row.ID;
                  return "<div style='display:inline-flex'><button onclick=OpenCongesEditor('" + ID + "') style='width:30px;height:30px;' class='d-none d-sm-inline-block btn btn-sm btn-info shadow-sm'><span class='icon text-white-30'><i class='fas fa-edit'></i></span></button><button style='width:30px;height:30px;margin-left:10px;'onclick=DeleteConges('" + ID + "')  class='d-none d-sm-inline-block btn btn-sm btn-danger shadow-sm'><span class='icon text-white-30'><i class='fas fa-trash'></i></span></button></div>"
              }
          },

         { "data": "PersonName" },
         { "data": "Libelle" },
 
         { "data": "MotifName" },
         { "data": "DateFrom" ,
         render: function (data, type, row) {
             if (type === "sort" || type === "type") {
                 return data;
             }
             return moment(data).format("YYYY-MM-DD");
         }
         },
         { "data": "DateTo" ,
         render: function (data, type, row) {
             if (type === "sort" || type === "type") {
                 return data;
             }
             return moment(data).format("YYYY-MM-DD");
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




