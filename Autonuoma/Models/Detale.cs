namespace Org.Ktu.Isk.P175B602.Autonuoma.Models;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;


/// <summary>
/// Model of 'Darbas' entity.
/// </summary>
public class Detale
{
	public int Id { get; set; }

	public string Pavadinimas { get; set; }

	public decimal Kaina { get; set; }
		
	public string Bukle { get; set; }
}