function LoadNotificationsListData() {
    var dateFrom = $("#DateFromFilter").val();
    var table = $('#NotificationsListGrid').DataTable({
        "order": [[2, "desc"]],
        "autoWidth": true,
        
        ajax: {
            url: "/Common/GetAllNotifications",
            data: { dateFrom: dateFrom },
            dataSrc: ''
        },
        columns: [


        { "data": "ID" },
      
        {
            "data": "title"
        },
        { "data": "description" },
               {
            "data": "Date",
            render: function (data, type, row) {
                if (type === "sort" || type === "type") {
                    return data;
                }
                return moment(data).format("DD-MM-YYYY");
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
function RefreshNotificationsGrid()
{
    $('#NotificationsListGrid').DataTable().destroy();
    LoadNotificationsListData();
}