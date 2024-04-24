namespace Org.Ktu.Isk.P175B602.Autonuoma.Models;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;


/// <summary>
/// Model of 'atliktas_darbas' entity.
/// </summary>
public class AtliktasDarbas
{
	public int Id { get; set; }

	public string Pavadinimas { get; set; }

	public int Kaina { get; set; }
		
	public int Nuvaziuota { get; set; }

    public int FkSutartis { get; set; }
}