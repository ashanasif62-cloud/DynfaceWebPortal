using GeneralAuxiliary;
using PortalIntegration;
using PortalIntegration.HcmWorkerDetailsSvcReference;
using System;
using System.Web.UI;

namespace DynamicsPortal
{
    public partial class Dashboard : MainForm
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
            //System.Diagnostics.Stopwatch sw1 = System.Diagnostics.Stopwatch.StartNew();
            //sw1.Stop();
            showActionPanel = false;
            pageMenuId = "ESSPRHRDefault";
            base.Page_Load(sender, e);

            if (!isUserAuthenticated)
                return;

            if (!IsPostBack)
            {
                setUserDetails();

                chartsAdvances();
                chartsLeaves();
                chartsTax("'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec', 'Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun'", "5000, 4800, 5500, 5000, 2000, 7000, 5000, 5500, 4800, 5000, 4950, 3000");            //
                chartsPayrollTotals("'Basic Salary', 'Allowances', 'Tax', 'Other Deductions', 'PF', 'Overtime'", "25000, 5000, 1500, 500, 1500, 7000");  //
            }
        }

        private void setUserDetails()
        {
            string employeeId = SessionVariables.getCurrentEmployeeId();
            string employeeName = SessionVariables.getCurrentEmployeeName();
            string userLoginTime = String.Format("{0:dd/MM/yyyy HH:mm:ss}", SessionVariables.getCurrentUserLoginTime());

            lblUserFullName.InnerText = employeeName;
            lblUserLoginTime.InnerText = userLoginTime;//"24/09/2018 16:38:48";

            //string userImageData = ControlsHelper.getCurrentUserImage();

            //if (string.IsNullOrEmpty(userImageData))
            //    imgUser.Src = "/distribution/img/User.png";
            //else
            //    imgUser.Src = "data:image/png;base64," + userImageData;


            HcmWorkerDetailsSvcContract currentUserDetails = ControlsHelper.getWorkerDetails(employeeId);
            if (currentUserDetails == null)
                return;

            string userDepartment = currentUserDetails.DepartmentName;
            string userJob = currentUserDetails.Job;
            string userEmailId = currentUserDetails.workerEmail;
            string userPhoneNo = currentUserDetails.Phone;
            string userFullAddress = currentUserDetails.Address;

            lblUserDepartment.InnerText = userDepartment;
            lblUserJob.InnerText = userJob;
            lblUserEmailId.InnerText = userEmailId;
            lblUserPhoneNo.InnerText = userPhoneNo;
            lblUserFullAddress.InnerText = userFullAddress;
        }

        private void chartsAdvances()
        {       //, 
            string employeeId = SessionVariables.getCurrentEmployeeId();
            //ESSDashboardDetails eSSDashboardDetails = new ESSDashboardDetails();
            //string[] reportData = eSSDashboardDetails.retrieveAdvancesDetails(employeeId);
            string chartLabels = "'Recovery', 'Outstanding'";   //reportData[0];
            string chartData = "8000, 12000";   //reportData[1];

            string script = @"$(document).ready(
                            function () {
                            'use strict';
                            var chartLabels = [" + chartLabels + @"];
                            var chartData = [" + chartData + @"];
                            var PIECHART = $('#chartAdvances');
                            var myPieChart = new Chart(PIECHART, {
                                type: 'pie',//doughnut
                                options: {
                                    legend: { 
                                                display: true,
                                                position: 'bottom',
                                                fullWidth: true,
                                                labels: {
                                                    fontSize: 9,
                                                    padding:  5
                                                        }
                                            }
                                },
                                data: {
                                    labels: chartLabels,
                                    datasets: [
                                        {
                                            data: chartData,
                                            borderWidth: [1, 1],
                                            backgroundColor: [
                                                '#4bc0c0',
                                                '#FFCE56'
                                            ],
                                            hoverBackgroundColor: [
                                                '#4bc0c0',
                                                '#FFCE56'
                                            ]
                                        }
                                    ]
                                }
                            });
                        });
