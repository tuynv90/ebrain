// ======================================
// Author: Ebrain Team
// Email:  johnpham@ymail.com
// Copyright (c) 2017 supperbrain.visualstudio.com
// 
// ==> Contact Us: supperbrain@outlook.com
// ======================================

using System;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Ebrain.ViewModels;
using System.Text;
using System.Globalization;
using ebrain.admin.bc.Utilities;
using ebrain.admin.bc.Report;
using ebrain.admin.bc.Models;

namespace Ebrain.Helpers
{
    public static class EmailTemplates
    {
        static IHostingEnvironment _hostingEnvironment;
        static string testEmailTemplate;
        static string plainTextTestEmailTemplate;
        static IOptions<SmtpConfig> _serviceSmtpConfig;

        public static void Initialize(IHostingEnvironment hostingEnvironment, IOptions<SmtpConfig> serviceSmtpConfig)
        {
            _hostingEnvironment = hostingEnvironment;
            _serviceSmtpConfig = serviceSmtpConfig;
        }

        public static string GetTempalteEmail(IOptions<SmtpConfig> _serviceSmtpConfig, IOStockViewModel ios)
        {

            testEmailTemplate = ReadPhysicalFile("Helpers/Templates/exampletemplate.html");

            return testEmailTemplate
                .Replace("{{IONumber}}", ios.Code)
                .Replace("{{PurchaseOrderCode}}", ios.PurchaseOrderCode)
                .Replace("{{CreatedDate}}", ios.CreateDate.ToString("MM/dd/yyyy HH:mm:ss.fff", CultureInfo.InvariantCulture))
                .Replace("{{CreatedBy}}", ios.UpdatedByName)
                .Replace("{{Note}}", ios.Note)
                .Replace("{{rows}}", MappingIOTable(ios.IODetails));
        }

        public static string GetTempalteEmail(IOptions<SmtpConfig> _serviceSmtpConfig, PaymentViewModel ios)
        {

            testEmailTemplate = ReadPhysicalFile("Helpers/Templates/examplepaymenttemplate.html");

            return testEmailTemplate
                .Replace("{{PaymentCode}}", ios.Code)
                .Replace("{{PaymentTypeName}}", ios.PaymentTypeName)
                .Replace("{{CreatedDate}}", ios.CreateDate.ToString("MM/dd/yyyy HH:mm:ss.fff", CultureInfo.InvariantCulture))
                .Replace("{{CreatedBy}}", ios.UpdatedByName)
                .Replace("{{Note}}", ios.Note)
                .Replace("{{rows}}", MappingIOPayment(ios.IODetails));
        }

        public static bool SendEmail_IOStock(IOptions<SmtpConfig> _serviceSmtpConfig, IOStockViewModel ios)
        {
            var emailMessage = GetTempalteEmail(_serviceSmtpConfig, ios);

            return SendEmailTemplate(_serviceSmtpConfig, emailMessage);
        }

        public static bool SendEmail_Payment(IOptions<SmtpConfig> _serviceSmtpConfig, PaymentViewModel ios)
        {
            var emailMessage = GetTempalteEmail(_serviceSmtpConfig, ios);

            return SendEmailTemplate(_serviceSmtpConfig, emailMessage);
        }

        private static bool SendEmailTemplate(IOptions<SmtpConfig> _serviceSmtpConfig, string emailMessage)
        {
            var configStmp = _serviceSmtpConfig.Value;
            EmailSender.SendEmailAsync(
               "Superbrain",
               new string[] { "qmvnn2000@gmail.com", "Lt.quang.it@gmail.com", "trucdoan.spb.lotte@gmail.com", "Sthapanangkun@superbrain.vn", "happygot3am@gmail.com" },
               "[Not Reply Email] Auto send email",
               emailMessage,
               configStmp
               );
            return true;
        }

        private static string MappingIOPayment(PaymentDetailViewModel[] ios)
        {
            var strInnerHtml = new StringBuilder();
            foreach (var item in ios)
            {
                strInnerHtml.AppendFormat("<tr>");
                strInnerHtml.AppendFormat($"<td>{item.Code}</td>");
                strInnerHtml.AppendFormat($"<td style=\"text-align:right;\">{item.TotalPrice.ConvertDecimalToCurrency()}</td>");
                strInnerHtml.AppendFormat($"<td style=\"text-align:right;\">{item.TotalPricePayment.ConvertDecimalToCurrency()}</td>");
                strInnerHtml.AppendFormat($"<td style=\"text-align:right;\">{item.TotalPriceExist.ConvertDecimalToCurrency()}</td>");
                strInnerHtml.AppendFormat($"<td>{item.Note}</td>");
                strInnerHtml.AppendFormat("</tr>");
            }
            return strInnerHtml.ToString();
        }

