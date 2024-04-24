namespace Org.Ktu.Isk.P175B602.Autonuoma.Models;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;


/// <summary>
/// Model of 'Servisas' entity.
/// </summary>
public class Servisas
{
	public int Id { get; set; }

	public string Pavadinimas { get; set; }

    public string Telefonas { get; set; }

	public string Epastas { get; set; }

    public string Miestas { get; set; }
	
	public string Gatve { get; set; }

    public string PastatoNr { get; set; }

    public string PastoKodas { get; set; }
}