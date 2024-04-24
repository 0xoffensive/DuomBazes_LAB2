namespace Org.Ktu.Isk.P175B602.Autonuoma.Repositories;

using System.Data;
using Microsoft.AspNetCore.Components.Web;
using MySql.Data.MySqlClient;

using Newtonsoft.Json;
using Org.Ktu.Isk.P175B602.Autonuoma.Models;
using Org.Ktu.Isk.P175B602.Autonuoma.Models.SutartisF2;


/// <summary>
/// Database operations related to 'Sutartis' entity.
/// </summary>
public class SutartisF2Repo
{
	public static List<SutartisL> ListSutartis()
	{
		var query =
			$@"SELECT
				s.nr,
				s.automobilio_priemimo_data as data,
				CONCAT(d.vardas,' ', d.pavarde) as darbuotojas,
				CONCAT(k.vardas,' ',k.pavarde) as klientas,
				s.remonto_busena as busena
			FROM
				`remonto_sutartis` s
				LEFT JOIN `darbuotojas` d ON s.fk_DARBUOTOJAStabelio_nr=d.tabelio_nr
				LEFT JOIN `klientas` k ON s.fk_KLIENTASasmens_kodas=k.asmens_kodas
			ORDER BY s.nr DESC";

		var drc = Sql.Query(query);

		var result =
			Sql.MapAll<SutartisL>(drc, (dre, t) => {
				t.Nr = dre.From<int>("nr");
				t.Darbuotojas = dre.From<string>("darbuotojas");
				t.Klientas = dre.From<string>("klientas");
				t.Data = dre.From<DateTime>("data");
				t.Busena = dre.From<string>("busena");
			});

		return result;
	}

	public static SutartisCE FindSutartisCE(int nr)
	{
		var query = $@"SELECT * FROM `remonto_sutartis` WHERE nr=?nr";
		var drc =
			Sql.Query(query, args => {
				args.Add("?nr", nr);
			});

		var result =
			Sql.MapOne<SutartisCE>(drc, (dre, t) => {
				//make a shortcut
				var sut = t.Sutartis;

				sut.Nr = dre.From<int>("nr");
				sut.FkAutomobilis = dre.From<string>("fk_AUTOMOBILISvin");
				sut.PriemimoData = dre.From<DateTime>("automobilio_priemimo_data");
				sut.PlanuojamaSuremontuotiData = dre.From<DateTime?>("numatoma_suremontavimo_data");
				sut.PradineRida = dre.From<int>("pradine_rida");
				sut.GalineRida = dre.From<int>("galutine_rida");
				sut.Kaina = dre.From<decimal?>("remonto_kaina");
				sut.Busena = dre.From<string>("remonto_busena");
				sut.FkKlientas = dre.From<string>("fk_KLIENTASasmens_kodas");
				sut.FkDarbuotojas = dre.From<int>("fk_DARBUOTOJAStabelio_nr");
			});

		return result;
	}

	public static int InsertSutartis(SutartisCE sutCE)
	{
		var query =
			$@"INSERT INTO `remonto_sutartis`
			(
				`nr`,
				`fk_AUTOMOBILISvin`,
				`automobilio_priemimo_data`,
				`numatoma_suremontavimo_data`,
				`pradine_rida`,
				`galutine_rida`,
				`remonto_kaina`,
				`remonto_busena`,
				`fk_KLIENTASasmens_kodas`,
				`fk_DARBUOTOJAStabelio_nr`
			)
			VALUES(
				?nr,
				?vin,
				?pri_data,
				?num_data,
				?prrida,
				?glrida,
				?kaina,
				?busena,
				?fk_klientas,
				?fk_darbuotojas
			)";

		var nr =
			Sql.Insert(query, args => {
				//make a shortcut
				var sut = sutCE.Sutartis;
				//
				args.Add("?nr", sut.Nr);
				args.Add("?vin", sut.FkAutomobilis);
				args.Add("?pri_data", sut.PriemimoData);
				args.Add("?num_data", sut.PlanuojamaSuremontuotiData);
				args.Add("?prrida", sut.PradineRida);
				args.Add("?glrida", sut.GalineRida);
				args.Add("?kaina", sut.Kaina);
				args.Add("?busena", sut.Busena);
				args.Add("?fk_klientas", sut.FkKlientas);
				args.Add("?fk_darbuotojas", sut.FkDarbuotojas);
			});

		return (int)nr;
	}

