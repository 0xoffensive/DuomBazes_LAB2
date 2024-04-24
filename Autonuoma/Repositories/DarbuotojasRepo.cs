namespace Org.Ktu.Isk.P175B602.Autonuoma.Repositories;

using MySql.Data.MySqlClient;

using Org.Ktu.Isk.P175B602.Autonuoma.Models;


/// <summary>
/// Database operations related to 'Darbuotojas' entity.
/// </summary>
public class DarbuotojasRepo
{
	public static List<DarbuotojasL> List()
	{
		var query =
			$@"SELECT
				d.tabelio_nr,
				d.vardas,
				d.pavarde,
				d.telefonas,

				s.pavadinimas as servisopav
			FROM
				darbuotojas d

				LEFT JOIN `servisas` s ON s.ID_ = d.fk_SERVISASID_
			ORDER BY d.tabelio_nr ASC";

		var drc = Sql.Query(query);

		var result = 
			Sql.MapAll<DarbuotojasL>(drc, (dre, t) => {
				t.Tabelis = dre.From<int>("tabelio_nr");
				t.Vardas = dre.From<string>("vardas");
				t.Pavarde = dre.From<string>("pavarde");
				t.Telefonas = dre.From<string>("telefonas");
				t.Servisas = dre.From<string>("servisopav");
			});

		return result;
	}

	public static DarbuotojasCE Find(int tabnr)
	{
		var query = $@"SELECT * FROM `darbuotojas` WHERE tabelio_nr=?tab";

		var drc = 
			Sql.Query(query, args => {
				args.Add("?tab", tabnr);
			});

		if( drc.Count > 0 )
		{
			var result = 
				Sql.MapOne<DarbuotojasCE>(drc, (dre, t) => {
					t.Model.Tabelis = dre.From<int>("tabelio_nr");
					t.Model.Vardas = dre.From<string>("vardas");
					t.Model.Pavarde = dre.From<string>("pavarde");
					t.Model.Telefonas = dre.From<string>("telefonas");
					t.Model.FkServisas = dre.From<int>("fk_SERVISASID_");
				});
			
			return result;
		}

		return null;
	}

	public static DarbuotojasL FindL(int tabnr)
	{
		var query =
			$@"SELECT
				d.tabelio_nr,
				d.vardas,
				d.pavarde,
				d.telefonas,

				s.pavadinimas as servisopav
			FROM
				darbuotojas d
				LEFT JOIN `servisas` s ON s.ID_ = d.fk_SERVISASID_
				WHERE
					d.tabelio_nr=?tab
			ORDER BY d.tabelio_nr ASC";

		var drc = 
			Sql.Query(query, args => {
				args.Add("?tab", tabnr);
			});

		if( drc.Count > 0 )
		{
			var result = 
				Sql.MapOne<DarbuotojasL>(drc, (dre, t) => {
					t.Tabelis = dre.From<int>("tabelio_nr");
					t.Vardas = dre.From<string>("vardas");
					t.Pavarde = dre.From<string>("pavarde");
					t.Telefonas = dre.From<string>("telefonas");
					t.Servisas = dre.From<string>("servisopav");
				});
			
			return result;
		}

		return null;
	}

	public static void Update(DarbuotojasCE darb)
	{						
		var query = 
			$@"UPDATE `darbuotojas`
			SET 
				vardas=?vardas, 
				pavarde=?pavarde,
				telefonas=?telefonas,
				fk_SERVISASID_=?servisas
			WHERE 
				tabelio_nr=?tab";

		Sql.Update(query, args => {
			args.Add("?vardas", darb.Model.Vardas);
			args.Add("?pavarde", darb.Model.Pavarde);
			args.Add("?telefonas", darb.Model.Telefonas);
			args.Add("?servisas", darb.Model.FkServisas);
			args.Add("?tab", darb.Model.Tabelis);
		});				
	}

	public static void Insert(DarbuotojasCE darb)
	{							
		var query = 
			$@"INSERT INTO `darbuotojas`
			(
				tabelio_nr,
				vardas,
				pavarde,
				telefonas,
				fk_SERVISASID_
			)
			VALUES(
				?tabelis,
				?vardas,
				?pavarde,
				?telefonas,
				?servisas
			)";

		Sql.Insert(query, args => {
			args.Add("?tabelis", darb.Model.Tabelis);
			args.Add("?vardas", darb.Model.Vardas);
			args.Add("?pavarde", darb.Model.Pavarde);
			args.Add("?telefonas", darb.Model.Telefonas);
			args.Add("?servisas" , darb.Model.FkServisas);
		});				
	}

	public static void Delete(int id)
	{			
		var query = $@"DELETE FROM `darbuotojas` WHERE tabelio_nr=?id";
		Sql.Delete(query, args => {
			args.Add("?id", id);
		});			
	}
}
