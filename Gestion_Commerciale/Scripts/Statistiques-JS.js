var VenteByClient;
var ReglementsStatByClient;
function RefreshClientsStats()
{

    var ctx = document.getElementById("VenteByClientBarChart").getContext("2d");
    if (window.VenteByClient != undefined) {
        window.VenteByClient.destroy();
    }
    window.VenteByClient = new Chart(ctx, {})
    GetClientStatValueBarChart();


    var ctx2 = document.getElementById("ReglementsByClient").getContext("2d");
    if (window.ReglementsStatByClient != undefined) {
        window.ReglementsStatByClient.destroy();
    }
    window.ReglementsStatByClient = new Chart(ctx, {})
    GetClientStatValueDonutChart();
}
function GetClientStatValueDonutChart() {
    var ClientFilter = $("#ClientSelector").val();
    $.ajax({

        url: "/Statistiques/GetReglementsStatByClient",
        data: { ClientFilter: ClientFilter },


        success: function (ResultData) {
            debugger;
            var array = [];
            for (var i = 0; i < ResultData.length ; i++) {
                array[i] = ResultData[i];
            }
            var ctx = document.getElementById("ReglementsByClient");
              ReglementsStatByClient = new Chart(ctx, {
  type: 'doughnut',
  data: {
      labels: ["Non payés", "Payés"],
    datasets: [{
        data: array,
      backgroundColor: ['Red', '#1cc88a'],
      hoverBackgroundColor: ['#bb2222', '#17a673'],
      hoverBorderColor: "rgba(234, 236, 244, 1)",
    }],
  },
  options: {
    maintainAspectRatio: false,
    tooltips: {
      backgroundColor: "rgb(255,255,255)",
      bodyFontColor: "#858796",
      borderColor: '#dddfeb',
      borderWidth: 1,
      xPadding: 15,
      yPadding: 15,
      displayColors: false,
      caretPadding: 10,
    },
    legend: {
      display: false
    },
    cutoutPercentage: 80,
  },
});
        },
    });
}
function PrintReleveCompte() {
    debugger;
    var DateFromFilter = $("#DateFromFilter").val();
    var DateToFilter = $("#DateToFilter").val();
    var ClientFilter = $("#ClientFilter").val();
    $.ajax({
        url: "/Print/ImprimerReleveCompte",
        beforeSend: function () {
            $.blockUI({ message: 'Patientez un peu...' });


        },
        complete: function () {
            $.unblockUI();
        },
        data: { DateFromFilter: DateFromFilter, DateToFilter: DateToFilter, ClientFilter: ClientFilter},

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
                OPEN_URL_IN_BLANK(Value, true);
            }
        }
    });

}
function GetClientStatValueBarChart()
{
    var ClientFilter = $("#ClientSelector").val();
    var AnneeSelector = $("#AnneeSelector").val();
    
    $.ajax({

        url: "/Statistiques/GetPaymentStatByClient",
        data: { ClientFilter: ClientFilter, Annee: AnneeSelector },


        success: function (ResultData) {
            debugger;
            var newfirstChar = 0;
            var maxValue = 0;
            for (var i = 0; i < 12; i++)
            {
                if (parseInt(ResultData[i]) > maxValue)
                {
                    maxValue = parseInt(ResultData[i]);
                }
                
            }
            var lenghtMax = String(maxValue).length;
            var firstChar = String(maxValue)[0];
            if (firstChar < 9) {
                newfirstChar = parseInt(firstChar) + 1;
                for (var j = 0; j < lenghtMax - 1; j++) {
                    newfirstChar = newfirstChar * 10;
                }

            }
            else {
                newfirstChar = 1;
                for (var j = 0; j < lenghtMax ; j++) {
                    newfirstChar = newfirstChar * 10;
                }
            }
            var ctx = document.getElementById("VenteByClientBarChart");
             VenteByClient = new Chart(ctx, {
                type: 'bar',
                data: {
                    labels: ["Janvier", "Février", "Mars", "Avril", "Mai", "Juin", "Juillet", "Aout", "September", "Octobre", "Novembre", "Décembre"],
                    datasets: [{
                        label: "Revenue",
                        backgroundColor: "#4e73df",
                        hoverBackgroundColor: "#2e59d9",
                        borderColor: "#4e73df",
                        data: ResultData,
                    }],
                },
                options: {
                    maintainAspectRatio: false,
                    layout: {
                        padding: {
                            left: 10,
                            right: 25,
                            top: 25,
                            bottom: 0
                        }
                    },
                    scales: {
                        xAxes: [{
                            time: {
                                unit: 'month'
                            },
                            gridLines: {
                                display: false,
                                drawBorder: false
                            },
                            ticks: {
                                maxTicksLimit: 12
                            },
                            maxBarThickness: 25,
                        }],
                        yAxes: [{
                            ticks: {
                                min: 0,
                                max: newfirstChar,
                                maxTicksLimit: 5,
                                padding: 10,
                                // Include a dollar sign in the ticks
                                callback: function (value, index, values) {
                                    return  value+' DT';
                                }
                            },
                            gridLines: {
                                color: "rgb(234, 236, 244)",
                                zeroLineColor: "rgb(234, 236, 244)",
                                drawBorder: false,
                                borderDash: [2],
                                zeroLineBorderDash: [2]
                            }
                        }],
                    },
                    legend: {
                        display: false
                    },
                    tooltips: {
                        titleMarginBottom: 10,
                        titleFontColor: '#6e707e',
                        titleFontSize: 14,
                        backgroundColor: "rgb(255,255,255)",
                        bodyFontColor: "#858796",
                        borderColor: '#dddfeb',
                        borderWidth: 1,
                        xPadding: 15,
                        yPadding: 15,
                        displayColors: false,
                        caretPadding: 10,
                        callbacks: {
                            label: function (tooltipItem, chart) {
                                var datasetLabel = chart.datasets[tooltipItem.datasetIndex].label || '';
                                return datasetLabel + ': ' + tooltipItem.yLabel+' DT';
                            }
                        }
                    },
                }
            });
        },
    });
}




