namespace Org.Ktu.Isk.P175B602.Autonuoma.Repositories;

using MySql.Data.MySqlClient;

using Org.Ktu.Isk.P175B602.Autonuoma.Models;



/// <summary>
/// Database operations related to 'Aikstele' entity.
/// </summary>
public class MechanikasRepo
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
				mechanikas d

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
}