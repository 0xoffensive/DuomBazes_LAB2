namespace Org.Ktu.Isk.P175B602.Autonuoma.Repositories;

using MySql.Data.MySqlClient;

using Org.Ktu.Isk.P175B602.Autonuoma.Models;



/// <summary>
/// Database operations related to 'Detale' entity.
/// </summary>
public class DetaleRepo
{
	public static List<Detale> List()
	{
		var query = $@"SELECT * FROM `detale` ORDER BY ID_ ASC";
		var drc = Sql.Query(query);

		var result = 
			Sql.MapAll<Detale>(drc, (dre, t) => {
				t.Id = dre.From<int>("ID_");
				t.Pavadinimas = dre.From<string>("pavadinimas");
				t.Kaina = dre.From<decimal>("detales_kaina");
				t.Bukle = dre.From<string>("detales_bukle");
			});

		return result;
	}
}