namespace Org.Ktu.Isk.P175B602.Autonuoma.Repositories;

using MySql.Data.MySqlClient;

using Org.Ktu.Isk.P175B602.Autonuoma.Models.Modelis;


public class ModelisRepo
{
	public static List<ModelisL> List()
	{
		var query =
			$@"SELECT
				md.ID_,
				md.modelio_pavadinimas,
				gam.pavadinimas AS gamintojas
			FROM
				`modelis` md
				LEFT JOIN `gamintojas` gam ON gam.ID_=md.fk_GAMINTOJASID_
			ORDER BY gam.pavadinimas ASC, md.ID_ ASC";

		var drc = Sql.Query(query);

		var result = 
			Sql.MapAll<ModelisL>(drc, (dre, t) => {
				t.Id = dre.From<int>("ID_");
				t.Pavadinimas = dre.From<string>("modelio_pavadinimas");
				t.Gamintojas = dre.From<string>("gamintojas");
			});

		return result;
	}

	public static List<Modelis> ListForGamintojas(int gamintojasId)
	{
		var query = $@"SELECT * FROM `modelis` WHERE fk_GAMINTOJASID_=?gamintojasId ORDER BY ID_ ASC";
		var drc =
			Sql.Query(query, args => {
				args.Add("?gamintojasId", gamintojasId);
			});

		var result = 
			Sql.MapAll<Modelis>(drc, (dre, t) => {
				t.Id = dre.From<int>("ID_");
				t.Pavadinimas = dre.From<string>("modelio_pavadinimas");
				t.FkGamintojas = dre.From<int>("fk_GAMINTOJASID_");
			});

		return result;
	}

	public static ModelisCE Find(int id)
	{
		var query = $@"SELECT * FROM `{Config.TblPrefix}modelis` WHERE ID_=?id";
		var drc =
			Sql.Query(query, args => {
				args.Add("?id", id);
			});

		var result = 
			Sql.MapOne<ModelisCE>(drc, (dre, t) => {
				t.Model.Id = dre.From<int>("ID_");
				t.Model.Pavadinimas = dre.From<string>("modelio_pavadinimas");
				t.Model.FkGamintojas = dre.From<int>("fk_GAMINTOJASID_");
			});

		return result;
	}

	public static ModelisL FindForDeletion(int id)
	{
		var query =
			$@"SELECT
				md.ID_,
				md.modelio_pavadinimas,
				gam.pavadinimas AS gamintojas
			FROM
				`modelis` md
				LEFT JOIN `gamintojas` gam ON gam.ID_=md.fk_GAMINTOJASID_
			WHERE
				md.ID_ = ?id";

		var drc =
			Sql.Query(query, args => {
				args.Add("?id", id);
			});

		var result = 
			Sql.MapOne<ModelisL>(drc, (dre, t) => {
				t.Id = dre.From<int>("ID_");
				t.Pavadinimas = dre.From<string>("modelio_pavadinimas");
				t.Gamintojas = dre.From<string>("Gamintojas");
			});

		return result;
	}

	public static void Update(ModelisCE modelisEvm)
	{
		var query =
			$@"UPDATE `modelis`
			SET
				modelio_pavadinimas=?pavadinimas,
				fk_GAMINTOJASID_=?gamintojas
			WHERE
				ID_=?ID_";

		Sql.Update(query, args => {
			args.Add("?pavadinimas", modelisEvm.Model.Pavadinimas);
			args.Add("?gamintojas", modelisEvm.Model.FkGamintojas);
			args.Add("?ID_", modelisEvm.Model.Id);
		});
	}

	public static void Insert(ModelisCE modelisEvm)
	{
		var query =
			$@"INSERT INTO `modelis`
			(
				modelio_pavadinimas,
				fk_GAMINTOJASID_
			)
			VALUES(
				?pavadinimas,
				?gamintojas
			)";

		Sql.Insert(query, args => {
			args.Add("?pavadinimas", modelisEvm.Model.Pavadinimas);
			args.Add("?gamintojas", modelisEvm.Model.FkGamintojas);
		});
	}

	public static void Delete(int id)
	{
		var query = $@"DELETE FROM `{Config.TblPrefix}modelis` WHERE ID_=?id";
		Sql.Delete(query, args => {
			args.Add("?id", id);
		});
	}
}