var myBarChart;
var myLineChart;
function RefreshChiffreAffaireStats()
{
   
    var ctx = document.getElementById("CharChiffreAffaireMois").getContext("2d");
    if (window.myBarChart != undefined)
    {
        window.myBarChart.destroy();
    }
    window.myBarChart = new Chart(ctx, {})

    var ctx2 = document.getElementById("ChiffreAffaireChart").getContext("2d");
    if (window.myLineChart != undefined) {
        window.myLineChart.destroy();
    }
    window.myLineChart = new Chart(ctx2, {})
    GetChiffreAffaireStats();
    GetChiffreAffaireMoisStats();
}
function GetChiffreAffaireStats() {
    debugger;
    var AnneeSelector = $("#AnneeSelector").val();
    $.ajax({

        url: "/Statistiques/GetChiffreAffaireStats",
        data: {Annee: AnneeSelector },


        success: function (ResultData) {
            debugger;
            var newfirstChar = 0;
            var maxValue = 0;
            for (var i = 0; i < 12; i++) {
                if (parseInt(ResultData[i]) > maxValue) {
                    maxValue = parseInt(ResultData[i]);
                }

            }
            var lenghtMax = String(maxValue).length;
            var firstChar = String(maxValue)[0];
            if (firstChar < 9) {
                newfirstChar = parseInt(firstChar) + 1;
                for (var j = 0; j < lenghtMax - 1; j++) {
                    newfirstChar = newfirstChar * 10;
                }

            }
            else {
                newfirstChar = 1;
                for (var j = 0; j < lenghtMax ; j++) {
                    newfirstChar = newfirstChar * 10;
                }
            }
            var ctx = document.getElementById("ChiffreAffaireChart");

            myLineChart = new Chart(ctx, {
                type: 'line',
                data: {
                    labels: ["Janvier", "Février", "Mars", "Avril", "Mai", "Juin", "Juillet", "Aout", "September", "Octobre", "Novembre", "Décembre"],
                    datasets: [{
                        label: "Earnings",
                        lineTension: 0.3,
                        backgroundColor: "rgba(78, 115, 223, 0.05)",
                        borderColor: "rgba(78, 115, 223, 1)",
                        pointRadius: 3,
                        pointBackgroundColor: "rgba(78, 115, 223, 1)",
                        pointBorderColor: "rgba(78, 115, 223, 1)",
                        pointHoverRadius: 3,
                        pointHoverBackgroundColor: "rgba(78, 115, 223, 1)",
                        pointHoverBorderColor: "rgba(78, 115, 223, 1)",
                        pointHitRadius: 10,
                        pointBorderWidth: 2,
                        data: ResultData,
                    }],
                },
                options: {
                    maintainAspectRatio: false,
                    layout: {
                        padding: {
                            left: 10,
                            right: 25,
                            top: 25,
                            bottom: 0
                        }
                    },
                    scales: {
                        xAxes: [{
                            time: {
                                unit: 'date'
                            },
                            gridLines: {
                                display: false,
                                drawBorder: false
                            },
                            ticks: {
                                maxTicksLimit: 12
                            }
                        }],
                        yAxes: [{
                            ticks: {
                                maxTicksLimit: 5,
                                padding: 10,
                                // Include a dollar sign in the ticks
                                callback: function (value, index, values) {
                                    return value+ 'DT';
                                }
                            },
                            gridLines: {
                                color: "rgb(234, 236, 244)",
                                zeroLineColor: "rgb(234, 236, 244)",
                                drawBorder: false,
                                borderDash: [2],
                                zeroLineBorderDash: [2]
                            }
                        }],
                    },
                    legend: {
                        display: false
                    },
                    tooltips: {
                        backgroundColor: "rgb(255,255,255)",
                        bodyFontColor: "#858796",
                        titleMarginBottom: 10,
                        titleFontColor: '#6e707e',
                        titleFontSize: 14,
                        borderColor: '#dddfeb',
                        borderWidth: 1,
                        xPadding: 15,
                        yPadding: 15,
                        displayColors: false,
                        intersect: false,
                        mode: 'index',
                        caretPadding: 10,
                        callbacks: {
                            label: function (tooltipItem, chart) {
                                var datasetLabel = chart.datasets[tooltipItem.datasetIndex].label || '';
                                return datasetLabel + ': ' +tooltipItem.yLabel+'DT';
                            }
                        }
                    }
                }
            });
        },
    });
}
function GetChiffreAffaireMoisStats() {
    var AnneeSelector = $("#AnneeSelector").val();

    $.ajax({

        url: "/Statistiques/GetChiffreAffaireMoisStats",
        data: { Annee: AnneeSelector },


        success: function (ResultData) {
            debugger;
            var newfirstChar = 0;
            var maxValue = 0;
            for (var i = 0; i < 12; i++) {
                if (parseInt(ResultData[i]) > maxValue) {
                    maxValue = parseInt(ResultData[i]);
                }

            }
            var lenghtMax = String(maxValue).length;
            var firstChar = String(maxValue)[0];
            if (firstChar < 9) {
                newfirstChar = parseInt(firstChar) + 1;
                for (var j = 0; j < lenghtMax - 1; j++) {
                    newfirstChar = newfirstChar * 10;
                }

            }
            else {
                newfirstChar = 1;
                for (var j = 0; j < lenghtMax ; j++) {
                    newfirstChar = newfirstChar * 10;
                }
            }
            var ctx = document.getElementById("CharChiffreAffaireMois");
            myBarChart = new Chart(ctx, {
                type: 'bar',
                data: {
                    labels: ["Janvier", "Février", "Mars", "Avril", "Mai", "Juin", "Juillet", "Aout", "September", "Octobre", "Novembre", "Décembre"],
                    datasets: [{
                        label: "Revenue",
                        backgroundColor: "#4e73df",
                        hoverBackgroundColor: "#2e59d9",
                        borderColor: "#4e73df",
                        data: ResultData,
                    }],
                },
                options: {
                    maintainAspectRatio: false,
                    layout: {
                        padding: {
                            left: 10,
                            right: 25,
                            top: 25,
                            bottom: 0
                        }
                    },
                    scales: {
                        xAxes: [{
                            time: {
                                unit: 'month'
                            },
                            gridLines: {
                                display: false,
                                drawBorder: false
                            },
                            ticks: {
                                maxTicksLimit: 12
                            },
                            maxBarThickness: 25,
                        }],
                        yAxes: [{
                            ticks: {
                                min: 0,
                                max: newfirstChar,
                                maxTicksLimit: 5,
                                padding: 10,
                                // Include a dollar sign in the ticks
                                callback: function (value, index, values) {
                                    return value + ' DT';
                                }
                            },
                            gridLines: {
                                color: "rgb(234, 236, 244)",
                                zeroLineColor: "rgb(234, 236, 244)",
                                drawBorder: false,
                                borderDash: [2],
                                zeroLineBorderDash: [2]
                            }
                        }],
                    },
                    legend: {
                        display: false
                    },
                    tooltips: {
                        titleMarginBottom: 10,
                        titleFontColor: '#6e707e',
                        titleFontSize: 14,
                        backgroundColor: "rgb(255,255,255)",
                        bodyFontColor: "#858796",
                        borderColor: '#dddfeb',
                        borderWidth: 1,
                        xPadding: 15,
                        yPadding: 15,
                        displayColors: false,
                        caretPadding: 10,
                        callbacks: {
                            label: function (tooltipItem, chart) {
                                var datasetLabel = chart.datasets[tooltipItem.datasetIndex].label || '';
                                return datasetLabel + ': ' + tooltipItem.yLabel + ' DT';
                            }
                        }
                    },
                }
            });
        },
    });
}




