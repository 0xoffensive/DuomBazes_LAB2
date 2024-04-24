namespace Org.Ktu.Isk.P175B602.Autonuoma.Repositories;

using MySql.Data.MySqlClient;

using Org.Ktu.Isk.P175B602.Autonuoma.Models;



/// <summary>
/// Database operations related to 'Aikstele' entity.
/// </summary>
public class ServisasRepo
{
	public static List<Servisas> List()
	{
		var query = $@"SELECT * FROM `servisas` ORDER BY ID_ ASC";
		var drc = Sql.Query(query);

		var result = 
			Sql.MapAll<Servisas>(drc, (dre, t) => {
				t.Id = dre.From<int>("ID_");
				t.Pavadinimas = dre.From<string>("pavadinimas");
				t.Telefonas = dre.From<string>("telefonas");
                t.Epastas = dre.From<string>("epastas");
                t.Miestas = dre.From<string>("miestas");
                t.Gatve = dre.From<string>("gatve");
				t.PastatoNr = dre.From<string>("pastato_nr");
                t.PastoKodas = dre.From<string>("pasto_kodas");
			});

		return result;
	}
}