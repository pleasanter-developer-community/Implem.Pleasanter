﻿using Implem.Libraries.Utilities;
using Implem.Pleasanter.Libraries.DataTypes;
using Implem.Pleasanter.Libraries.General;
using Implem.Pleasanter.Libraries.Requests;
using Implem.Pleasanter.Libraries.Responses;
using Implem.Pleasanter.Libraries.Search;
using Implem.Pleasanter.Libraries.Settings;
using Implem.Pleasanter.Libraries.Web;
using Implem.Pleasanter.Models;
using Implem.PleasanterFilters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
namespace Implem.Pleasanter.Controllers
{
    [Authorize]
    public class ItemsController : Controller
    {
        [AcceptVerbs(HttpVerbs.Get, HttpVerbs.Post)]
        public ActionResult Index(long id = 0)
        {
            var context = new Context();
            if (!context.Ajax)
            {
                var log = new SysLogModel(context: context);
                var html = new ItemModel(context: context, referenceId: id).Index(context: context);
                ViewBag.HtmlBody = html;
                log.Finish(context: context, responseSize: html.Length);
                return context.RedirectData.Url.IsNullOrEmpty()
                    ? View()
                    : (ActionResult)Redirect(context.RedirectData.Url);
            }
            else
            {
                var log = new SysLogModel(context: context);
                var json = new ItemModel(context: context, referenceId: id).IndexJson(context: context);
                log.Finish(context: context, responseSize: json.Length);
                return Content(json);
            }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult DashboardPart(long id, string dashboardPartId)
        {
            var context = new Context(); 
            var log = new SysLogModel(context: context);
            var json = new ItemModel(context: context, referenceId: id).DashboardPartJson(context: context, dashboardPartId: dashboardPartId);
            log.Finish(context: context, responseSize: json.Length);
            return Content(json);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DashboardPartLayout(long id)
        {
            var context = new Context();
            var log = new SysLogModel(context: context);
            var json = new ItemModel(context: context, referenceId: id).DashboardPartLayout(context: context);
            log.Finish(context: context, responseSize: json.Length);
            return Content(json);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Pdf(long id, int reportId = 0, bool download = false)
        {
            var context = new Context();
            var log = new SysLogModel(context: context);
            var (pdfData, error) = new ItemModel(context: context, referenceId: id)
                .Pdf(
                    context: context,
                    reportId: reportId);
            if (pdfData?.Stream != null)
            {
                var result = new FileStreamResult(pdfData.Stream, "application/pdf");
                if (download)
                {
                    result.FileDownloadName = pdfData.FileName;
                }
                log.Finish(context: context, responseSize: (int)result.FileStream.Length);
                return result;
            }
            var html = error.ToString();
            ViewBag.HtmlBody = html;
            log.Finish(context: context, responseSize: html.Length);
            return View();
        }

        [AcceptVerbs(HttpVerbs.Get, HttpVerbs.Post)]
        public ActionResult TrashBox(long id = 0)
        {
            var context = new Context();
            if (!context.Ajax)
            {
                var log = new SysLogModel(context: context);
                var html = new ItemModel(context: context, referenceId: id).TrashBox(context: context);
                ViewBag.HtmlBody = html;
                log.Finish(context: context, responseSize: html.Length);
                return context.RedirectData.Url.IsNullOrEmpty()
                    ? View()
                    : (ActionResult)Redirect(context.RedirectData.Url);
            }
            else
            {
                var log = new SysLogModel(context: context);
                var json = new ItemModel(context: context, referenceId: id).TrashBoxJson(context: context);
                log.Finish(context: context, responseSize: json.Length);
                return Content(json);
            }
        }

        [AcceptVerbs(HttpVerbs.Get, HttpVerbs.Post)]
        public ActionResult Calendar(long id = 0)
        {
            var context = new Context();
            if (!context.Ajax)
            {
                var log = new SysLogModel(context: context);
                var html = new ItemModel(context: context, referenceId: id).Calendar(context: context);
                ViewBag.HtmlBody = html;
                log.Finish(context: context, responseSize: html.Length);
                return context.RedirectData.Url.IsNullOrEmpty()
                    ? View()
                    : (ActionResult)Redirect(context.RedirectData.Url);
            }
            else
            {
                var log = new SysLogModel(context: context);
                var json = new ItemModel(context: context, referenceId: id).CalendarJson(context: context);
                log.Finish(context: context, responseSize: json.Length);
                return Content(json);
            }
        }

        [AcceptVerbs(HttpVerbs.Get, HttpVerbs.Post)]
        public ActionResult Crosstab(long id = 0)
        {
            var context = new Context();
            if (!context.Ajax)
            {
                var log = new SysLogModel(context: context);
                var html = new ItemModel(context: context, referenceId: id).Crosstab(context: context);
                ViewBag.HtmlBody = html;
                log.Finish(context: context, responseSize: html.Length);
                return context.RedirectData.Url.IsNullOrEmpty()
                    ? View()
                    : (ActionResult)Redirect(context.RedirectData.Url);
            }
            else
            {
                var log = new SysLogModel(context: context);
                var json = new ItemModel(context: context, referenceId: id).CrosstabJson(context: context);
                log.Finish(context: context, responseSize: json.Length);
                return Content(json);
            }
        }

        [AcceptVerbs(HttpVerbs.Get, HttpVerbs.Post)]
        public ActionResult Gantt(long id = 0)
        {
            var context = new Context();
            if (!context.Ajax)
            {
                var log = new SysLogModel(context: context);
                var html = new ItemModel(context: context, referenceId: id).Gantt(context: context);
                ViewBag.HtmlBody = html;
                log.Finish(context: context, responseSize: html.Length);
                return context.RedirectData.Url.IsNullOrEmpty()
                    ? View()
                    : (ActionResult)Redirect(context.RedirectData.Url);
            }
            else
            {
                var log = new SysLogModel(context: context);
                var json = new ItemModel(context: context, referenceId: id).GanttJson(context: context);
                log.Finish(context: context, responseSize: json.Length);
                return Content(json);
            }
        }

        [AcceptVerbs(HttpVerbs.Get, HttpVerbs.Post)]
        public ActionResult BurnDown(long id = 0)
        {
            var context = new Context();
            if (!context.Ajax)
            {
                var log = new SysLogModel(context: context);
                var html = new ItemModel(context: context, referenceId: id).BurnDown(context: context);
                ViewBag.HtmlBody = html;
                log.Finish(context: context, responseSize: html.Length);
                return context.RedirectData.Url.IsNullOrEmpty()
                    ? View()
                    : (ActionResult)Redirect(context.RedirectData.Url);
            }
            else
            {
                var log = new SysLogModel(context: context);
                var json = new ItemModel(context: context, referenceId: id).BurnDownJson(context: context);
                log.Finish(context: context, responseSize: json.Length);
                return Content(json);
            }
        }

        [AcceptVerbs(HttpVerbs.Get, HttpVerbs.Post)]
        public ActionResult TimeSeries(long id = 0)
        {
            var context = new Context();
            if (!context.Ajax)
            {
                var log = new SysLogModel(context: context);
                var html = new ItemModel(context: context, referenceId: id).TimeSeries(context: context);
                ViewBag.HtmlBody = html;
                log.Finish(context: context, responseSize: html.Length);
                return context.RedirectData.Url.IsNullOrEmpty()
                    ? View()
                    : (ActionResult)Redirect(context.RedirectData.Url);
            }
            else
            {
                var log = new SysLogModel(context: context);
                var json = new ItemModel(context: context, referenceId: id).TimeSeriesJson(context: context);
                log.Finish(context: context, responseSize: json.Length);
                return Content(json);
            }
        }

        [AcceptVerbs(HttpVerbs.Get, HttpVerbs.Post)]
        public ActionResult Analy(long id = 0)
        {
            var context = new Context();
            if (!context.Ajax)
            {
                var log = new SysLogModel(context: context);
                var html = new ItemModel(context: context, referenceId: id).Analy(context: context);
                ViewBag.HtmlBody = html;
                log.Finish(context: context, responseSize: html.Length);
                return context.RedirectData.Url.IsNullOrEmpty()
                    ? View()
                    : (ActionResult)Redirect(context.RedirectData.Url);
            }
            else
            {
                var log = new SysLogModel(context: context);
                var json = new ItemModel(context: context, referenceId: id).AnalyJson(context: context);
                log.Finish(context: context, responseSize: json.Length);
                return Content(json);
            }
        }

        [HttpPost]
        public string OpenAnalyPartDialog(long id)
        {
            var context = new Context();
            var log = new SysLogModel(context: context);
            var json = new ItemModel(context: context, referenceId: id).OpenAnalyPartDialog(context: context);
            log.Finish(context: context, responseSize: json.Length);
            return json;
        }

        [AcceptVerbs(HttpVerbs.Get, HttpVerbs.Post)]
        public ActionResult Kamban(long id = 0)
        {
            var context = new Context();
            if (!context.Ajax)
            {
                var log = new SysLogModel(context: context);
                var html = new ItemModel(context: context, referenceId: id).Kamban(context: context);
                ViewBag.HtmlBody = html;
                log.Finish(context: context, responseSize: html.Length);
                return context.RedirectData.Url.IsNullOrEmpty()
                    ? View()
                    : (ActionResult)Redirect(context.RedirectData.Url);
            }
            else
            {
                var log = new SysLogModel(context: context);
                var json = new ItemModel(context: context, referenceId: id).KambanJson(context: context);
                log.Finish(context: context, responseSize: json.Length);
                return Content(json);
            }
        }

        [AcceptVerbs(HttpVerbs.Get, HttpVerbs.Post)]
        public ActionResult ImageLib(long id = 0)
        {
            var context = new Context();
            if (!context.Ajax)
            {
                var log = new SysLogModel(context: context);
                var html = new ItemModel(context: context, referenceId: id).ImageLib(context: context);
                ViewBag.HtmlBody = html;
                log.Finish(context: context, responseSize: html.Length);
                return context.RedirectData.Url.IsNullOrEmpty()
                    ? View()
                    : (ActionResult)Redirect(context.RedirectData.Url);
            }
            else
            {
                var log = new SysLogModel(context: context);
                var json = new ItemModel(context: context, referenceId: id).ImageLibJson(context: context);
                log.Finish(context: context, responseSize: json.Length);
                return Content(json);
            }
        }

        [AcceptVerbs(HttpVerbs.Get, HttpVerbs.Post)]
        public ActionResult New(long id = 0)
        {
            var context = new Context();
            if (!context.Ajax)
            {
                var log = new SysLogModel(context: context);
                var html = new ItemModel(context: context, referenceId: id).New(context: context);
                ViewBag.HtmlBody = html;
                log.Finish(context: context, responseSize: html.Length);
                return context.RedirectData.Url.IsNullOrEmpty()
                    ? View()
                    : (ActionResult)Redirect(context.RedirectData.Url);
            }
            else
            {
                var log = new SysLogModel(context: context);
                var json = new ItemModel(context: context, referenceId: id).NewJson(context: context);
                log.Finish(context: context, responseSize: json.Length);
                return Content(json);
            }
        }

        [HttpPost]
        public ActionResult NewOnGrid(long id = 0)
        {
            var context = new Context();
            var log = new SysLogModel(context: context);
            var json = new ItemModel(
                context: context,
                referenceId: id)
                    .NewOnGrid(context: context);
            log.Finish(
                context: context,
                responseSize: json.Length);
            return Content(json);
        }

        [HttpPost]
        public ActionResult CancelNewRow(long id = 0)
        {
            var context = new Context();
            var log = new SysLogModel(context: context);
            var json = new ItemModel(
                context: context,
                referenceId: id)
                    .CancelNewRow(context: context);
            log.Finish(
                context: context,
                responseSize: json.Length);
            return Content(json);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult SmartDesign(long id = 0)
        {
            var context = new Context();
            if (!context.Ajax)
            {
                return BadRequest();
            }
            else
            {
                var log = new SysLogModel(context: context);
                var json = new ItemModel(context: context, referenceId: id).SmartDesignJson(context: context);
                log.Finish(context: context, responseSize: json.Length);
                return Content(json);
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdateSmartDesign(long id = 0)
        {
            var jsonBody = default(string);
            var context = new Context();
            using (var reader = new StreamReader(Request.Body)) jsonBody = reader.ReadToEnd();
            var log = new SysLogModel(context: context);
            var json = new ItemModel(context: context, referenceId: id).UpdateSmartDesign(
                context: context,
                jsonBody: jsonBody);
            return Content(json);
        }

        [AcceptVerbs(HttpVerbs.Get, HttpVerbs.Post)]
        public ActionResult Edit(long id)
        {
            var context = new Context();
            if (!context.Ajax)
            {
                var log = new SysLogModel(context: context);
                var html = new ItemModel(context: context, referenceId: id).Editor(context: context);
                ViewBag.HtmlBody = html;
                log.Finish(context: context, responseSize: html.Length);
                return context.RedirectData.Url.IsNullOrEmpty()
                    ? View()
                    : (ActionResult)Redirect(context.RedirectData.Url);
            }
            else
            {
                var log = new SysLogModel(context: context);
                var json = new ItemModel(context: context, referenceId: id).EditorJson(context: context);
                log.Finish(context: context, responseSize: json.Length);
                return Content(json);
            }
        }

        [HttpPost]
        public string SelectedIds(long id)
        {
            var context = new Context();
            var log = new SysLogModel(context: context);
            var json = new ItemModel(context: context, referenceId: id).SelectedIds(context: context);
            log.Finish(context: context, responseSize: json.Length);
            return json;
        }

        [HttpPost]
        public string LinkTable(long id)
        {
            var context = new Context();
            var log = new SysLogModel(context: context);
            var json = new ItemModel(context: context, referenceId: id).LinkTable(context: context);
            log.Finish(context: context, responseSize: json.Length);
            return json;
        }

        [HttpPost]
        public string Import(long id, ICollection<IFormFile> file)
        {
            var context = new Context(files: file);
            var log = new SysLogModel(context: context);
            var json = new ItemModel(context: context, referenceId: id).Import(context: context);
            log.Finish(context: context, responseSize: json.Length);
            return json;
        }

        [HttpPost]
        public string OpenExportSelectorDialog(long id)
        {
            var context = new Context();
            var log = new SysLogModel(context: context);
            var json = new ItemModel(context: context, referenceId: id).OpenExportSelectorDialog(context: context);
            log.Finish(context: context, responseSize: json.Length);
            return json;
        }

        [HttpPost]
        public string OpenBulkUpdateSelectorDialog(long id)
        {
            var context = new Context();
            var log = new SysLogModel(context: context);
            var json = new ItemModel(context: context, referenceId: id).OpenBulkUpdateSelectorDialog(context: context);
            log.Finish(context: context, responseSize: json.Length);
            return json;
        }

        [HttpPost]
        public string BulkUpdateSelectChanged(long id)
        {
            var context = new Context();
            var log = new SysLogModel(context: context);
            var json = new ItemModel(context: context, referenceId: id).BulkUpdateSelectChanged(context: context);
            log.Finish(context: context, responseSize: json.Length);
            return json;
        }

        [HttpPost]
        public string OpenSetNumericRangeDialog(long id)
        {
            var context = new Context();
            var log = new SysLogModel(context: context);
            var json = new ItemModel(context: context, referenceId: id).OpenSetNumericRangeDialog(context: context);
            log.Finish(context: context, responseSize: json.Length);
            return json;
        }

        [HttpPost]
        public string OpenSetDateRangeDialog(long id)
        {
            var context = new Context();
            var log = new SysLogModel(context: context);
            var json = new ItemModel(context: context, referenceId: id).OpenSetDateRangeDialog(context: context);
            log.Finish(context: context, responseSize: json.Length);
            return json;
        }

        [HttpPost]
        public FileContentResult Export(long id)
        {
            var context = new Context();
            var log = new SysLogModel(context: context);
            var responseFile = new ItemModel(context: context, referenceId: id).Export(context: context);
            if (responseFile != null)
            {
                log.Finish(context: context, responseSize: responseFile.Length);
                return responseFile.ToFile();
            }
            else
            {
                log.Finish(context: context);
                return null;
            }
        }

        [HttpPost]
        public string ExportAndMailNotify(long id)
        {
            var context = new Context();
            var log = new SysLogModel(context: context);
            var json = new ItemModel(context: context, referenceId: id).ExportAndMailNotify(context: context);
            log.Finish(context: context, responseSize: json.Length);
            return json;
        }

        [HttpGet]
        public FileContentResult ExportCrosstab(long id)
        {
            var context = new Context();
            var log = new SysLogModel(context: context);
            var responseFile = new ItemModel(context: context, referenceId: id).ExportCrosstab(context: context);
            if (responseFile != null)
            {
                log.Finish(context: context, responseSize: responseFile.Length);
                return responseFile.ToFile();
            }
            else
            {
                log.Finish(context: context);
                return null;
            }
        }

        [AcceptVerbs(HttpVerbs.Get, HttpVerbs.Post)]
        public ActionResult Search()
        {
            var context = new Context();
            if (!context.Ajax)
            {
                var log = new SysLogModel(context: context);
                var html = Indexes.Search(context: context);
                ViewBag.HtmlBody = html;
                log.Finish(context: context, responseSize: html.Length);
                return context.RedirectData.Url.IsNullOrEmpty()
                    ? View()
                    : (ActionResult)Redirect(context.RedirectData.Url);
            }
            else
            {
                var log = new SysLogModel(context: context);
                var json = Indexes.SearchJson(context: context);
                log.Finish(context: context, responseSize: json.Length);
                return Content(json);
            }
        }

        [HttpPost]
        public ActionResult SearchDropDown(long id = 0)
        {
            var context = new Context();
            var log = new SysLogModel(context: context);
            var json = new ItemModel(context: context, referenceId: id).SearchDropDown(context: context);
            log.Finish(context: context, responseSize: json.Length);
            return Content(json);
        }

        [HttpPost]
        public ActionResult RelatingDropDown(long id = 0)
        {
            var context = new Context();
            var log = new SysLogModel(context: context);
            var json = new ItemModel(context: context, referenceId: id).RelatingDropDown(context: context);
            log.Finish(context: context, responseSize: json.Length);
            return Content(json);
        }

        [HttpPost]
        public ActionResult SelectSearchDropDown(long id = 0)
        {
            var context = new Context();
            var log = new SysLogModel(context: context);
            var json = new ItemModel(context: context, referenceId: id).SelectSearchDropDown(context: context);
            log.Finish(context: context, responseSize: json.Length);
            return Content(json);
        }

        [HttpPost]
        public string GridRows(long id)
        {
            var context = new Context();
            var log = new SysLogModel(context: context);
            var json = new ItemModel(
                context: context,
                referenceId: id)
                    .GridRows(context: context);
            log.Finish(
                context: context,
                responseSize: json.Length);
            return json;
        }

        [HttpPost]
        public string ReloadRow(long id)
        {
            var context = new Context();
            var log = new SysLogModel(context: context);
            var json = new ItemModel(
                context: context,
                referenceId: id)
                    .ReloadRow(context: context);
            log.Finish(
                context: context,
                responseSize: json.Length);
            return json;
        }

        [HttpPost]
        public string CopyRow(long id)
        {
            var context = new Context();
            var log = new SysLogModel(context: context);
            var json = new ItemModel(
                context: context,
                referenceId: id)
                    .CopyRow(context: context);
            log.Finish(
                context: context,
                responseSize: json.Length);
            return json;
        }

        [HttpPost]
        public string TrashBoxGridRows(long id)
        {
            var context = new Context();
            var log = new SysLogModel(context: context);
            var json = new ItemModel(context: context, referenceId: id).TrashBoxGridRows(context: context);
            log.Finish(context: context, responseSize: json.Length);
            return json;
        }

        [HttpPost]
        public string ImageLibNext(long id)
        {
            var context = new Context();
            var log = new SysLogModel(context: context);
            var json = new ItemModel(context: context, referenceId: id).ImageLibNext(context: context);
            log.Finish(context: context, responseSize: json.Length);
            return json;
        }

        [HttpPost]
        public string Create(long id)
        {
            var context = new Context();
            var log = new SysLogModel(context: context);
            var json = new ItemModel(context: context, referenceId: id).Create(context: context);
            log.Finish(context: context, responseSize: json.Length);
            return json;
        }

        [HttpPost]
        public string PreviewTemplate(long id)
        {
            var context = new Context();
            var log = new SysLogModel(context: context);
            var json = SiteUtilities.PreviewTemplate(context: context);
            log.Finish(context: context, responseSize: json.Length);
            return json;
        }

        [HttpPost]
        public string Templates(long id)
        {
            var context = new Context();
            var log = new SysLogModel(context: context);
            var json = new ItemModel(context: context, referenceId: id).Templates(context: context);
            log.Finish(context: context, responseSize: json.Length);
            return json;
        }

        [HttpPost]
        public string CreateByTemplate(long id)
        {
            var context = new Context();
            var log = new SysLogModel(context: context);
            var json = new ItemModel(context: context, referenceId: id).CreateByTemplate(context: context);
            log.Finish(context: context, responseSize: json.Length);
            return json;
        }

        [HttpPost]
        public string SiteMenu(long id)
        {
            var context = new Context();
            var log = new SysLogModel(context: context);
            var json = new ItemModel(context: context, referenceId: id).SiteMenu(context: context);
            log.Finish(context: context, responseSize: json.Length);
            return json;
        }

        [HttpPut]
        public string Update(long id)
        {
            var context = new Context();
            var log = new SysLogModel(context: context);
            var json = new ItemModel(
                context: context,
                referenceId: id)
                    .Update(context: context);
            log.Finish(
                context: context,
                responseSize: json.Length);
            return json;
        }

        [HttpPost]
        public string BulkUpdate(long id)
        {
            var context = new Context();
            var log = new SysLogModel(context: context);
            var json = new ItemModel(
                context: context,
                referenceId: id)
                    .BulkUpdate(context: context);
            log.Finish(
                context: context,
                responseSize: json.Length);
            return json;
        }

        [HttpPost]
        public string UpdateByGrid(long id)
        {
            var context = new Context();
            var log = new SysLogModel(context: context);
            var json = new ItemModel(
                context: context,
                referenceId: id)
                    .UpdateByGrid(context: context);
            log.Finish(
                context: context,
                responseSize: json.Length);
            return json;
        }

        [HttpPost]
        public string BulkProcess(long id)
        {
            var context = new Context();
            var log = new SysLogModel(context: context);
            var json = new ItemModel(
                context: context,
                referenceId: id)
                    .BulkProcess(context: context);
            log.Finish(
                context: context,
                responseSize: json.Length);
            return json;
        }

        [HttpPost]
        public string Copy(long id)
        {
            var context = new Context();
            var log = new SysLogModel(context: context);
            var json = new ItemModel(context: context, referenceId: id).Copy(context: context);
            log.Finish(context: context, responseSize: json.Length);
            return json;
        }

        [HttpGet]
        public string MoveTargets(long id)
        {
            var context = new Context();
            var log = new SysLogModel(context: context);
            var json = new ItemModel(context: context, referenceId: id).MoveTargets(context: context);
            log.Finish(context: context, responseSize: json.Length);
            return json;
        }

        [HttpPut]
        public string Move(long id)
        {
            var context = new Context();
            var log = new SysLogModel(context: context);
            var json = new ItemModel(context: context, referenceId: id).Move(context: context);
            log.Finish(context: context, responseSize: json.Length);
            return json;
        }

        [HttpPost]
        public string MoveSiteMenu(long id)
        {
            var context = new Context();
            var log = new SysLogModel(context: context);
            var json = SiteUtilities.MoveSiteMenu(context: context, id: id);
            log.Finish(context: context, responseSize: json.Length);
            return json;
        }

        [HttpPost]
        public string MoveTargetsSourceColumns(int id)
        {
            var context = new Context();
            var log = new SysLogModel(context: context);
            var json = SiteUtilities.MoveTargetsSourceColumnsJson(context: context, referenceId: id);
            log.Finish(context: context, responseSize: json.Length);
            return json;
        }

        [HttpPost]
        public string CreateLink(long id)
        {
            var context = new Context();
            var log = new SysLogModel(context: context);
            var json = SiteUtilities.CreateLink(context: context, id: id);
            log.Finish(context: context, responseSize: json.Length);
            return json;
        }

        [HttpPut]
        public string SortSiteMenu(long id)
        {
            var context = new Context();
            var log = new SysLogModel(context: context);
            var json = SiteUtilities.SortSiteMenu(context: context, siteId: id);
            log.Finish(context: context, responseSize: json.Length);
            return json;
        }

        [HttpPut]
        public string BulkMove(long id)
        {
            var context = new Context();
            var log = new SysLogModel(context: context);
            var json = new ItemModel(context: context, referenceId: id).BulkMove(context: context);
            log.Finish(context: context, responseSize: json.Length);
            return json;
        }

        [HttpDelete]
        public string Delete(long id)
        {
            var context = new Context();
            var log = new SysLogModel(context: context);
            var json = new ItemModel(context: context, referenceId: id).Delete(context: context);
            log.Finish(context: context, responseSize: json.Length);
            return json;
        }

        [HttpDelete]
        public string BulkDelete(long id)
        {
            var context = new Context();
            var log = new SysLogModel(context: context);
            var json = new ItemModel(context: context, referenceId: id).BulkDelete(context: context);
            log.Finish(context: context, responseSize: json.Length);
            return json;
        }

        [HttpDelete]
        public string DeleteComment(long id)
        {
            var context = new Context();
            var log = new SysLogModel(context: context);
            var json = new ItemModel(context: context, referenceId: id).DeleteComment(context: context);
            log.Finish(context: context, responseSize: json.Length);
            return json;
        }

        [HttpDelete]
        public string DeleteHistory(long id)
        {
            var context = new Context();
            var log = new SysLogModel(context: context);
            var json = new ItemModel(context: context, referenceId: id).DeleteHistory(context: context);
            log.Finish(context: context, responseSize: json.Length);
            return json;
        }

        [HttpDelete]
        public string PhysicalDelete(long id)
        {
            var context = new Context();
            var log = new SysLogModel(context: context);
            var json = new ItemModel(context: context, referenceId: id).PhysicalBulkDelete(context: context);
            log.Finish(context: context, responseSize: json.Length);
            return json;
        }

        [HttpPost]
        public string Restore(long id)
        {
            var context = new Context();
            var log = new SysLogModel(context: context);
            var json = new ItemModel(context: context, referenceId: id).Restore(context: context);
            log.Finish(context: context, responseSize: json.Length);
            return json;
        }

        [HttpPost]
        public string RestoreFromHistory(long id)
        {
            var context = new Context();
            var log = new SysLogModel(context: context);
            var json = new ItemModel(context: context, referenceId: id).RestoreFromHistory(context: context);
            log.Finish(context: context, responseSize: json.Length);
            return json;
        }

        [HttpPost]
        public string EditSeparateSettings(long id)
        {
            var context = new Context();
            var log = new SysLogModel(context: context);
            var json = new ItemModel(context: context, referenceId: id).EditSeparateSettings(context: context);
            log.Finish(context: context, responseSize: json.Length);
            return json;
        }

        [HttpPut]
        public string Separate(long id)
        {
            var context = new Context();
            var log = new SysLogModel(context: context);
            var json = new ItemModel(context: context, referenceId: id).Separate(context: context);
            log.Finish(context: context, responseSize: json.Length);
            return json;
        }

        [AcceptVerbs(HttpVerbs.Put, HttpVerbs.Post, HttpVerbs.Delete)]
        public string SetSiteSettings(long id)
        {
            var context = new Context();
            var log = new SysLogModel(context: context);
            var json = SiteUtilities.SetSiteSettings(
                context: context,
                siteId: id);
            log.Finish(context: context, responseSize: json.Length);
            return json;
        }

        [HttpPost]
        public string OpenImportSitePackageDialog(long id)
        {
            var context = new Context();
            var log = new SysLogModel(context: context);
            var json = new ItemModel(
                context: context,
                referenceId: id)
                    .OpenImportSitePackageDialog(context: context);
            log.Finish(context: context, responseSize: json.Length);
            return json;
        }

        [HttpPost]
        public string ImportSitePackage(long id, ICollection<IFormFile> file)
        {
            var context = new Context(files: file);
            var log = new SysLogModel(context: context);
            var json = new ItemModel(
                context: context,
                referenceId: id)
                    .ImportSitePackage(context: context);
            log.Finish(
                context: context,
                responseSize: json.Length);
            return json;
        }

        [HttpPost]
        public string OpenExportSitePackageDialog(long id)
        {
            var context = new Context();
            var log = new SysLogModel(context: context);
            var json = new ItemModel(
                context: context,
                referenceId: id)
                    .OpenExportSitePackageDialog(context: context);
            log.Finish(
                context: context,
                responseSize: json.Length);
            return json;
        }

        [HttpGet]
        public FileContentResult ExportSitePackage(long id)
        {
            var context = new Context();
            var log = new SysLogModel(context: context);
            var responseFile = new ItemModel(
                context: context,
                referenceId: id)
                    .ExportSitePackage(context: context);
            if (responseFile != null)
            {
                log.Finish(
                    context: context,
                    responseSize: responseFile.Length);
                return responseFile.ToFile();
            }
            else
            {
                log.Finish(context: context);
                return null;
            }
        }

        [HttpPost]
        public string RebuildSearchIndexes(long id)
        {
            var context = new Context();
            var log = new SysLogModel(context: context);
            var json = Indexes.RebuildSearchIndexes(
                context: context,
                siteModel: new SiteModel(
                    context: context,
                    siteId: id));
            log.Finish(
                context: context,
                responseSize: json.Length);
            return json;
        }

        [HttpPost]
        public string Histories(long id)
        {
            var context = new Context();
            var log = new SysLogModel(context: context);
            var json = new ItemModel(context: context, referenceId: id).Histories(context: context);
            log.Finish(context: context, responseSize: json.Length);
            return json;
        }

        [HttpPost]
        public string History(long id)
        {
            var context = new Context();
            var log = new SysLogModel(context: context);
            var json = new ItemModel(context: context, referenceId: id).History(context: context);
            log.Finish(context: context, responseSize: json.Length);
            return json;
        }

        [HttpPost]
        public string Permissions(long id)
        {
            var context = new Context();
            var log = new SysLogModel(context: context);
            var json = PermissionUtilities.Permission(context: context, referenceId: id);
            log.Finish(context: context, responseSize: json.Length);
            return json;
        }

        [HttpPost]
        public string SearchPermissionElements(long id)
        {
            var context = new Context();
            var log = new SysLogModel(context: context);
            var json = PermissionUtilities.SearchPermissionElements(
                context: context, referenceId: id);
            log.Finish(context: context, responseSize: json.Length);
            return json;
        }

        [AcceptVerbs(HttpVerbs.Post, HttpVerbs.Delete)]
        public string SetPermissions(long id)
        {
            var context = new Context();
            var log = new SysLogModel(context: context);
            var json = PermissionUtilities.SetPermissions(context: context, referenceId: id);
            log.Finish(context: context, responseSize: json.Length);
            return json;
        }

        [HttpPost]
        public string OpenPermissionsDialog(long id)
        {
            var context = new Context();
            var log = new SysLogModel(context: context);
            var json = PermissionUtilities.OpenPermissionsDialog(
                context: context, referenceId: id);
            log.Finish(context: context, responseSize: json.Length);
            return json;
        }

        [HttpPost]
        public string PermissionForRecord(long id)
        {
            var context = new Context();
            var log = new SysLogModel(context: context);
            var json = PermissionUtilities.PermissionForRecord(
                context: context, referenceId: id);
            log.Finish(context: context, responseSize: json.Length);
            return json;
        }

        [AcceptVerbs(HttpVerbs.Post, HttpVerbs.Delete)]
        public string SetPermissionForCreating(long id)
        {
            var context = new Context();
            var log = new SysLogModel(context: context);
            var json = PermissionUtilities.SetPermissionForCreating(
                context: context, referenceId: id);
            log.Finish(context: context, responseSize: json.Length);
            return json;
        }

        [HttpPost]
        public string OpenPermissionForCreatingDialog(long id)
        {
            var context = new Context();
            var log = new SysLogModel(context: context);
            var json = PermissionUtilities.OpenPermissionForCreatingDialog(
                context: context, referenceId: id);
            log.Finish(context: context, responseSize: json.Length);
            return json;
        }

        [AcceptVerbs(HttpVerbs.Post, HttpVerbs.Delete)]
        public string SetPermissionForUpdating(long id)
        {
            var context = new Context();
            var log = new SysLogModel(context: context);
            var json = PermissionUtilities.SetPermissionForUpdating(
                context: context, referenceId: id);
            log.Finish(context: context, responseSize: json.Length);
            return json;
        }

        [HttpPost]
        public string OpenPermissionForUpdatingDialog(long id)
        {
            var context = new Context();
            var log = new SysLogModel(context: context);
            var json = PermissionUtilities.OpenPermissionForUpdatingDialog(
                context: context, referenceId: id);
            log.Finish(context: context, responseSize: json.Length);
            return json;
        }

        [HttpPost]
        public string ColumnAccessControl(long id)
        {
            var context = new Context();
            var log = new SysLogModel(context: context);
            var json = PermissionUtilities.ColumnAccessControl(context: context, referenceId: id);
            log.Finish(context: context, responseSize: json.Length);
            return json;
        }

        [HttpPost]
        public string SetColumnAccessControl(long id)
        {
            var context = new Context();
            var log = new SysLogModel(context: context);
            var json = PermissionUtilities.SetColumnAccessControl(
                context: context, referenceId: id);
            log.Finish(context: context, responseSize: json.Length);
            return json;
        }

        [HttpPost]
        public string OpenColumnAccessControlDialog(long id)
        {
            var context = new Context();
            var log = new SysLogModel(context: context);
            var json = PermissionUtilities.OpenColumnAccessControlDialog(
                context: context, referenceId: id);
            log.Finish(context: context, responseSize: json.Length);
            return json;
        }

        [HttpPost]
        public string SearchColumnAccessControl(long id)
        {
            var context = new Context();
            var log = new SysLogModel(context: context);
            var json = PermissionUtilities.SearchColumnAccessControl(
                context: context, referenceId: id);
            log.Finish(context: context, responseSize: json.Length);
            return json;
        }

        [HttpPost]
        public string BurnDownRecordDetails(long id)
        {
            var context = new Context();
            var log = new SysLogModel(context: context);
            var json = new ItemModel(context: context, referenceId: id).BurnDownRecordDetailsJson(context: context);
            log.Finish(context: context, responseSize: json.Length);
            return json;
        }

        public string UpdateByCalendar(long id)
        {
            var context = new Context();
            var log = new SysLogModel(context: context);
            var json = new ItemModel(context: context, referenceId: id).UpdateByCalendar(context: context);
            log.Finish(context: context, responseSize: json.Length);
            return json;
        }

        public string UpdateByKamban(long id)
        {
            var context = new Context();
            var log = new SysLogModel(context: context);
            var json = new ItemModel(context: context, referenceId: id).UpdateByKamban(context: context);
            log.Finish(context: context, responseSize: json.Length);
            return json;
        }

        [HttpPut]
        public string SynchronizeTitles(long id)
        {
            var context = new Context();
            var log = new SysLogModel(context: context);
            var json = new ItemModel(context: context, referenceId: id).SynchronizeTitles(context: context);
            log.Finish(context: context, responseSize: json.Length);
            return json;
        }

        [HttpPut]
        public string SynchronizeSummaries(long id)
        {
            var context = new Context();
            var log = new SysLogModel(context: context);
            var json = new ItemModel(context: context, referenceId: id).SynchronizeSummaries(context: context);
            log.Finish(context: context, responseSize: json.Length);
            return json;
        }

        [HttpPut]
        public string SynchronizeFormulas(long id)
        {
            var context = new Context();
            var log = new SysLogModel(context: context);
            var json = new ItemModel(context: context, referenceId: id).SynchronizeFormulas(context: context);
            log.Finish(context: context, responseSize: json.Length);
            return json;
        }

        [HttpPost]
        public string LockTable(long id)
        {
            var context = new Context();
            var log = new SysLogModel(context: context);
            var json = new ItemModel(
                context: context,
                referenceId: id)
                    .LockTable(context: context);
            log.Finish(
                context: context,
                responseSize: json.Length);
            return json;
        }

        [HttpPost]
        public string UnlockTable(long id)
        {
            var context = new Context();
            var log = new SysLogModel(context: context);
            var json = new ItemModel(
                context: context,
                referenceId: id)
                    .UnlockTable(context: context);
            log.Finish(
                context: context,
                responseSize: json.Length);
            return json;
        }

        [HttpPost]
        public string ForceUnlockTable(long id)
        {
            var context = new Context();
            var log = new SysLogModel(context: context);
            var json = new ItemModel(
                context: context,
                referenceId: id)
                    .ForceUnlockTable(context: context);
            log.Finish(
                context: context,
                responseSize: json.Length);
            return json;
        }

        [HttpPost]
        public string UnlockRecord(long id)
        {
            var context = new Context();
            var log = new SysLogModel(context: context);
            var json = new ItemModel(
                context: context,
                referenceId: id)
                    .UnlockRecord(
                        context: context,
                        id: id);
            log.Finish(
                context: context,
                responseSize: json.Length);
            return json;
        }

        [HttpPost]
        public string ImportUserTemplate(long id, ICollection<IFormFile> file)
        {
            var context = new Context(files: file);
            var log = new SysLogModel(context: context);
            var json = new ItemModel(
                context: context,
                referenceId: id)
                    .ImportUserTemplate(context: context);
            log.Finish(
                context: context,
                responseSize: json.Length);
            return json;
        }

        [HttpPost]
        public string DeleteUserTemplate(long id)
        {
            var context = new Context();
            var log = new SysLogModel(context: context);
            var json = new ItemModel(
                context: context,
                referenceId: id)
                    .DeleteUserTemplate(context: context);
            log.Finish(
                context: context,
                responseSize: json.Length);
            return json;
        }

        [HttpPost]
        public string UpdateUserTemplate(long id)
        {
            var context = new Context();
            var log = new SysLogModel(context: context);
            var json = new ItemModel(
                context: context,
                referenceId: id)
                    .UpdateUserTemplate(context: context);
            log.Finish(
                context: context,
                responseSize: json.Length);
            return json;
        }

        [HttpPost]
        public string SearchUserTemplate(long id)
        {
            var context = new Context();
            var log = new SysLogModel(context: context);
            var json = new ItemModel(
                context: context,
                referenceId: id)
                    .SearchUserTemplate(context: context);
            log.Finish(
                context: context,
                responseSize: json.Length);
            return json;
        }

        [HttpPost]
        public string OpenEditUserTemplateDialog(long id)
        {
            var context = new Context();
            var log = new SysLogModel(context: context);
            var json = new ItemModel(
                context: context,
                referenceId: id)
                    .OpenEditUserTemplateDialog(context: context);
            log.Finish(
                context: context,
                responseSize: json.Length);
            return json;
        }
    }
}