var myStockBarChart;
var myStockDonutChar;
function RefreshStocksStats()
{


    var ctx = document.getElementById("ChartMostSoldProducteMois").getContext("2d");
    if (window.myStockBarChart != undefined) {
        window.myStockBarChart.destroy();
    }
    window.myStockBarChart = new Chart(ctx, {})


    GetMostSoldStats();
    var ctx2 = document.getElementById("QteDisponiblesChart").getContext("2d");
    if (window.myStockDonutChar != undefined) {
        window.myStockDonutChar.destroy();
    }
    window.myStockDonutChar = new Chart(ctx, {})
    GetQteDisponibleStats();
}
function GetMostSoldStats() {
    debugger;
    var AnneeSelector = $("#AnneeSelector").val();
    var MoisSelector = $("#MoisSelector").val();
    $.ajax({

        url: "/Statistiques/GetMostSoldStats",
        data: { Annee: AnneeSelector, MoisSelector: MoisSelector },


        success: function (ResultData) {
            var ResultText = [];
            var ResultValue = [];
            for (var i = 0; i < ResultData.length; i++) {
                ResultText[i] = ResultData[i].Text;
                ResultValue[i] = ResultData[i].Val1;
                };
            debugger;
            var newfirstChar = 0;
            var maxValue = 0;
            for (var i = 0; i < 12; i++) {
                if (parseInt(ResultValue[i]) > maxValue) {
                    maxValue = parseInt(ResultValue[i]);
                }

            }
            var lenghtMax = String(maxValue).length;
            var firstChar = String(maxValue)[0];
            if (firstChar < 9) {
                newfirstChar = parseInt(firstChar) + 1;
                for (var j = 0; j < lenghtMax - 1; j++) {
                    newfirstChar = newfirstChar * 10;
                }

            }
            else {
                newfirstChar = 1;
                for (var j = 0; j < lenghtMax ; j++) {
                    newfirstChar = newfirstChar * 10;
                }
            }
            var ctx = document.getElementById("ChartMostSoldProducteMois");
            myStockBarChart = new Chart(ctx, {
                type: 'bar',
                data: {
                    labels: ResultText,
                    datasets: [{
                        label: "Quantité",
                        backgroundColor: "#4e73df",
                        hoverBackgroundColor: "#2e59d9",
                        borderColor: "#4e73df",
                        data: ResultValue,
                    }],
                },
                options: {
                    maintainAspectRatio: false,
                    layout: {
                        padding: {
                            left: 10,
                            right: 25,
                            top: 25,
                            bottom: 0
                        }
                    },
                    scales: {
                        xAxes: [{
                            time: {
                                unit: 'month'
                            },
                            gridLines: {
                                display: false,
                                drawBorder: false
                            },
                            ticks: {
                                maxTicksLimit: 12
                            },
                            maxBarThickness: 25,
                        }],
                        yAxes: [{
                            ticks: {
                                min: 0,
                                max: newfirstChar,
                                maxTicksLimit: 5,
                                padding: 10,
                                // Include a dollar sign in the ticks
                                callback: function (value, index, values) {
                                    return value ;
                                }
                            },
                            gridLines: {
                                color: "rgb(234, 236, 244)",
                                zeroLineColor: "rgb(234, 236, 244)",
                                drawBorder: false,
                                borderDash: [2],
                                zeroLineBorderDash: [2]
                            }
                        }],
                    },
                    legend: {
                        display: false
                    },
                    tooltips: {
                        titleMarginBottom: 10,
                        titleFontColor: '#6e707e',
                        titleFontSize: 14,
                        backgroundColor: "rgb(255,255,255)",
                        bodyFontColor: "#858796",
                        borderColor: '#dddfeb',
                        borderWidth: 1,
                        xPadding: 15,
                        yPadding: 15,
                        displayColors: false,
                        caretPadding: 10,
                        callbacks: {
                            label: function (tooltipItem, chart) {
                                var datasetLabel = chart.datasets[tooltipItem.datasetIndex].label || '';
                                return datasetLabel + ': ' + tooltipItem.yLabel ;
                            }
                        }
                    },
                }
            });
        },
    });
}