	public static void UpdateSutartis(SutartisCE sutCE)
	{
		var query =
			$@"UPDATE `remonto_sutartis`
			SET
				`fk_AUTOMOBILISvin` = ?vin,
				`automobilio_priemimo_data` = ?pdata,
				`numatoma_suremontavimo_data` = ?rdata,
				`pradine_rida` = ?prrida,
				`galutine_rida` = ?glrida,
				`remonto_kaina` = ?kaina,
				`remonto_busena` = ?busena,
				`fk_KLIENTASasmens_kodas` = ?klientas,
				`fk_DARBUOTOJAStabelio_nr` = ?darbuotojas
			WHERE nr=?nr";

		Sql.Update(query, args => {
			//make a shortcut
			var sut = sutCE.Sutartis;
			//
			args.Add("?vin", sut.FkAutomobilis);
			args.Add("?pdata", sut.PriemimoData);
			args.Add("?rdata", sut.PlanuojamaSuremontuotiData);
			args.Add("?prrida", sut.PradineRida);
			args.Add("?glrida", sut.GalineRida);
			args.Add("?kaina", sut.Kaina);
			args.Add("?busena", sut.Busena);
			args.Add("?klientas", sut.FkKlientas);
			args.Add("?darbuotojas", sut.FkDarbuotojas);

			args.Add("?nr", sut.Nr);
		});
	}

	public static void DeleteSutartis(int nr)
	{
		var query = $@"DELETE FROM `remonto_sutartis` where nr=?nr";
		Sql.Delete(query, args => {
			args.Add("?nr", nr);
		});
	}

	public static List<SutartisCE.PrirasytasMech> ListAptarnauja(int sutartisId)
	{
		var query =
			$@"SELECT *
			FROM `aptarnauja`
			WHERE fk_REMONTO_SUTARTISnr = ?sutartisId
			ORDER BY fk_MECHANIKAStabelio_nr ASC";

		var drc =
			Sql.Query(query, args => {
				args.Add("?sutartisId", sutartisId);
			});

		var result =
			Sql.MapAll<SutartisCE.PrirasytasMech>(drc, (dre, t) => {
				t.Mechanikas = dre.From<int?>("fk_MECHANIKAStabelio_nr");
			});

		for( int i = 0; i < result.Count; i++ )
			result[i].InListId = i;

		return result;
	}

	public static void InsertAptarnauja(int sutartisID, int? mechanikasID)
	{
		var query =
			$@"INSERT INTO `aptarnauja`
				(
					fk_MECHANIKAStabelio_nr,
					fk_REMONTO_SUTARTISnr
				)
				VALUES(
					?fk_mechanikas,
					?fk_sutartis
				)";
		Sql.Insert(query, args => {

			args.Add("?fk_mechanikas", mechanikasID);
			args.Add("?fk_sutartis", sutartisID);
		});
	}

	public static void DeleteAptarnauja(int sutartis)
	{
		var query =
			$@"DELETE FROM a
			USING `aptarnauja` as a
			WHERE a.fk_REMONTO_SUTARTISnr=?fk";

		Sql.Delete(query, args => {
			args.Add("?fk", sutartis);
		});
	}

	public static List<SutartisCE.Gedimas> ListTuriGedima(int sutartisId)
	{
		var query =
			$@"SELECT *
			FROM `turi`
			WHERE fk_REMONTO_SUTARTISnr = ?sutartisId
			ORDER BY fk_GEDIMASID_ ASC";

		var drc =
			Sql.Query(query, args => {
				args.Add("?sutartisId", sutartisId);
			});

		var result =
			Sql.MapAll<SutartisCE.Gedimas>(drc, (dre, t) => {
				t.GedimoId = dre.From<int?>("fk_GEDIMASID_");
			});

		for( int i = 0; i < result.Count; i++ )
			result[i].InListId = i;

		return result;
	}

	public static void InsertTuriGedima(int sutartisID, int? gedimasID)
	{
		var query =
			$@"INSERT INTO `turi`
				(
					fk_GEDIMASID_,
					fk_REMONTO_SUTARTISnr
				)
				VALUES(
					?fk_gedimas,
					?fk_sutartis
				)";
		Sql.Insert(query, args => {

			args.Add("?fk_gedimas", gedimasID);
			args.Add("?fk_sutartis", sutartisID);
		});
	}

