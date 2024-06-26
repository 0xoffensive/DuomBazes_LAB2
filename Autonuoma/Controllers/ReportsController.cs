﻿namespace Org.Ktu.Isk.P175B602.Autonuoma.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using Org.Ktu.Isk.P175B602.Autonuoma.Repositories;

using LateContractsReport = Org.Ktu.Isk.P175B602.Autonuoma.Models.LateContractsReport;
using ContractsReport = Org.Ktu.Isk.P175B602.Autonuoma.Models.ContractsReport;
using ServicesReport = Org.Ktu.Isk.P175B602.Autonuoma.Models.ServicesReport;


/// <summary>
/// Controller for producing reports.
/// </summary>
public class ReportsController : Controller
{
	/// <summary>
	/// Produces contracts report.
	/// </summary>
	/// <param name="dateFrom">Starting date. Can be null.</param>
	/// <param name="dateTo">Ending date. Can be null.</param>
	/// <returns>Report view.</returns>
	[HttpGet]
	public ActionResult Contracts(DateTime? dateFrom, DateTime? dateTo, string? klientasid)
	{
		var report = new ContractsReport.Report();
		report.DateFrom = dateFrom;
		report.DateTo = dateTo?.AddHours(23).AddMinutes(59).AddSeconds(59); //move time of end date to end of day
		report.klientasid = klientasid;

		report.Sutartys = AtaskaitaRepo.GetContracts(report.DateFrom, report.DateTo, report.klientasid);

		var klientai = KlientasRepo.List();

		report.Klientai =
			klientai
				.Select(it =>
				{
					return
						new SelectListItem
						{
							Value = it.AsmensKodas.ToString(),
							Text = $"{it.Vardas} {it.Pavarde}"
						};
				})
				.ToList();

		foreach (var item in report.Sutartys)
		{
			report.VisoSumaSutartciu += item.Kaina;
			report.VisoSumaDetaliu += item.DetaliuKaina;
		}

		return View(report);
	}

	/// <summary>
	/// Produces late car returns reports.
	/// </summary>
	/// <param name="dateFrom">Starting date. Can be null.</param>
	/// <param name="dateTo">Ending date. Can be null.</param>
	/// <returns>Report view.</returns>
	[HttpGet]
	public ActionResult LateContracts(DateTime? dateFrom, DateTime? dateTo)
	{
		var report = new LateContractsReport.Report();
		report.DateFrom = dateFrom;
		report.DateTo = dateTo?.AddHours(23).AddMinutes(59).AddSeconds(59); //move time of end date to end of day

		report.Sutartys = AtaskaitaRepo.GetLateReturnContracts(report.DateFrom, report.DateTo);

		return View(report);
	}

	/*private void PopulateLists(ContractsReport.Report rt)
	{

	}*/
}
