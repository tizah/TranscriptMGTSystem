using OfficeOpenXml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TranscriptMGTSystem.Controllers
{
    public class ExcelValidation
    {
        public string ValidateExcel(int noOfRow, ExcelWorksheet workSheet, int noOfRequired)
        {
            int colum = 0;
            for (int row = 2; row <= noOfRow; row++)
            {
                ArrayList myList = new ArrayList();
                try
                {
                    for (int i = 1; i <= noOfRequired; i++)
                    {
                        myList.Add(workSheet.Cells[row, i].Value.ToString().Trim());
                        colum = i + 1;
                    }
                }
                catch (Exception e)
                {
                    //Validation = row.ToString();
                    var message = e.Message;
                    return row.ToString() + " " + colum.ToString();
                }
            }
            return "Success";
        }
    }
}