function GetQteDisponibleStats() {

    $.ajax({

        url: "/Statistiques/GetQteDisponibleStats",
        


        success: function (ResultData) {
            var ResultText = [];
            var ResultValue = [];
            var ResultColor = [];
            var ResultHover = [];
            for (var i = 0; i < ResultData.length; i++) {
                ResultText[i] = ResultData[i].Text;
                ResultValue[i] = ResultData[i].Val1;
                ResultColor[i] = ResultData[i].Value;
                /*ResultHover[i] = ResultData[i].Value2;*/
            };
            debugger;
            var newfirstChar = 0;
            var maxValue = 0;
            for (var i = 0; i < 12; i++) {
                if (parseInt(ResultData[i]) > maxValue) {
                    maxValue = parseInt(ResultData[i]);
                }

            }
            var lenghtMax = String(maxValue).length;
            var firstChar = String(maxValue)[0];
            if (firstChar < 9) {
                newfirstChar = parseInt(firstChar) + 1;
                for (var j = 0; j < lenghtMax - 1; j++) {
                    newfirstChar = newfirstChar * 10;
                }

            }
            else {
                newfirstChar = 1;
                for (var j = 0; j < lenghtMax ; j++) {
                    newfirstChar = newfirstChar * 10;
                }
            }
            var ctx = document.getElementById("QteDisponiblesChart");
             myStockDonutChar = new Chart(ctx, {
                type: 'doughnut',
                data: {
                    labels: ResultText,
                    datasets: [{
                        data: ResultValue,
                        backgroundColor: ["#ea9e70", "#a48a9e", "#c6e1e8", "#648177", "#0d5ac1", "#f205e6", "#4ca2f9", "#a4e43f", "#d298e2", "#6119d0", "#d2737d", "#c0a43c", "#f2510e", "#651be6", "#79806e", "#61da5e", "#cd2f00", "#9348af", "#01ac53", "#1c0365", "#14a9ad", "#c5a4fb", "#63b598", "#ce7d78", "#996635", "#b11573", "#4bb473", "#75d89e", "#2f3f94", "#2f7b99", "#da967d", "#34891f", "#b0d87b", "#ca4751", "#7e50a8", "#c4d647", "#e0eeb8", "#11dec1", "#289812", "#566ca0", "#ffdbe1", "#2f1179", "#935b6d", "#916988", "#513d98", "#aead3a", "#9e6d71", "#4b5bdc", "#0cd36d", "#250662", "#cb5bea", "#228916", "#ac3e1b", "#df514a", "#539397", "#880977", "#f697c1", "#ba96ce", "#679c9d", "#c6c42c", "#5d2c52", "#48b41b", "#e1cf3b", "#5be4f0", "#57c4d8", "#a4d17a", "#225b8", "#be608b", "#96b00c", "#088baf", "#f158bf", "#e145ba", "#ee91e3", "#05d371", "#5426e0", "#4834d0", "#802234", "#6749e8", "#0971f0", "#8fb413", "#b2b4f0", "#c3c89d", "#c9a941", "#41d158", "#fb21a3", "#51aed9", "#5bb32d", "#807fb", "#21538e", "#89d534", "#d36647", "#7fb411", "#0023b8", "#3b8c2a", "#986b53", "#f50422", "#983f7a", "#ea24a3", "#79352c", "#521250", "#c79ed2", "#d6dd92", "#e33e52", "#b2be57", "#fa06ec", "#1bb699", "#6b2e5f", "#64820f", "#1c271", "#21538e", "#89d534", "#d36647", "#7fb411", "#0023b8", "#3b8c2a", "#986b53", "#f50422", "#983f7a", "#ea24a3", "#79352c", "#521250", "#c79ed2", "#d6dd92", "#e33e52", "#b2be57", "#fa06ec", "#1bb699", "#6b2e5f", "#64820f", "#1c271", "#9cb64a", "#996c48", "#9ab9b7", "#06e052", "#e3a481", "#0eb621", "#fc458e", "#b2db15", "#aa226d", "#792ed8", "#73872a", "#520d3a", "#cefcb8", "#a5b3d9", "#7d1d85", "#c4fd57", "#f1ae16", "#8fe22a", "#ef6e3c", "#243eeb", "#1dc18", "#dd93fd", "#3f8473", "#e7dbce", "#421f79", "#7a3d93", "#635f6d", "#93f2d7", "#9b5c2a", "#15b9ee", "#0f5997", "#409188", "#911e20", "#1350ce", "#10e5b1", "#fff4d7", "#cb2582", "#ce00be", "#32d5d6", "#17232", "#608572", "#c79bc2", "#00f87c", "#77772a", "#6995ba", "#fc6b57", "#f07815", "#8fd883", "#060e27", "#96e591", "#21d52e", "#d00043", "#b47162", "#1ec227", "#4f0f6f", "#1d1d58", "#947002", "#bde052", "#e08c56", "#28fcfd", "#bb09b", "#36486a", "#d02e29", "#1ae6db", "#3e464c", "#a84a8f", "#911e7e", "#3f16d9", "#0f525f", "#ac7c0a", "#b4c086", "#c9d730", "#30cc49", "#3d6751", "#fb4c03", "#640fc1", "#62c03e", "#d3493a", "#88aa0b", "#406df9", "#615af0", "#4be47", "#2a3434", "#4a543f", "#79bca0", "#a8b8d4", "#00efd4", "#7ad236", "#7260d8", "#1deaa7", "#06f43a", "#823c59", "#e3d94c", "#dc1c06", "#f53b2a", "#b46238", "#2dfff6", "#a82b89", "#1a8011", "#436a9f", "#1a806a", "#4cf09d", "#c188a2", "#67eb4b", "#b308d3", "#fc7e41", "#af3101", "#ff065", "#71b1f4", "#a2f8a5", "#e23dd0", "#d3486d", "#00f7f9", "#474893", "#3cec35", "#1c65cb", "#5d1d0c", "#2d7d2a", "#ff3420", "#5cdd87", "#a259a4", "#e4ac44", "#1bede6", "#8798a4", "#d7790f", "#b2c24f", "#de73c2", "#d70a9c", "#25b67", "#88e9b8", "#c2b0e2", "#86e98f", "#ae90e2", "#1a806b", "#436a9e", "#0ec0ff", "#f812b3", "#b17fc9", "#8d6c2f", "#d3277a", "#2ca1ae", "#9685eb", "#8a96c6", "#dba2e6", "#76fc1b", "#608fa4", "#20f6ba", "#07d7f6", "#dce77a", "#77ecca"],
                      /*  hoverBackgroundColor: ResultHover,*/
                        hoverBorderColor: "rgba(234, 236, 244, 1)",
                    }],
                },
                options: {
                    maintainAspectRatio: false,
                    tooltips: {
                        backgroundColor: "rgb(255,255,255)",
                        bodyFontColor: "#858796",
                        borderColor: '#dddfeb',
                        borderWidth: 1,
                        xPadding: 15,
                        yPadding: 15,
                        displayColors: false,
                        caretPadding: 10,
                    },
                    legend: {
                        display: false
                    },
                    cutoutPercentage: 80,
                },
            });
        },
    });
}
function RefreshEtatClientsGrid()
{
    $('#EtatClientsGrid').DataTable().destroy();
    LoadEtatClientsData();
}
function LoadEtatClientsData() {
    debugger;
    var DateFromFilter = $("#DateFromFilter").val();
    var DateToFilter = $("#DateToFilter").val();
    var table = $('#EtatClientsGrid').DataTable({
        "order": [[0, "desc"]],
        "autoWidth": true,
        ajax: {
            url: "/Statistiques/GetEtatClients",
            data: { DateFromFilter: DateFromFilter, DateToFilter: DateToFilter },
            dataSrc: ''
        },
        columns: [

                  { "data": "ID" },
                  { "data": "OwnerName" },
                  { "data": "H_T" },
                  { "data": "TTC" },
                  { "data": "Reglee" },
                  { "data": "Solde" }




        ]

    });

}