        private static string MappingIOTable(IOStockDetailViewModel[] ios)
        {
            var strInnerHtml = new StringBuilder();
            foreach (var item in ios)
            {
                strInnerHtml.AppendFormat("<tr>");
                strInnerHtml.AppendFormat($"<td>{item.TypeMaterial}</td>");
                strInnerHtml.AppendFormat($"<td>{item.GrpMaterial}</td>");
                strInnerHtml.AppendFormat($"<td>{item.MaterialCode}</td>");
                strInnerHtml.AppendFormat($"<td>{item.MaterialName}</td>");
                strInnerHtml.AppendFormat($"<td style=\"text-align:right;\">{item.Quantity}</td>");
                strInnerHtml.AppendFormat($"<td style=\"text-align:right;\">{item.SellPrice.ConvertDecimalToCurrency()}</td>");
                strInnerHtml.AppendFormat($"<td style=\"text-align:right;\">{item.TotalPrice.ConvertDecimalToCurrency()}</td>");
                strInnerHtml.AppendFormat($"<td >{item.Note}</td>");
                strInnerHtml.AppendFormat("</tr>");
            }
            return strInnerHtml.ToString();
        }

        public static string GetTemplate_Schedules(IOptions<SmtpConfig> _serviceSmtpConfig, ClassList[] classList,
            BranchViewModel branch, Student st, Class cl, Supplier sup)
        {
            testEmailTemplate = ReadPhysicalFile("Helpers/Templates/schedules/schedules.html");
            var itemRow = Mapping_Schedules(classList);
            var itemFirst = classList[0];
            testEmailTemplate = testEmailTemplate
                .Replace("{{BranchName}}", branch.Name.ToUpper())
                .Replace("{{BranchAddress}}", branch.Address)
                .Replace("{{BranchPhone1}}", branch.PhoneNumber)
                .Replace("{{BranchPhone2}}", branch.PhoneNumber)
                .Replace("{{BranchFacebook}}", branch.Fax)
                .Replace("{{BranchLogo}}", branch.Logo.Name)
                // header
                .Replace("{{UserName}}", $"{st.StudentCode.ToUpper()} - {st.StudentName.ToUpper()}")
                .Replace("{{MaterialName}}", itemFirst.MaterialName)
                .Replace("{{ClassName}}", cl.ClassName)
                .Replace("{{TeacherName}}", sup.SupplierName)
                .Replace("{{UserPhone}}", st.Phone)
                // body
                .Replace("{{rows1}}", itemRow.Item1.ToString())
                .Replace("{{rows2}}", itemRow.Item2.ToString());

            return testEmailTemplate;
        }

        private static Tuple<StringBuilder, StringBuilder> Mapping_Schedules(ClassList[] ios)
        {
            var strInnerHtml = new StringBuilder();
            var strInnerHtml1 = new StringBuilder();
            // fixed 36 rows on page
            for (var row = 1; row < 39; row++)
            {
                var html = row > 19 ? strInnerHtml1 : strInnerHtml;
                html.AppendFormat("<tr>");
                html.AppendFormat($"<td class=\"center stt\">{row}</td>");
                // set data
                var learnDate = string.Empty;
                var todayName = string.Empty;
                var time = string.Empty;
                if (row <= ios.Length)
                {
                    var item = ios[row - 1];
                    learnDate = item.LearnDate.FormatYYMMDD();
                    todayName = item.TodayName;
                    time = $"{ item.StartTime.FormatHHMM()} - { item.EndTime.FormatHHMM()}";
                }
                html.AppendFormat($"<td>{learnDate}</td>");
                html.AppendFormat($"<td>{todayName}</td>");
                html.AppendFormat($"<td style=\"text-align:right;\">{time}</td>");
                html.AppendFormat("</tr>");
            }
            return Tuple.Create(strInnerHtml, strInnerHtml1);// new { str: strInnerHtml, str1: strInnerHtml1 }; // strInnerHtml.ToString();
        }

        public static string GetPlainTextTestEmail(DateTime date)
        {
            if (plainTextTestEmailTemplate == null)
                plainTextTestEmailTemplate = ReadPhysicalFile("Helpers/Templates/PlainTextTestEmail.template");


            string emailMessage = plainTextTestEmailTemplate
                .Replace("{date}", date.ToString());

            return emailMessage;
        }




        private static string ReadPhysicalFile(string path)
        {
            try
            {
                if (_hostingEnvironment == null)
                    throw new InvalidOperationException($"{nameof(EmailTemplates)} is not initialized");

                IFileInfo fileInfo = _hostingEnvironment.ContentRootFileProvider.GetFileInfo(path);

                if (!fileInfo.Exists)
                    throw new FileNotFoundException($"Template file located at \"{path}\" was not found");

                using (var fs = fileInfo.CreateReadStream())
                {
                    using (var sr = new StreamReader(fs))
                    {
                        return sr.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static async Task<(bool success, string errorMsg)> GenerateSendEmail(Func<string> Func_GetBodyHtml, string[] recepientEmails,
            string subject)
        {
            // get body html
            var bodyHtml = Func_GetBodyHtml?.Invoke();
            var configStmp = _serviceSmtpConfig.Value;
            var result = await EmailSender.SendEmailAsync(
                EmailSender.GetMailboxAddress(recepientEmails).ToArray(), //"qmvnn2000@gmail.com",
                subject,
                bodyHtml,
                configStmp
                );
            return result;
        }
    }
}
