﻿namespace Org.Ktu.Isk.P175B602.Autonuoma.Repositories;

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
				`{Config.TblPrefix}paslaugos` pasl,
				`{Config.TblPrefix}uzsakytos_paslaugos` up,
				`{Config.TblPrefix}sutartys` sut
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
				kln.vardas,
				kln.pavarde,
				kln.asmens_kodas,
				sut.remonto_kaina,
				bs1.bendra_suma
			FROM
				`remonto_sutartis` sut
				INNER JOIN `klientas` kln ON kln.asmens_kodas = sut.fk_KLIENTASasmens_kodas
				LEFT JOIN
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
							kln2.asmens_kodas
						FROM
							`remonto_sutartis` sut2
							INNER JOIN `klientas` kln2 ON kln2.asmens_kodas = sut2.fk_KLIENTASasmens_kodas
						WHERE
							sut2.automobilio_priemimo_data >= IFNULL(?nuo, sut2.automobilio_priemimo_data)
							AND sut2.automobilio_priemimo_data <= IFNULL(?iki, sut2.automobilio_priemimo_data)
						GROUP BY kln2.asmens_kodas
					) AS bs2
					ON bs2.asmens_kodas = kln.asmens_kodas
			WHERE
				sut.automobilio_priemimo_data >= IFNULL(?nuo, sut.automobilio_priemimo_data)
				AND sut.automobilio_priemimo_data <= IFNULL(?iki, sut.automobilio_priemimo_data)
			GROUP BY 
				sut.nr, kln.asmens_kodas
			ORDER BY 
				kln.pavarde ASC";

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
				t.Kaina = dre.From<decimal?>("remonto_kaina");
				//t.BendraSuma = dre.From<decimal>("bendra_suma");
				//t.BendraSumaPaslaug = dre.From<decimal>("bendra_suma_paslaugu");
			});

		return result;
	}

	/*public static List<LateContractsReport.Sutartis> GetLateReturnContracts(DateTime? dateFrom, DateTime? dateTo)
	{
		var query =
			$@"SELECT
				sut.nr,
				sut.automobilio_priemimo_data,
				CONCAT(kln.vardas, ' ',kln.pavarde) as klientas,
				sut.planuojama_grazinimo_data_laikas,
				IF(
					IFNULL(sut.faktine_grazinimo_data_laikas,'0000-00-00') LIKE '0000%',
					'negražinta',
					sut.faktine_grazinimo_data_laikas
				) as grazinimo_data
			FROM `{Config.TblPrefix}sutartys` sut, `{Config.TblPrefix}klientai` kln
			WHERE
				kln.asmens_kodas = sut.fk_klientas
				AND (
					DATEDIFF(sut.faktine_grazinimo_data_laikas, sut.planuojama_grazinimo_data_laikas) >= 1
					OR IFNULL(sut.faktine_grazinimo_data_laikas, '0000-00-00') like '0000%'
					AND DATEDIFF(NOW(), sut.planuojama_grazinimo_data_laikas) >=1
				)
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
				t.SutartiesData = dre.From<DateTime>("sutarties_data");
				t.Klientas = dre.From<string>("klientas");
				t.PlanuojamaGrData = dre.From<DateTime>("planuojama_grazinimo_data_laikas");
				t.FaktineGrData = dre.From<string>("grazinimo_data");
			});

		return result;
	}*/
}