	public static void DeleteTuriGedima(int sutartis)
	{
		var query =
			$@"DELETE FROM a
			   USING `turi` as a
			   WHERE a.fk_REMONTO_SUTARTISnr=?fk";

		Sql.Delete(query, args => {
			args.Add("?fk", sutartis);
		});
	}

	public static List<SutartisCE.AtlDarb> ListAtliktasDarbas(int sutartisId)
	{
		var query =
			$@"SELECT
				d.darbo_pavadinimas,
				d.darbo_kaina,
				d.nuvaziuota,
				k.fk_DETALEID_ as detaleid,
				k.kiekis
			FROM
				`atliktas_darbas` d
				LEFT JOIN `detales_kiekis` k ON k.fk_ATLIKTAS_DARBASID_ = d.ID_
			WHERE
				d.fk_REMONTO_SUTARTISnr = ?sutartisId
			ORDER BY d.ID_ ASC";

		var drc1 =
			Sql.Query(query, args => {
				args.Add("?sutartisId", sutartisId);
			});

		var result =
			Sql.MapAll<SutartisCE.AtlDarb>(drc1, (dre, t) => {
				t.DarboPav = dre.From<string>("darbo_pavadinimas");
				t.DarbKaina = dre.From<int?>("darbo_kaina");
				t.Nuvaziuota = dre.From<int?>("nuvaziuota");
				t.DetaleId = dre.From<int>("detaleid");
				t.DetaleKiekis = dre.From<int>("kiekis");
			});




		for( int i = 0; i < result.Count; i++ )
			result[i].InListId = i;

		return result;
	}

	public static int InsertAtliktasDarbas(int sutartisId, SutartisCE.AtlDarb up)
	{
		var query =
			$@"INSERT INTO `atliktas_darbas`
				(
					fk_REMONTO_SUTARTISnr,
					darbo_pavadinimas,
					darbo_kaina,
					nuvaziuota
				)
				VALUES(
					?fk_sutartis,
					?pavadinimas,
					?kaina,
					?nuvaziuota
				)";
		var id =
		Sql.Insert(query, args => {

			args.Add("?fk_sutartis", sutartisId);
			args.Add("?pavadinimas", up.DarboPav);
			args.Add("?kaina", up.DarbKaina);
			args.Add("?nuvaziuota", up.Nuvaziuota);
		});
		return (int)id;
	}

	public static void DeleteAtliktasDarbas(int sutartis)
	{
		var queryDelete2 =
			$@"DELETE FROM `atliktas_darbas`
			   WHERE fk_REMONTO_SUTARTISnr=?kys";

		Sql.Delete(queryDelete2, args => {
				args.Add("?kys", sutartis);
			});
	}

	public static void InsertDetalesKiekis(int darbasId, SutartisCE.AtlDarb up)
	{
		var query =
			$@"INSERT INTO `detales_kiekis`
				(
					fk_ATLIKTAS_DARBASID_,
					fk_DETALEID_,
					kiekis
				)
				VALUES(
					?fk_darbas,
					?fk_detale,
					?kiekis
				)";
		Sql.Insert(query, args => {

			args.Add("?fk_darbas", darbasId);
			args.Add("?fk_detale", up.DetaleId);
			args.Add("?kiekis", up.DetaleKiekis);
		});
	}

	public static void DeleteDetalesKiekis(int sutartis)
	{
		var query =
			$@"SELECT
				ID_
			FROM
				`atliktas_darbas`
			WHERE
				fk_REMONTO_SUTARTISnr=?sutartis";

		var drc1 =
			Sql.Query(query, args => {
				args.Add("?sutartis", sutartis);
			});

		var result =
			Sql.MapAll<AtliktasDarbas>(drc1, (dre, t) => {
				t.Id = dre.From<int>("ID_");
			});


		var queryDelete =
			$@"DELETE FROM `detales_kiekis`
			   WHERE fk_ATLIKTAS_DARBASID_=?fk";
		foreach (var id in result)
		{
			Sql.Delete(queryDelete, args => {
				args.Add("?fk", (int)id.Id);
			});
		}
	}
}