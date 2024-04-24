namespace Org.Ktu.Isk.P175B602.Autonuoma.Repositories;

using MySql.Data.MySqlClient;

using Org.Ktu.Isk.P175B602.Autonuoma.Models;



/// <summary>
/// Database operations related to 'Aikstele' entity.
/// </summary>
public class GedimasRepo
{
	public static List<Gedimas> List()
	{
		var query = $@"SELECT * FROM `gedimas` ORDER BY ID_ ASC";
		var drc = Sql.Query(query);

		var result = 
			Sql.MapAll<Gedimas>(drc, (dre, t) => {
				t.Id = dre.From<int>("ID_");
				t.Pavadinimas = dre.From<string>("pavadinimas");
				t.EnumTipas = dre.From<string>("tipas");
			});

		return result;
	}
}