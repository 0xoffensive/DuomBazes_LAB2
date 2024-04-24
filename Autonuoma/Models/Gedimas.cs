namespace Org.Ktu.Isk.P175B602.Autonuoma.Models;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;


/// <summary>
/// Model for 'Gedimas' entity.
/// </summary>
public class Gedimas
{
	[DisplayName("ID")]
	public int Id { get; set; }

	[DisplayName("Pavadinimas")]
	[Required]
	public string Pavadinimas { get; set; }

	[Required]
	public string EnumTipas { get; set; }
}
