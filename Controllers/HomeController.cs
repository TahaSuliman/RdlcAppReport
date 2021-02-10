using AspNetCore.Reporting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RdlcAppReport.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace RdlcAppReport.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Print()
        {
            var dt = new DataTable();
            DataTable[] dtArray = new DataTable[2];

            dtArray = GetEmployeeList();
            string mimetype = "";
            int extension = 1;
            var path = $"{this._webHostEnvironment.WebRootPath}\\Reports\\ReportRdlc.rdlc";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("prm", "Z. Apple. An apple is just an apple. BMW. The accidental propeller. Chupa");
            LocalReport localReport= new LocalReport(path);
            localReport.AddDataSource("EmployeeDataSet", dtArray[0]);
            //localReport.AddDataSource("EmployeeDetails", dtArray[1]);

            var result = localReport.Execute(RenderType.Pdf, extension, parameters, mimetype);

            return File(result.MainStream,"application/pdf");
        }

        public IActionResult PrintTwo()
        {
            var dt = new DataTable();
            var dtSub = new DataTable();

            DataTable[] dtArray = new DataTable[2];

            dtArray = GetEmployeeList();
            string mimetype = "";
            int extension = 1;
            var path = $"{this._webHostEnvironment.WebRootPath}\\Reports\\ReportRdlcTwo.rdlc";
            var pathSub = $"{this._webHostEnvironment.WebRootPath}\\Reports\\subReport.rdlc";

            //Dictionary<string, string> parameters = new Dictionary<string, string>();
            //Dictionary<string, string> parametersSub = new Dictionary<string, string>();

            //parameters.Add("prm", "Z. Apple. An apple is just an apple. BMW. The accidental propeller. Chupa");
            //parametersSub.Add("subReportParameter", "0");
            LocalReport localReportSub = new LocalReport(pathSub);
            LocalReport localReport = new LocalReport(path);

            localReportSub.AddDataSource("DataSetSub", dtArray[1]);
            localReport.AddDataSource("DataSetContact", dtArray[0]);
            //localReport.AddDataSource("DataSetContact", dtArray);


            dt = dtArray[0];
            dtSub = dtArray[1];
            var result = localReport.Execute(RenderType.Pdf, extension, null, mimetype);
            //var result = localReportSub.Execute(RenderType.Pdf, extension, null, mimetype);

            return File(result.MainStream, "application/pdf");
        }

        public DataTable[] GetEmployeeList()
        {
            var dt = new DataTable();
            DataTable[] dtArray = new DataTable[2];
            dtArray[0] = new DataTable("DataTable0");
            dtArray[1] = new DataTable("DataTable1");

            dtArray[0].Columns.Add("EmpId", typeof(Int32));
            dtArray[0].Columns.Add("EmpName");
            dtArray[0].Columns.Add("EmpDepart");
            dtArray[0].Columns.Add("EmpDesignation");

            dtArray[1].Columns.Add("EmpContId", typeof(Int32));
            dtArray[1].Columns.Add("EmpId", typeof(Int32));
            dtArray[1].Columns.Add("EmpContMobile");
            dtArray[1].Columns.Add("EmpContEmail");
            dtArray[1].Columns.Add("EmpContAddress");
            dtArray[1].Columns.Add("EmpContCity");

            DataRow row;
            DataRow rowTwo;

            for (int i = 0;i < 25; i++)
            {
                row = dtArray[0].NewRow();
                row["EmpId"]=i;
                row["EmpName"]= "Taha Suliman Ramadan" + i.ToString();
                row["EmpDepart"]= "Accounting DeprtMemt" + i.ToString();
                row["EmpDesignation"]= "SoftWare Engineer" + i.ToString();
                dtArray[0].Rows.Add(row);
                //------------------------------
                for (int j = 0; j < 5; j++)
                {
                    int x = i * 5 + j;
                    rowTwo = dtArray[1].NewRow();
                    rowTwo["EmpContId"] = x;
                    rowTwo["EmpId"] = i;
                    rowTwo["EmpContMobile"] = "00971523215230";
                    rowTwo["EmpContEmail"] = "taha@gmail.com" + j.ToString();
                    rowTwo["EmpContAddress"] = "Sharjah Gasimiya Mahata" + j.ToString();
                    rowTwo["EmpContCity"] = "United Emarate Sharjah" + j.ToString();
                    dtArray[1].Rows.Add(rowTwo);
                }

            }
            return dtArray;
        }
    }
}
