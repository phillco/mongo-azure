﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MongoDB.WindowsAzure.Tools;
using MongoDB.WindowsAzure.Manager.Models;

namespace MongoDB.WindowsAzure.Manager.Controllers
{
    public class BackupController : Controller
    {
        //=========================================================================
        //
        //  REGULAR ACTIONS
        //
        //=========================================================================

        /// <summary>
        /// Cretes a new snapshot based on the current primary.
        /// </summary>
        /// <returns></returns>
        public ActionResult NewSnapshot()
        {
            var uri = SnapshotManager.MakeSnapshot(ServerStatus.Primary.Id);

            TempData["flashSuccess"] = "Snapshot created: " + uri;
            return RedirectToAction("Index", "Dashboard");
        }

        //=========================================================================
        //
        //  AJAX ACTIONS
        //
        //=========================================================================

        /// <summary>
        /// Returns a list of all snapshots, including their URLs, blob names, and dates.
        /// </summary>
        /// <returns></returns>
        public JsonResult GetSnapshots()
        {
            var snapshots = SnapshotManager.GetSnapshots("DefaultEndpointsProtocol=http;AccountName=managerstorage3;AccountKey=OMtTMtI5AtLLK8fBrDAUxJBqo9js+4jcd10SmKV2hiZwsUfPJVu5neaAM3OV2d5hgWZeZyaiqM6SP03pzvI7hw==");

            var pairs = snapshots.Select(blob => new { dateString = ToString(blob.Attributes.Snapshot), blob = blob.Name, uri = SnapshotManager.ToSnapshotUri(blob) });
            return Json(new { snapshots = pairs }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Turns the date into a string presentation.
        /// </summary>
        private string ToString(DateTime? snapshotTime)
        {
            return snapshotTime.Value.ToShortDateString() + " " + snapshotTime.Value.ToShortTimeString();
        }
    }
}
