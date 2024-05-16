namespace Org.Ktu.Isk.P175B602.Autonuoma.Repositories;

using MySql.Data.MySqlClient;

using LateContractsReport = Org.Ktu.Isk.P175B602.Autonuoma.Models.LateContractsReport;
using ContractsReport = Org.Ktu.Isk.P175B602.Autonuoma.Models.ContractsReport;
using ServicesReport = Org.Ktu.Isk.P175B602.Autonuoma.Models.ServicesReport;


/// <summary>
/// Database operations related to reports.
/// </summary>
public class AtaskaitaRepo
{
	public static List<ServicesReport.Paslauga> GetServicesOrdered(DateTime? dateFrom, DateTime? dateTo)
	{
		var query =
			$@"SELECT
				pasl.id,
				pasl.pavadinimas,
				SUM(up.kiekis) AS kiekis,
				SUM(up.kiekis*up.kaina) AS suma
			FROM
				`paslaugos` pasl,
				`uzsakytos_paslaugos` up,
				`sutartys` sut
			WHERE
				pasl.id = up.fk_paslauga
				AND up.fk_sutartis = sut.nr
				AND sut.sutarties_data >= IFNULL(?nuo, sut.sutarties_data)
				AND sut.sutarties_data <= IFNULL(?iki, sut.sutarties_data)
			GROUP BY
				pasl.id, pasl.pavadinimas
			ORDER BY
				suma DESC";

		var drc =
			Sql.Query(query, args => {
				args.Add("?nuo", dateFrom);
				args.Add("?iki", dateTo);
			});

		var result = 
			Sql.MapAll<ServicesReport.Paslauga>(drc, (dre, t) => {
				t.Id = dre.From<int>("id");
				t.Pavadinimas = dre.From<string>("pavadinimas");
				t.Kiekis = dre.From<int>("kiekis");
				t.Suma = dre.From<decimal>("suma");
			});

		return result;
	}

	public static ServicesReport.Report GetTotalServicesOrdered(DateTime? dateFrom, DateTime? dateTo)
	{
		var query =
			$@"SELECT
				SUM(up.kiekis) AS kiekis,
				SUM(up.kiekis*up.kaina) AS suma
			FROM
				`{Config.TblPrefix}paslaugos` pasl,
				`{Config.TblPrefix}uzsakytos_paslaugos` up,
				`{Config.TblPrefix}sutartys` sut
			WHERE
				pasl.id = up.fk_paslauga
				AND up.fk_sutartis = sut.nr
				AND sut.sutarties_data >= IFNULL(?nuo, sut.sutarties_data)
				AND sut.sutarties_data <= IFNULL(?iki, sut.sutarties_data)";

		var drc =
			Sql.Query(query, args => {
				args.Add("?nuo", dateFrom);
				args.Add("?iki", dateTo);
			});

		var result = 
			Sql.MapOne<ServicesReport.Report>(drc, (dre, t) => {
				t.VisoUzsakyta = dre.From<int?>("kiekis") ?? 0;
				t.BendraSuma = dre.From<decimal?>("suma") ?? 0;
			});

		return result;
	}

