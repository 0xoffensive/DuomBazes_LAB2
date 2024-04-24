namespace Org.Ktu.Isk.P175B602.Autonuoma.Models.Automobilis;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;


/// <summary>
/// 'Automobilis' in list form.
/// </summary>
public class AutomobilisL
{
	[DisplayName("VIN")]
	public string Vin { get; set; }

	[DisplayName("Valstybinis Nr.")]
	public string ValstybinisNr { get; set; }

	[DisplayName("Modelis")]
	public string Modelis { get; set; }

	[DisplayName("Gamintojas")]
	public string Gamintojas { get; set; }
}

/// <summary>
/// 'Automobilis' in create and edit forms.
/// </summary>
public class AutomobilisCE
{
	/// <summary>
	/// Automobilis.
	/// </summary>
	public class AutomobilisM
	{
		[DisplayName("VIN")]
		[Required(ErrorMessage = "Without VIN!?!?!?!?")]
		public string Vin { get; set; }


		[DisplayName("Valstybinis Nr.")]
		[MaxLength(6)]
		[Required]
		public string ValstybinisNr { get; set; }

		[DisplayName("Registracijos metai")]
		[Range(1900, 2100, ErrorMessage = "Invalid registration year (1900-2100)")]
		public int PirmRegMetai { get; set; }

		[DisplayName("Modelis")]
		[Required]
		public int FkModelis { get; set; }

		[DisplayName("Pavarų dėžė")]
		[Required]
		public string PavaruDeze { get; set; }

		[DisplayName("Variklio pavadinimas")]

		public string Variklis { get; set; }

		[DisplayName("Galia [HP/kW]")]
		public string Galia { get; set; }

		[DisplayName("Degalų tipas")]
		[Required]
		public string DegaluTipas { get; set; }

		[DisplayName("Kėbulo tipas")]
		[Required]
		public string KebuloTipas { get; set; }

		[DisplayName("Spalva")]
		public string Spalva { get;set; }
	}

	/// <summary>
	/// Select lists for making drop downs for choosing values of entity fields.
	/// </summary>
	public class ListsM
	{
		public IList<SelectListItem> Modeliai { get; set; }
		public IList<SelectListItem> PavaruDezes { get; set; }
		public IList<SelectListItem> KebuloTipai { get; set; }
		public IList<SelectListItem> DegaluTipai { get; set; }

	}


	/// <summary>
	/// Automobilis.
	/// </summary>
	public AutomobilisM Automobilis { get ; set; } = new AutomobilisM();

	/// <summary>
	/// Lists for drop down controls.
	/// </summary>
	public ListsM Lists { get; set; } = new ListsM();
}
	