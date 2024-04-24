namespace Org.Ktu.Isk.P175B602.Autonuoma.Repositories;

using MySql.Data.MySqlClient;

using Org.Ktu.Isk.P175B602.Autonuoma.Models;


public class GamintojasRepo
{
	public static List<Gamintojas> List()
	{
		string query = $@"SELECT * FROM `gamintojas` ORDER BY ID_ ASC";
		var drc = Sql.Query(query);

		var result = 
			Sql.MapAll<Gamintojas>(drc, (dre, t) => {
				t.Id = dre.From<int>("ID_");
				t.Pavadinimas = dre.From<string>("pavadinimas");
			});

		return result;
	}

	public static Gamintojas Find(int id)
	{
		var query = $@"SELECT * FROM `gamintojas` WHERE ID_=?id";
		var drc = 
			Sql.Query(query, args => {
				args.Add("?id", id);
			});

		var result = 
			Sql.MapOne<Gamintojas>(drc, (dre, t) => {
				t.Id = dre.From<int>("ID_");
				t.Pavadinimas = dre.From<string>("pavadinimas");
			});

		return result;
	}

	public static void Update(Gamintojas gamintojas)
	{			
		var query = 
			$@"UPDATE `gamintojas` 
			SET 
				pavadinimas=?pavadinimas 
			WHERE 
				ID_=?id";

		Sql.Update(query, args => {
			args.Add("?pavadinimas", gamintojas.Pavadinimas);
			args.Add("?id", gamintojas.Id);
		});							
	}

	public static void Insert(Gamintojas gamintojas)
	{			
		var query = $@"INSERT INTO `gamintojas` ( pavadinimas ) VALUES ( ?pavadinimas )";
		Sql.Insert(query, args => {
			args.Add("?pavadinimas", gamintojas.Pavadinimas);
		});
	}

	public static void Delete(int id)
	{			
		var query = $@"DELETE FROM `gamintojas` where ID_=?id";
		Sql.Delete(query, args => {
			args.Add("?id", id);
		});			
	}
}