	public static List<ContractsReport.Sutartis> GetContracts(DateTime? dateFrom, DateTime? dateTo)
	{
		var query =
			$@"SELECT
				sut.nr,
				sut.automobilio_priemimo_data,
				sut.remonto_busena,
				kln.vardas,
				kln.pavarde,
				kln.asmens_kodas,
				IFNULL(sut.remonto_kaina, 0) as kaina,
			    bs1.bendra_suma,
				bs3.detaliu_kaina,
				bs4.detales_bendra as bdk
			FROM
				`remonto_sutartis` sut
				INNER JOIN `klientas` kln ON kln.asmens_kodas = sut.fk_KLIENTASasmens_kodas
				INNER JOIN
					(
						SELECT
							kln1.asmens_kodas,
							sum(sut1.remonto_kaina) as bendra_suma
						FROM `remonto_sutartis` sut1, `klientas` kln1
						WHERE
							kln1.asmens_kodas=sut1.fk_KLIENTASasmens_kodas
							AND sut1.automobilio_priemimo_data >= IFNULL(?nuo, sut1.automobilio_priemimo_data)
							AND sut1.automobilio_priemimo_data <= IFNULL(?iki, sut1.automobilio_priemimo_data)
							GROUP BY kln1.asmens_kodas
					) AS bs1
					ON bs1.asmens_kodas = kln.asmens_kodas
				LEFT JOIN
					(
						SELECT
							sut3.nr,
							IFNULL(SUM(kiekis.bendra_kaina), 0) as detaliu_kaina
						FROM
							`remonto_sutartis` sut3
							LEFT JOIN `atliktas_darbas` atl ON atl.fk_REMONTO_SUTARTISnr = sut3.nr
							LEFT JOIN `detales_kiekis` kiekis ON kiekis.fk_ATLIKTAS_DARBASID_ = atl.ID_
						WHERE
							sut3.automobilio_priemimo_data >= IFNULL(?nuo, sut3.automobilio_priemimo_data)
							AND sut3.automobilio_priemimo_data <= IFNULL(?iki, sut3.automobilio_priemimo_data)
						GROUP BY sut3.nr
					) AS bs3
					ON bs3.nr = sut.nr
				LEFT JOIN
					(
						SELECT
							sut4.nr,
							sut4.fk_KLIENTASasmens_kodas,
							IFNULL(SUM(kiekis2.bendra_kaina), 0) as detales_bendra
						FROM
							`remonto_sutartis` sut4
							LEFT JOIN `atliktas_darbas` atl2 ON atl2.fk_REMONTO_SUTARTISnr = sut4.nr
							LEFT JOIN `detales_kiekis` kiekis2 ON kiekis2.fk_ATLIKTAS_DARBASID_ = atl2.ID_
						WHERE
							sut4.automobilio_priemimo_data >= IFNULL(?nuo, sut4.automobilio_priemimo_data)
							AND sut4.automobilio_priemimo_data <= IFNULL(?iki, sut4.automobilio_priemimo_data)
						GROUP BY sut4.fk_KLIENTASasmens_kodas
					) AS bs4
					ON bs4.fk_KLIENTASasmens_kodas = kln.asmens_kodas
			WHERE
				sut.automobilio_priemimo_data >= IFNULL(?nuo, sut.automobilio_priemimo_data)
				AND sut.automobilio_priemimo_data <= IFNULL(?iki, sut.automobilio_priemimo_data)
			GROUP BY 
				sut.nr, kln.asmens_kodas
			ORDER BY 
				kln.pavarde ASC, sut.nr ASC";

		var drc =
			Sql.Query(query, args => {
				args.Add("?nuo", dateFrom);
				args.Add("?iki", dateTo);
			});

		var result = 
			Sql.MapAll<ContractsReport.Sutartis>(drc, (dre, t) => {
				t.Nr = dre.From<int>("nr");
				t.SutartiesData = dre.From<DateTime>("automobilio_priemimo_data");
				t.AsmensKodas = dre.From<string>("asmens_kodas");
				t.Vardas = dre.From<string>("vardas");
				t.Pavarde = dre.From<string>("pavarde");
				t.Kaina = dre.From<decimal>("kaina");
				t.DetaliuKaina = dre.From<decimal>("detaliu_kaina");
				t.BendraSuma = dre.From<decimal>("bendra_suma");
				t.Busena = dre.From<string>("remonto_busena");
				t.BendraSumaDetaliu = dre.From<decimal>("bdk");
			});

		return result;
	}

	public static List<LateContractsReport.Sutartis> GetLateReturnContracts(DateTime? dateFrom, DateTime? dateTo)
	{
		var query =
			$@"SELECT
				sut.nr,
				sut.automobilio_priemimo_data,
				sut.numatoma_suremontavimo_data as plandata,
				CONCAT(kln.vardas, ' ',kln.pavarde) as klientas,
				CASE
					WHEN sut.numatoma_suremontavimo_data = '0001-01-01' THEN 'DATOS NĖRA'
					ELSE DATE_FORMAT(sut.numatoma_suremontavimo_data, '%Y-%m-%d %H:%i')
				END as grazinimo_data
			FROM `remonto_sutartis` sut, `klientas` kln
			WHERE
				kln.asmens_kodas = sut.fk_KLIENTASasmens_kodas
				AND DATEDIFF(sut.numatoma_suremontavimo_data, NOW()) <= 1
				AND sut.automobilio_priemimo_data >= IFNULL(?nuo, sut.automobilio_priemimo_data)
				AND sut.automobilio_priemimo_data <= IFNULL(?iki, sut.automobilio_priemimo_data)";

		var drc =
			Sql.Query(query, args => {
				args.Add("?nuo", dateFrom);
				args.Add("?iki", dateTo);
			});

		var result = 
			Sql.MapAll<LateContractsReport.Sutartis>(drc, (dre, t) => {
				t.Nr = dre.From<int>("nr");
				t.SutartiesData = dre.From<DateTime>("automobilio_priemimo_data");
				t.Klientas = dre.From<string>("klientas");
				t.PlanuojamaStData = dre.From<string>("grazinimo_data");
			});

		return result;
	}
}