function RefreshReleveDeCompteGrid() {
    $('#ReleveDeCompteGrid').DataTable().destroy();
    LoadReleveDeCompteData();
}
function LoadReleveDeCompteData() {
    debugger;
    var DateFromFilter = $("#DateFromFilter").val();
    var DateToFilter = $("#DateToFilter").val();
    var ClientFilter = $("#ClientFilter").val();
    var table = $('#ReleveDeCompteGrid').DataTable({
        //"order": [[0, "desc"]],
        "autoWidth": true,
        ajax: {
            url: "/Statistiques/ReleveDeComptes",
            data: { DateFromFilter: DateFromFilter, DateToFilter: DateToFilter, ClientFilter: ClientFilter },
            dataSrc: ''
        },
        columns: [

            {
                "data": "Date",
                render: function (data, type, row) {
                    if (type === "sort" || type === "type") {
                        return data;
                    }
                    return moment(data).format("DD/MM/YYYY");
                }
            },
            { "data": "REFERENCE" },
            { "data": "DEBIT" },
            { "data": "CREDIT" },
            { "data": "SOLDE" }
        ]

    });

}
function LoadEngFournisseurData() {
    debugger;
    var DateFromFilter = $("#DateFromFilter").val();
    var DateToFilter = $("#DateToFilter").val();
    var table = $('#EngFournisseurGrid').DataTable({
        "order": [[0, "desc"]],
        "autoWidth": true,
        ajax: {
            url: "/Statistiques/GetEngagementsFournisseurs",
            data: { DateFromFilter: DateFromFilter, DateToFilter: DateToFilter },
            dataSrc: ''
        },
        columns: [

                  { "data": "ID" },
                  { "data": "OwnerName" },
                  { "data": "H_T" },
                  { "data": "TTC" },
                  { "data": "Reglee" },
                  { "data": "Solde" }




        ]

    });

}