";
            //document.getElementById('chartAdvances').getContext('2d');
            //document.getElementById('addData').addEventListener('click', function () {
            //    chartEmployeeAdvances.config.data.datasets.push({
            //        label: 'Dataset',
            //        data: [10, 2, 30, 44],
            //        borderColor: 'rgb(255, 99, 132)',
            //        fill: false
            //    });
            //    chartEmployeeAdvances.update();
            //});

            Page currentPage = System.Web.HttpContext.Current.CurrentHandler as Page;
            currentPage.ClientScript.RegisterStartupScript(currentPage.GetType(), "jsChartAdvances", script, true);
        }

        private void chartsLeaves()
        {
            string employeeId = SessionVariables.getCurrentEmployeeId();
            ESSDashboardDetails eSSDashboardDetails = new ESSDashboardDetails();
            //string[] reportData = eSSDashboardDetails.retrieveLeavesDetails(employeeId);
            string leaveCodes = "'Annual', 'Sick', 'Hajj', 'Casual' ";  //reportData[0];
            string accuredLeaveData = "10, 20, 30, 10";                 //reportData[1];
            string takenLeaveData = "4, 8, 0, 2";                       //reportData[2];


            string script = @"$(document).ready(function () {
                            'use strict';
                            // Main Template Color
                            var brandPrimary = '#4bc0c0';
                            var brandSecondary = '#FFCE56';       //'#C70039';
                            var LINECHART = $('#chartLeaves');
                            var myLineChart = new Chart(LINECHART, {
                                type: 'bar',
                                options: {
                                    legend: { display: false },
                                    scales: { yAxes: [{ ticks: { beginAtZero:true } }] }
                                },
                                data: {
                                    labels: [" + leaveCodes + @"],
                                    datasets: [
                                        {
                                            label: 'Accured Leaves',
                                            fill: false,
                                            lineTension: 0.3,
                                            backgroundColor: brandPrimary,
                                            borderColor: brandPrimary,
                                            borderCapStyle: 'butt',
                                            borderDash: [],
                                            borderDashOffset: 0.0,
                                            borderJoinStyle: 'miter',
                                            borderWidth: 1,
                                            pointBorderColor: brandPrimary,
                                            pointBackgroundColor: '#fff',
                                            pointBorderWidth: 1,
                                            pointHoverRadius: 5,
                                            pointHoverBackgroundColor: brandPrimary,
                                            pointHoverBorderColor: 'rgba(220,220,220,1)',
                                            pointHoverBorderWidth: 2,
                                            pointRadius: 1,
                                            pointHitRadius: 10,
                                            data: [" + accuredLeaveData + @"],
                                            spanGaps: false
                                        },
                                        {
                                            label: 'Taken Leaves',
                                            fill: true,
                                            lineTension: 0.3,
                                            backgroundColor: brandSecondary,
                                            borderColor: brandSecondary,
                                            borderCapStyle: 'butt',
                                            borderDash: [],
                                            borderDashOffset: 0.0,
                                            borderJoinStyle: 'miter',
                                            borderWidth: 1,
                                            pointBorderColor: brandSecondary,
                                            pointBackgroundColor: '#fff',
                                            pointBorderWidth: 1,
                                            pointHoverRadius: 5,
                                            pointHoverBackgroundColor: brandSecondary,
                                            pointHoverBorderColor: 'rgba(220,220,220,1)',
                                            pointHoverBorderWidth: 2,
                                            pointRadius: 1,
                                            pointHitRadius: 10,
                                            data: [" + takenLeaveData + @"],
                                            spanGaps: false
                                        }
                                    ]
                                }
                            });
                        });";

            Page currentPage = System.Web.HttpContext.Current.CurrentHandler as Page;
            currentPage.ClientScript.RegisterStartupScript(currentPage.GetType(), "jsChartLeaves", script, true);
        }

        private void chartsTax(string _labels, string _Data)
        {
            string script = @"$(document).ready(function () {
                            'use strict';
                            // Main Template Color
                            var brandPrimary = '#4bc0c0';//'#C70039';
                            var LINECHART = $('#chartTax');
                            var myLineChart = new Chart(LINECHART, {
                                type: 'bar',
                                options: {
                                    legend: { display: false },
                                    scales: { yAxes: [{ ticks: { beginAtZero:true } }] }
                                },
                                data: {
                                    labels: [" + _labels + @"],
                                    datasets: [
                                        {
                                            label: 'Tax',
                                            fill: false,
                                            lineTension: 0.3,
                                            backgroundColor: brandPrimary,
                                            borderColor: brandPrimary,
                                            borderCapStyle: 'butt',
                                            borderDash: [],
                                            borderDashOffset: 0.0,
                                            borderJoinStyle: 'miter',
                                            borderWidth: 1,
                                            pointBorderColor: brandPrimary,
                                            pointBackgroundColor: '#fff',
                                            pointBorderWidth: 1,
                                            pointHoverRadius: 5,
                                            pointHoverBackgroundColor: brandPrimary,
                                            pointHoverBorderColor: 'rgba(220,220,220,1)',
                                            pointHoverBorderWidth: 2,
                                            pointRadius: 1,
                                            pointHitRadius: 20,
                                            data: [" + _Data + @"],
                                            spanGaps: false
                                        }
                                    ]
                                }
                            });
                        });";
            Page currentPage = System.Web.HttpContext.Current.CurrentHandler as Page;
            currentPage.ClientScript.RegisterStartupScript(currentPage.GetType(), "jsChartTax", script, true);
        }

        private void chartsPayrollTotals(string _chartLabels, string _chartData)
        {
            string script = @"$(document).ready(
                            function () {
                            'use strict';
                            var chartLabels = [" + _chartLabels + @"];
                            var chartData = [" + _chartData + @"];
                            var PIECHART = $('#chartPayrollTotals');
                            var myPieChart = new Chart(PIECHART, {
                                type: 'pie',//doughnut
                                options: {
                                    legend: { 
                                                display: true,
                                                position: 'bottom',
                                                fullWidth: true,
                                                labels: {
                                                    fontSize: 9,
                                                    padding:  5
                                                        }
                                            }
                                },
                                data: {
                                    labels: chartLabels,
                                    datasets: [
                                        {
                                            data: chartData,
                                            borderWidth: [1, 1],
                                            backgroundColor: [
                                                '#4bc0c0',
                                                '#FFCE56',
                                                '#c0864b',
                                                '#c04b86',
                                                '#4b86c0',
                                                '#c04b4b',
                                                '#4bc04b',
                                                '#4bc085',
                                                '#c04b86',
                                                '#86c04b',
                                                '#4bc085',
                                                '#c04b86',
                                                '#c0854b',
                                            ],
                                            hoverBackgroundColor: [
                                                '#4bc0c0',
                                                '#FFCE56',
                                                '#c0864b',
                                                '#c04b86',
                                                '#4b86c0',
                                                '#c04b4b',
                                                '#4bc04b',
                                                '#4bc085',
                                                '#c04b86',
                                                '#86c04b',
                                                '#4bc085',
                                                '#c04b86',
                                                '#c0854b',
                                            ]
                                        }
                                    ]
                                }
                            });
                        });";

            Page currentPage = System.Web.HttpContext.Current.CurrentHandler as Page;
            currentPage.ClientScript.RegisterStartupScript(currentPage.GetType(), "jsChartPayrollTotals", script, true);
        }



    }
}