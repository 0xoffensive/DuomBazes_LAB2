namespace Org.Ktu.Isk.P175B602.Autonuoma.Repositories;

using System.Collections;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc.Rendering;
using MySql.Data.MySqlClient;

using Org.Ktu.Isk.P175B602.Autonuoma.Models.Automobilis;


/// <summary>
/// Database operations related to 'Automobilis'.
/// </summary>
public class AutomobilisRepo
{
	public static List<AutomobilisL> ListAutomobilis()
	{
		var query =
			$@"SELECT
				a.vin,
				a.valstybinis_nr,
				
				m.modelio_pavadinimas AS modelis,
				mm.pavadinimas AS gamintojas
			FROM
				automobilis a
				
				LEFT JOIN `modelis` m ON m.ID_ = a.fk_MODELISID_
				LEFT JOIN `gamintojas` mm ON mm.ID_ = m.fk_GAMINTOJASID_
			ORDER BY a.vin ASC";

		var drc = Sql.Query(query);

		var result =
			Sql.MapAll<AutomobilisL>(drc, (dre, t) => {
				t.Vin = dre.From<string>("vin");
				t.ValstybinisNr = dre.From<string>("valstybinis_nr");
				t.Modelis = dre.From<string>("modelis");
				t.Gamintojas = dre.From<string>("gamintojas");
			});

		return result;
	}

	public static AutomobilisCE FindAutomobolisCE(string vin)
	{
		var query = $@"SELECT * FROM `automobilis` WHERE vin=?vin";

		var drc =
			Sql.Query(query, args => {
				args.Add("?vin", vin);
			});

		var result =
			Sql.MapOne<AutomobilisCE>(drc, (dre, t) => {
				//make a shortcut
				var auto = t.Automobilis;

				//
				auto.Vin = dre.From<string>("vin");
				auto.ValstybinisNr = dre.From<string>("valstybinis_nr");
				auto.PirmRegMetai = dre.From<int>("pirmos_registracijos_metai");
				auto.Variklis = dre.From<string>("variklio_pavadinimas");
				auto.Galia = dre.From<string>("faktine_variklio_galia");
				auto.Spalva = dre.From<string>("spalva");
				auto.PavaruDeze = dre.From<string>("pavaru_deze");
				auto.DegaluTipas = dre.From<string>("kuro_tipas");
				auto.KebuloTipas = dre.From<string>("kebulas");
				auto.FkModelis = dre.From<int>("fk_MODELISID_");
			});

		return result;
	}

	public static void InsertAutomobilis(AutomobilisCE autoCE)
	{
		var query =
			$@"INSERT INTO `automobilis`
			(
				`vin`,
				`valstybinis_nr`,
				`pirmos_registracijos_metai`,
				`variklio_pavadinimas`,
				`spalva`,
				`faktine_variklio_galia`,
				`pavaru_deze`,
				`kebulas`,
				`kuro_tipas`,
				`fk_MODELISID_`
			)
			VALUES (
				?vin,
				?vlst_nr,
				?reg_mt,
				?variklis,
				?spalva,
				?galia,
				?pav_deze,
				?kebulas,
				?dega_tip,
				?fk_mod
			)";

		Sql.Insert(query, args => {
			var auto = autoCE.Automobilis;

			//
			args.Add("?vin", auto.Vin);
			args.Add("?vlst_nr", auto.ValstybinisNr);
			args.Add("?reg_mt", auto.PirmRegMetai);
			args.Add("?variklis", auto.Variklis);
			args.Add("?spalva", auto.Spalva);
			args.Add("?galia", auto.Galia);
			args.Add("?pav_deze", auto.PavaruDeze);
			args.Add("?kebulas", auto.KebuloTipas);
			args.Add("?dega_tip", auto.DegaluTipas);
			args.Add("?fk_mod", auto.FkModelis);
		});
	}

	public static void UpdateAutomobilis(AutomobilisCE autoCE)
	{
		var query =
			$@"UPDATE `automobilis`
			SET
				`valstybinis_nr` = ?vlst_nr,
				`pirmos_registracijos_metai` = ?pag_mt,
				`variklio_pavadinimas` = ?variklis,
				`spalva` = ?spalva,
				`faktine_variklio_galia` = ?galia,
				`pavaru_deze` = ?pav_deze,
				`kebulas` = ?kebulas,
				`kuro_tipas` = ?kuras,
				`fk_MODELISID_` = ?fk_mod
			WHERE
				vin=?vin";

		Sql.Update(query, args => {
			//make a shortcut
			var auto = autoCE.Automobilis;
			//
			args.Add("?vlst_nr", auto.ValstybinisNr);
			args.Add("?pag_mt", auto.PirmRegMetai);
			args.Add("?variklis", auto.Variklis);
			args.Add("?spalva", auto.Spalva);
			args.Add("?galia", auto.Galia);
			args.Add("?pav_deze", auto.PavaruDeze);
			args.Add("?kuras", auto.DegaluTipas);
			args.Add("?kebulas", auto.KebuloTipas);
			args.Add("?fk_mod", auto.FkModelis);
			args.Add("?vin", auto.Vin);
		});
	}

	public static void DeleteAutomobilis(string vin)
	{
		var query = $@"DELETE FROM `automobilis` WHERE vin=?vin";
		Sql.Delete(query, args => {
			args.Add("?vin", vin);
		});
	}
}
