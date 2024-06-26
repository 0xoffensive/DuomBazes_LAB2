﻿namespace Org.Ktu.Isk.P175B602.Autonuoma.Models.ContractsReport;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;


/// <summary>
/// View model for single contract in a report.
/// </summary>
public class Sutartis
{
	[DisplayName("Sutartis")]
	public int Nr { get; set; }

	[DisplayName("Data")]
	[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
	public DateTime SutartiesData { get; set; }

	public string Vardas { get; set; }

	public string Pavarde { get; set; }

	public string AsmensKodas { get; set; }

	[DisplayName("Sutarties būsena")]
	public string Busena { get; set; }

	[DisplayName("Sutarčių vertė")]
	public decimal Kaina { get; set; }

	[DisplayName("Detalių kainos")]
	public decimal DetaliuKaina { get; set; }

	public decimal BendraSuma { get; set; }

	public decimal BendraSumaDetaliu { get; set; }
}

/// <summary>
/// View model for whole report.
/// </summary>
public class Report
{
	[DataType(DataType.DateTime)]
	[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
	public DateTime? DateFrom { get; set; }

	[DataType(DataType.DateTime)]
	[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
	public DateTime? DateTo { get; set; }

	public string? klientasid { get; set; }

	public List<Sutartis> Sutartys { get; set; }

	public decimal VisoSumaSutartciu { get; set; }

	public decimal VisoSumaDetaliu { get; set; }

	public IList<SelectListItem> Klientai { get; set; }

}
