namespace Org.Ktu.Isk.P175B602.Autonuoma.Models.SutartisF2;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;


/// <summary>
/// 'Sutartis' in list form.
/// </summary>
public class SutartisL
{
	[DisplayName("Nr.")]
	public int Nr { get; set; }


	[DisplayName("Priemimo data")]
	[DataType(DataType.Date)]
	[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
	public DateTime Data { get; set; }


	[DisplayName("Darbuotojas")]
	public string Darbuotojas { get; set; }


	[DisplayName("Klientas")]
	public string Klientas { get; set; }


	[DisplayName("Būsena")]
	public string Busena { get; set; }
}


/// <summary>
/// 'Sutartis' in create and edit forms.
/// </summary>
public class SutartisCE
{
	/// <summary>
	/// Entity data.
	/// </summary>
	public class SutartisM
	{
		[DisplayName("Nr.")]
		public int Nr { get; set; }

		[DisplayName("Priemimo data ir laikas")]
		[DataType(DataType.DateTime)]
		[Required(ErrorMessage = "Laukas privalomas.")]
		public DateTime PriemimoData { get; set; }

		[DisplayName("Planuojama suremontavimo data")]
		[DataType(DataType.DateTime)]
		public DateTime? PlanuojamaSuremontuotiData { get; set; }

		[DisplayName("Rida paimant")]
		[Required(ErrorMessage = "Laukas privalomas.")]
		public int PradineRida { get; set; }

		[DisplayName("Rida grąžinus")]
		public int GalineRida { get; set; }

		[DisplayName("Remonto kaina [€]")]
		public decimal? Kaina { get; set; }

		[DisplayName("Sutarties būsena")]
		public string Busena { get; set; }

		[DisplayName("Klientas")]
		[Required(ErrorMessage = "Laukas privalomas.")]
		public string FkKlientas { get; set; }

		[DisplayName("Darbuotojas")]
		[Required(ErrorMessage = "Laukas privalomas.")]
		public int FkDarbuotojas { get; set; }

		[DisplayName("Automobilis")]
		[Required(ErrorMessage = "Laukas privalomas.")]
		public string FkAutomobilis { get; set; }
	}

	public class PrirasytasMech
	{
		public int InListId { get; set; }

		[DisplayName("Mechanikas")]
		[Required(ErrorMessage = "Laukas privalomas.")]
		public int? Mechanikas { get; set; }
	}

	public class Gedimas
	{
		public int InListId { get; set; }

		[DisplayName("Gedimas")]
		[Required(ErrorMessage = "Laukas privalomas.")]
		public int? GedimoId { get; set; }
	}

	public class AtlDarb
	{
		public int InListId { get; set; }

		[DisplayName("Darbo pav.")]
		[Required(ErrorMessage = "Laukas privalomas.")]
		public string DarboPav { get; set; }

		[DisplayName("Nuvažiuota")]
		[Required(ErrorMessage = "Laukas privalomas.")]
		public int? Nuvaziuota { get; set; }

		[DisplayName("Kaina")]
		[Required(ErrorMessage = "Laukas privalomas.")]
		public int? DarbKaina { get; set; }

		[DisplayName("Detalė")]
		[Required(ErrorMessage = "Laukas privalomas.")]
		public int? DetaleId { get; set; }

		[DisplayName("Detalės kiekis")]
		[Required(ErrorMessage = "Laukas privalomas.")]
		[Range(1, 999, ErrorMessage = "Netinkamas kiekis (1 - 999).")]
		public int DetaleKiekis { get; set; }
	}

	/// <summary>
	/// Select lists for making drop downs for choosing values of entity fields.
	/// </summary>
	public class ListsM
	{
		public IList<SelectListItem> Busenos { get; set; }
		public IList<SelectListItem> Klientai { get; set; }
		public IList<SelectListItem> Darbuotojai { get; set; }
		public IList<SelectListItem> Mechanikai { get; set; }
		public IList<SelectListItem> Gedimai { get; set; }
		public IList<SelectListItem> Detales { get; set; }
		public IList<SelectListItem> Automobiliai { get; set; }
	}


	/// <summary>
	/// Sutartis.
	/// </summary>
	public SutartisM Sutartis { get; set; } = new SutartisM();

	/// <summary>
	/// Related 'UzsakytaPaslauga' records.
	/// </summary>
	public IList<PrirasytasMech> PriklausantysMechanikai { get; set; } = new List<PrirasytasMech>();
	public IList<Gedimas> Gedimai { get; set; } = new List<Gedimas>();
	public IList<AtlDarb> AtliktiDarbai { get; set; } = new List<AtlDarb>();

	/// <summary>
	/// Lists for drop down controls.
	/// </summary>
	public ListsM Lists { get; set; } = new ListsM();
}