function LoadFournitureData()
{
    debugger;
    var DateFilter = $("#DateFilter").val();
    var table = $('#DashbordFournitureGrid').DataTable({
        searching: false, paging: false, info: false, 
        "order": false,
        "autoWidth": true,
        ajax: {
            url: "/Statistiques/LoadFournitureData",
            data: { DateFilter: DateFilter },
            dataSrc: ''
        },
        columns: [

                  { "data": "OwnerName" },
                  { "data": "Montant" }
        ],

    });

}
function LoadMarchandisesData()
{
    debugger;
    var DateFilter = $("#DateFilter").val();
    var table = $('#DashbordMarchandiseGrid').DataTable({
        searching: false, paging: false, info: false, 
        "order": false,
        "autoWidth": true,
        ajax: {
            url: "/Statistiques/LoadMarchandisesData",
            data: { DateFilter: DateFilter },
            dataSrc: ''
        },
        columns: [

                  { "data": "OwnerName" },
                  { "data": "Montant" }
        ],
    });

}
function LoadBeneficesData()
{
    debugger;
    var DateFilter = $("#DateFilter").val();
    var table = $('#DashbordBeneficesGrid').DataTable({
        searching: false, paging: false, info: false, sortable: false,
        "order": false,
        "autoWidth": true,
        ajax: {
            url: "/Statistiques/LoadBeneficesData",
            data: { DateFilter: DateFilter },
            dataSrc: ''
        },
        columns: [

                  { "data": "OwnerName" },
                  { "data": "Montant" },
            
        ],
    });

}

function RefreshDashbordData()
{
    $('#DashbordFournitureGrid').DataTable().destroy();
    LoadFournitureData();
    $('#DashbordMarchandiseGrid').DataTable().destroy();
    LoadMarchandisesData();
    $('#DashbordBeneficesGrid').DataTable().destroy();
    LoadBeneficesData();
}