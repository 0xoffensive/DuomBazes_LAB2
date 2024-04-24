namespace Org.Ktu.Isk.P175B602.Autonuoma.Models;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;


/// <summary>
/// 'Darbuotojas' and 'Mechanikas' in list form.
/// </summary>
public class DarbuotojasL
{
	[DisplayName("Tabelio Nr.")]
	public int Tabelis { get; set; }

	[DisplayName("Vardas")]
	[MaxLength(20)]
	[Required]
	public string Vardas { get; set; }

	[DisplayName("Pavardė")]
	[MaxLength(20)]
	[Required]
	public string Pavarde { get; set; }

	[DisplayName("Telefonas")]
	[Required]
	public string Telefonas { get; set; }

	[DisplayName("Servisas")]
	public string Servisas { get; set; }
}

/// <summary>
/// Model of 'Darbuotojas' and 'Mechanikas' entity.
/// </summary>
public class DarbuotojasCE
{
	public class ModelM
	{
		[DisplayName("Tabelio Nr.")]
		public int Tabelis { get; set; }

		[DisplayName("Vardas")]
		[MaxLength(20)]
		[Required]
		public string Vardas { get; set; }

		[DisplayName("Pavardė")]
		[MaxLength(20)]
		[Required]
		public string Pavarde { get; set; }

		[DisplayName("Telefonas")]
		[Required]
		public string Telefonas { get; set; }

		[DisplayName("Servisas")]
		[Required]
		public int FkServisas { get; set; }
	}

	public class ListsM
	{
		public IList<SelectListItem> Servisai { get; set; }
	}

	public ModelM Model { get; set; } = new ModelM();

	public ListsM Lists { get; set; } = new ListsM();
}