namespace Org.Ktu.Isk.P175B602.Autonuoma.Models.Modelis;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;


/// <summary>
/// Model of 'Modelis' entity.
/// </summary>
public class Modelis
{
	[DisplayName("ID")]
	public int Id { get; set; }

	[DisplayName("Pavadinimas")]
	public string Pavadinimas { get; set; }

	//Markė
	[DisplayName("FkGamintojas")]
	public int FkGamintojas { get; set; }
}


/// <summary>
/// Model of 'Modelis' entity used in lists.
/// </summary>
public class ModelisL
{
	[DisplayName("ID")]
	public int Id { get; set; }

	[DisplayName("Pavadinimas")]
	public string Pavadinimas { get; set; }		

	[DisplayName("Gamintojas")]
	public string Gamintojas { get; set; }
}


/// <summary>
/// Model of 'Modelis' entity used in creation and editing forms.
/// </summary>
public class ModelisCE
{
	/// <summary>
	/// Entity data
	/// </summary>
	public class ModelM
	{
		[DisplayName("ID")]
		public int Id { get; set; }

		[DisplayName("Pavadinimas")]
		[MaxLength(20)]
		[Required]
		public string Pavadinimas { get; set; }

		[DisplayName("Gamintojas")]
		[Required]
		public int FkGamintojas { get; set; }
	}

	/// <summary>
	/// Select lists for making drop downs for choosing values of entity fields.
	/// </summary>
	public class ListsM
	{
		public IList<SelectListItem> Gamintojai { get; set; }
	}

	/// <summary>
	/// Entity view.
	/// </summary>
	public ModelM Model { get; set; } = new ModelM();

	/// <summary>
	/// Lists for drop down controls.
	/// </summary>
	public ListsM Lists { get; set; } = new ListsM();
}

